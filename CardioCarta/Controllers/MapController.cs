using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using CardioCarta.Models;

namespace CardioCarta.Controllers
{
    public class MapController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        //GET: Data
        public List<string> GetPointsWKT()
        {
            List<string> pointsWKT = new List<string>();
            NpgsqlConnection connection = new NpgsqlConnection(
            System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            connection.TypeMapper.UseNetTopologySuite();
            using (var cmd = new NpgsqlCommand(
                "SELECT \"Location\" " +
                "FROM  \"Diary\" " +
                "WHERE \"Location\" IS NOT NULL;", 
                connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    pointsWKT.Add(reader.GetValue(0).ToString());
                }
                reader.Close();
            }
            connection.Close();
            return pointsWKT;
        }

        public Dictionary<string, string> GetLocationWithDiaryId()
        {
            Dictionary<string, string> pointWithValues = new Dictionary<string, string>();
            NpgsqlConnection connection = new NpgsqlConnection(
            System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            connection.TypeMapper.UseNetTopologySuite();
            using (var cmd = new NpgsqlCommand(
                "SELECT \"Location\", \"Id\" " +
                "FROM  \"Diary\" " +
                "WHERE \"Location\" IS NOT NULL;",
                connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pointWKT = reader.GetValue(0).ToString();
                    try
                    {
                        string diaryId = reader.GetString(1);
                        Diary diary = db.Diary.Single(d => d.Id == diaryId);
                        var imageName = GetHealth(diary)
                        + GetAirly(diary)
                        + GetBiometeo(diary);
                        if (imageName.Length == 3) pointWithValues[pointWKT] = imageName;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                reader.Close();
            }
            connection.Close();
            return pointWithValues;
        }

        private string GetHealth(Diary diary)
        {
            int value = diary.Mood / 2
                + (diary.RespirationProblem ? 1 : 0)
                + (diary.Haemorrhage ? 1 : 0)
                + (diary.Dizziness ? 1 : 0)
                + (diary.ChestPain ? 1 : 0)
                + (diary.SternumPain ? 1 : 0)
                + (diary.HeartPain ? 1 : 0)
                + (diary.Other != null ? 1 : 0)
                + Pressure(diary);
            return value.ToString();
                //czy uwzlędniac
                //jego choroby
                //że jest uzalezniony od tytoniu
                //ze nie wziął leków
                //kawa, alkohol
        }

        private int Pressure(Diary diary)
        {
            if (diary.SystolicPressure > 180 && diary.DiastolicPressure > 110)
                return 3;
            if (diary.SystolicPressure > 160 && diary.DiastolicPressure > 100)
                return 2;
            if (diary.SystolicPressure > 140 && diary.DiastolicPressure > 90)
                return 1;
            return 0;
        }


        private string GetBiometeo(Diary diary)
        {
            return "3";
        }

        private string GetAirly(Diary diary)
        {
            return "3";
        }
    }
}