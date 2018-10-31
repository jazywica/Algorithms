using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Ex1_ConsistencyOfCurriculum // DECOMPOSITION OF GRAPHS 2: EXCERCISE 1 - Check whether a given directed graph with 𝑛 vertices and 𝑚 edges contains a cycle.
{
    class Acyclicity // the whole solution is based on STATIC method 'Acyclic' as such was the provided Java template
    {
        static bool cyclic = false; // flags if we have found a a cycle in a graph

        static int Acyclic(List<int>[] adj)
        {
            bool[] nodes = new bool[adj.Length]; // the storage for node status has to be initialized with unvisited values
            int cycleValue; // field for storing a potential cycle inside a group

            for (int v = 0; v < nodes.Length; v++)
            {
                cycleValue = v; // here we store the first member of the group we are visiting 
                if (!nodes[v])
                    Explore(adj, nodes, cycleValue, v);
            }

            if (cyclic == true)
                return 1;
            else
                return 0;
        }


        static void Explore(List<int>[] adj, bool[] nodes, int cycleValue, int v) // helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
        {
            if (adj[v].Contains(cycleValue)) { cyclic = true; return; } // checks if the initial node value reappears and returns immediately

            nodes[v] = true;
            foreach (int w in adj[v])
                if (!nodes[w])
                    Explore(adj, nodes, cycleValue, w);
        }


        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            List<int>[] adj = new List<int>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            for (int i = 0; i < m; i++) // importing EDGES into the GRAPH. Input vertices start from 1, so the correction to ARRAY/LIST indexes of -1 has to be applied !!!!!!!!!!!!!!!!
            {
                int x, y;
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1); // in ORDERED GRAPHS we take the start vertex and append the END VERTEX to it, applying correction for 0-based indexing
            }

            Console.WriteLine(Acyclic(adj)); // Good job! (Max time used: 0.06/1.50, max memory used: 8863744/536870912.)

            Console.ReadKey();
        }
    }
}
