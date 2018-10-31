package _05_Ex1_ConsistencyOfCurriculum; // must be removed for submission

import java.util.ArrayList;
import java.util.Scanner;


public class Acyclicity // DECOMPOSITION OF GRAPHS 2: EXCERCISE 1 - Check whether a given directed graph with 𝑛 vertices and 𝑚 edges contains a cycle.
{
	private static boolean cyclic = false; // flags if we have found a a cycle in a graph
	
	private static int acyclic(ArrayList<Integer>[] adj)
	{
		boolean[] nodes = new boolean[adj.length]; // the storage for node status has to be initialised with unvisited values
		int cycleValue; // field for storing a potential cycle inside a group

		for (int v = 0; v < nodes.length; v++)
		{
			cycleValue = v; // here we store the first member of the group we are visiting
			if (!nodes[v])
				explore(adj, nodes, cycleValue, v);
		}

		if (cyclic == true)
			return 1;
		else
			return 0;
	}
	
	
	private static void explore(ArrayList<Integer>[] adj, boolean[] nodes, int cycleValue, int v) // helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
	{
		if (adj[v].contains(cycleValue)) { cyclic = true; return; } // checks if the initial node value reappears and returns immediately

		nodes[v] = true;
		for (int w : adj[v])
		{
			if (!nodes[w])
				explore(adj, nodes, cycleValue, w);
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
        }
        
        System.out.println(acyclic(adj)); // Good job! (Max time used: 0.58/1.50, max memory used: 35131392/536870912.)
    }
}
