using System;
using System.Collections.Generic;
using System.Linq;
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
        private static List<Place> assignWH(double[] wh)
        {
            List<Place> result = new List<Place>();
            bool lWest = false;
            if (wh[1] < city[1])
            {
                lWest = true;
            }

            for (int i = 0; i < places.GetLength(0); i++)
            {
                bool ptWest = (places[i, 1] <= city[1]) || (places[i, 0] >= city[0]);
                if ((ptWest && lWest) || (!ptWest && !lWest))
                {
                    result.Add(new Place(places[i, 0], places[i, 1]));
                    continue;
                }
            }
            return result;

        }

        private static List<Place> eastOrWest(int driver)
        {
            bool lWest = driver < 3;
            List<Place> result;
            if (lWest)
            {
                result = assignWH(whWest);
            }
            else
            {
                result = assignWH(whEast);
            }
            return result;
        }

        public static List<Place> getPlaces(int driver)
        {
            List<Place> result = new List<Place>();


            List<Place> test = eastOrWest(driver);

            double[] whC;

            //Place wh; 
            if (driver == 1)
            {
                whC = whWest;
            }
            else
            {
                whC = whEast;
            }
            result.Add(new Place(whC[0], whC[1]));
            for (int i=0;i< 5; i++)
            {
                result.Add(new Place(places[i, 0], places[i, 1]));
            }
            return result;
        }

        /*private static void TwoOpt()
        {
            // Get tour size
            var size = _tour.TourSize();

            //CHECK THIS!!     
            for (var i = 0; i < size; i++)
            {
                _newTour.SetCity(i, _tour.GetCity(i));
            }

            // repeat until no improvement is made 
            var improve = 0;
            var iteration = 0;

            while (improve < 500)
            {
                var bestDistance = _tour.TourDistance();

                for (var i = 1; i < size - 1; i++)
                {
                    for (var k = i + 1; k < size; k++)
                    {
                        TwoOptSwap(i, k);
                        iteration++;

                        var newDistance = _newTour.TourDistance();

                        if (!(newDistance < bestDistance)) continue;

                        // Improvement found so reset
                        improve = 0;

                        for (var j = 0; j < size; j++)
                        {
                            _tour.SetCity(j, _newTour.GetCity(j));
                        }

                        bestDistance = newDistance;

                        // Update the display
                        SetTourCoords();

                        TourUpdated.Raise(this, new Tuple<double, int>(bestDistance, iteration));
                    }
                }

                improve++;
            }
        }

        private static void TwoOptSwap(int i, int k)
        {
            var size = _tour.TourSize();

            // 1. take route[0] to route[i-1] and add them in order to new_route
            for (var c = 0; c <= i - 1; ++c)
            {
                _newTour.SetCity(c, _tour.GetCity(c));
            }

            // 2. take route[i] to route[k] and add them in reverse order to new_route
            var dec = 0;
            for (var c = i; c <= k; ++c)
            {
                _newTour.SetCity(c, _tour.GetCity(k - dec));
                dec++;
            }

            // 3. take route[k+1] to end and add them in order to new_route
            for (var c = k + 1; c < size; ++c)
            {
                _newTour.SetCity(c, _tour.GetCity(c));
            }
        }*/
    }
}
