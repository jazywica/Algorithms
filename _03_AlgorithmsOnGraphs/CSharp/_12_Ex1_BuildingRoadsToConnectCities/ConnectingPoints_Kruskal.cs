using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_Ex1_BuildingRoadsToConnectCities // MINIMUM SPANNING TREES: EXCERCISE 1 - Given n points on a plane, connect them with segments of minimum total length such that there is a path between any two points.
{
    class ConnectingPoints // Recall that the length of a segment with endpoints (x1; y1) and (x2; y2) is equal to: SQRT((x1 - x2)^2 + (y1 - y2)^2)
    {
        static double MinimumDistance(int[] x, int[] y) // Used Kruskal's algorithm, implemented with a Disjoint Set (HashSet of a HashSet)
        {
            double result = 0;
            List<double[]> edges = MakeEdges(x, y);
            HashSet<HashSet<int>> nodes = new HashSet<HashSet<int>>();
            for (int i = 0; i < x.Length; i++)  // we first initialize the set with single sets containing only one element
                nodes.Add(new HashSet<int>() { i });

            edges.Sort((a, b) => (a[0].CompareTo(b[0]))); // Kruskal algorithm sorts all edges and then processes them independently

            for (int idx = 0; idx < edges.Count; idx++)
            {
                HashSet<int> u = Find(nodes, edges[idx][1]);
                HashSet<int> v = Find(nodes, edges[idx][2]);
				
                if (!u.SetEquals(v)) // if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
                {
                    u.UnionWith(v);
                    nodes.Remove(v);
                    result += edges[idx][0];
                }
            }

            return result;
        }


        static List<double[]> MakeEdges(int[] x, int[] y) // helper function that will join the input arrays into one list with three elements per edge: distance, from, to
        {
            List<double[]> result = new List<double[]>();

            for (int i = 0; i < x.Length - 1; i++) {
                for (int j = i + 1; j < y.Length; j++) {
                    result.Add(new double[3]);
                    double[] current = result.Last();
                    current[0] = CalculateDistance(x[i], y[i], x[j], y[j]);
                    current[1] = (int)i;
                    current[2] = (int)j;
                }
            }
            return result;
        }


        static double CalculateDistance (int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        static HashSet<int> Find(HashSet<HashSet<int>> nodes, double node) // helper function that will return the set, in which the desired node currently is
        {
            foreach (var item in nodes)
            {
                if (item.Contains((int)node))
                    return item;
            }
            return new HashSet<int>();
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

            Console.WriteLine(MinimumDistance(x, y)); // Good job!(Max time used: 0.22 / 3.00, max memory used: 11038720 / 536870912.)

            Console.ReadKey();
        }
    }
}
