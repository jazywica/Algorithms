using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_DirectedGraphs // Simple example checking ACYCLICITY and performing TOPOLOGICAL SORT, based on the '_03_DirectedGraph.JPG'
{
    // CYCLES - a 'cycle' in a graph G is a sequence of vertices v1,v2,v2..vn so that (v1, v2), (v2, v3)....,(vn-1, vn),(vn, v1) area all edges. Which means that vertices are connected in a loop
    // *Any graph that contains a cycle v1,...vn. can not be LINEARLY ORDERED, because if a start vertex vk comes first then vk comes before vk-1 and it is a contradiction
    // DIRECTLY ACYCLIC GRAPH (DAG) is a a graph with no cycles and therefore can be linearly ordered
    // DAG has two characteristic components: SOURCE (vertex with no incoming edges) and SINK (vertex with no outgoing edges)

    class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
    {
        public bool visited = false; // flags if a VERTEX was visited or not
        public int? group = null; // keeps track of CONNECTED COMPONENTS within the graph
        public int? previsit = null; // these two are for storing the order in which DFS went in and out each NODE
        public int? postvisit = null;
    }


    class DGraph // class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests
    {
        List<int>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        Vertex[] nodes; // array for storing the graph
        int counter; // field for the 'group' function
        int clock = 1; // field for the pre and post visiting functions
        int cycleValue; // field for storing a potential cycle inside a group
        bool cyclic; // flags if we have found a a cycle in a graph
        Stack<int> order = new Stack<int>(); // stack for the TOPOLOGICAL SORT


        DGraph(List<int>[] inputList) // inside the constructor we commence all the preparatory work
        {
            adj = inputList;
            nodes = new Vertex[inputList.Length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
            for (int i = 0; i < nodes.Length; i++) { nodes[i] = new Vertex(); }
        }


        void Explore(int v) // recursive DFS method that looks around the given VERTEX and finds all other connected VERTICES (direct and indirect)
        {
            if (adj[v].Contains(cycleValue)) { cyclic = true; return; } // part of the 'isCyclic()' procedure which checks if the initial node value reappears

            nodes[v].visited = true;
            nodes[v].group = counter;
            PreVisit(v);

            foreach (int w in adj[v])
            {
                if (!nodes[w].visited)
                    Explore(w); // here we call 'Explore' recursively (DFS) before we even finish with looking at all the neighbors
            }

            PostVisit(v);
            order.Push(v); // here we push the values on a stack for the TOPOLOGICAL SORT while we are backtracking
        }


        void PreVisit(int v) { nodes[v].previsit = clock; clock += 1; } // two functions that should keep track of what the DFS is doing inside the GRAPH
        void PostVisit(int v) { nodes[v].postvisit = clock; clock += 1; }


        void GroupComponents() // 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|)
        {
            counter = 1; // 'counter = 1' initializes the first group of connected nodes
            for (int v = 0; v < nodes.Length; v++)
                if (!nodes[v].visited)
                {
                    Explore(v);
                    counter += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
                }

            DisplayValues();
        }


        bool IsCyclic() // function stores the value we are starting with and runs 'Explore' to check if it is present in all connected components
        {
            for (int v = 0; v < nodes.Length; v++)
            {
                cycleValue = v;
                if (!nodes[v].visited)
                    Explore(v);
            }

            if (cyclic == true)
                return true;
            else
                return false;
        }


        Stack<int> TopologicalSort() // function uses STACK, as we start placing the elements from the end to the front
        {
            for (int v = 0; v < nodes.Length; v++)
            {
                if (!nodes[v].visited)
                    Explore(v);
            }
            return order;
        }


        void DisplayValues() // testing function that prints all node properties
        {
            for (int i = 0; i < nodes.Length; i++)
                Console.WriteLine("node index: {0}, belongs to group: {1}, pre: {2}, post: {3}", i, nodes[i].group, nodes[i].previsit, nodes[i].postvisit);
        }


        static void Main(string[] args)
        {
            List<int>[] adjDag = { new List<int>() { 1, 2 }, new List<int>() { 2, 3, 5, 6 }, new List<int>() { 4, 5, 6 }, new List<int>() { 5 }, new List<int>() { 6 }, new List<int>(), new List<int>() { 7 }, new List<int>() };
            List<int>[] adjCyclic = { new List<int>() { 1 }, new List<int>(), new List<int>() { 3 }, new List<int>() { 4 }, new List<int>() { 5 }, new List<int>() { 2 } }; // 0-> 1,  2-> 3-> 4-> 2

            DGraph dag_1 = new DGraph(adjDag);
            DGraph cyclic_1 = new DGraph(adjCyclic);
            dag_1.GroupComponents(); // grouping components works just the same way as with undirected graph
            Console.WriteLine();
            cyclic_1.GroupComponents();
            Console.WriteLine();

            DGraph dag_2 = new DGraph(adjDag);
            DGraph cyclic_2 = new DGraph(adjCyclic);
            Console.WriteLine(dag_2.IsCyclic()); // recognizes what is cyclic and what is acyclic
            Console.WriteLine(cyclic_2.IsCyclic());
            Console.WriteLine();

            DGraph dag_3 = new DGraph(adjDag);
            Stack<int> topo = dag_3.TopologicalSort();
            foreach (var x in topo) // the product of a topological sort may not be the same in all cases, all depends on the order of elements in the adjacency lists
                Console.Write(x + " ");

            Console.ReadKey();
        }
    }
}
