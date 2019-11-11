using System;
using System.Collections.Generic;
using System.Linq;
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

        // No. of vertices in graph  
        private int numberVertices;
        List<string> listVertices = new List<string>();//750 tram

        // adjacency list  
        private Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>();

        private void initListVertices(int numberVertices)
        {
    
            for (int i = 0; i < numberVertices; i++)
            {
                this.listVertices.Add(i.ToString());//change i thanh busstop id
            }
        }
        // Constructor  
        public GraphBusStop(int NumVertices)
        {
            // initialise vertex count  
            this.numberVertices = NumVertices;
            initListVertices(this.numberVertices);

            // initialise adjacency list  
            initAdjacencyList();
        }

        // utility method to initialise  
        // adjacency list  
        private void initAdjacencyList()
        {
            //adjacencyList = new Dictionary<string, List<string>>();

            foreach(string vertices in this.listVertices)
            {
                List<string> listvaule = new List<string>();//de luu danh sach cac dinh ke

                adjacencyList.Add(vertices, listvaule);
            }
        }

        // add edge from u to v  --> u: busstop, v:bustop next
        public void addEdge(string u, string v)
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

            // add source to path[]  
            pathList.Add(s);

            // Call recursive utility  
            printAllPathsUtil(s, d, isVisited, pathList);
        }

        // A recursive function to print  
        // all paths from 'u' to 'd'.  
        // isVisited[] keeps track of  
        // vertices in current path.  
        // localPathList<> stores actual  
        // vertices in the current path  
        private void printAllPathsUtil(string u, string d, Dictionary<string, bool> isVisited, List<string> localPathList)
        {
            // Mark the current node  
            isVisited[u] = true;

            if (u.Equals(d))
            {
                Console.WriteLine(string.Join("-->", localPathList));

                // if match found then no need  
                // to traverse more till depth  
                isVisited[u] = false;
                return;
            }

            // Recur for all the vertices  
            // adjacent to current vertex  
            foreach (string i in adjacencyList[u])
            {
                if (!isVisited[i])
                {
                    // store current node  
                    // in path[]  
                    localPathList.Add(i);
                    printAllPathsUtil(i, d, isVisited, localPathList);

                    // remove current node  
                    // in path[]  
                    localPathList.Remove(i);
                }
            }

            // Mark the current node  
            isVisited[u] = false;
        }

        
    }

    public class ClassMain
    {
        // Driver code  
        public static void Main(String[] args)
        {
            // Create a sample graph  
            GraphBusStop g = new GraphBusStop(4);
            g.addEdge("0", "1");//add canh
            g.addEdge("0", "2");
            g.addEdge("0", "3");
            g.addEdge("2", "0");
            g.addEdge("2", "1");
            g.addEdge("1", "3");

            // arbitrary source  
            string s = "2";

            // arbitrary destination  
            string d = "3";

            Console.WriteLine("Following are all different" + " paths from " + s + " to " + d);
            g.printAllPaths(s, d);
            Console.ReadLine();
        }
    }
}