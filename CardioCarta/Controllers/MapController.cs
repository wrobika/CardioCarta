using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;

namespace CardioCarta.Controllers
{
    public class MapController : Controller
    {
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
    }
}