using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

using AccountingManager.Core.Abstract;
using AccountingManager.Core.Infrastructures;
using AccountingManager.Core.Models;

namespace AccountingManager.Core.Concrete {
    public class MariaDbManager : IDbManager {
        public Result Connect(string address, Int16 port, string uid, string pwd) {
            string cmdTxt = string.Format("Server={0};Port={1};Uid={2};Pwd={3};", address, port, uid, pwd);

            try {
                connection = new MySqlConnection(cmdTxt);
                
                connection.Open();
            }
            catch (Exception e) {
                connection = null;
                return new Result { Status = false, ErrMsg = e.Message };
            }

            return Result.Success;
        }

        public void Disconnect() {
            if (connection != null) connection.Close();
            connection = null;
        }

        public Result ExecuteNonQuery(string cmdTxt) {
            try {
                MySqlCommand command = new MySqlCommand(cmdTxt, connection);

                int status = command.ExecuteNonQuery();
                if (status == -1) return new Result { Status = false, ErrMsg = "Failed to execute non query command" };
            }
            catch (Exception e) {
                return new Result { Status = false, ErrMsg = e.Message };
            }

            return Result.Success;
        }

        public Result Use(string database) {
            string cmdTxt = string.Format("USE {0}", database);

            return ExecuteNonQuery(cmdTxt);
        }

        public Result Add(AccountingData data) {
            string cmdTxt = string.Format(
                "INSERT INTO {0}(client_name, date, steel_weight, supply_price, tax_amount, data_type, deposit_confirmed, deposit_date, deleted) VALUES (\'{1}\', \'{2}\', {3}, {4}, {5}, {6}, {7}, \'{8}\', 0);",
                GlobalSettings.DataTableName, data.ClientName, data.Date.ToString("yyyy-MM-dd"), data.SteelWeight, data.SupplyPrice, data.TaxAmount, data.DataType ? 1 : 0, data.DepositConfirmed ? 1 : 0, data.DepositDate);

            return ExecuteNonQuery(cmdTxt);
        }

        public Result Update(AccountingData data, AccountingDataQueryKeys keys) {
            if (keys == AccountingDataQueryKeys.ENone) return Result.Success;

            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.AppendFormat("UPDATE {0} SET", GlobalSettings.DataTableName);

            if ((keys & AccountingDataQueryKeys.EClientName)        != 0) cmdBuilder.Append(string.Format(" client_name=\'{0}\',",    data.ClientName));
            if ((keys & AccountingDataQueryKeys.EDate)              != 0) cmdBuilder.Append(string.Format(" date=\'{0}\',",           data.Date.ToString("yyyy-MM-dd")));
            if ((keys & AccountingDataQueryKeys.ESteelWeight)       != 0) cmdBuilder.Append(string.Format(" steel_weight={0},",       data.SteelWeight));
            if ((keys & AccountingDataQueryKeys.ESupplyPrice)       != 0) cmdBuilder.Append(string.Format(" supply_price={0},",       data.SupplyPrice));
            if ((keys & AccountingDataQueryKeys.ETaxAmount)         != 0) cmdBuilder.Append(string.Format(" tax_amount={0},",         data.TaxAmount));
            if ((keys & AccountingDataQueryKeys.EDataType)          != 0) cmdBuilder.Append(string.Format(" data_type={0},",          data.DataType ? 1 : 0));
            if ((keys & AccountingDataQueryKeys.EDepositConfirmed)  != 0) cmdBuilder.Append(string.Format(" deposit_confirmed={0},",  data.DepositConfirmed ? 1 : 0));
            if ((keys & AccountingDataQueryKeys.EDepositDate)       != 0) cmdBuilder.Append(string.Format(" deposit_date=\'{0}\',",   data.DepositDate));

            cmdBuilder.Remove(cmdBuilder.Length - 1, 1);
            cmdBuilder.AppendFormat(" WHERE uid={0};", data.Uid);

            return ExecuteNonQuery(cmdBuilder.ToString());
        }

        public Result Delete(AccountingData data) {
            string cmdTxt = string.Format("UPDATE {0} SET deleted=1 WHERE uid={1}", GlobalSettings.DataTableName, data.Uid);

            return ExecuteNonQuery(cmdTxt);
        }

