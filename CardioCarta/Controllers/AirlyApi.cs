using System;
using System.Collections.Generic;
using System.Linq;
using CardioCarta.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Npgsql;

namespace CardioCarta.Controllers
{
    public class AirlyApi
    {
        public static async Task<bool> GetMeasurements()
        {
            HttpClient httpClient = GetAirlyApiClient();
            List<Sensor> sensors = null;
            HttpResponseMessage response = await httpClient.GetAsync("installations/nearest?lat=50.052024&lng=19.992891&maxDistanceKM=20&maxResults=-1");
            if (response.IsSuccessStatusCode)
            {
                sensors = await response.Content.ReadAsAsync<List<Sensor>>();
                var sensorKrakow = sensors.Where(s => s.Address.City == "Kraków");
                //var sensorKrakow = sensors;
                CardioCartaEntities db = new CardioCartaEntities();
                foreach (Sensor sensor in sensorKrakow)
                {
                    if (db.AirlySensor.Find(sensor.Id) == null)
                    {
                        AddSensor(sensor);
                    }
                }
                DateTime lastTimeStamp = db.Airly.OrderByDescending(a => a.TimeStamp).First().TimeStamp;
                if (lastTimeStamp < DateTime.Now.AddHours(-5))
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE \"AirlyForecast\";");
                    db.SaveChanges();
                    int i = 1;
                    foreach (Sensor sensor in sensorKrakow)
                    {
                        if (i % 49 == 0)
                        {
                            Thread.Sleep(60000);
                        }
                        await GetRequest(sensor);
                        i++;
                    }
                }
            }
            return true;
        }

        public static async void GetMeasurements2(Object state)
        {
            HttpClient httpClient = GetAirlyApiClient();
            List<Sensor> sensors = null;
            HttpResponseMessage response = await httpClient.GetAsync("installations/nearest?lat=50.052024&lng=19.992891&maxDistanceKM=20&maxResults=-1");
            if (response.IsSuccessStatusCode)
            {
                sensors = await response.Content.ReadAsAsync<List<Sensor>>();
                var sensorKrakow = sensors.Where(s => s.Address.City == "Kraków");
                //var sensorKrakow = sensors;
                CardioCartaEntities db = new CardioCartaEntities();
                foreach (Sensor sensor in sensorKrakow)
                {
                    if (db.AirlySensor.Find(sensor.Id) == null)
                    {
                        AddSensor(sensor);
                    }
                }
                DateTime lastTimeStamp = db.Airly.OrderByDescending(a => a.TimeStamp).First().TimeStamp;
                if (lastTimeStamp < DateTime.Now.AddHours(-1))
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE \"AirlyForecast\";");
                    db.SaveChanges();
                    int i = 1;
                    foreach (Sensor sensor in sensorKrakow)
                    {
                        if (i % 49 == 0)
                        {
                            Thread.Sleep(60000);
                        }
                        await GetRequest(sensor);
                        i++;
                    }
                }
            }
         }

        public static void AirlyTrigger()
        {
            var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromHours(2);
            var periodTimeSpan = TimeSpan.FromMinutes(5);

            //var timer = new Timer((e) =>
            //{
            //    GetMeasurements2();
            //}, null, startTimeSpan, periodTimeSpan);

            var timer = new Timer(new TimerCallback(GetMeasurements2),
                                   null, 0, 120000);
            GC.KeepAlive(timer);
        }

        private static HttpClient GetAirlyApiClient()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://airapi.airly.eu/v2/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("apikey", "ASvjtvSqUqXBt6cAESJYcdvyN0Ticp5o");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            return httpClient;
        }

        private static async Task<Measurements> GetRequest(Sensor sensor)
        {
            HttpClient httpClient = GetAirlyApiClient();
            Measurements measurements = null;
            string latitude = sensor.Location.Latitude.ToString().Replace(',', '.');
            string longitude = sensor.Location.Longitude.ToString().Replace(',', '.');
            HttpResponseMessage response = await httpClient.GetAsync("measurements/nearest?indexType=AIRLY_CAQI&lat=" + latitude + "&lng=" + longitude + "&maxDistanceKM=50");
            if (response.IsSuccessStatusCode)
            {
                measurements = await response.Content.ReadAsAsync<Measurements>();
                CardioCartaEntities db = new CardioCartaEntities();
                if (measurements.Current.Values.Count > 0 && measurements.Current.Indexes.First(item => item.Name == "AIRLY_CAQI").Value != null)
                {
                    Airly airly = GetCurrent(measurements, sensor.Id);
                    db.Airly.Add(airly);
                    LinkedList<AirlyForecast> airlyForecasts = GetForecast(measurements, sensor.Id);
                    db.AirlyForecast.AddRange(airlyForecasts);
                    db.SaveChanges();
                }
                else
                {
                    Console.Write(sensor.Id + ": " + measurements.Current.Indexes.ElementAt(0).Description);
                }
            }
            else
            {
                Console.Write("kkk");
            }
            return measurements;
        }

        private static LinkedList<AirlyForecast> GetForecast(Measurements measurements, int sensorId)
        {
            LinkedList<AirlyForecast> airlyForecasts = new LinkedList<AirlyForecast>();
            foreach (Forecast forecast in measurements.Forecast)
            {
                if (forecast.Values.Count > 0 && forecast.Indexes.First(item => item.Name == "AIRLY_CAQI").Value != null)
                {
                    AirlyForecast airlyForecast = new AirlyForecast();
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
                    airlyForecast.SensorId = sensorId;
                    airlyForecasts.AddLast(airlyForecast);
                }
            }
            return airlyForecasts;
        }

        private static Airly GetCurrent(Measurements measurements, int sensorId)
        {
            Airly airly = new Airly();
            
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

            airly.TimeStamp = measurements.Current.TillDateTime;
                //.AddMinutes(-measurements.Current.TillDateTime.Minute)
                //.AddSeconds(-measurements.Current.TillDateTime.Second)
                //.AddMilliseconds(-measurements.Current.TillDateTime.Millisecond);
            airly.SensorId = sensorId;
            return airly;
        }

        private static void AddSensor(Sensor sensor)
        {
            AirlySensor airlySensor = new AirlySensor
            {
                Id = sensor.Id
            };
            CardioCartaEntities db = new CardioCartaEntities();
            db.AirlySensor.Add(airlySensor);
            db.SaveChanges();

            NpgsqlConnection connection = new NpgsqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            string latitude = sensor.Location.Latitude.ToString().Replace(',', '.');
            string longitude = sensor.Location.Longitude.ToString().Replace(',', '.');
            NpgsqlCommand command = new NpgsqlCommand(
                "UPDATE \"AirlySensor\" " +
                "SET \"Location\" = ST_PointFromText('POINT(" + longitude + " " + latitude + ")', 4326) " +
                "WHERE \"Id\" = " + sensor.Id + ";", connection);
            Console.WriteLine(command.ToString());
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}