using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FinanceBuddy_API.Controllers.Services {
    public class IncomeService {

        readonly SqlConnectionStringBuilder builder =
            new SqlConnectionStringBuilder
            {
                DataSource = "samsamjon.database.windows.net",
                UserID = "samsamjon",
                Password = "Test1234",
                InitialCatalog = "samjonDB"
            };

        /// <summary>
        /// Method that creates an income for a specific user.
        /// </summary>
        /// <param name="amount"></param> Amount of money of the income.
        /// <param name="date"></param> When the income was received.
        /// <param name="userName"></param> The user name of the user logged in.
        /// <param name="description"></param> A short description of the income.
        public bool CreateIncome(float amount, string date, string userName, string description) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO Income ([Amount], [Date], [userName], [Description])");
                    sb.Append("VALUES ('" + amount + "', '" + date + "', '" + userName + "', '" + description + "')");
                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        if (command.ExecuteNonQuery() > 0) {
                            return true;
                        }
                    }
                }
            }
            catch (SqlException exception) {
                Console.WriteLine(exception.ToString());
            }
            return false;
        }

        /// <summary>
        /// Method for getting the total income of a user from and to a specific date.
        /// </summary>
        /// <param name="userName"></param> Chosen user name for the user.
        /// <param name="firstDay"></param> Chosen start date.
        /// <param name="lastDay"></param> Chosen end date.
        public float GetIncome(string userName, string firstDay, string lastDay) {
            try {
                float amount;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select SUM(Amount) from Income where userName = '" + userName + "'" +
                              " AND Date between '" + firstDay + "' AND '" + lastDay + "'");
                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        var value = command.ExecuteScalar();
                        if (!String.IsNullOrEmpty(value.ToString())) {
                            amount = float.Parse(value.ToString());
                        }
                        else {
                            return 0;
                        }
                    }
                }
                return amount;
            }
            catch (SqlException exception) {
                Console.WriteLine(exception.ToString());
            }
            return -1;
        }
    }
}