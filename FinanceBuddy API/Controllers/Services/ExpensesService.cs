using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FinanceBuddy_API.Controllers.Services
{
    public class ExpensesService
    {
        readonly SqlConnectionStringBuilder builder =
            new SqlConnectionStringBuilder
            {
                DataSource = "samsamjon.database.windows.net",
                UserID = "samsamjon",
                Password = "Test1234",
                InitialCatalog = "samjonDB"
            };

        /// <summary>
        /// Method for creating an expense related to a specific user.
        /// </summary>
        /// <param name="category"></param> Which category the expense falls under.
        /// <param name="description"></param> Short description of the expense.
        /// <param name="date"></param> Which date the expense occured.
        /// <param name="userName"></param> User name of current logged in user.
        /// <param name="amount"></param> Cost of the expense.
        public bool CreateExpense(string category, string description, string date, string userName, float amount) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO TransItem ([Category], [Description], [Date], [userName], [Amount])");
                    sb.Append("VALUES ('" + category + "', '" + description + "', '" + date + "', '" + userName + "', '" + amount + "')");
                    //sb.Append("VALUES ('{0}', '{1}', '{2}', '{3}')", lastName, firstName, userName, password
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
        /// Method for getting the sum of the user's expenses in categorized order.
        /// </summary>
        /// <param name="userName"></param> User name of current logged in user.
        /// <param name="firstDay"></param> Chosen start date.
        /// <param name="lastDay"></param> Chosen end date.
        public Dictionary<string, float> GetExpenses(string userName, string firstDay, string lastDay) {
            try {
                List<KeyValuePair<string, float>> expenses = new List<KeyValuePair<string, float>>();
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select Category, Amount from TransItem where userName = '" + userName + "' " +
                              " AND Date between '" + firstDay + "' AND '" + lastDay + "'");
                    string sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                expenses.Add(new KeyValuePair<string, float>(reader.GetString(0), (float)reader.GetDouble(1)));
                            }
                        }
                    }
                }
                var myResults = expenses.GroupBy(p => p.Key)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Value));
                return myResults;
            }
            catch (SqlException exception) {
                Console.WriteLine(exception.ToString());
            }
            return new Dictionary<string, float>();
        }

        /// <summary>
        /// Method for getting the average expenses of all users not including the user currently logged in.
        /// </summary>
        /// <param name="userName"></param> User name of current logged in user.
        /// <param name="cat"></param> Chosen category to compare expenses.
        /// <param name="firstDay"></param> Chosen start date.
        /// <param name="lastDay"></param> Chosen end date.
        public float GetAvgExpensesOthers(string userName, string cat, string firstDay, string lastDay) {
            try {
                float amount;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select SUM(Amount) / COUNT(Distinct userName) from TransItem where userName NOT IN ('" + userName + "')" +
                              " AND Category = '" + cat + "' AND Date between '" + firstDay + "' AND '" + lastDay + "'");
                    string sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        var value = command.ExecuteScalar();
                        if (!string.IsNullOrEmpty(value.ToString())) {
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

        /// <summary>
        /// Method for getting the sum of the user's expenses from a single category in a specific time frame.
        /// </summary>
        /// <param name="userName"></param> User name of current logged in user.
        /// <param name="cat"></param> Chosen category to compare expenses.
        /// <param name="firstDay"></param> Chosen start date.
        /// <param name="lastDay"></param> Chosen end date.
        public float GetAvgExpenses(string userName, string cat, string firstDay, string lastDay) {
            try {
                float amount;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select SUM(Amount) from TransItem where userName = '" + userName + "'" +
                              " AND Category = '" + cat + "' AND Date between '" + firstDay + "' AND '" + lastDay + "'");
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