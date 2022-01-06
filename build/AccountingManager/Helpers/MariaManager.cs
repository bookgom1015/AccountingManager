using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using AccountingManager.Core.Models;

namespace AccountingManager.Helpers
{
    public class MariaManager
    {
        public bool ConnectToDB(string address, Int16 port, string uid, string pwd, out string errMsg)
        {
            string connectionCommand = string.Format("Server={0};Port={1};Uid={2};Pwd={3}", address, port, uid, pwd);

            try
            {
                SqlConnection = new MySqlConnection(connectionCommand);

                SqlConnection.Open();
            }
            catch (InvalidOperationException e)
            {
                errMsg = "[InvalidOperationException] " + e.Message;
                return false;
            }
            catch (MySqlException e)
            {
                errMsg = "[MySqlException] " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return false;
            }

            errMsg = null;
            return true;
        }

        public void DisconnnectFromDB()
        {
            if (SqlConnection != null) SqlConnection.Close();
            SqlConnection = null;
        }

        public bool ExecuteNonQuery(string inCommandText, out string outErrMsg)
        {
            MySqlCommand command = new MySqlCommand(inCommandText, SqlConnection);
            if (command.ExecuteNonQuery() == -1)
            {
                outErrMsg = "NonQuery: " + inCommandText + " failed.";
                return false;
            }

            outErrMsg = null;
            return true;
        }

        public void GetDatabaseNames(ref List<string> refDatabaseNameList)
        {
            MySqlCommand showCommand = new MySqlCommand("SHOW DATABASES;", SqlConnection);
            MySqlDataReader databases = showCommand.ExecuteReader();
            
            /*
             * The SqlDataReader class is used to retrieve data one record (one row) at a time while maintaining a connection with SQL Server.
             * Since the SqlDataReader object returned from SqlCommand.ExecuteReader() places the pointer before the first Row (like the file's BOF),
             * the developer must move to the first row by using the SqlDataReader's Read() method.
             */
            while (databases.Read())
            {
                string databaseName = databases["Database"] as string;
                refDatabaseNameList.Add(databaseName);
            }
            // MySqlDataReader should close after processing is completed.
            databases.Close();
        }

        public bool UseDatabase(string inDatabaseName, out string outErrMsg)
        {
            List<string> databaseNameList = new List<string>();
            GetDatabaseNames(ref databaseNameList);

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
                if (!ExecuteNonQuery(createCommandText, out outErrMsg)) return false;
            }

            string useCommandText = "USE " + inDatabaseName;
            if (!ExecuteNonQuery(useCommandText, out outErrMsg)) return false;

            outErrMsg = null;
            return true;
        }

        public bool DatabaseContains(string inDatabaseName, string inTablename)
        {
            MySqlCommand showCommand = new MySqlCommand("SHOW TABLES;", SqlConnection);
            MySqlDataReader tables = showCommand.ExecuteReader();

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
            return false;
        }

        public bool CreateTable(string inTableName, out string outErrMsg)
        {
            string createCommandText = "CREATE TABLE " + inTableName + "(" + TableParameters + ") ENGINE=" + StorageEngine + ";";
            if (!ExecuteNonQuery(createCommandText, out outErrMsg)) return false;

            outErrMsg = null;
            return true;
        }

        public void GetTables(string inTableame, ref List<AccountingData> refDataList)
        {
            string selectCommandText = "SELECT * FROM " + inTableame + " WHERE deleted = 0;";
            MySqlCommand selectCommand = new MySqlCommand(selectCommandText, SqlConnection);
            MySqlDataReader dataInTable = selectCommand.ExecuteReader();
            
            while (dataInTable.Read())
            {
                int  id = (int)dataInTable["id"];
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
        }

        public bool InsertData(string inTableName, int inId, string inClientName, string inDate, int inWeight, int inPrice, int inTax, bool inType, bool inConfirm, out string outErrMsg)
        {
            int intType = inType ? 1 : 0;
            int intConfirm = inConfirm ? 1 : 0;
            string values = inId + ", \'" + inClientName + "\', \'" + inDate + "\', " + inWeight + ", " + inPrice + ", " + inTax + ", " + intType + ", " + intConfirm + ", 0";

            string insertCommandText = "INSERT INTO " + inTableName + "(" + TableFields + ") VALUES (" + values + ");";
            if (!ExecuteNonQuery(insertCommandText, out outErrMsg)) return false;

            outErrMsg = null;
            return true;
        }

        private string TableParameters
        {
            // fd1: id
            // fd2: client_name
            // fd3: date
            // fd4: weight
            // fd5: price
            // fd6: tax
            // fd7: data_type
            // fd8: deposit_confirm
            // fd9: deleted
            get => "fd1 INT, fd2 VARCHAR(60), fd3 VARCHAR(10), fd4 INT, fd5 INT, fd6 INT, fd7 BOOLEAN, fd8 BOOLEAN, fd9 BOOLEAN";
        }

        private string TableFields
        {
            get => "fd1, fd2, fd3, fd4, fd5, fd6, fd7, fd8, fd9";
        }

        private string StorageEngine
        {
            get => "InnoDB";
        }

        private MySqlConnection mSqlConnection = null;
        public MySqlConnection SqlConnection
        {
            get => mSqlConnection;
            set => mSqlConnection = value;
        }
    }
}
