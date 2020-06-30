using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WarehouseTransport.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RouterAPIController : ControllerBase
    {
        [HttpGet("directions/{driver}")]
        public IEnumerable<Directions.Place> directions(string driver)
        {
            int driverNo = 1;
            int.TryParse(driver,out driverNo);
            return Directions.getPlaces(driverNo);
        }
    }

    public static class Directions
    {
        private static double[] city = new double[] { -37.8, 144.96 };

        private static double[] whWest = new double[] { -37.788706, 144.823430 };
        private static double[] whEast= new double[] { -37.850792, 145.180893 };

        private static double[,] places = new double[,] {
            { -37.762553,144.954236 },{ -37.814355,144.986959},{ -37.822579,145.006292},{ -37.828511,145.038285},
            { -37.828613,145.039208},{ -37.828613,145.042834},{ -37.860381,145.018458},{ -37.870172,145.041418},
            { -37.870172,145.041418},{ -37.829443,145.077081},{ -37.781902,144.856238},{ -37.769284,144.857354},
            { -37.748893,144.905334},{ -37.719806,144.929452},{ -37.707109,144.969234},{ -37.707109,144.969234},
            { -37.707109,144.969234},{ -37.746585,144.994211},{ -37.778276,145.003996},{ -37.778276,145.003996},
            { -38.185706,145.079098},{ -37.894563,144.660587},{ -37.875064,144.981079},{ -37.884353,144.982474},
            { -37.893883,144.993868},{ -37.890801,145.017042},{ -37.920897,145.013523},{ -37.930683,145.000209},
            { -37.949728,145.017965},{ -37.887586,145.169842}
        };
        
        public class Place
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public Place(double y,double x)
            {
                latitude = y;
                longitude = x;
            }
        }
        /*
         * For assign west of city points to western warehouse,assign east of city points to eastern warehouse
         */

        private static List<Place> assignWH(double[] wh, List<Place> listCleaned)
        {
            List<Place> result = new List<Place>();
            bool lWest = false;
            if (wh[1] < city[1])
            {
                lWest = true;
            }
            result.Add(new Place(wh[0], wh[1]));
            for (int i = 0; i < listCleaned.Count; i++)
            {
                bool ptWest = (listCleaned[i].longitude <= city[1]) || (listCleaned[i].latitude >= city[0]);
                if ((ptWest && lWest) || (!ptWest && !lWest))
                {
                    result.Add(listCleaned[i]);
                    continue;
                }
            }
            return result;

        }
        private static List<Place> eastOrWest(int driver, List<Place> listCleaned)
        {
            bool lWest = driver < 3;
            List<Place> result;
            if (lWest)
            {
                result = assignWH(whWest, listCleaned);
            }
            else
            {
                result = assignWH(whEast, listCleaned);
            }
            return result;
        }

        public static List<Place> getPlaces(int driver)
        {
            List<Place> listCleaned = clearDuplicates();
            List<Place> ordered = OrderByDistance(listCleaned);
            List<Place> wharehouse = eastOrWest(driver, ordered);
            List<Place> driverList = getDriverList(driver, wharehouse);
            return driverList;

        }

        private static List<Place> clearDuplicates()
        {
            List<Place> result = new List<Place>();
            for (int i = 0; i < places.GetLength(0); i++)
            {
                if (result.Find(aa => aa.latitude == places[i, 0] && aa.longitude == places[i, 1]) == null)
                {
                    result.Add(new Place(places[i, 0], places[i, 1]));
                }
            }
            return result;
        }

        private static List<Place> getDriverList(int driver, List<Place> ordered)
        {
            int driverIndex = driver;
            if (driverIndex < 1 || driverIndex > 5)
            {
                driverIndex = 1;
            }
            int driverSplit = 2;
            if (driverIndex > 2)
            {
                driverIndex -= 2;
                driverSplit = 3;
            }
            int nSize = ordered.Count();
            int nDivision = nSize / driverSplit;
            List<List<Place>> splitList = ordered.SplitList(nDivision);
            List<Place> result = splitList[driverIndex - 1];
            if (driverIndex > 1)
            {
                result.Insert(0, ordered[0]);
            }
            return result;
        }
        public static List<List<T>> SplitList<T>(this List<T> me, int size)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < me.Count; i += size)
                list.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
            return list;
        }

        private static double DistanceQuick(Place p1, Place p2)
        {
            double deltaX = Math.Abs(p2.longitude - p1.longitude);
            double deltaY = Math.Abs(p2.latitude - p1.latitude);
            return deltaX > deltaY ? deltaX : deltaY;
        }

        private static double Distance(Place p1, Place p2)
        {
            return Math.Sqrt(Math.Pow(p2.longitude - p1.longitude, 2) + Math.Pow(p2.latitude - p1.latitude, 2));
        }

        private static List<Place> OrderByDistance(List<Place> pointList)
        {

            var orderedList = new List<Place>();
            var currentPoint = pointList[0];
            while (pointList.Count > 1)
            {
                orderedList.Add(currentPoint);
                pointList.RemoveAt(pointList.IndexOf(currentPoint));
                var closestPointIndex = 0;
                var closestDistance = double.MaxValue;
                for (var i = 0; i < pointList.Count; i++)
                {
                    var distanceQuick = DistanceQuick(currentPoint, pointList[i]);
                    if (distanceQuick > closestDistance)
                        continue;
                    var distance = Distance(currentPoint, pointList[i]);
                    if (distance < closestDistance)
                    {
                        closestPointIndex = i;
                        closestDistance = distance;
                    }
                }
                currentPoint = pointList[closestPointIndex];
            }
            orderedList.Add(currentPoint);
            return orderedList;
        }      
    }
}
