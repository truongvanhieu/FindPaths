using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
// C# program to print all  
// paths from a source to  
// destination.  
// A directed graph using  
// adjacency list representation  

namespace FindAllPaths
{
    public class GraphBusStop
    {
        public static int NUMBER_ROUTE = 2;//Số lần chuyển tuyến
        public static List<List<BusStopModel>> lstcasefindresult = new List<List<BusStopModel>>();

        List<string> listVertices = new List<string>();//Lưu 751 trạm dừng-thông tin đỉnh trạm
        List<Data> lstfindata = new List<Data>();
        // adjacency list - lưu tất cả danh sách thông tin các cạnh
        private Dictionary<string, List<BusStopModel>> adjacencyList = new Dictionary<string, List<BusStopModel>>();//Lưu 1187 cạnh-Thông tin cạnh

        private void initListVertices()
        {
            var lstbusstop = lstfindata.Select(x => x.BusStopId).Distinct();
            var dem = lstbusstop.Count();
            foreach (var item in lstbusstop)
            {
                this.listVertices.Add(item);//change i thanh busstop id
            }
        }
        // Constructor  
        public GraphBusStop()
        {
        }

        public async void Innit()
        {
            await GetListFindData();
            initListVertices();
            // initialise adjacency list  
            initAdjacencyList();

            List<CaseRouteResultByRadius> lstcaseradius = new List<CaseRouteResultByRadius>();
            List<string> lstbusstopremove = new List<string>();

            double LatOrigin = 15.886176;
            double LngOrigin = 108.321097;

            double LatDestination = 16.059432;
            double LngDestination = 108.174953;

            //int[] lstRadius = { 500, 1000, 2000 };
            List<string> lstbusstoprogin = new List<string>();
            List<string> lstbusstopdestination = new List<string>();
            var r = 100;
            var r2 = 200;
            foreach (var item in listVertices)
            {
                var lstbusstopnext = lstfindata.FindAll(x => x.BusStopId == item);

                foreach (var busstopnext in lstbusstopnext)
                {
                    BusStopModel bstopnextdata = new BusStopModel()
                    {
                        BusStopId = busstopnext.BusStopId,
                        NextBusStopId = busstopnext.NextBusStopId,
                        IsOutWard = Convert.ToBoolean(busstopnext.IsOutWard.ToString()),
                        RouteId = busstopnext.RouteId,
                        DecodeString = busstopnext.DecodeString,
                        Distance = busstopnext.Distance
                    };

                    addEdge(item, bstopnextdata);
                }

                var distanceorgin = Distance(LatOrigin, LngOrigin, lstbusstopnext.ElementAt(0).Lat, lstbusstopnext.ElementAt(0).Lng);
                if (distanceorgin < r)
                {
                    lstbusstoprogin.Add(lstbusstopnext.ElementAt(0).BusStopId);
                }

                var distancedestination = Distance(LatDestination, LngDestination, lstbusstopnext.ElementAt(0).Lat, lstbusstopnext.ElementAt(0).Lng);
                if (distancedestination < r2)
                {
                    lstbusstopdestination.Add(lstbusstopnext.ElementAt(0).BusStopId);
                }
            }


            ////------------------------------------------------------

            DateTime t = DateTime.Now;

            foreach (var origin in lstbusstoprogin)
            {
                foreach (var destination in lstbusstopdestination)
                {
                    var busstoporigin = lstfindata.FindAll(x => x.BusStopId == origin).FirstOrDefault();
                    var busstopdestination = lstfindata.FindAll(x => x.BusStopId == destination).FirstOrDefault();
                    var ditance = Distance(busstoporigin.Lat, busstoporigin.Lng, busstopdestination.Lat, busstopdestination.Lng);
                    printAllPaths(origin, destination, ditance);
                }
            }

            var now = (DateTime.Now - t).TotalMilliseconds;
        }
        // utility method to initialise adjacency list  
        private void initAdjacencyList()
        {
            foreach (string vertices in this.listVertices)
            {
                List<BusStopModel> listvaule = new List<BusStopModel>();//de luu danh sach cac dinh ke

                adjacencyList.Add(vertices, listvaule);
            }
        }

        // add edge from u to v  --> u: busstop, v:bustop next
        public void addEdge(string u, BusStopModel v)
        {
            // Add v to u's list.  
            adjacencyList[u].Add(v);
        }

