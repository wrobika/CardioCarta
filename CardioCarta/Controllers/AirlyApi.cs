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

        public static async Task<Measurements> GetRequest(string coord, string diaryId)
        {
            string latitude = coord.Split(' ')[0];
            string longitude = coord.Split(' ')[1]; 
            HttpClient httpClient = getAirlyApiClient();
            Measurements measurements = null;
            HttpResponseMessage response = await httpClient.GetAsync("nearest?indexType=AIRLY_CAQI&lat=" + latitude + "&lng=" + longitude + "&maxDistanceKM=50");
            if (response.IsSuccessStatusCode)
            {
                measurements = await response.Content.ReadAsAsync<Measurements>();
                CardioCartaEntities db = new CardioCartaEntities();
                if (measurements.Current.Values.Count > 0 && measurements.Current.Indexes.First(item => item.Name == "AIRLY_CAQI").Value != null)
                {
                    Airly airly = GetCurrent(diaryId, measurements);
                    db.Airly.Add(airly);
                    LinkedList<AirlyForecast> airlyForecasts = GetForecast(diaryId, measurements);
                    db.AirlyForecast.AddRange(airlyForecasts);
                    db.SaveChanges();
                }
            }
            return measurements;
        }

        private static HttpClient getAirlyApiClient()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://airapi.airly.eu/v2/measurements/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("apikey", "ASvjtvSqUqXBt6cAESJYcdvyN0Ticp5o");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            return httpClient;
        }

        private static LinkedList<AirlyForecast> GetForecast(string diaryId, Measurements measurements)
        {
            LinkedList<AirlyForecast> airlyForecasts = new LinkedList<AirlyForecast>();
            foreach (Forecast forecast in measurements.Forecast)
            {
                if (forecast.Values.Count > 0 && forecast.Indexes.First(item => item.Name == "AIRLY_CAQI").Value != null)
                {
                    AirlyForecast airlyForecast = new AirlyForecast();
                    airlyForecast.Diary_Id = diaryId;
                    airlyForecast.TimeStamp = forecast.TillDateTime;
                    try
                    {
                        airlyForecast.Airly_CAQI = (float)(forecast.Indexes.First(item => item.Name == "AIRLY_CAQI").Value);
                    }
                    catch (InvalidOperationException)
                    {
                        airlyForecast.Airly_CAQI = null;
                    }
                    try
                    {
                        airlyForecast.PM10 = (float)(forecast.Values.First(item => item.Name == "PM10").Value);
                    }
                    catch (InvalidOperationException)
                    {
                        airlyForecast.PM10 = null;
                    }
                    try
                    {
                        airlyForecast.PM25 = (float)(forecast.Values.First(item => item.Name == "PM25").Value);
                    }
                    catch (InvalidOperationException)
                    {
                        airlyForecast.PM25 = null;
                    }
                    airlyForecasts.AddLast(airlyForecast);
                }
            }
            return airlyForecasts;
        }

        private static Airly GetCurrent(string diaryId, Measurements measurements)
        {
            Airly airly = new Airly();
            airly.Diary_Id = diaryId;
            try
            {
                airly.Airly_CAQI = (float)(measurements.Current.Indexes.First(item => item.Name == "AIRLY_CAQI").Value);
            }
            catch (InvalidOperationException)
            {
                airly.Airly_CAQI = null;
            }
            try
            {
                airly.PM1 = (float)(measurements.Current.Values.First(item => item.Name == "PM1").Value);
            }
            catch (InvalidOperationException)
            {
                airly.PM1 = null;
            }
            try
            {
                airly.PM10 = (float)(measurements.Current.Values.First(item => item.Name == "PM10").Value);
            }
            catch (InvalidOperationException)
            {
                airly.PM10 = null;
            }
            try
            {
                airly.PM25 = (float)(measurements.Current.Values.First(item => item.Name == "PM25").Value);
            }
            catch (InvalidOperationException)
            {
                airly.PM25 = null;
            }

            try
            {
                airly.Humidity = (float)(measurements.Current.Values.First(item => item.Name == "HUMIDITY").Value);
            }
            catch (InvalidOperationException)
            {
                airly.Humidity = null;
            }

            try
            {
                airly.Pressure = (float)(measurements.Current.Values.First(item => item.Name == "PRESSURE").Value);
            }
            catch (InvalidOperationException)
            {
                airly.Pressure = null;
            }

            try
            {
                airly.Temperature = (float)(measurements.Current.Values.First(item => item.Name == "TEMPERATURE").Value);
            }
            catch(InvalidOperationException)
            {
                airly.Temperature = null;
            }
            return airly;
        }
    }
}