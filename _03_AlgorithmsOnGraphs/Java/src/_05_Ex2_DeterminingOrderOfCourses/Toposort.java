package _05_Ex2_DeterminingOrderOfCourses; // must be removed for submission

import java.util.ArrayList;
import java.util.Collections;
import java.util.Scanner;


public class Toposort // DECOMPOSITION OF GRAPHS 2: EXCERCISE 2 - Compute a topological ordering of a given directed acyclic graph (DAG) with n vertices and m edges.
{
	private static ArrayList<Integer> toposort(ArrayList<Integer>[] adj) // we are going to put the elements on a STACK, as we start placing the elements from the end to the start
	{
		boolean[] nodes = new boolean[adj.length]; // the storage for node status has to be initialized with unvisited values
		ArrayList<Integer> order = new ArrayList<Integer>(); // this is for storing the TOPOLOGICAL SORT

		for (int v = 0; v < nodes.length; v++)
		{
			if (!nodes[v])
				dfs(adj, nodes, order, v);
		}

		Collections.reverse(order);
		return order;
	}
				
    private static void dfs(ArrayList<Integer>[] adj, boolean[] nodes, ArrayList<Integer> order, int v) // helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
	{
    	nodes[v] = true;

		for (int w : adj[v])
		{
			if (!nodes[w])
				dfs(adj, nodes, order, w);
		}

		order.add(v); // this is where we normally store the 'post-order' value for topological sort - where we exit the recursion
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
        
        ArrayList<Integer> order = toposort(adj); // Good job! (Max time used: 2.17/3.00, max memory used: 207482880/536870912.)
        
        for (int x : order) {
            System.out.print((x + 1) + " ");
        }
    }
}
