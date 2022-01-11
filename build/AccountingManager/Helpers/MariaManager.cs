using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using AccountingManager.Core.Models;

namespace AccountingManager.Helpers
{
    public class MariaManager
    {
        public async Task<bool> ConnectToDBAsync(string address, Int16 port, string uid, string pwd)
        {
            string connectionCommand = string.Format("Server={0};Port={1};Uid={2};Pwd={3}", address, port, uid, pwd);

            try
            {
                mSqlConnection = new MySqlConnection(connectionCommand);

                await mSqlConnection.OpenAsync();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("MySqlException: " + e.Message + "; Command: \'" + connectionCommand + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("Exception: " + e.Message + "; Command: \'" + connectionCommand + "\' failed;");
                return false;
            }

            return true;
        }

        public void DisconnnectFromDB()
        {
            if (mSqlConnection != null) mSqlConnection.Close();
            mSqlConnection = null;
        }
        
        public async Task<bool> ExecuteNonQueryAsync(string inCommandText, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(inCommandText, mSqlConnection);

                int status = await command.ExecuteNonQueryAsync();
                if (status == -1)
                {
                    await Logger.Logln("NonQuery: " + inCommandText + " failed");
                    return false;
                }
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + inCommandText + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + inCommandText + "\' failed;");
                return false;
            }

            return true;
        }

        public async Task<bool> GetDatabaseNamesAsync(List<string> outDatabaseNameList, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string showCommandText = "SHOW DATABASES;";
            
            try
            {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader databases = await showCommand.ExecuteReaderAsync();

                /*
                 * The SqlDataReader class is used to retrieve data one record (one row) at a time while maintaining a connection with SQL Server.
                 * Since the SqlDataReader object returned from SqlCommand.ExecuteReader() places the pointer before the first Row (like the file's BOF),
                 * the developer must move to the first row by using the SqlDataReader's Read() method.
                 */
                while (databases.Read())
                {
                    string databaseName = databases["Database"] as string;
                    outDatabaseNameList.Add(databaseName);
                }

                // MySqlDataReader should close after processing is completed.
                databases.Close();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
                return false;
            }

            return true;
        }

        public async Task<bool> UseDatabaseAsync(string inDatabaseName)
        {
            List<string> databaseNameList = new List<string>();

            bool getResult = await GetDatabaseNamesAsync(databaseNameList);
            if (!getResult) return false;

            bool found = false;

            foreach (string databaseName in databaseNameList)
            {
                if (databaseName == inDatabaseName)
                {
                    found = true;
                    break;
                }
            }

            // Handled when there is no database name to find.
            if (!found)
            {
                string createCommandText = "CREATE DATABASE " + inDatabaseName + " CHARACTER SET UTF8;";

                bool creationResult = await ExecuteNonQueryAsync(createCommandText);
                if (!creationResult) return false;
            }

            string useCommandText = "USE " + inDatabaseName;

            bool useResult = await ExecuteNonQueryAsync(useCommandText);
            if (!useResult) return false;

            return true;
        }

        public async Task<bool> DatabaseContainsAsync(string inDatabaseName, string inTablename, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string showCommandText = "SHOW TABLES;";

            try
            {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader tables = await showCommand.ExecuteReaderAsync();

                while (tables.Read())
                {
                    string tableName = tables["Tables_in_" + inDatabaseName] as string;
                    if (tableName == inTablename)
                    {
                        tables.Close();

                        return true;
                    }
                }

                tables.Close();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
            }

            return false;
        }

        public async Task<bool> CreateTableAsync(string inTableName)
        {
            string createCommandText = "CREATE TABLE " + inTableName + "(" + TableParameters + ") ENGINE=" + StorageEngine + ";";

            bool creationResult = await ExecuteNonQueryAsync(createCommandText);
            if (!creationResult) return false;

            return true;
        }

        public async Task<bool> GetTableNamesLikeAsync(string inDatabaseName, string inLike, List<string> outTableNameList, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string showCommandText = "SHOW TABLES LIKE \'" + inLike + "\';";

            try
            {
                MySqlCommand showCommand = new MySqlCommand(showCommandText, mSqlConnection);
                DbDataReader dataInTable = await showCommand.ExecuteReaderAsync();

                while (dataInTable.Read())
                {
                    string tableName = dataInTable["Tables_in_" + inDatabaseName + " (" + inLike + ")"] as string;

                    outTableNameList.Add(tableName);
                }

                dataInTable.Close();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + showCommandText + "\' failed;");
                return false;
            }

