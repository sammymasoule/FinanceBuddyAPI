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
    public class IncomeController : ApiController
    {
        private readonly IncomeService incomeService= new IncomeService();

        // GET api/income/id
        [Route("api/income/{username}")]
        public IHttpActionResult GetIncomeByDate(string username, [FromUri]string firstday, [FromUri]string lastday)
        {
            var income = incomeService.GetIncome(username, firstday, lastday);
            return Content(HttpStatusCode.OK, income);
        }
        
        // POST api/Income
        [Route("api/Income")]
        public IHttpActionResult Post([FromBody]object value) {
            var json = JObject.Parse(value.ToString());
            var username = json["Username"].ToString();
            var date = DateTime.Parse(json["Date"].ToString()).ToString("yyyy-MM-dd");
            var description = json["Description"].ToString();
            float.TryParse(json["Amount"].ToString(), out var amount);
            if (!String.IsNullOrEmpty(username) && !date.IsNullOrWhiteSpace() &&
                !String.IsNullOrEmpty(description) && amount > -1) {
                if (incomeService.CreateIncome(amount, date, username, description)) {
                    return Content(HttpStatusCode.OK, "Indtægt oprettet");
                }
            }
            return Content(HttpStatusCode.BadRequest, "Forkert input, tjek værdierne og prøv igen.");
        }
    }
}