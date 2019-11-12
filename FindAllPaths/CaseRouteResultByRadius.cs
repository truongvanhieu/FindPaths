using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAllPaths
{
    public class CaseRouteResultByRadius
    {
        public int Radius { get; set; }
        public List<string> BusStopIdOrigin { get; set; }
        public List<string> BusStopDestination { get; set; }
        public List<List<BusStopModel>> ListResult { get; set; }
    }
}
