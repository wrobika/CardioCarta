using System;
using System.Collections.Generic;
using System.Linq;
using CardioCarta.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CardioCarta.Controllers
{
    public class AirlyApi
    {

        public static async Task<Measurements> GetRequest(string url, string diaryId)
        {
            HttpClient httpClient = getAirlyApiClient();
            Measurements measurements = null;
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                measurements = await response.Content.ReadAsAsync<Measurements>();

                Airly airly = getCurrent(diaryId, measurements);
                LinkedList<AirlyForecast> airlyForecasts = getForecast(diaryId, measurements);

                CardioCartaEntities db = new CardioCartaEntities();
                db.Airly.Add(airly);
                db.AirlyForecast.AddRange(airlyForecasts);
                db.SaveChanges();
            }
            return measurements;
        }

        private static HttpClient getAirlyApiClient()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://airapi.airly.eu/v2/measurements/point?indexType=AIRLY_CAQI&lat=50.062006&lng=19.940984")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("apikey", "ASvjtvSqUqXBt6cAESJYcdvyN0Ticp5o");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            return httpClient;
        }

        private static LinkedList<AirlyForecast> getForecast(string diaryId, Measurements measurements)
        {
            LinkedList<AirlyForecast> airlyForecasts = new LinkedList<AirlyForecast>();
            foreach (Forecast forecast in measurements.forecast)
            {
                AirlyForecast airlyForecast = new AirlyForecast()
                {
                    Diary_Id = diaryId,
                    TimeStamp = forecast.tillDateTime,
                    Airly_CAQI = (float)forecast.indexes.Single(item => item.name == "AIRLY_CAQI").value,
                    PM10 = (float)forecast.values.Single(item => item.name == "PM10").value,
                    PM25 = (float)forecast.values.Single(item => item.name == "PM25").value,
                };
                airlyForecasts.AddLast(airlyForecast);
            }
            return airlyForecasts;
        }

        private static Airly getCurrent(string diaryId, Measurements measurements)
        {
            return new Airly()
            {
                Diary_Id = diaryId,
                Airly_CAQI = (float)measurements.current.indexes.Single(item => item.name == "AIRLY_CAQI").value,
                PM1 = (float)measurements.current.values.Single(item => item.name == "PM1").value,
                PM10 = (float)measurements.current.values.Single(item => item.name == "PM10").value,
                PM25 = (float)measurements.current.values.Single(item => item.name == "PM25").value,
                Humidity = (float)measurements.current.values.Single(item => item.name == "HUMIDITY").value,
                Pressure = (float)measurements.current.values.Single(item => item.name == "PRESSURE").value,
                Temperature = (float)measurements.current.values.Single(item => item.name == "TEMPERATURE").value,
            };
        }
    }
}