using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_Ex2_Clustering // MINIMUM SPANNING TREES: EXCERCISE 2 - Given n points on a plane and an integer k, compute the largest possible value of d such that the given points can be partitioned into k non-empty subsets in such a way that the distance between any two points from different subsets is at least d
{
    class Clustering
    {
        static double clustering(int[] x, int[] y, int k) // Used Kruskal's algorithm, implemented with a Disjoint Set (HashSet of HashSets)
        {
            List<double[]> edges = MakeEdges(x, y);
            HashSet<HashSet<int>> nodes = new HashSet<HashSet<int>>();
            for (int i = 0; i < x.Length; i++) // we first initialize the set with single sets containing only one element
                nodes.Add(new HashSet<int>() { i });

            edges.Sort((a, b) => (a[0].CompareTo(b[0]))); // Kruskal algorithm sorts all edges and then processes them independently

            for (int idx = 0; idx < edges.Count; idx++)
            {
                HashSet<int> u = Find(nodes, edges[idx][1]);
                HashSet<int> v = Find(nodes, edges[idx][2]);

                if (!u.SetEquals(v)) // if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
                {
                    if (nodes.Count > k)
                    {
                        u.UnionWith(v);
                        nodes.Remove(v);
                    }
                    else // if we already have the desired amount of clusters we wait for the first edge that is between two different sets - this is our answer
                        return edges[idx][0];
                }
            }
            return -1;
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


        static double CalculateDistance(int x1, int y1, int x2, int y2)
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

            int k = int.Parse(Console.ReadLine()); // number k of clusters (non-empty subsets) which we are going to split our dataset into

            Console.WriteLine(clustering(x, y, k)); // Good job! (Max time used: 0.09/3.00, max memory used: 10985472/536870912.)

            Console.ReadKey();
        }
    }
}