        // Prints all paths from  po tìm đường đi từ 's' --> to 'd'  
        public void printAllPaths(string s, string d, double ditance)
        {
            Dictionary<string, bool> isVisited = new Dictionary<string, bool>();
            foreach (string vertices in this.listVertices)
            {
                isVisited.Add(vertices, false);
            }

            List<string> pathList = new List<string>();
            List<string> pathListRouteID = new List<string>();

            List<BusStopModel> pathList_Ok = new List<BusStopModel>();
            // add source to path[]  
            pathList.Add(s);//Lưu đỉnh xuất phát s vào mảng đường đi qua các đỉnh

            // Call recursive utility  
            printAllPathsUtil(s, d, isVisited, pathList_Ok, ditance);
        }

        // A recursive function to print  
        // all paths from 'u' to 'd'.  
        // isVisited[] keeps track of  
        // vertices in current path.  
        // localPathList<> stores actual  
        // vertices in the current path  
        private void printAllPathsUtil(string u, string d, Dictionary<string, bool> isVisited, List<BusStopModel> localpathList, double ditance)
        {
            //localpathList_Ok Luu danh sach cac canh da di qua, chua co Đỉnh cuối cùng.
            var listRouteDId = localpathList.Where(x => !string.IsNullOrEmpty(x.RouteId)).Select(x => x.RouteId).Distinct();
            if (listRouteDId.Count() <= GraphBusStop.NUMBER_ROUTE) // Điều kiện 1 (số lần chuyển tuyến <= 3 lần) số lần chuyển tuyến chỉ nhỏ hơn hoặc bằng 2
            {
                // Mark the current node  
                isVisited[u] = true;

                if (u.Equals(d))//Đã tới đích
                {
                    var busstoplast = localpathList.ElementAt(localpathList.Count() - 1);

                    BusStopModel bstopstopdination = new BusStopModel()
                    {
                        BusStopId = busstoplast.NextBusStopId,
                        SumDistance = busstoplast.SumDistance
                    };

                    localpathList.Add(bstopstopdination);//add đỉnh đích vào list đường đi

                    var lstreuslt = new List<BusStopModel>(localpathList);//lưu trữ cách đi
                    lstcasefindresult.Add(lstreuslt);

                    // if match found then no need to traverse more till depth  
                    isVisited[u] = false;
                    return;
                }

                // Recur for all the vertices adjacent to current vertex  
                foreach (BusStopModel i in adjacencyList[u])//danh sách busstop next (các đỉnh kề)
                {
                    if (!string.IsNullOrEmpty(i.NextBusStopId))//không có trạm kề
                    {
                        if (!isVisited[i.NextBusStopId])//điều kiện đi tiếp
                        {
                            // store current node  
                            // in path[]  
                            //kiểm tra điều kiện đưa điểm kề vào (busstop next vào danh sách đường đi)- cạnh u [i]
                            var routIdNext = i.RouteId;

                            //check thỏa 3 điều kiện lưu vào:
                            if (localpathList.Count() == 0)
                            {
                                // add đỉnh đi qua
                                BusStopModel bstopstopNext = new BusStopModel()
                                {
                                    BusStopId = i.BusStopId,
                                    NextBusStopId = i.NextBusStopId,
                                    IsOutWard = i.IsOutWard,
                                    RouteId = i.RouteId,
                                    DecodeString = i.DecodeString,
                                    Distance = i.Distance,
                                    SumDistance = i.Distance
                                };

                                if (bstopstopNext.SumDistance < (ditance * 2)) //kiểm tra khoảng cách đường bus đã đi < khoảng cách chim bay*2
                                {
                                    localpathList.Add(bstopstopNext);// KẾT NỘP đỉnh(cạnh) mới vào đường đi

                                    printAllPathsUtil(i.NextBusStopId, d, isVisited, localpathList, ditance);

                                    // remove current node  
                                    localpathList.RemoveAt(localpathList.Count() - 1);
                                }
                            }
                            else
                            {
                                if (i.RouteId != localpathList.ElementAt(localpathList.Count() - 1).RouteId) // Điều kiện 3: Tuyến mới sẽ đi chưa có trong danh sách tuyến hiện tại - tuyến mới khác # với tuyến hiện tại
                                {
                                    if (!listRouteDId.Contains(i.RouteId))  // tuyến mới không có trong danh sách tuyến đã đi
                                    {
                                        // add đỉnh đi qua
                                        BusStopModel bstopstopNext = new BusStopModel()
                                        {
                                            BusStopId = i.BusStopId,
                                            NextBusStopId = i.NextBusStopId,
                                            IsOutWard = i.IsOutWard,
                                            RouteId = i.RouteId,
                                            DecodeString = i.DecodeString,
                                            Distance = i.Distance,
                                            SumDistance = localpathList.ElementAt(localpathList.Count() - 1).SumDistance + i.Distance
                                        };

                                        if (bstopstopNext.SumDistance < (ditance * 2)) //kiểm tra khoảng cách đường bus đã đi < khoảng cách chim bay*2
                                        {
                                            localpathList.Add(bstopstopNext);// KẾT NỘP đỉnh(cạnh) mới vào đường đi

                                            printAllPathsUtil(i.NextBusStopId, d, isVisited, localpathList, ditance);

                                            // remove current node  
                                            localpathList.RemoveAt(localpathList.Count() - 1);
                                        }
                                    }
                                }
                                else//đi cùng tuyến
                                {
                                    //-ui get route
                                    //Kiểm tra đi cùng tuyến thì ko được đổi chiều đi?
                                    //Đang cùng tuyến: get chiều của cạnh cuối cùng, so sánh với chiều của cạnh chuẩn bị thêm vào:
                                    //Nếu cùng chiều thì thêm vào ok
                                    if (localpathList.ElementAt(localpathList.Count() - 1).IsOutWard == i.IsOutWard) //Điều kiện 2: Nếu cạnh sẽ đi CÙNG chiều với CHIỀU cạnh trước đó
                                    {
                                        // add đỉnh đi qua
                                        BusStopModel bstopstopNext = new BusStopModel()
                                        {
                                            BusStopId = i.BusStopId,
                                            NextBusStopId = i.NextBusStopId,
                                            IsOutWard = i.IsOutWard,
                                            RouteId = i.RouteId,
                                            DecodeString = i.DecodeString,
                                            Distance = i.Distance,
                                            SumDistance = localpathList.ElementAt(localpathList.Count() - 1).SumDistance + i.Distance
                                        };

                                        if (bstopstopNext.SumDistance < (ditance * 2)) //kiểm tra khoảng cách đường bus đã đi < khoảng cách chim bay*2
                                        {
                                            localpathList.Add(bstopstopNext);// KẾT NỘP đỉnh(cạnh) mới vào đường đi

                                            printAllPathsUtil(i.NextBusStopId, d, isVisited, localpathList, ditance);

                                            // remove current node  
                                            localpathList.RemoveAt(localpathList.Count() - 1);
                                        }
                                    }
                                    //Nếu KHÁC chiều thì dừng
                                }
                            }
                        }
                    }

                }
            }

            // Mark the current node  
            isVisited[u] = false;
        }

