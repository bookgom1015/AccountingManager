using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using AccountingManager.Core.Models;

namespace AccountingManager.Helpers {
    public class MariaDbManager {
        public async Task<bool> ConnectToDBAsync(string address, Int16 port, string uid, string pwd) {
            string connectionCommand = string.Format("Server={0};Port={1};Uid={2};Pwd={3}", address, port, uid, pwd);
            
            try {
                mSqlConnection = new MySqlConnection(connectionCommand);

                await mSqlConnection.OpenAsync();
            }
            catch (MySqlException e) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("MySqlException: ");
                stringBuilder.Append(e.Message);
                stringBuilder.Append("; Command: \'Server=[Server Address];Port=[Port Number];Uid=[Unique Identification];Pwd=[Password]\' failed;");

                await Logger.Logln(stringBuilder.ToString());
                return false;
            }
            catch (Exception e) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Exception: ");
                stringBuilder.Append(e.Message);
                stringBuilder.Append("; Command: \'Server=[Server Address];Port=[Port Number];Uid=[Unique Identification];Pwd=[Password]\' failed;");

                await Logger.Logln(stringBuilder.ToString());
                return false;
            }

            return true;
        }

        public void DisconnnectFromDB() {
            if (mSqlConnection != null) mSqlConnection.Close();
            mSqlConnection = null;
        }

        private async Task LogMySqlException(string inErrMsg, string inCommandText, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("File Path: ");
            stringBuilder.Append(inFilePath);
            stringBuilder.Append("; Line Number: ");
            stringBuilder.Append(inLineNumber);
            stringBuilder.Append("; MySqlException: ");
            stringBuilder.Append(inErrMsg);
            stringBuilder.Append("; Command: \'");
            stringBuilder.Append(inCommandText);
            stringBuilder.Append("\' failed;");

            await Logger.Logln(stringBuilder.ToString());
        }

        private async Task LogException(string inErrMsg, string inCommandText, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("File Path: ");
            stringBuilder.Append(inFilePath);
            stringBuilder.Append("; Line Number: ");
            stringBuilder.Append(inLineNumber);
            stringBuilder.Append("; Exception: ");
            stringBuilder.Append(inErrMsg);
            stringBuilder.Append("; Command: \'");
            stringBuilder.Append(inCommandText);
            stringBuilder.Append("\' failed;");

            await Logger.Logln(stringBuilder.ToString());
        }

        public async Task<bool> ExecuteNonQueryAsync(string inCommandText) {
            try {
                MySqlCommand command = new MySqlCommand(inCommandText, mSqlConnection);

                int status = await command.ExecuteNonQueryAsync();
                if (status == -1) {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("NonQuery: ");
                    stringBuilder.Append(inCommandText);
                    stringBuilder.Append(" failed");

                    await Logger.Logln(stringBuilder.ToString());
                    return false;
                }
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, inCommandText);
                return false;
            }
            catch (Exception e) {
                await LogException(e.Message, inCommandText);
                return false;
            }

            return true;
        }

        public async Task<bool> GetDatabaseNamesAsync(List<string> outDatabaseNameList) {
            string showCommandText = "SHOW DATABASES;";

            try {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader databases = await showCommand.ExecuteReaderAsync();

                /*
                 * The SqlDataReader class is used to retrieve data one record (one row) at a time while maintaining a connection with SQL Server.
                 * Since the SqlDataReader object returned from SqlCommand.ExecuteReader() places the pointer before the first Row (like the file's BOF),
                 * the developer must move to the first row by using the SqlDataReader's Read() method.
                 */
                while (databases.Read()) {
                    string databaseName = databases["Database"] as string;
                    outDatabaseNameList.Add(databaseName);
                }

                // MySqlDataReader should close after processing is completed.
                databases.Close();
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, showCommandText);
                return false;
            }
            catch (Exception e) {
                await LogException(e.Message, showCommandText);
                return false;
            }

            return true;
        }

        public async Task<bool> UseDatabaseAsync(string inDatabaseName) {
            List<string> databaseNameList = new List<string>();

            bool getResult = await GetDatabaseNamesAsync(databaseNameList);
            if (!getResult) return false;

            bool found = false;

            foreach (string databaseName in databaseNameList) {
                if (databaseName == inDatabaseName) {
                    found = true;
                    break;
                }
            }

            // Handled when there is no database name to find.
            if (!found) {
                StringBuilder createCommandBuilder = new StringBuilder();
                createCommandBuilder.Append("CREATE DATABASE ");
                createCommandBuilder.Append(inDatabaseName);
                createCommandBuilder.Append(" CHARACTER SET UTF8;");

                string createCommandText = createCommandBuilder.ToString();

                bool creationResult = await ExecuteNonQueryAsync(createCommandText);
                if (!creationResult) return false;
            }

            StringBuilder useCommandBuilder = new StringBuilder();
            useCommandBuilder.Append("USE ");
            useCommandBuilder.Append(inDatabaseName);

            string useCommandText = useCommandBuilder.ToString();

            bool useResult = await ExecuteNonQueryAsync(useCommandText);
            if (!useResult) return false;

            return true;
        }

        public async Task<bool> DatabaseContainsAsync(string inDatabaseName, string inTablename) {
            string showCommandText = "SHOW TABLES;";

            try {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader tables = await showCommand.ExecuteReaderAsync();

                while (tables.Read()) {
                    string tableName = tables[string.Format("Tables_in_{0}", inDatabaseName)] as string;
                    if (tableName == inTablename) {
                        tables.Close();

                        return true;
                    }
                }

                tables.Close();
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, showCommandText);
            }
            catch (Exception e) {
                await LogException(e.Message, showCommandText);
            }

            return false;
        }

        public async Task<bool> GetTableNamesLikeAsync(string inDatabaseName, string inLike, List<string> outTableNameList) {
            StringBuilder showCommandBuilder = new StringBuilder();
            showCommandBuilder.Append("SHOW TABLES LIKE \'");
            showCommandBuilder.Append(inLike);
            showCommandBuilder.Append("\';");

            string showCommandText = showCommandBuilder.ToString();

            try {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader dataInTable = await showCommand.ExecuteReaderAsync();

                while (dataInTable.Read()) {
                    StringBuilder dataKeyBuilder = new StringBuilder();
                    dataKeyBuilder.Append("Tables_in_");
                    dataKeyBuilder.Append(inDatabaseName);
                    dataKeyBuilder.Append(" (");
                    dataKeyBuilder.Append(inLike);
                    dataKeyBuilder.Append(")");

                    string tableName = dataInTable[dataKeyBuilder.ToString()] as string;

                    outTableNameList.Add(tableName);
                }

                dataInTable.Close();
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, showCommandText);
                return false;
            }
            catch (Exception e)
            {
                await LogException(e.Message, showCommandText);
                return false;
            }

            return true;
        }

        public async Task<bool> CreateTableAsync(string inTableName) {
            StringBuilder createCommandBuilder = new StringBuilder();
            createCommandBuilder.Append("CREATE TABLE ");
            createCommandBuilder.Append(inTableName);
            createCommandBuilder.Append("(");
            createCommandBuilder.Append(TableParameters);
            createCommandBuilder.Append(") ENGINE=");
            createCommandBuilder.Append(StorageEngine);
            createCommandBuilder.Append(";");

            string createCommandText = createCommandBuilder.ToString();

            bool creationResult = await ExecuteNonQueryAsync(createCommandText);
            if (!creationResult) return false;

            return true;
        }

        public async Task<bool> TableContainsAsync(string inTableName, AccountingData.QueryKeys inQueryKeys, AccountingData.QueryValues inQueryValues) {
            StringBuilder selectCommandBuilder = new StringBuilder();

            selectCommandBuilder.Append("SELECT ");

            StringBuilder existsBuilder = new StringBuilder();
            existsBuilder.Append("EXISTS (SELECT 1 FROM ");
            existsBuilder.Append(inTableName);
            existsBuilder.Append(" WHERE deleted=0 AND");

            if ((inQueryKeys & AccountingData.QueryKeys.EClientName) != 0) {
                existsBuilder.Append(" client_name LIKE \'%");
                existsBuilder.Append(inQueryValues.ClientName);
                existsBuilder.Append("%\' AND"); ;
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDate) != 0) {
                existsBuilder.Append(" date=\'");
                existsBuilder.Append(inQueryValues.Date);
                existsBuilder.Append("\' AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ESteelWeight) != 0) {
                existsBuilder.Append(" steel_weight=");
                existsBuilder.Append(inQueryValues.SteelWeight);
                existsBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ESupplyPrice) != 0) {
                existsBuilder.Append(" supply_price=");
                existsBuilder.Append(inQueryValues.SupplyPrice);
                existsBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ETaxAmount) != 0) {
                existsBuilder.Append(" tax_amount=");
                existsBuilder.Append(inQueryValues.TaxAmount);
                existsBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDataType) != 0) {
                existsBuilder.Append(" data_type=");
                existsBuilder.Append(inQueryValues.DataType ? 1 : 0);
                existsBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDepositConfirm) != 0) {
                existsBuilder.Append(" deposit_confirm=");
                existsBuilder.Append(inQueryValues.DepositConfirm ? 1 : 0);
                existsBuilder.Append(" AND");
            }

            existsBuilder.Remove(existsBuilder.Length - 4, 4);
            existsBuilder.Append(")");

            string existsCommandText = existsBuilder.ToString();

            selectCommandBuilder.Append(existsCommandText);
            selectCommandBuilder.Append(";");

            string selectCommandText = selectCommandBuilder.ToString();

            try {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                dataInTable.Read();

                int exists = (int)(long)dataInTable[existsCommandText];

                dataInTable.Close();

                if (exists == 1) return true;
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, selectCommandText);
            }
            catch (Exception e) {
                await LogException(e.Message, selectCommandText);
            }

            return false;
        }

        public async Task<bool> GetAccountingDataAsync(string inTableName, AccountingData.QueryKeys inQueryKeys, AccountingData.QueryValues inQueryValues, List<AccountingData> outDataList) {
            StringBuilder commandBuilder = new StringBuilder();

            commandBuilder.Append("SELECT * FROM ");
            commandBuilder.Append(inTableName);
            commandBuilder.Append(" WHERE deleted=0 AND");

            if ((inQueryKeys & AccountingData.QueryKeys.EClientName) != 0) {
                commandBuilder.Append(" client_name LIKE \'%");
                commandBuilder.Append(inQueryValues.ClientName);
                commandBuilder.Append("%\' AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDate) != 0) {
                commandBuilder.Append(" date=\'");
                commandBuilder.Append(inQueryValues.Date);
                commandBuilder.Append("\' AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ESteelWeight) != 0) {
                commandBuilder.Append(" steel_weight=");
                commandBuilder.Append(inQueryValues.SteelWeight);
                commandBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ESupplyPrice) != 0) {
                commandBuilder.Append(" supply_price=");
                commandBuilder.Append(inQueryValues.SupplyPrice);
                commandBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.ETaxAmount) != 0) {
                commandBuilder.Append(" tax_amount=");
                commandBuilder.Append(inQueryValues.TaxAmount);
                commandBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDataType) != 0) {
                commandBuilder.Append(" data_type=");
                commandBuilder.Append(inQueryValues.DataType ? 1 : 0);
                commandBuilder.Append(" AND");
            }
            if ((inQueryKeys & AccountingData.QueryKeys.EDepositConfirm) != 0) {
                commandBuilder.Append(" deposit_confirm=");
                commandBuilder.Append(inQueryValues.DepositConfirm ? 1 : 0);
                commandBuilder.Append(" AND");
            }

            commandBuilder.Remove(commandBuilder.Length - 4, 4);
            commandBuilder.Append(";");

            string selectCommandText = commandBuilder.ToString();

            try {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                while (dataInTable.Read()) {
                    int id = (int)dataInTable["id"];
                    string clientName = dataInTable["client_name"] as string;
                    string date = dataInTable["date"] as string;
                    int steelWeight = (int)dataInTable["steel_weight"];
                    int supplyPrice = (int)dataInTable["supply_price"];
                    int taxAmount = (int)dataInTable["tax_amount"];
                    bool dataType = (bool)dataInTable["data_type"];
                    bool depositConfirm = (bool)dataInTable["deposit_confirm"];
                    string depositDate = dataInTable["deposit_date"] as string;

                    AccountingData data = new AccountingData(id, clientName, date, depositDate, steelWeight, supplyPrice, taxAmount, dataType, depositConfirm);

                    outDataList.Add(data);
                }

                dataInTable.Close();
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, selectCommandText);
                return false;
            }
            catch (Exception e) {
                await LogException(e.Message, selectCommandText);
                return false;
            }

            return true;
        }

        public async Task<int> GetLastIdInTableAsync(string inTableName) {
            StringBuilder selectCommandBuilder = new StringBuilder();
            selectCommandBuilder.Append("SELECT id FROM ");
            selectCommandBuilder.Append(inTableName);
            selectCommandBuilder.Append(" ORDER BY id DESC LIMIT 1");

            string selectCommandText = selectCommandBuilder.ToString();

            try {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                dataInTable.Read();

                int id = (int)dataInTable["id"];

                dataInTable.Close();

                return id;
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, selectCommandText);
            }
            catch (Exception e) {
                await LogException(e.Message, selectCommandText);
            }

            return -1;
        }

        public async Task<bool> GetDistinctDateDataAsync(string inTableName, List<string> outDateList) {
            StringBuilder selectCommandBuilder = new StringBuilder();
            selectCommandBuilder.Append("SELECT DISTINCT date FROM ");
            selectCommandBuilder.Append(inTableName);
            selectCommandBuilder.Append(" WHERE deleted = 0;");

            string selectCommandText = selectCommandBuilder.ToString();

            try {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                while (dataInTable.Read()) {
                    string date = dataInTable["date"] as string;

                    outDateList.Add(date);
                }

                dataInTable.Close();
            }
            catch (MySqlException e) {
                await LogMySqlException(e.Message, selectCommandText);
                return false;
            }
            catch (Exception e) {
                await LogException(e.Message, selectCommandText);
                return false;
            }

            return true;
        }

        public async Task<bool> InsertDataAsync(string inTableName, int inId, string inClientName, string inDate, string inDepositDate, int inSteelWeight, int inSupplyPrice, int inTaxAmount, bool inDataType, bool inDepositConfirm) {
            int intDataType = inDataType ? 1 : 0;
            int intConfirm = inDepositConfirm ? 1 : 0;

            string values = string.Format("{0}, \'{1}\', \'{2}\', {3}, {4}, {5}, {6}, {7}, \'{8}\', 0", inId, inClientName, inDate, inSteelWeight, inSupplyPrice, inTaxAmount, intDataType, intConfirm, inDepositDate);
            //inId + ", \'" + inClientName + "\', \'" + inDate + "\', " + inSteelWeight + ", " + inSupplyPrice + ", " + inTaxAmount + ", " + intDataType + ", " + intConfirm + ", \'" + inDepositDate + "\', 0";            

            StringBuilder insertCommandBuilder = new StringBuilder();
            insertCommandBuilder.Append("INSERT INTO ");
            insertCommandBuilder.Append(inTableName);
            insertCommandBuilder.Append("(");
            insertCommandBuilder.Append(TableFields);
            insertCommandBuilder.Append(") VALUES (");
            insertCommandBuilder.Append(values);
            insertCommandBuilder.Append(");");

            string insertCommandText = insertCommandBuilder.ToString();

            bool insertionResult = await ExecuteNonQueryAsync(insertCommandText);
            if (!insertionResult) return false;

            return true;
        }

        public async Task<bool> UpdateDataAsync(string inTableName, AccountingData inData, AccountingData.QueryKeys inQueryKeys) {
            StringBuilder updateCommandBuilder = new StringBuilder();
            updateCommandBuilder.Append("UPDATE " + inTableName + " SET ");

            int dataType = inData.DataType ? 1 : 0;
            int depositConfirm = inData.DepositConfirm ? 1 : 0;

            if ((inQueryKeys & AccountingData.QueryKeys.EClientName) != 0)      updateCommandBuilder.Append(string.Format("client_name=\'{0}\', "   , inData.ClientName));
            if ((inQueryKeys & AccountingData.QueryKeys.EDate) != 0)            updateCommandBuilder.Append(string.Format("date=\'{0}\', "          , inData.Date));
            if ((inQueryKeys & AccountingData.QueryKeys.ESteelWeight) != 0)     updateCommandBuilder.Append(string.Format("steel_weight={0}, "      , inData.SteelWeight));
            if ((inQueryKeys & AccountingData.QueryKeys.ESupplyPrice) != 0)     updateCommandBuilder.Append(string.Format("supply_price={0}, "      , inData.SupplyPrice));
            if ((inQueryKeys & AccountingData.QueryKeys.ETaxAmount) != 0)       updateCommandBuilder.Append(string.Format("tax_amount={0}, "        , inData.TaxAmount));
            if ((inQueryKeys & AccountingData.QueryKeys.EDataType) != 0)        updateCommandBuilder.Append(string.Format("data_type={0}, "         , dataType));
            if ((inQueryKeys & AccountingData.QueryKeys.EDepositConfirm) != 0)  updateCommandBuilder.Append(string.Format("deposit_confirm={0}, "   , depositConfirm));
            if ((inQueryKeys & AccountingData.QueryKeys.EDepositDate) != 0)     updateCommandBuilder.Append(string.Format("deposit_date=\'{0}\', "  , inData.DepositDate));

            updateCommandBuilder.Remove(updateCommandBuilder.Length - 2, 2);
            updateCommandBuilder.Append(" WHERE id=" + inData.Id + ";");

            string updateCommandText = updateCommandBuilder.ToString();

            bool updateResult = await ExecuteNonQueryAsync(updateCommandText);
            if (!updateResult) return false;

            return true;
        }

        public async Task<bool> DeleteDataAsync(string inTableName, int inId) {
            StringBuilder updateCommandBuilder = new StringBuilder();
            updateCommandBuilder.Append("UPDATE ");
            updateCommandBuilder.Append(inTableName);
            updateCommandBuilder.Append(" SET deleted=1 WHERE id=");
            updateCommandBuilder.Append(inId);
            updateCommandBuilder.Append(";");

            string updateCommandText = updateCommandBuilder.ToString();

            bool updateResult = await ExecuteNonQueryAsync(updateCommandText);
            if (!updateResult) return false;

            return true;
        }

        private string TableParameters { get => "id INT, client_name VARCHAR(60), date VARCHAR(5), steel_weight INT, supply_price INT, tax_amount INT, data_type BOOLEAN, deposit_confirm BOOLEAN, deposit_date VARCHAR(8), deleted BOOLEAN"; }
        private string TableFields { get => "id, client_name, date, steel_weight, supply_price, tax_amount, data_type, deposit_confirm, deposit_date, deleted"; }
        private string StorageEngine { get => "InnoDB"; }

        private MySqlConnection mSqlConnection = null;
    }
}
