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
        public static int NumRoute = 1;//Số lần chuyển tuyến
        // No. of vertices in graph  
        private int numberVertices;
        List<string> listVertices = new List<string>();//750 tram-thông tin đỉnh trạm
        List<Data> lstfindata = new List<Data>();
        // adjacency list  
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
            }


            //------------------------------------------------------
            // arbitrary source  
            string s = "2f41a813-94ad-4f3e-b557-88cbe1768c40";

            // arbitrary destination  
            string d = "834a8e8e-33e9-42ff-b424-163febe6ed5b";

            DateTime t = DateTime.Now;
            Console.WriteLine("Following are all different" + " paths from " + s + " to " + d);
            printAllPaths(s, d);

            var now = (DateTime.Now - t).TotalMilliseconds;


        }
        // utility method to initialise  
        // adjacency list  
        private void initAdjacencyList()
        {
            //adjacencyList = new Dictionary<string, List<string>>();

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
            //adjacencyList[u].Add(v);
            adjacencyList[u].Add(v);
        }

        // Prints all paths from  po
        // 's' to 'd'  
        public void printAllPaths(string s, string d)
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
            BusStopModel bstopstopStart = new BusStopModel()
            {
                BusStopId = s
            };

            pathList_Ok.Add(bstopstopStart);
            
            // Call recursive utility  
            printAllPathsUtil(s, d, isVisited, pathList, pathListRouteID, pathList_Ok);
        }

        // A recursive function to print  
        // all paths from 'u' to 'd'.  
        // isVisited[] keeps track of  
        // vertices in current path.  
        // localPathList<> stores actual  
        // vertices in the current path  
        private void printAllPathsUtil(string u, string d, Dictionary<string, bool> isVisited, List<string> localPathList, List<string> localPathListRouteID, List<BusStopModel> localpathList_Ok)
        {
            // Mark the current node  
            isVisited[u] = true;

            if (u.Equals(d))//Đã tới đích
            {
                Console.WriteLine(string.Join("-->", localPathListRouteID));
                Console.WriteLine("/n Tong BStop: " + localPathList.Count());
                Console.WriteLine(string.Join("-->", localPathList));//in đường đi

                Console.WriteLine("-------------------------------- \n");

                Console.WriteLine(string.Join("/n Tong BStop", localpathList_Ok.Count()));
                var dem = 0;

                // if match found then no need  
                // to traverse more till depth  
                isVisited[u] = false;
                return;
            }

            // Recur for all the vertices  
            // adjacent to current vertex  
            foreach (BusStopModel i in adjacencyList[u])//danh sách busstop next (các đỉnh kề)
            {
                if (!string.IsNullOrEmpty(i.NextBusStopId))
                {
                    if (!isVisited[i.NextBusStopId])//điều kiện đi tiếp
                    {
                        // store current node  
                        // in path[]  
                        //kiểm tra điều kiện đưa điểm kề vào (busstop next vào danh sách đường đi)- cạnh u [i]
                        var route = i.RouteId;

                        if (localPathListRouteID.Count() == 0)
                        {
                            localPathListRouteID.Add(route);
                        }

                        //check thỏa 3 điều kiện lưu vào:

                        if (localPathListRouteID.Count() <= GraphBusStop.NumRoute) // Điều kiện 1 (số lần chuyển tuyến <= 3 lần) số lần chuyển tuyến chỉ nhỏ hơn hoặc bằng 2
                        {
                            if (i.RouteId != localPathListRouteID.ElementAt(localPathListRouteID.Count() - 1)) // Điều kiện 3: Tuyến mới sẽ đi chưa có trong danh sách tuyến hiện tại - tuyến mới khác # với tuyến hiện tại
                            {
                                if (!localPathListRouteID.Contains(i.RouteId))  // tuyến mới không có trong danh sách tuyến đã đi
                                {
                                    localPathListRouteID.Add(route);
                                    //-ui get route
                                    localPathList.Add(i.NextBusStopId);//ADD đỉnh đi tiếp vào NÈ - khi add điểm đi tiếp theo vào đường đi KHÔNG ĐƯỢC thỏa 3 điều kiện trên:

                                    // add đỉnh đi qua
                                    localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).NextBusStopId = i.NextBusStopId;
                                    localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).IsOutWard = i.IsOutWard;
                                    localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).RouteId = i.RouteId;
                                    localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).DecodeString = i.DecodeString;
                                    localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).Distance = i.Distance;

                                    BusStopModel bstopstopStart = new BusStopModel()
                                    {
                                        BusStopId = i.NextBusStopId
                                    };

                                    localpathList_Ok.Add(bstopstopStart);

                                    printAllPathsUtil(i.NextBusStopId, d, isVisited, localPathList, localPathListRouteID, localpathList_Ok);
 
                                    // remove current node  
                                    // in path[]  
                                    localPathList.Remove(i.NextBusStopId);
                                    localPathListRouteID.Remove(i.RouteId);
                                    localpathList_Ok.RemoveAt(localpathList_Ok.Count() -1);
                                }
                            }
                            else//đi cùng tuyến
                            {
                                //-ui get route
                                //Kiểm tra đi cùng tuyến thì ko được đổi chiều đi?
                                if(localpathList_Ok.Count() <= 1)
                                {
                                    localPathList.Add(i.NextBusStopId);//khi add điểm đi tiếp theo vào đường đi KHÔNG ĐƯỢC thỏa 3 điều kiện trên:
                                
                                        // add đỉnh đi qua
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).NextBusStopId = i.NextBusStopId;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).IsOutWard = i.IsOutWard;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).RouteId = i.RouteId;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).DecodeString = i.DecodeString;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).Distance = i.Distance;

                                        BusStopModel bstopstopNext = new BusStopModel()
                                        {
                                            BusStopId = i.NextBusStopId
                                        };

                                        localpathList_Ok.Add(bstopstopNext);

                                        printAllPathsUtil(i.NextBusStopId, d, isVisited, localPathList, localPathListRouteID, localpathList_Ok);

                                        // remove current node  
                                        // in path[]  
                                        localPathList.Remove(i.NextBusStopId);
                                        localpathList_Ok.RemoveAt(localpathList_Ok.Count() - 1);
                                }
                                else
                                {
                                    //Đang cùng tuyến: get chiều của cạnh cuối cùng, so sánh với chiều của cạnh chuẩn bị thêm vào:
                                    //Nếu cùng chiều thì thêm vào ok
                                    if(localpathList_Ok.ElementAt(localpathList_Ok.Count() - 2).IsOutWard  == i.IsOutWard) //Điều kiện 2: Nếu cạnh sẽ đi CÙNG chiều với CHIỀU cạnh trước đó
                                    {
                                        localPathList.Add(i.NextBusStopId);//khi add điểm đi tiếp theo vào đường đi KHÔNG ĐƯỢC thỏa 3 điều kiện trên:
                                
                                        // add đỉnh đi qua
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).NextBusStopId = i.NextBusStopId;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).IsOutWard = i.IsOutWard;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).RouteId = i.RouteId;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).DecodeString = i.DecodeString;
                                        localpathList_Ok.ElementAt(localpathList_Ok.Count() - 1).Distance = i.Distance;

                                        BusStopModel bstopstopNext = new BusStopModel()
                                        {
                                            BusStopId = i.NextBusStopId
                                        };

                                        localpathList_Ok.Add(bstopstopNext);

                                        printAllPathsUtil(i.NextBusStopId, d, isVisited, localPathList, localPathListRouteID, localpathList_Ok);

                                        // remove current node  
                                        // in path[]  
                                        localPathList.Remove(i.NextBusStopId);
                                        localpathList_Ok.RemoveAt(localpathList_Ok.Count() - 1);
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