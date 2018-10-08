using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public List<string> GetSpatialData()
        {
            List<string> spatialDataList = new List<string>();
            //dbConnection.Open();
            //var command = new SqlCommand("select [GEOMETRY].STAsText() from TEST_SPATIAL where GEOMETRY IS NOT NULL", dbConnection);
            //var rdr = command.ExecuteReader();
            //while (rdr.Read())
            //{
            //    spatialDataList.Add(Convert.ToString(rdr[0]));
            //}
            //dbConnection.Close();

            //spatialDataList.Add("POINT(50.062006 19.940984)");
            spatialDataList.Add("LINESTRING(1 2 3, 3 4 5)");
            return spatialDataList;
        }
    }
}