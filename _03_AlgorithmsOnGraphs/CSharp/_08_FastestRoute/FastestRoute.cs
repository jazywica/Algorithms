using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_FastestRoute // Simple example with Naive and Dijkstra algorithm on a directed graph G(V, E), based on the '_08_FastestRoute.JPG'
{
    // EDGE RELAXATION:
    // Observation: any sub-path of an optimal path is also optimal, which takes us to the following property: If S-> ..-> u-> t is the shortest path from S to then d(S, t) = d(S, u) + w(u, t)
    // dist[v] will be an upper bound on the actual distance from S to v, unlike in BFS, this value will most likely be updated many times during the procedure
    // the EDGE RELAXATION for an edge (u, v) checks whether going from S to v through u improves the current value of dist, it pertains to the last edge before t at a given stage

    class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
    {
        public bool known = false; // flags if a VERTEX is confirmed to be safe (Dijkstra)
        public long dist = int.MaxValue; // initializes the node distances
        public int path = -1;
    }

    class FastestRoute
    {
        List<int>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        List<int>[] cost; // list for storing the initial uncorrected costs
        Vertex[] nodes; // array for storing the graph

        FastestRoute(List<int>[] adjList, List<int>[] costList)
        {
            adj = adjList;
            cost = costList;
            nodes = new Vertex[adjList.Length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = new Vertex(); }
        }


        Stack<long> NaiveFastestRoute(int s, int t) // based on iterative correction of distances from origin until there is nothing to update anymore
        {
            nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
            bool is_changed = false;

            do {
                is_changed = false;
                for (int u = 0; u < adj.Length; u++) {
                    for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    {
                        int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                        if (nodes[u].dist == int.MaxValue) // if the starting node u hasn't been discovered yet, we just move on
                            continue;
                        else if (nodes[v].dist > nodes[u].dist + cost[u][i]) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            nodes[v].dist = nodes[u].dist + cost[u][i];
                            nodes[v].path = u;
                            is_changed = true; // this variable will become true each time we change something
                        }
                    }
                }
            } while (is_changed); // the 'do' loop stops as soon as there is an iteration with no further updates

            DisplayValues();

            return ReconstructPath(s, t);
        }


        Stack<long> Dijkstra(int s, int t) // based on a fact that the smallest distance to all available nodes is enough to move onto next node, the other nodes can only be bigger
        {
            nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

            while (true)
            {
                int u = ExtractMin();
                if (u == -1 || nodes.All(x => x.known == true)) // first condition checks for non-connected nodes and the second if all nodes have been verified
                    break;

                for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                {
                    int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                    if (nodes[v].dist > nodes[u].dist + cost[u][i]) // for direct neighbours we RELAX THE EDGES if possible
                    {
                        nodes[v].dist = nodes[u].dist + cost[u][i];
                        nodes[v].path = u;
                    }
                }
                nodes[u].known = true; // this is how we can CHANGE THE PRIORITY
            }

            DisplayValues();

            return ReconstructPath(s, t);
        }


        int ExtractMin() // helper function that returns the element u index which has the smallest distance
        {
            long smallest = int.MaxValue;
            int index = -1;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].dist < smallest && nodes[i].known == false)
                {
                    smallest = nodes[i].dist;
                    index = i;
                }
            }
            return index; // we always compare the distances, but we return the index
        }

        Stack<long> ReconstructPath(int s, int u)
        {
            Stack<long> result = new Stack<long>();
            while (u != s)
            {
                result.Push(u);
                u = nodes[u].path;
            }
            result.Push(s); // the loop above stops when reaching the start node, so if we want, we can include it here
            return result;
        }

        void DisplayValues() // testing function that prints all node properties
        {
            for (int i = 0; i < nodes.Length; i++)
                Console.WriteLine("node index: {0}, distance to the source: {1}, received from: {2}", i, nodes[i].dist, nodes[i].path);
        }


        static void Main(string[] args)
        {
            List<int>[] adj_small = { new List<int>() { 1, 2 }, new List<int>() { 2 }, new List<int>(), new List<int>() { 0 } }; // first case from the exercises
            List<int>[] cost_small = { new List<int>() { 1, 5 }, new List<int>() { 2 }, new List<int>(), new List<int>() { 2 } };

            FastestRoute graph_small = new FastestRoute(adj_small, cost_small);
            Stack<long> route_small = graph_small.NaiveFastestRoute(0, 2);
            foreach (int x in route_small)
                Console.Write(x + " ");
            Console.WriteLine();

            FastestRoute graph_small_2 = new FastestRoute(adj_small, cost_small);
            Stack<long> route_small_2 = graph_small_2.Dijkstra(0, 2);
            foreach (int x in route_small_2)
                Console.Write(x + " ");
            Console.WriteLine();


            List<int>[] adj_big = { new List<int>() { 1, 2 }, new List<int>() { 2, 3 }, new List<int>() { 4, 5 }, new List<int>() { 1 }, new List<int>(), new List<int>() { 1 }, new List<int>() { 2 }, new List<int>() };
            List<int>[] cost_big = { new List<int>() { 9, 5 }, new List<int>() { 2, 2 }, new List<int>() { 9, 8 }, new List<int>() { 5 }, new List<int>(), new List<int>() { 1 }, new List<int>() { 4 }, new List<int>() };

            FastestRoute graph_big = new FastestRoute(adj_big, cost_big);
            Stack<long> route_big = graph_big.NaiveFastestRoute(0, 5);
            foreach (int x in route_big)
                Console.Write(x + " ");
            Console.WriteLine();

            FastestRoute graph_big_2 = new FastestRoute(adj_big, cost_big);
            Stack<long> route_big_2 = graph_big_2.Dijkstra(0, 5);
            foreach (int x in route_big_2)
                Console.Write(x + " ");

            Console.ReadKey();
        }
    }
}
