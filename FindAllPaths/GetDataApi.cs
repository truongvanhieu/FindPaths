using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAllPaths
{
    public class GetDataApi
    {
        public string status { get; set; }
        public string id { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string meta { get; set; }
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public string BusStopId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string IsOutWard { get; set; }

        public string RouteId { get; set; }

        public string NextBusStopId { get; set; }
        public string DecodeString { get; set; }
        public double Distance { get; set; }
    }
}
