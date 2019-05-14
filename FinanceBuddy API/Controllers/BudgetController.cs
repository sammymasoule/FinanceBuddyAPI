using System.Net;
using System.Web.Http;
using FinanceBuddy_API.Controllers.Services;
using Newtonsoft.Json.Linq;

namespace FinanceBuddy_API.Controllers {
        public class BudgetController : ApiController {
            private readonly BudgetService budgetService = new BudgetService();

            // GET budget for specific user.
            [Route("api/budget/{username}")]
            public IHttpActionResult Get(string username) {
                var budgetLimits = budgetService.GetBudgetLimits(username);
                if (budgetLimits != null) {
                    return Content(HttpStatusCode.OK, budgetLimits);
                }
                return Content(HttpStatusCode.NotFound, "Denne bruger eksisterer ikke");
            }

            // POST Create a budget for specific user
            [HttpPost]
            [AcceptVerbs("POST")]
            [Route("api/budget/{username}")]
            public IHttpActionResult Post(string username, [FromBody]object value) {
                float loanLimit = -1, groceryLimit = -1, householdLimit = -1, consumptionLimit = -1, transportLimit = -1, savingsLimit = -1;
                var json = JObject.Parse(value.ToString());
                if (float.TryParse(json["LoanLimit"].ToString(), out var tmp)) loanLimit = tmp;
                if (float.TryParse(json["GroceryLimit"].ToString(), out var tmp1)) groceryLimit = tmp1;
                if (float.TryParse(json["HouseholdLimit"].ToString(), out var tmp2)) householdLimit = tmp2;
                if (float.TryParse(json["ConsumptionLimit"].ToString(), out var tmp3)) consumptionLimit = tmp3;
                if (float.TryParse(json["TransportLimit"].ToString(), out var tmp4)) transportLimit = tmp4;
                if (float.TryParse(json["SavingsLimit"].ToString(), out var tmp5)) savingsLimit = tmp5;

                if (username != null && loanLimit != -1 && groceryLimit != 1 && householdLimit != -1 && consumptionLimit != -1 && transportLimit != -1 && savingsLimit != -1) {
                    if (budgetService.CreateBudget(username, loanLimit, householdLimit, groceryLimit, consumptionLimit,
                        transportLimit, savingsLimit)) {
                        return Content(HttpStatusCode.Created, "Budget oprettet");
                    }
                }
                return Content(HttpStatusCode.BadRequest, "Forkert input, tjek værdierne og prøv igen.");
            }

            // PUT Update budget for specific user
            [HttpPut]
            [AcceptVerbs("PUT")]
            [Route("api/budget/{username}")]
            public IHttpActionResult Put(string username, [FromBody]object value) {

                float loanLimit = -1, groceryLimit = -1, householdLimit = -1, consumptionLimit = -1, transportLimit = -1, savingsLimit = -1;
                var json = JObject.Parse(value.ToString());
                if (float.TryParse(json["LoanLimit"].ToString(), out var tmp)) loanLimit = tmp;
                if (float.TryParse(json["GroceryLimit"].ToString(), out var tmp1)) groceryLimit = tmp1;
                if (float.TryParse(json["HouseholdLimit"].ToString(), out var tmp2)) householdLimit = tmp2;
                if (float.TryParse(json["ConsumptionLimit"].ToString(), out var tmp3)) consumptionLimit = tmp3;
                if (float.TryParse(json["TransportLimit"].ToString(), out var tmp4)) transportLimit = tmp4;
                if (float.TryParse(json["SavingsLimit"].ToString(), out var tmp5)) savingsLimit = tmp5;


                if (username != null && loanLimit != -1 && groceryLimit != 1 && householdLimit != -1 && consumptionLimit != -1 && transportLimit != -1 && savingsLimit != -1) {
                    if (budgetService.UpdateBudget(username, loanLimit, groceryLimit, householdLimit, consumptionLimit,
                        transportLimit, savingsLimit)) {
                        return Content(HttpStatusCode.Created, "Budget ændret");
                    }
                }
                return Content(HttpStatusCode.BadRequest, "Forkert input, tjek værdierne og prøv igen.");
            }

            // DELETE budget for specific user
            [Route("api/budget/{username}")]
            public IHttpActionResult Delete(string username) {
                if (budgetService.DeleteBudget(username)) {
                    return Content(HttpStatusCode.OK, "Budget er nu slettet.");
                }

                return Content(HttpStatusCode.NotFound, "Denne bruger eksisterer ikke.");
            }
        }
    }
