using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using jonasalden_travelplanner.ViewModels;
using Newtonsoft.Json.Linq;

namespace jonasalden_travelplanner.Controllers
{
    public class TravelsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TicketsViewModel();
            model.DepName = "Örebro Centralstation";
            return View(model);
        }
        //GET: Travel

        [HttpPost]
        public async Task<ActionResult> Index(TicketsViewModel newTicket)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:51901/api/travels/{destName}" + newTicket.DestName);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var json = JObject.Parse(content);
                        var ticket = new TicketsViewModel()
                        {
                            DepName = json["DepName"].Value<string>(),
                            DepTime = (DateTime)json["DepTime"],
                            DestName = json["DestName"].Value<string>(),
                            DestTime = (DateTime)json["DestTime"],
                            DestLon = json["DestLon"].Value<string>(),
                            DestLat = json["DestLat"].Value<string>(),
                            Celsius = json["Celsius"].Value<decimal>(),
                            ForecastName = json["ForecastName"].Value<string>()

                        };
                        return View(ticket);
                    }
                }
            }
            else
            {
                TempData["Message"] = "Destination can not be empty.";
                TempData["Type"] = "alert-danger";
                return RedirectToAction("Index");
            }
               return HttpNotFound();        
        }
    }
}