        private async Task GetListFindData()
        {
            var url = "http://danang.vibus.vn/api/Route/GetListRouteFindData";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table = Newtonsoft.Json.JsonConvert.DeserializeObject<GetDataApi>(data);
                    lstfindata = table.data.ToList();
                }
            }
        }

        private double Distance(double lat1, double lng1, double lat2, double lng2)
        {
            var r = 6378137; // Earth’s mean radius in meter
            var dlat = Rad(lat2 - lat1);
            var dlong = Rad(lng2 - lng1);
            var a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) +
                (Math.Cos(Rad(lat1)) * Math.Cos(Rad(lat2)) *
                Math.Sin(dlong / 2) * Math.Sin(dlong / 2));
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = r * c;
            return d;
        }

        public double Rad(double x)
        {
            return x * Math.PI / 180;
        }
    }

    public class ClassMain
    {
        // Driver code  
        public static void Main(String[] args)
        {
            // Create a sample graph  
            GraphBusStop g = new GraphBusStop();
            //--> mo ta g.addEdge("BusStop", "BusStopNext");//add canh tung canh vao tong: 1187 canh
            g.Innit();
            //g.addEdge("0", "1");//add canh
            //g.addEdge("0", "2");
            //g.addEdge("0", "3");
            //g.addEdge("2", "0");
            //g.addEdge("2", "1");
            //g.addEdge("1", "3");

            //// arbitrary source  
            //string s = "7ddcd411-97c2-40ef-a1df-c284820a958f";

            //// arbitrary destination  
            //string d = "278997f7-83fa-4d51-bc41-2b5827f3b6da";

            //Console.WriteLine("Following are all different" + " paths from " + s + " to " + d);
            //g.printAllPaths(s, d);
            //Console.ReadLine();
            Console.ReadLine();
        }
    }
}