        public Result GetDates(out IEnumerable<int> dates, int? year = null, int? month = null, bool receivable = false) {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.Append("deleted=0");
            if (receivable) whereBuilder.Append(" AND deposit_confirmed=0");

            string cmdTxt;
            if (month != null) cmdTxt = string.Format("SELECT DISTINCT DAY(date) FROM {0} WHERE {1} AND date LIKE \'{2}-{3:D2}-%\' ORDER BY DAY(date);", GlobalSettings.DataTableName, whereBuilder.ToString(), year, month);
            else if (year != null) cmdTxt = string.Format("SELECT DISTINCT MONTH(date) FROM {0} WHERE {1} AND date LIKE \'{2}-%\' ORDER BY MONTH(date);", GlobalSettings.DataTableName, whereBuilder.ToString(), year);
            else cmdTxt = string.Format("SELECT DISTINCT YEAR(date) FROM {0} WHERE {1} ORDER BY YEAR(date);", GlobalSettings.DataTableName, whereBuilder.ToString());

            MySqlDataReader dataInTable = null;

            try {
                MySqlCommand command = new MySqlCommand(cmdTxt, connection);
                dataInTable = command.ExecuteReader();

                List<int> dateList = new List<int>();

                if (!dataInTable.HasRows) {
                    dates = dateList;
                    dataInTable.Close();
                    return Result.Success;
                }

                if (month != null) {
                    while (dataInTable.Read()) {
                        int clearDay = (int)dataInTable["DAY(date)"];

                        dateList.Add(clearDay);
                    }
                }
                else if (year != null) {
                    while (dataInTable.Read()) {
                        int clearMonth = (int)dataInTable["MONTH(date)"];

                        dateList.Add(clearMonth);
                    }
                }
                else {
                    while (dataInTable.Read()) {
                        int clearYear = (int)dataInTable["YEAR(date)"];
                        
                        dateList.Add(clearYear);
                    }
                }

                dates = dateList;
                dataInTable.Close();
            }
            catch (Exception e) {
                dates = null;
                if (dataInTable != null && !dataInTable.IsClosed) dataInTable.Close();
                return new Result { Status = false, ErrMsg = e.Message };
            }

            return Result.Success;
        }        

        private Result GetData(out IEnumerable<AccountingData> data, string cmdTxt) {
            MySqlDataReader dataInTable = null;

            try {
                MySqlCommand command = new MySqlCommand(cmdTxt, connection);
                dataInTable = command.ExecuteReader();

                List<AccountingData> dataList = new List<AccountingData>();

                if (!dataInTable.HasRows) {
                    data = dataList;
                    dataInTable.Close();
                    return Result.Success;
                }

                while (dataInTable.Read()) {
                    uint uid = (uint)dataInTable["uid"];
                    string clientName = dataInTable["client_name"] as string;
                    DateTime date = (DateTime)dataInTable["date"];
                    float steelWeight = (float)dataInTable["steel_weight"];
                    uint supplyPrice = (uint)dataInTable["supply_price"];
                    uint taxAmount = (uint)dataInTable["tax_amount"];
                    bool dataType = Convert.ToBoolean(dataInTable["data_type"]);
                    bool depositConfirmed = Convert.ToBoolean(dataInTable["deposit_confirmed"]);
                    string depositDate = dataInTable["deposit_date"] as string;

                    AccountingData newData = new AccountingData {
                        Uid = uid,
                        ClientName = clientName,
                        Date = date,
                        SteelWeight = steelWeight,
                        SupplyPrice = supplyPrice,
                        TaxAmount = taxAmount,
                        DataType = dataType,
                        DepositConfirmed = depositConfirmed,
                        DepositDate = depositDate
                    };

                    dataList.Add(newData);
                }

                data = dataList;
                dataInTable.Close();
            }
            catch (Exception e) {
                data = null;
                if (dataInTable != null && !dataInTable.IsClosed) dataInTable.Close();
                return new Result { Status = false, ErrMsg = e.Message };
            }

            return Result.Success;
        }

        public Result GetData(out IEnumerable<AccountingData> data, int? year = null, int? month = null, int? day = null, bool receivable = false) {
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.AppendFormat("SELECT * FROM {0} WHERE deleted=0", GlobalSettings.DataTableName);
            if (receivable) cmdBuilder.AppendFormat(" AND deposit_confirmed=0");
            if (day != null) cmdBuilder.AppendFormat(" AND date=\'{0}-{1:D2}-{2:D2}\'", year, month, day);
            else if (month != null) cmdBuilder.AppendFormat(" AND date LIKE \'{0}-{1:D2}-%\'", year, month);
            else if (year != null) cmdBuilder.AppendFormat(" AND date LIKE \'{0}-%\'", year);
            cmdBuilder.Append(" ORDER BY date;");

            return GetData(out data, cmdBuilder.ToString());
        }

        public Result GetData(out IEnumerable<AccountingData> data, DateTime begin, DateTime end, string clientName, bool receivable = false) {
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.AppendFormat("SELECT * FROM {0} WHERE deleted=0", GlobalSettings.DataTableName);
            if (receivable) cmdBuilder.Append(" AND deposit_confirmed=0");
            cmdBuilder.AppendFormat(" AND client_name LIKE \'%{0}%\' AND date>=\'{1}\' AND date<=\'{2}\' ORDER BY date;", clientName, begin.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

            return GetData(out data, cmdBuilder.ToString());
        }

        private MySqlConnection connection;

        public bool IsConnected { get => connection != null; }
    }
}
