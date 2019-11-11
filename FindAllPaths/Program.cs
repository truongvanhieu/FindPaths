﻿using System;
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
    public class Graph
    {

        // No. of vertices in graph  
        private int v;

        // adjacency list  
        private List<int>[] adjList;

        // Constructor  
        public Graph(int vertices)
        {

            // initialise vertex count  
            this.v = vertices;

            // initialise adjacency list  
            initAdjList();
        }

        // utility method to initialise  
        // adjacency list  
        private void initAdjList()
        {
            adjList = new List<int>[v];

            for (int i = 0; i < v; i++)
            {
                adjList[i] = new List<int>();
            }
        }

        // add edge from u to v  
        public void addEdge(int u, int v)
        {
            // Add v to u's list.  
            adjList[u].Add(v);
        }

        // Prints all paths from  
        // 's' to 'd'  
        public void printAllPaths(int s, int d)
        {
            bool[] isVisited = new bool[v];
            List<int> pathList = new List<int>();

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
        private void printAllPathsUtil(int u, int d, bool[] isVisited, List<int> localPathList)
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
            foreach (int i in adjList[u])
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
            List<string> l = new List<string> { "1", "2", "3", "4" };
            Console.WriteLine(string.Join("-->", l));
            // Create a sample graph  
            Graph g = new Graph(4);
            g.addEdge(0, 1);
            g.addEdge(0, 2);
            g.addEdge(0, 3);
            g.addEdge(2, 0);
            g.addEdge(2, 1);
            g.addEdge(1, 3);

            // arbitrary source  
            int s = 2;

            // arbitrary destination  
            int d = 3;

            Console.WriteLine("Following are all different" + " paths from " + s + " to " + d);
            g.printAllPaths(s, d);
            Console.ReadLine();
        }
    }
}