using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_CurrencyExchange // Example with Bellman-Ford algorithm and Negative Cycles on a directed graph G(V, E) and negative weights, based on the _09_CurrencyExchange.JPG
{
    // ARBITRARY IN CURRENCY EXHCANGE
    // Conversions use multiplications of pair rates: x*y = 2^(log2(x)) * 2^(log2(y)) = 2^(log2(x) + log2(y)) but we can use a simple log property and turn it to a sum of numbers instead
    // to maximize result 4 * 1 * 0.5 = 2 = 2^1 we use a sum of logarithms: log2(4)+log2(1)+log2(0.5) = 2 + 0 + -1 = 1 which is the exponent of the result we got from numbers
    // to minimize a sum we can do the following: min = - SUM(log(xi)) or better: SUM(-log(xi)), so we have to convert each conversion rate to: -log2(x)

    // Assume that a cycle ci -> cj -> ck -> ci has negative weight. This means that -(log cij + log cjk + log cki) < 0 and hence log cij + log cjk + log cki > 0.
    // This, in turn, means that: rij rjk rki = 2log cij 2log cjk2log cki = 2log cij+log cjk+log cki > 1 - this means that we take out more than we put in.
    // Negative cycles have a (logarithmic) negative sum, which is easy to detect, as it endlessly decreases the numbers within it. 

    class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
    {
        public bool known = false; // flags if a VERTEX is visited
        public long dist = int.MaxValue; // initializes the node distances
        public int path = -1; // stores the previous node for the fastest path
        public int path_bfs = -1; // stores the previous node for the shortest path
    }

    class Bellman_Ford
    {
        List<int>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        List<int>[] cost; // list for storing the initial uncorrected costs
        Vertex[] nodes; // array for storing the graph
        Queue<long> q; // queue to store nodes in BFS
        HashSet<long> A_nodes; // storage for the Arbitrage, all the nodes affected by the cycle
        HashSet<long> cycle; // stores only the cycle

        Bellman_Ford(List<int>[] adjList, List<int>[] costList)
        {
            adj = adjList;
            cost = costList;
            nodes = new Vertex[adjList.Length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = new Vertex(); }
        }


        Stack<long> BellmanFord(int s, int t) // based on iterative correction of distances from origin until there is nothing to update anymore
        {
            nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

            for (int iter = 0; iter < adj.Length - 1; iter++) // theoretically it is enough to run |V| - 1 iterations on all edges to be sure that the full optimization of distances has been made
            {
                for (int u = 0; u < adj.Length; u++) // here we start running and optimizing all edges in a graph
                {
                    for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    {
                        int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                        if (nodes[u].dist == int.MaxValue) // if the starting node u hasn't been discovered yet, we just move on
                            continue;
                        else if (nodes[v].dist > nodes[u].dist + cost[u][i]) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            nodes[v].dist = nodes[u].dist + cost[u][i];
                            nodes[v].path = u;
                        }
                    }
                }
            }

            DisplayValues();

            return ReconstructPath(s, t);
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


        Stack<long> NegativeCycles(int s, int t) // based on iterative correction of distances from origin to detect if there are negative cycles in the graph
        {
            nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
            int n = adj.Length;
            Stack<long> result = new Stack<long>();
            A_nodes = new HashSet<long>(); // stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set
            cycle = new HashSet<long>(); // stores only the cycle


            // 1. Bellman-Ford that collects all the affected by the cycle nodes
            for (int iter = 0; iter < n; iter++) // theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
            {
                for (int u = 0; u < n; u++) // here we start running and optimizing all edges in a graph
                {
                    for (int i = 0; i < adj[u].Count; i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    {
                        int v = adj[u][i]; // v stores the index of the node on the other end, as usual

                        if (nodes[u].dist == int.MaxValue) // if the starting node u hasn't been discovered yet, we just move on
                            continue;
                        else if (nodes[v].dist > nodes[u].dist + cost[u][i]) // for visited neighbours we RELAX THE EDGES if possible
                        {
                            nodes[v].dist = nodes[u].dist + cost[u][i];
                            nodes[v].path = u; // here we store the fastest path

                            if (iter == n - 1)
                            {
                                cycle.Add(u); // this should pick up the cycle itself
                                A_nodes.Add(u);
                                foreach (int k in adj[u]) // this should pick up all affected nodes
                                    A_nodes.Add(k);
                            }
                        }
                    }
                }
            }


            // 2. BFS for checking if the end currency is reachable from the cycle
            q = new Queue<long>(cycle.ToArray()); // here we convert the SET into a QUEUE for BFS
            while (q.Count > 0) // Breadth-First Search for looking for the shortest path in a graph by storing the nodes that the current node was discovered from
            {
                long u = q.Dequeue();
                foreach (int v in adj[u])
                {
                    if (nodes[v].known == false)
                    {
                        q.Enqueue(v);
                        nodes[v].known = true;
                        nodes[v].path_bfs = (int)u; // here we store the shortest path for comparison
                    }
                }
            }

            result = ReconstructArbitrage(t);


            // 3. BFS for checking if the cycle is reachable from the start
            q = new Queue<long>(new long[] { s });
            for (int i = 0; i < nodes.Length; i++) { nodes[i].known = false; }

            while (q.Count > 0)
            {
                long u = q.Dequeue();
                if (u == result.Peek())
                {
                    Console.WriteLine("Yes, there is a path with an infinite cycle !!!\n"); // actually it is enough to get to the first element of the cycle
                    break;
                }

                foreach (int v in adj[u])
                {
                    if (nodes[v].known == false)
                    {
                        q.Enqueue(v);
                        nodes[v].known = true;
                        nodes[v].path_bfs = (int)u; // here we store the shortest path for comparison
                    }
                }
            }

            Console.WriteLine("Values after BFS:");
            DisplayValues();

            return result;
        }


        Stack<long> ReconstructArbitrage(int u)
        {
            Stack<long> result = new Stack<long>();
            while (u != -1)
            {
                if (cycle.Contains(u))
                {
                    result.Push(u);
                    return result;
                }
                result.Push(u);
                u = nodes[u].path_bfs;
            }
            return new Stack<long>(); 
        }


        void DisplayValues() // testing function that prints shortest paths and compares fastest and shortest paths
        {
            for (int i = 0; i < nodes.Length; i++)
                Console.WriteLine("node index: {0}, distance to the source: {1}, fastest path: {2}, shortest path: {3}", i, nodes[i].dist, nodes[i].path, nodes[i].path_bfs);
        }


        static void Main(string[] args)
        {
            //Currency Exchange Example: We can either directly exchange RUR to USD with 0.015 rate or exchange it via EUR with rates 0.013 and 1.16
            void ExchangeExample(double RUR_USD, double RUR_EUR, double EUR_USD)
            {
                Console.WriteLine("Single conversion to USD: {0}, conversion via EUR: {1}", RUR_USD, RUR_EUR * EUR_USD); // this means that it is better to buy EUR first and then change it to USD
                double R_U = -Math.Log(RUR_USD, 2);
                double R_E = -Math.Log(RUR_EUR, 2);
                double E_U = -Math.Log(EUR_USD, 2);
                Console.WriteLine("Logarithmic values for single rates, conversion to USD: {0:0.0000}, to EUR: {1:0.0000}, to EUR: {2:0.0000}", R_U, R_E, E_U);
                Console.WriteLine("Logarithmic sums of two alternative exchanges, direct conversion to USD: {0:0.0000}, to USD via EUR: {1:0.0000}", R_U, R_E + E_U); // now we can add the sum
            }

            ExchangeExample(0.015, 0.013, 1.16); // testing numbers from the video
            Console.WriteLine();


            // To solve the exchange problem we have to use the naive solution that scans all edges separately in order to handle negative edges (which we need to 
            //Dijkstra algorithm doesn't work for negative edges as it relies on a fact that the shortest path goes through vertices that are closer to the start and can not predict for negative edges
            List<int>[] adjDag = { new List<int>() { 1, 2 }, new List<int>() { 2, 3 }, new List<int>() { 3, 4 }, new List<int>() { 4 }, new List<int>() };
            List<int>[] adjDag_cost = { new List<int>() { 4, 3 }, new List<int>() { -2, 4 }, new List<int>() { -3, 1 }, new List<int>() { 2 }, new List<int>() };

            Bellman_Ford graph = new Bellman_Ford(adjDag, adjDag_cost);
            Stack<long> route = graph.BellmanFord(0, 3);
            foreach (int x in route)
                Console.Write(x + " ");
            Console.WriteLine();


            Console.WriteLine("\nNegativeCycles:");
            List<int>[] adjCyclic = { new List<int>() { 1, 2 }, new List<int>() { 2, 3, 7 }, new List<int>() { 3, 4 }, new List<int>() { 6 }, new List<int>() { 3, 5 }, new List<int>() { 2, 8 }, new List<int>(), new List<int>() { 3 }, new List<int>() };
            List<int>[] adjCyclic_cost = { new List<int>() { 4, 3 }, new List<int>() { -2, 4, 4 }, new List<int>() { -3, 1 }, new List<int>() { 1 }, new List<int>() { 2, 2 }, new List<int>() { -5, 1 }, new List<int>(), new List<int>() { 1 }, new List<int>() };

            Bellman_Ford graphCyclic = new Bellman_Ford(adjCyclic, adjCyclic_cost);
            Stack<long> routeCyclic = graphCyclic.NegativeCycles(0, 6);
            foreach (int x in routeCyclic)
                Console.Write(x + " ");
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