            return true;
        }

        public async Task<int> GetLastIdInTableAsync(string inTableName, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string selectCommandText = "SELECT id FROM " + inTableName + " ORDER BY id DESC LIMIT 1";;

            try
            {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                dataInTable.Read();

                int id = (int)dataInTable["id"];

                dataInTable.Close();

                return id;
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
            }

            return -1;
        }

        public async Task<bool> TableNotEmptyAsync(string inTableName, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string selectCommandText = "SELECT EXISTS (SELECT 1 FROM " + inTableName + " WHERE deleted=0);";

            try
            {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                dataInTable.Read();

                int exists = (int)(long)dataInTable["EXISTS (SELECT 1 FROM " + inTableName + " WHERE deleted=0)"];

                dataInTable.Close();

                if (exists == 1) return true;
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
            }

            return false;
        }
        
        public async Task<bool> GetDataAsync(string inTableame, List<AccountingData> refDataList, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string selectCommandText = "SELECT * FROM " + inTableame + " WHERE deleted = 0;";

            try
            {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();
                
                while (dataInTable.Read())
                {
                    int id = (int)dataInTable["id"];
                    string clientName = dataInTable["client_name"] as string;
                    string date = dataInTable["date"] as string;
                    int steelWeight = (int)dataInTable["steel_weight"];
                    int supplyPrice = (int)dataInTable["supply_price"];
                    int taxAmount = (int)dataInTable["tax_amount"];
                    bool dataType = (bool)dataInTable["data_type"];
                    bool depositConfirm = (bool)dataInTable["deposit_confirm"];

                    AccountingData data = new AccountingData(id, clientName, date, steelWeight, supplyPrice, taxAmount, dataType, depositConfirm);

                    refDataList.Add(data);
                }

                dataInTable.Close();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
                return false;
            }

            return true;
        }

        public async Task<bool> GetDistinctDateDataAsync(string inTableName, List<string> outDateList, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            string selectCommandText = "SELECT DISTINCT date FROM " + inTableName + " WHERE deleted = 0;";

            try
            {
                MySqlCommand selectCommand = new MySqlCommand(selectCommandText, mSqlConnection);
                DbDataReader dataInTable = await selectCommand.ExecuteReaderAsync();

                while (dataInTable.Read())
                {
                    string date = dataInTable["date"] as string;

                    outDateList.Add(date);
                }

                dataInTable.Close();
            }
            catch (MySqlException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; MySqlException: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
                return false;
            }
            catch (Exception e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; Exception: " + e.Message + "; Command: \'" + selectCommandText + "\' failed;");
                return false;
            }

            return true;
        }

        public async Task<bool> InsertDataAsync(string inTableName, int inId, string inClientName, string inDate, int inWeight, int inPrice, int inTax, bool inDataType, bool inConfirm)
        {
            int intDataType = inDataType ? 1 : 0;
            int intConfirm = inConfirm ? 1 : 0;
            string values = inId + ", \'" + inClientName + "\', \'" + inDate + "\', " + inWeight + ", " + inPrice + ", " + inTax + ", " + intDataType + ", " + intConfirm + ", 0";

            string insertCommandText = "INSERT INTO " + inTableName + "(" + TableFields + ") VALUES (" + values + ");";

            bool insertionResult = await ExecuteNonQueryAsync(insertCommandText);
            if (!insertionResult) return false;

            return true;
        }

        public async Task<bool> UpdateDataAsync(string inTableName, AccountingData inData, AccountingData.ChangedData inChangedData)
        {
            StringBuilder updateCommandBuilder = new StringBuilder();
            updateCommandBuilder.Append("UPDATE " + inTableName + " SET ");

            int dataType = inData.DataType ? 1 : 0;
            int depositConfirm = inData.DepositConfirm ? 1 : 0;

            if ((inChangedData & AccountingData.ChangedData.EClientName) != 0) updateCommandBuilder.Append("client_name=\'" + inData.ClientName + "\', ");
            if ((inChangedData & AccountingData.ChangedData.EDate) != 0) updateCommandBuilder.Append("date=\'" + inData.Date + "\', ");
            if ((inChangedData & AccountingData.ChangedData.ESteelWeight) != 0) updateCommandBuilder.Append("steel_weight=" + inData.SteelWeight + ", ");
            if ((inChangedData & AccountingData.ChangedData.ESupplyPrice) != 0) updateCommandBuilder.Append("supply_price=" + inData.SupplyPrice + ", ");
            if ((inChangedData & AccountingData.ChangedData.ETaxAmount) != 0) updateCommandBuilder.Append("tax_amount=" + inData.TaxAmount + ", ");
            if ((inChangedData & AccountingData.ChangedData.EDataType) != 0) updateCommandBuilder.Append("data_type=" + dataType + ", ");
            if ((inChangedData & AccountingData.ChangedData.EDepositConfirm) != 0) updateCommandBuilder.Append("deposit_confirm=" + depositConfirm + ", ");

            updateCommandBuilder.Remove(updateCommandBuilder.Length - 2, 2);
            updateCommandBuilder.Append(" WHERE id=" + inData.Id + ";");

            string updateCommandText = updateCommandBuilder.ToString();

            bool updateResult = await ExecuteNonQueryAsync(updateCommandText);
            if (!updateResult) return false;

            return true;
        }

        public async Task<bool> DeleteDataAsync(string inTableName, int inId)
        {            
            string updateCommandText = "UPDATE " + inTableName + " SET " + "deleted=1" + " WHERE id=" + inId + ";";

            bool updateResult = await ExecuteNonQueryAsync(updateCommandText);
            if (!updateResult) return false;

            return true;
        }

        private string TableParameters
        {
            get => "id INT, client_name VARCHAR(60), date VARCHAR(5), steel_weight INT, supply_price INT, tax_amount INT, data_type BOOLEAN, deposit_confirm BOOLEAN, deleted BOOLEAN";
        }

        private string TableFields
        {
            get => "id, client_name, date, steel_weight, supply_price, tax_amount, data_type, deposit_confirm, deleted";
        }

        private string StorageEngine
        {
            get => "InnoDB";
        }

        private MySqlConnection mSqlConnection = null;
    }
}
