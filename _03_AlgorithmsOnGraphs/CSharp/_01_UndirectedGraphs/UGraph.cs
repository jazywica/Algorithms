using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_UndirectedGraphs // Simple example showing an undirected graph's G(V, E) basic functionality, based on the '_01_UndirectedGraph.JPG'
{
    class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
    {
        public bool visited = false; // flags if a VERTEX was visited or not
        public int? group = null; // keeps track of CONNECTED COMPONENTS within the graph
        public int? previsit = null; // these two are for storing the order in which DFS went in and out each NODE
        public int? postvisit = null;
    }


    class UGraph  // class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests
    {
        List<int>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        Vertex[] nodes; // array for storing the graph
        int counter; // field for the 'group' function
        int clock = 1; // field for the pre and post visiting functions
        List<int> order = new List<int>(); // EXTRA ITEM for storing the order that NODES were looked at. It has to be instantiated here as it is an object and we don't put it in the constructor


        UGraph(List<int>[] inputList) // inside the constructor we commence all the preparatory work
        {
            adj = inputList;
            nodes = new Vertex[inputList.Length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = new Vertex(); }
        }


        void Explore(int v) // recursive DFS method that looks around the given VERTEX and finds all other connected VERTICES (direct and indirect)
        {
            nodes[v].visited = true; // before we do anything we first mark the current NODE as 'visited'
            nodes[v].group = counter; // at this point we can use the property called 'group' that will segregate all the reachable nodes into one group
            PreVisit(v); // we first introduce the pre-visit method before we start the recursion

            foreach (int w in adj[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
            {
                if (!nodes[w].visited) // if we haven't explored the vertex yet, then we step into it
                {
                    order.Add(w);
                    Explore(w); // here we call 'Explore' recursively (DFS) before we even finish with looking at all the neighbors
                }
            }

            PostVisit(v); // we 'wrap up' the post-order as we go out of recursion
        }


        // INTERVALS can be either nested (one contained in the other) or disjoint (non-overlapping). Interleaved (overlapping over part of their lengths) cases are not possible
        // - Case 1 (nested): explore v while we are exploring u, we can not finish exploring u until we are done with v, therefore post(u) > post(v)
        // - Case 2 (disjoint): explore v after we finished exploring u, therefore post(u) < pre(v)
        void PreVisit(int v) { nodes[v].previsit = clock; clock += 1; } // two functions that should keep track of what the DFS is doing inside the GRAPH
        void PostVisit(int v) { nodes[v].postvisit = clock; clock += 1; }


        void GroupComponents() // 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|)
        {
            counter = 1; // 'counter = 1' initializes the first group of connected nodes
            for (int v = 0; v < nodes.Length; v++)
            {
                if (!nodes[v].visited)
                {
                    order.Add(v);
                    Explore(v);
                    counter += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
                }
            }

            DisplayValues();
        }


        void DisplayValues() // testing function that prints all node properties
        {
            for (int i = 0; i < nodes.Length; i++)
                Console.WriteLine("node index: {0}, belongs to group: {1}, visited order: {2}, pre: {3}, post: {4}", i, nodes[i].group, order[i], nodes[i].previsit, nodes[i].postvisit);
        }


        static void Main(string[] args)
        {
            List<int>[] adjList = { new List<int>() { 4 }, new List<int>(), new List<int>() { 5, 6 }, new List<int>() { 5 }, new List<int>() { 0 }, new List<int>() { 2, 3, 6, 7 }, new List<int>() { 2, 5 }, new List<int>() { 5 } };
            List<List<int>> adjList_ver2 = new List<List<int>>() { new List<int>() { 4 }, new List<int>(), new List<int>() { 5, 6 }, new List<int>() { 5 }, new List<int>() { 0 }, new List<int>() { 2, 3, 6, 7 }, new List<int>() { 2, 5 }, new List<int>() { 5 } };

            UGraph graph_1 = new UGraph(adjList);
            graph_1.Explore(2);

            UGraph graph_2 = new UGraph(adjList);
            graph_2.GroupComponents();

            Console.ReadKey();
        }
    }
}
