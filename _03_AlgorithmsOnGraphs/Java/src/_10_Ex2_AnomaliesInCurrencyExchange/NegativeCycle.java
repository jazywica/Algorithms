package _10_Ex2_AnomaliesInCurrencyExchange; // must be removed for submission

import java.util.ArrayList;
import java.util.Scanner;


public class NegativeCycle // PATHS IN GRAPHS 2: EXCERCISE 2 - Given an directed graph with possibly negative edge weights and with n vertices and m edges, check whether it contains a cycle of negative weight.
{
    private static int negativeCycle(ArrayList<Integer>[] adj, ArrayList<Integer>[] cost) {
    	int n = adj.length;
		long[] dist = new long[n];
		for (int i = 0; i < n; i++)
			dist[i] = 0; // since there is no start point we may as well initialise everything with 0, as the negative cycle will decrease the values to negative anyway

		for (int iter = 0; iter < n; iter++) { // theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
			boolean is_changed = false; // extra flag to close the procedure quicker if an iteration doesn't change anything
			for (int u = 0; u < n; u++) { // here we start running and optimising all edges in a graph
				for (int i = 0; i < adj[u].size(); i++) { // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
					int v = adj[u].get(i); // v stores the index of the node on the other end of the edge, as usual

					if (dist[v] > dist[u] + cost[u].get(i)) // for visited neighbours we RELAX THE EDGES if possible
					{
						dist[v] = dist[u] + cost[u].get(i);
						is_changed = true;

						if (iter == n - 1) // if there is a cycle, it will be detected on the |V|-1 iteration
							return 1;
					}
				}
			}
			if (is_changed == false) // here we check after each iteration if there has been a change, we return if there wasn't
				return 0;
		}
		return 0; // if no cycles have been found we just return an empty list
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
        
        System.out.println(negativeCycle(adj, cost)); // Good job! (Max time used: 2.16/3.00, max memory used: 63361024/536870912.)
    }
}
