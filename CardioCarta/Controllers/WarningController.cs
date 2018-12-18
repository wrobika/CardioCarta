using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardioCarta.Models;
using Npgsql;

namespace CardioCarta.Controllers
{
    public class WarningController : Controller
    {
        // GET: Warning
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Location")] WarningViewModel warning)
        {
            if (ModelState.IsValid)
            {
                Airly airly = new Airly();
                NpgsqlConnection connection = new NpgsqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                connection.Open();
                connection.TypeMapper.UseNetTopologySuite();
                using (var cmd = new NpgsqlCommand(
                    "SELECT \"Airly_CAQI\", \"Temperature\", \"Pressure\" " +
                    "FROM  \"Airly\" JOIN \"AirlySensor\" " +
                    "ON \"Airly\".\"SensorId\" = \"AirlySensor\".\"Id\" " +
                    "WHERE \"Location\" IS NOT NULL " +
                    "AND \"TimeStamp\" >= now() - interval '3h' " +
                    "AND ST_Distance(\"Location\", ST_GeomFromText('POINT(" + warning.Location + ")', 4326)) < 1000 " +
                    "ORDER BY ST_Distance(\"Location\", ST_GeomFromText('POINT(" + warning.Location + ")', 4326)), "+
                    "\"TimeStamp\" DESC LIMIT 1;",
                    connection))
                using (var reader = cmd.ExecuteReader())
                {
                    try
                    {
                        reader.Read();
                        warning.AirlyCAQI = reader.GetFloat(0);
                        warning.Temperature = reader.GetFloat(1);
                        warning.Pressure = reader.GetFloat(2);
                        warning.Quality = GetQuality(warning.AirlyCAQI);
                        warning.Description = GetDescription(warning);
                        reader.Close();
                    }
                    catch(Exception ex)
                    {
                        warning.Description = "Wystąpiły problemy przy pobieraniu ostrzeżeń dla Twojej lokalizacji";
                        Console.WriteLine(ex);
                    }
                }
                connection.Close();
                return RedirectToAction("Warning", "Warning", warning);
            }
            warning.Description = "Wystąpił błąd i nie udało się pobrać Twojej lokalizacji";
            return RedirectToAction("Warning", "Warning", warning);
        }

        public ActionResult Warning(WarningViewModel warning)
        {
            return View(warning);
        }

        private string GetDescription(WarningViewModel warning)
        {
            //jakie przedziały tu przyjąć?
            string description = "";
            int i = 0;
            if(warning.Temperature > 25)
            {
                description += "jest wysoka temperatura \r\n";
                i++;
            }
            if (warning.Pressure > 1013)
            {
                description += "panuje wysokie ciśnienie atmosferyczne \r\n";
                i++;
            }
            if (warning.Quality != "dobra" || warning.Quality != "bardzo dobra" || warning.Quality != "")
            {
                description += "jakość powietrza jest " + warning.Quality + "\r\n";
                i++;
            }
            //jakie komunikaty
            switch(i)
            {
                case 1: description += "uważaj na siebie"; break;
                case 2: description += "bardzo uważaj na siebie"; break;
                case 3: description += "zachowaj wszystkie środki ostrożności"; break;
                default: break;
            }
            return description;

        }

        private string GetQuality(double? airlyCAQI)
        {
            int range = airlyCAQI == null ? -1 : (int) airlyCAQI / 25;
            switch(range)
            {
                case 0: return "bardzo dobra";
                case 1: return "dobra";
                case 2: return "średnia";
                case 3: return "zła";
                case 4: return "bardzo zła";
                case 5: return "dramatyczna"; //sprawdzić to, jak określają w Airly !!!!!
                case 6: return "gorzej być nie może";
                default: return "";
            }
        }
    }
}