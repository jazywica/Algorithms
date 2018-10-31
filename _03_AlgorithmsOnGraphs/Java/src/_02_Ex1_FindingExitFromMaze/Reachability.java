package _02_Ex1_FindingExitFromMaze; // must be removed for submission

import java.util.ArrayList;
import java.util.Scanner;


public class Reachability // DECOMPOSITION OF GRAPHS 1: EXCERCISE 1 - Given an undirected graph and two distinct vertices 𝑢 and 𝑣, check if there is a path between 𝑢 and 𝑣
{
	private static boolean isFound; // extra flag to quickly get out of the RECURSION

	private static int reach(ArrayList<Integer>[] adj, int x, int y)
	{
		boolean[] nodes = new boolean[adj.length]; // the storage for node status has to be initialised with unvisited values
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = false;

		return explore(adj, nodes, x, y);
	}


	private static int explore(ArrayList<Integer>[] adj, boolean[] nodes, int x, int y)
	{
		nodes[x] = true; // before we do anything we first mark the NODE we are at as 'visited'
		for (int w : adj[x])
		{
			if (!nodes[w] && !isFound) // if we haven't explored the vertex yet (false) AND THE CONNECTION IS NOT DISCOVERED YET, then we go further
			{
				if (w == y) // here we check the main condition
				{
					isFound = true;
					return 1; // now we start backtracking straight away
				}
				explore(adj, nodes, w, y); // here we call 'explore' recursively with a new neighbour and the old 'y' value
			}
		}

		if (isFound == true) // now we check if the connection was found and return the appropriate value
			return 1;
		else
			return 0;
	}


    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int n = scanner.nextInt();
        int m = scanner.nextInt();
        ArrayList<Integer>[] adj = (ArrayList<Integer>[])new ArrayList[n];
        for (int i = 0; i < n; i++) {
            adj[i] = new ArrayList<Integer>();
        }
        for (int i = 0; i < m; i++) {
            int x, y;
            x = scanner.nextInt();
            y = scanner.nextInt();
            adj[x - 1].add(y - 1);
            adj[y - 1].add(x - 1);
        }
        int x = scanner.nextInt() - 1;
        int y = scanner.nextInt() - 1;
        
        System.out.println(reach(adj, x, y)); // Good job! (Max time used: 0.69/1.50, max memory used: 35098624/536870912.)
    }
}
