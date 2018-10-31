package _05_Ex3_StronglyConnectedComponents; // must be removed for submission

import java.util.ArrayList;
import java.util.Collections;
import java.util.Scanner;


public class StronglyConnected // DECOMPOSITION OF GRAPHS 2: EXCERCISE 3 - Compute the number of strongly connected components of a given directed graph with n vertices and m edges.
{
	private static int numberOfStronglyConnectedComponents(ArrayList<Integer>[] adj) // we have to run 'Explore' procedures on the postorder values, so each time we go through a new SCC we start from a SINK VERTEX
	{
		boolean[] nodes = new boolean[adj.length]; // the storage for node status has to be initialized with unvisited values
		boolean[] nodes_rev = new boolean[adj.length];
		ArrayList<Integer> postIndex = new ArrayList<Integer>(); // stack for storing the list of reversed 'post' indexes

		ArrayList<Integer>[] adj_rev = new ArrayList[adj.length]; // Initialising the reversed list
		for (int i = 0; i < adj.length; i++)
			adj_rev[i] = new ArrayList<Integer>();

		for (int i = 0; i < adj.length; i++) // reversing the order of the graph edges for DFS
			for (int element : adj[i])
				adj_rev[element].add(i);

		for (int v = 0; v < nodes_rev.length; v++) // 'DFS' procedure scans the REVERSED graph for SINK VERTEXES in SCCs
			if (!nodes_rev[v])
				explore(adj_rev, nodes_rev, postIndex, v, true);

		int result = 0; // we are going to use the 'counter' at this point to group the SCCs
		Collections.reverse(postIndex);
		for (int v : postIndex) // here we go by the whole list of recorded post-visit values that we obtained from DFS and explore the real graph
		{
			if (!nodes[v])
			{
				explore(adj, nodes, postIndex, v, false);
				result += 1; // when we finished with our recursive calls and we are on the way out, we increment the SCC counter
			}
		}

		return result;
	}
    
	
    private static void explore(ArrayList<Integer>[] adj, boolean[] nodes, ArrayList<Integer> postIndex, int v, boolean storeIndex) // multipurpose 'Explode' function for both the standard and reversed graph
	{
		nodes[v] = true; // before we do anything we first mark the NODE we are at as 'visited'

		for (int w : adj[v])
		{
			if (!nodes[w])
				explore(adj, nodes, postIndex, w, storeIndex);
		}

		if (storeIndex)
			postIndex.add(v); // we store the post-order values as we go out of RECURSION only when we scan the REVERSED GRAPH
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
        
        System.out.println(numberOfStronglyConnectedComponents(adj)); // Good job! (Max time used: 1.09/1.50, max memory used: 57974784/536870912.)
    }
}
