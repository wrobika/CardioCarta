﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using CardioCarta.Models;
using System.Threading.Tasks;
using System.Threading;

namespace CardioCarta.Controllers
{
    public class MapController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        //GET: Map
        //public async Task<ActionResult> Index()
        //{
        //    //await AirlyApi.GetMeasurements();
        //    return View();
        //}
        public ActionResult Index()
        {
            //Thread downloadAirly = new Thread(new ThreadStart(AirlyApi.GetMeasurements2))
            //{
            //    IsBackground = false
            //};
            //downloadAirly.Start();
            return View();
        }

        public Dictionary<string, double> GetAirly()
        {
            Dictionary<string, double> pointWithValues = new Dictionary<string, double>();
            DateTime lastTimeStamp = db.Airly.OrderByDescending(a => a.TimeStamp).First().TimeStamp;
            if (lastTimeStamp < DateTime.Now.AddHours(-6))
            {
                return pointWithValues;
            }
            NpgsqlConnection connection = new NpgsqlConnection(
            System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            connection.TypeMapper.UseNetTopologySuite();
            //4h = wpisy co 2h + przesuniecie api 1h + bufor 1h
            using (var cmd = new NpgsqlCommand(
                "SELECT \"Location\", \"Airly_CAQI\" " +
                "FROM  \"Airly\" JOIN \"AirlySensor\" " +
                "ON \"Airly\".\"SensorId\" = \"AirlySensor\".\"Id\" " +
                "WHERE \"Location\" IS NOT NULL " +
                "AND \"TimeStamp\" >= now() - interval '4h';",
                connection))
            //using (var cmd = new NpgsqlCommand(
            //    "SELECT \"Location\", \"Airly_CAQI\" " +
            //    "FROM  \"Airly\" JOIN \"AirlySensor\" " +
            //    "ON \"Airly\".\"SensorId\" = \"AirlySensor\".\"Id\" " +
            //    "WHERE \"Location\" IS NOT NULL;",
            //    connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pointWKT = reader.GetValue(0).ToString();
                    try
                    {
                        pointWithValues[pointWKT] = reader.GetDouble(1);
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

        public Dictionary<string, string> GetDiaries()
        {
            Dictionary<string, string> pointWithValues = new Dictionary<string, string>();
            NpgsqlConnection connection = new NpgsqlConnection(
            System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            connection.TypeMapper.UseNetTopologySuite();
            //using (var cmd = new NpgsqlCommand(
            //    "SELECT \"Location\", \"Id\" " +
            //    "FROM  \"Diary\" " +
            //    "WHERE \"Location\" IS NOT NULL " +
            //    "AND \"TimeStamp\" >= now()::date;",
            //    connection))
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
                        pointWithValues[pointWKT] = GetHealth(diary);
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
            int value = (10-diary.Mood) / 2
                + (diary.RespirationProblem ? 2 : 0)
                + (diary.Haemorrhage ? 2 : 0)
                + (diary.Dizziness ? 2 : 0)
                + (diary.ChestPain ? 2 : 0)
                + (diary.SternumPain ? 2 : 0)
                + (diary.HeartPain ? 2 : 0)
                + (diary.Other != null ? 2 : 0)
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
    }
}