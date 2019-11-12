using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAllPaths
{
    public class BusStopModel
    {
        public string BusStopId { get; set; }
        public string NextBusStopId { get; set; }
        public bool IsOutWard { get; set; }
        public string RouteId { get; set; }
        public string DecodeString { get; set; }
        public double Distance { get; set; }
    }

    public class SaveRouteData
    {
        public BusStopModel BusStop { get; set; }
        public List<string> ListRouteId { get; set; }
        public double SumDistance { get; set; }
    }
}
