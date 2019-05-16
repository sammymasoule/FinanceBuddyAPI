using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using FinanceBuddy_API.Controllers.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using Swashbuckle.Swagger.Annotations;

namespace FinanceBuddy_API.Controllers
{
    public class ExpensesController : ApiController
    {
        private readonly ExpensesService tiService = new ExpensesService();

        // GET request for sum of expenses for specific user.
        [Route("api/expense/sum/{username}")]
        public IHttpActionResult GetExpensesBy(string username, [FromUri]string firstday, [FromUri]string lastday)
        {
            var expenses = tiService.GetExpenses(username, firstday, lastday);
            //tjek det her
            return Content(HttpStatusCode.OK, expenses);

            //return Content(HttpStatusCode.OK, new List<KeyValuePair<string, float>>());
        }

        // GET request for the sum of a specific category for a specific user
        [Route("api/expense/sum/{username}/{category}")]
        public IHttpActionResult GetSumOfCategory(string username, string category, [FromUri]string firstday, [FromUri]string lastday)
        {
            var expenses = tiService.GetAvgExpenses(username, category, firstday, lastday);
            if (Math.Abs(expenses) < 0.1)
            {
                return Content(HttpStatusCode.NoContent, "");
            }

            return Content(HttpStatusCode.OK, expenses);
        }

        // GET request for average of categories for all users excluding the user requesting the info.
        [Route("api/expense/average")]
        public IHttpActionResult GetAvgOthers(string username, string cat, string firstday, string lastday)
        {
            var expenses = tiService.GetAvgExpensesOthers(username, cat, firstday, lastday);
            if (Math.Abs(expenses) < 0.1)
            {
                return Content(HttpStatusCode.NoContent, "");
            }

            return Content(HttpStatusCode.OK, expenses);
        }

        // POST request for creating an expense for a specific user.
        [Route("api/expense")]
        public IHttpActionResult Post([FromBody] object value)
        {
            var json = JObject.Parse(value.ToString());
            var username = json["Username"].ToString();
            var cat = json["Category"].ToString();
            var date = DateTime.Parse(json["Date"].ToString()).ToString("yyyy-MM-dd");
            var description = json["Description"].ToString();
            float.TryParse(json["Amount"].ToString(), out var amount);
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(cat) && !date.IsNullOrWhiteSpace() &&
                !String.IsNullOrEmpty(description) && amount > -1)
            {
                if (tiService.CreateExpense(cat, description, date, username, amount))
                {
                    return Content(HttpStatusCode.OK, "Udgift oprettet");
                }
            }

            return Content(HttpStatusCode.BadRequest, "Forkert input, tjek værdierne og prøv igen.");
        }
    }
}