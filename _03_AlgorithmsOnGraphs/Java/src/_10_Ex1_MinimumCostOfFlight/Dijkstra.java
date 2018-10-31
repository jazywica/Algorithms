package _10_Ex1_MinimumCostOfFlight; // must be removed for submission

import java.util.ArrayList;
import java.util.Scanner;


public class Dijkstra // PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v).
{
	private static long distance(ArrayList<Integer>[] adj, ArrayList<Integer>[] cost, int s, int t) // Dijkstra algorithm implemented with two simple arrays as a queue data structure
	{
		long[] dist = new long[adj.length]; // storage for the graph with values that are on purpose bigger than allowed
		boolean[] known = new boolean[adj.length]; // used 'Boolean' class here, primitive boolean is not supported in streams (line 24)
		for (int i = 0; i < adj.length; i++)
		{
			dist[i] = Integer.MAX_VALUE;
			known[i] = false; // we use max. integer values while 'dist' was declared as long
		}
		dist[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

		while (true)
		{
			int u = extractMin(dist, known);
			//boolean isAllTrue = Arrays.stream(known).allMatch(a -> a == true); // streaming in this case is slower
			if (u == -1 || isAllTrue(known)) // first condition checks for non-connected nodes and the second if all nodes have been verified
				break;

			for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
			{
				int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

				if (dist[v] > dist[u] + cost[u].get(i)) // for direct neighbours we RELAX THE EDGES if possible
					dist[v] = dist[u] + cost[u].get(i);
			}
			known[u] = true; // this is how we can CHANGE THE PRIORITY
		}

		if (dist[t] == Integer.MAX_VALUE)
			return -1;
		else
			return dist[t];
	}

	private static boolean isAllTrue(boolean[] known) {
		for (int i = 0; i < known.length; i++) {
			  if (!known[i]) return false;
			}
			return true;
	}

	private static int extractMin(long[] dist, boolean[] known) // helper function that returns the element u index which has the smallest distance
	{
		long smallest = Integer.MAX_VALUE;
		int index = -1;
		for (int i = 0; i < dist.length; i++)
		{
			if (dist[i] < smallest && known[i] == false)
			{
				smallest = dist[i];
				index = i;
			}
		}
		return index; // we always compare the distances, but we return the index
	}


	public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int n = scanner.nextInt();
        int m = scanner.nextInt();
        ArrayList<Integer>[] adj = (ArrayList<Integer>[])new ArrayList[n];
        ArrayList<Integer>[] cost = (ArrayList<Integer>[])new ArrayList[n];
        for (int i = 0; i < n; i++) {
            adj[i] = new ArrayList<Integer>();
            cost[i] = new ArrayList<Integer>();
        }
        for (int i = 0; i < m; i++) {
            int x, y, w;
            x = scanner.nextInt();
            y = scanner.nextInt();
            w = scanner.nextInt();
            adj[x - 1].add(y - 1);
            cost[x - 1].add(w);
        }
        int x = scanner.nextInt() - 1;
        int y = scanner.nextInt() - 1;
        
        System.out.println(distance(adj, cost, x, y)); // Good job! (Max time used: 1.72/3.00, max memory used: 247762944/536870912.)
    }
}
