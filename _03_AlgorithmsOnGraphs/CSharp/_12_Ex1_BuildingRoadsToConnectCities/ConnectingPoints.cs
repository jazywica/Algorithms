using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_Ex1_BuildingRoadsToConnectCities // MINIMUM SPANNING TREES: EXCERCISE 1 - Given n points on a plane, connect them with segments of minimum total length such that there is a path between any two points.
{
    class CustomComparer : IComparer<double[]> // custom comparer for sorting elements in SortedSet by distance
    {
        public int Compare(double[] left, double[] right)
        {
            int comp = left[1].CompareTo(right[1]);
            if (comp == 0)
                return left[0].CompareTo(right[0]); // this will prevent from deleting all nodes with the same initial value
            return comp;
        }
    }

    class ConnectingPoints // Recall that the length of a segment with endpoints (x1; y1) and (x2; y2) is equal to: SQRT((x1 - x2)^2 + (y1 - y2)^2)
    {
        static double MinimumDistance(int[] x, int[] y)  // Used Prim's algorithm, implemented with a Priority Queue (modified SortedSet)
        {
            double result = 0;
            int n = x.Length;
            List<int>[] adj = new List<int>[n];
            List<double>[] cost = new List<double>[n];
            
            SortedSet<double[]> priorityQueue = new SortedSet<double[]>(new CustomComparer()); // this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly
            double[] dist = new double[n];

            for (int i = 0; i < adj.Length; i++)
            {
                if (i == 0){
                    dist[i] = 0;
                    priorityQueue.Add(new double[] { (int)i, 0 });
                }
                else{
                    dist[i] = int.MaxValue;
                    priorityQueue.Add(new double[] { (int)i, int.MaxValue });
                }
                adj[i] = new List<int>();
                cost[i] = new List<double>();
            }

            MakeEdges(x, y, adj, cost);

            while (priorityQueue.Count != 0)
            {
                double[] U = priorityQueue.First(); // we need a handle for this element to update it later on
                priorityQueue.Remove(U);
                int u = (int)U[0];

                for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                {
                    int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                    if (dist[v] > cost[u][i] && priorityQueue.Contains(new double[] { v, dist[v] })) // since we are only comparing single distances, we can't look back, so we only take into account
                    {
                        double oldDist = dist[v]; // for direct neighbours we RELAX THE EDGES if possible
                        dist[v] = cost[u][i];

                        priorityQueue.Remove(new double[] { (int)v, oldDist }); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
                        priorityQueue.Add(new double[] { (int)v, cost[u][i] });
                    }
                }
                result += dist[u];
            }

            return result;
        }


        static void MakeEdges(int[] x, int[] y, List<int>[] adj, List<double>[] cost) // helper function that will join the coordinate arrays into adjacency and cost lists
        {
            for (int i = 0; i < x.Length - 1; i++){
                for (int j = i + 1; j < y.Length; j++){
                    double distance = CalculateDistance(x[i], y[i], x[j], y[j]);
                    adj[i].Add((int)j);
                    cost[i].Add(distance);
                    adj[j].Add((int)i);
                    cost[j].Add(distance);
                }
            }
        }


        static double CalculateDistance (int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int[] x = new int[n]; // storage for 'x' coordinates
            int[] y = new int[n]; // storage for 'y' coordinates

            for (int i = 0; i < n; i++) // importing points into two separate arrays 
            {
                string[] input = Console.ReadLine().Split();
                x[i] = int.Parse(input[0]);
                y[i] = int.Parse(input[1]);
            }

            Console.WriteLine(MinimumDistance(x, y)); // Good job! (Max time used: 0.10/3.00, max memory used: 12238848/536870912.)

            Console.ReadKey();
        }
    }
}
