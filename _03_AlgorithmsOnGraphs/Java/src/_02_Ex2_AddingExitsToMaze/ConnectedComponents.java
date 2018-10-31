package _02_Ex2_AddingExitsToMaze; // must be removed for submission

import java.util.ArrayList;
import java.util.Scanner;


public class ConnectedComponents // DECOMPOSITION OF GRAPHS 1: EXCERCISE 2 - Given an undirected graph with 𝑛 vertices and 𝑚 edges, compute the number of connected components
{
	private static int numberOfComponents(ArrayList<Integer>[] adj)
	{
		int result = 0; // this is for counting the number of groups, we only need to return the last value

		boolean[] nodes = new boolean[adj.length];
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = false;

		for (int v = 0; v < nodes.length; v++) // we use 'for' loop to have the INDEXES available as an argument for 'Explore' method
		{
			if (!nodes[v])
			{
				Explore(adj, nodes, v);
				result += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
			}
		}
		return result;
	}


	private static void Explore(ArrayList<Integer>[] adj, boolean[] nodes, int v) // recursive DFS method that looks around the given VERTEX and find all other connected VERTICES (direct and indirect)
	{
		nodes[v] = true;
		for (int w : adj[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbour
		{
			if (!nodes[w])
				Explore(adj, nodes, w);
		}
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
        
        System.out.println(numberOfComponents(adj)); // Good job! (Max time used: 0.57/1.50, max memory used: 33722368/536870912.)
    }
}
