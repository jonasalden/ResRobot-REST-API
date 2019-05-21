using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI;
using jonasalden_travelplanner.Models;
using jonasalden_travelplanner.ViewModels;
using Newtonsoft.Json.Linq;

namespace jonasalden_tpapi.Controllers.API
{
    public class TravelsApiController : ApiController
    {
        [HttpGet]
        [Route("api/travels/{destName}")]
        public async Task<IHttpActionResult> Get(string destName)
        {
            var model = new Trip();
            model.DepId = "740000133";

            using (var client = new HttpClient())
            {
                var locationResponse =
                    await client.GetAsync(
                        "https://api.resrobot.se/v2/location.name?key=2421cf20-34c0-4e25-be59-67525ad2eb0e&format=json&input=" +
                        destName);

                if (locationResponse.IsSuccessStatusCode)
                {
                    var content = await locationResponse.Content.ReadAsStringAsync();
                    var json = JObject.Parse(content);
                    var id = json["StopLocation"].First()["id"].Value<string>();
                    var travelResponse = await client.GetAsync(
                        "https://api.resrobot.se/v2/trip?key=<API KEY>&originId=" +
                        model.DepId + "&destId=" + id + "&format=json");

                    if (travelResponse.IsSuccessStatusCode)
                    {
                        var travelContent = await travelResponse.Content.ReadAsStringAsync();
                        var travelJson = JObject.Parse(travelContent);

                        model.DepName = travelJson["Trip"][0]["LegList"]["Leg"].First()["Origin"]["name"]
                            .Value<string>();
                        model.DepTime = travelJson["Trip"][0]["LegList"]["Leg"].First()["Origin"]["time"]
                            .Value<DateTime>();
                        model.DestId = travelJson["Trip"][0]["LegList"]["Leg"].Last()["Destination"]["id"]
                            .Value<string>();
                        model.DestName = travelJson["Trip"][0]["LegList"]["Leg"].Last()["Destination"]["name"]
                            .Value<string>();
                        model.DestTime = travelJson["Trip"][0]["LegList"]["Leg"].Last()["Destination"]["time"]
                            .Value<DateTime>();
                        model.DestLon = travelJson["Trip"][0]["LegList"]["Leg"].Last()["Destination"]["lon"]
                            .Value<string>();
                        model.DestLat = travelJson["Trip"][0]["LegList"]["Leg"].Last()["Destination"]["lat"]
                            .Value<string>();

                        var weatherResponse = await client.GetAsync
                        ("https://opendata-download-metfcst.smhi.se/api/category/pmp2g/version/2/geotype/point/lon/" +
                         model.DestLon + "/lat/" + model.DestLat + "/data.json");

                        if (weatherResponse.IsSuccessStatusCode)
                        {
                            var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
                            var weatherJson = JObject.Parse(weatherContent);
                            model.DestTime = new DateTime(model.DestTime.Year, model.DestTime.Month, model.DestTime.Day,
                                model.DestTime.Hour, 0, 0, model.DestTime.Kind);

                            var parameters = weatherJson["timeSeries"].Children()
                                .Where(x => x["validTime"].Value<DateTime>() == model.DestTime)
                                .Select(x => x["parameters"]).ToList();

                            model.Celsius =
                                (decimal)parameters.Children().Single(t => t["name"].Value<string>() == "t")["values"].Single();

                            model.Forecast = (int)parameters.Children().Single(t => t["name"].Value<string>() == "Wsymb")["values"].Single();

                            int value = model.Forecast;
                            switch (value)
                            {
                                case 1:
                                    model.ForecastName = "clear sky";
                                    break;
                                case 2:
                                    model.ForecastName = "Nearly clear sky";
                                    break;
                                case 3:
                                    model.ForecastName = "Variable cloudiness";
                                    break;
                                case 4:
                                    model.ForecastName = "Halfclear sky";
                                    break;
                                case 5:
                                    model.ForecastName = "Cloudy sky";
                                    break;
                                case 6:
                                    model.ForecastName = "Overcast";
                                    break;
                                case 7:
                                    model.ForecastName = "Fog";
                                    break;
                                case 8:
                                    model.ForecastName = "Light rain showers";
                                    break;
                                case 9:
                                    model.ForecastName = "Moderate rain showers";
                                    break;
                                case 10:
                                    model.ForecastName = "Heavy rain showers";
                                    break;
                                case 11:
                                    model.ForecastName = "Thunderstorm";
                                    break;
                                case 12:
                                    model.ForecastName = "Light sleet showers";
                                    break;
                                case 13:
                                    model.ForecastName = "Moderate sleet showers";
                                    break;
                                case 14:
                                    model.ForecastName = "Heavy sleet showers";
                                    break;
                                case 15:
                                    model.ForecastName = "Light snow showers";
                                    break;
                                case 16:
                                    model.ForecastName = "Moderate snow showers";
                                    break;
                                case 17:
                                    model.ForecastName = "Heavy snow showers";
                                    break;
                                case 18:
                                    model.ForecastName = "Light rain";
                                    break;
                                case 19:
                                    model.ForecastName = "Moderate rain";
                                    break;
                                case 20:
                                    model.ForecastName = "Heavy rain";
                                    break;
                                case 21:
                                    model.ForecastName = "Thunder";
                                    break;
                                case 22:
                                    model.ForecastName = "Light sleet";
                                    break;
                                case 23:
                                    model.ForecastName = "Moderate sleet";
                                    break;
                                case 24:
                                    model.ForecastName = "Heavy sleet";
                                    break;
                                case 25:
                                    model.ForecastName = "Light snowfall";
                                    break;
                                case 26:
                                    model.ForecastName = "Moderate snowfall";
                                    break;
                                case 27:
                                    model.ForecastName = "Heavy snowfall";
                                    break;
                            }
                            return Ok(model);
                        }
                        else
                        {
                            return InternalServerError();
                        }
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
                else
                {
                    return InternalServerError();
                }

            }
        }
    }
}
