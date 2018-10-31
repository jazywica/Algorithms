using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Ex1_FindingExitFromMaze // DECOMPOSITION OF GRAPHS 1: EXCERCISE 1 - Given an undirected graph and two distinct vertices 𝑢 and 𝑣, check if there is a path between 𝑢 and 𝑣
{
    class Reachability // the whole solution is based on STATIC method 'Reach' as such was the provided Java template. This implies the use of STATIC FIELDS
    {
        static List<int>[] adj; // global adjacency list
        static bool[] nodes; // global array to store the nodes
        static bool isFound; // extra flag to quickly get out of the RECURSION

        static int Reach(int x, int y)
        {
            
            nodes[x] = true; // before we do anything we first mark the NODE we are at as 'visited'
            foreach (int w in adj[x])
                if (!nodes[w] && !isFound) // if we haven't explored the vertex yet (false) AND THE CONNECTION IS NOT DISCOVERED YET, then we go further
                {
                    if (w == y) // here we check the main condition
                    {
                        isFound = true;
                        return 1; // now we start backtracking straight away
                    }
                    Reach(w, y); // here we call 'reach' recursively with a new neighbor and the old 'y' value
                }

            if (isFound == true) // now we check if the connection was found and return the appropriate value
                return 1;
            else
                return 0;
        }


        static void Main(string[] args)
        {
            int x, y;
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]); // number of NODES
            int m = int.Parse(input[1]); // number of EDGES

            adj = new List<int>[n]; // ADJACENCY LIST for the GRAPH, it has the same amount of elements as there are NODES
            for (int i = 0; i < n; i++)
                adj[i] = new List<int>();

            nodes = new bool[adj.Length];  // the storage for node status has to be initialized with unvisited values
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = false;

            for (int i = 0; i < m; i++) // importing EDGES into the GRAPH. Input vertices start from 1, so the correction to ARRAY/LIST indexes of -1 has to be applied !!!!!!!!!!!!!!!!
            {
                input = Console.ReadLine().Split();
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                adj[x - 1].Add(y - 1); // we simply take one side of the EDGE and assign it to the other end, applying correction for 0-based indexing
                adj[y - 1].Add(x - 1);
            }

            var xy = Console.ReadLine().Split(); // the last line is for storing the 'u' and 'v' VERTICES which represent entrance and exit from the maze
            x = int.Parse(xy[0]) - 1;
            y = int.Parse(xy[1]) - 1;


            Console.WriteLine(Reach(x, y)); // Good job! (Max time used: 0.06/1.50, max memory used: 8585216/536870912.)

            Console.ReadKey();
        }
    }
}
