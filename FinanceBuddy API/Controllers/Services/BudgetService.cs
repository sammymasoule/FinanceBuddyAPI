using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FinanceBuddy_API.Controllers.Services
{
    public class BudgetService
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
        /// Method for creating a budget for a specific user. The budget helps one keep track of how much money they are allowed to spend.
        /// </summary>
        /// <param name="username"></param> User name of current logged in user.
        /// <param name="loanLimit"></param> Budget limit that the user wishes to have for loan expenses.
        /// <param name="houseHoldLimit"></param> Budget limit that the user wishes to have for household expenses.
        /// <param name="consumptionLimit"></param> Budget limit that the user wishes to have for consumption expenses.
        /// <param name="transportLimit"></param> Budget limit that the user wishes to have for transportation expenses.
        /// <param name="savingsLimit"></param> Budget limit that the user wishes to have for savings expenses.
        public bool CreateBudget(string username, float loanLimit, float GroceryLimit, float houseHoldLimit, float consumptionLimit, float transportLimit, float savingsLimit) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO Budget ([userName], [LoanLimit], [GroceryLimit], [HouseHoldLimit], [ConsumptionLimit], [TransportationLimit], [SavingsLimit])");
                    sb.Append(" VALUES ('" + username + "', '" + loanLimit + "', '" + GroceryLimit + "', '" + houseHoldLimit + "', '" + consumptionLimit + "', '" + transportLimit + "', '" + savingsLimit + "')");
                    //sb.Append("VALUES ('{0}', '{1}', '{2}', '{3}')", lastName, firstName, userName, password);
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
        /// Method for retreiving the budget limits for each category.
        /// </summary>
        /// <param name="userName"></param> User name of current logged in user.
        public List<float> GetBudgetLimits(string userName) {
            try {
                List<float> limits = new List<float>();
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select LoanLimit, GroceryLimit, HouseHoldLimit, ConsumptionLimit, TransportationLimit, SavingsLimit from Budget where userName = '" + userName + "'");
                    string sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                limits.Add(float.Parse(reader["LoanLimit"].ToString()));
                                limits.Add(float.Parse(reader["GroceryLimit"].ToString()));
                                limits.Add(float.Parse(reader["HouseHoldLimit"].ToString()));
                                limits.Add(float.Parse(reader["ConsumptionLimit"].ToString()));
                                limits.Add(float.Parse(reader["TransportationLimit"].ToString()));
                                limits.Add(float.Parse(reader["SavingsLimit"].ToString()));
                            }
                        }
                    }
                }

                return limits;
            }
            catch (SqlException exception) {
                Console.WriteLine(exception.ToString());
            }
            return null;
        }

        /// <summary>
        /// Method for changing/updating a current budget for a specific user.
        /// </summary>
        /// <param name="username"></param> User name of current logged in user.
        /// <param name="loanLimit"></param> Budget limit that the user wishes to have for loan expenses.
        /// <param name="houseHoldLimit"></param> Budget limit that the user wishes to have for household expenses.
        /// <param name="consumptionLimit"></param> Budget limit that the user wishes to have for consumption expenses.
        /// <param name="transportLimit"></param> Budget limit that the user wishes to have for transportation expenses.
        /// <param name="savingsLimit"></param> Budget limit that the user wishes to have for savings expenses.
        public bool UpdateBudget(string username, float loanLimit, float groceryLimit, float houseHoldLimit, float consumptionLimit, float transportLimit, float savingsLimit) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Update Budget set LoanLimit =" + loanLimit + ", GroceryLimit = " + groceryLimit +  ", HouseHoldLimit = " + houseHoldLimit + ", ConsumptionLimit= " + consumptionLimit + ", TransportationLimit= " + transportLimit + ", SavingsLimit=" + savingsLimit + " where username='" + username + "'");
                    //sb.Append("VALUES ('{0}', '{1}', '{2}', '{3}')", lastName, firstName, userName, password);

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
        /// Method to delete budget for specific user.
        /// </summary>
        public bool DeleteBudget(string username) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("DELETE FROM Budget WHERE userName ='" + username + "'");
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
    }
}