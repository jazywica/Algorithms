package _10_Ex3_ExchangingMoneyOptimally; // must be removed for submission

import java.util.Arrays;
import java.util.HashSet;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Scanner;


public class ShortestPaths // PATHS IN GRAPHS 2: EXCERCISE 3 - Given an directed graph with possibly negative edge weights and with n vertices and m edges as well as its vertex s, compute the length of shortest paths from s to all other vertices of the graph.
{
    private static void shortestPaths(ArrayList<Integer>[] adj, ArrayList<Integer>[] cost, int s, long[] distance, int[] reachable, int[] shortest) {
    	int n = adj.length;
		HashSet<Integer> a_nodes = new HashSet<Integer>(); // stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set
		int[] path = new int[n];
		boolean[] visited = new boolean[n];
		for (int i = 0; i < n; i++)
		{
			path[i] = -1;
			visited[i] = false;
		}
		ArrayList<Integer> current = new ArrayList<Integer>(); // list for excluding non-reachable nodes
		LinkedList<Integer> q = new LinkedList<Integer>(Arrays.asList(s));

		// 1. Checking connectivity with BFS
		while (!q.isEmpty()) // Breadth-First Search for looking for all reachable bodes
		{
			int u = q.poll();
			reachable[u] = 1; // all reachable nodes are going to be enqueued sooner all later, including the first element
			current.add(u);
			for (int v : adj[u])
			{
				if (visited[v] == false)
				{
					q.offer(v);
					visited[v] = true;
				}
			}
		}

		// 2. Detecting negative cycles - there must be 'n' iterations over all reachable nodes
		distance[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
		for (int iter = 0; iter < n; iter++) {
			boolean is_changed = false; // extra flag to close the procedure quicker, if an iteration doesn't change anything
			for (int u : current) {
				for (int i = 0; i < adj[u].size(); i++) {
					int v = adj[u].get(i); // v stores the index of the node on the other end of the edge, as usual
					if (distance[u] == Long.MAX_VALUE) // if the starting node u hasn't been discovered yet, we just move on
						continue;

					else if (distance[v] > distance[u] + cost[u].get(i)) // for visited neighbours we RELAX THE EDGES if possible
					{
						distance[v] = distance[u] + cost[u].get(i);
						path[v] = u; // here we store the fastest path, we can't do it in the BFS above, as we are going to backtrack the cycle, as it was discovered
						is_changed = true;

						if (iter == n - 1)
						{
							a_nodes.add(u);
							for (int k : adj[u])
								a_nodes.add(k);
						}
					}
				}
			}
			if (is_changed == false) // here we check after each iteration if there has been a change, we exit function if it hasn't
				return;
		}

		// 3. Track all the cycles from the discovered nodes - we are going to remove all nodes present in the current cycle from 'a_nodes' on the fly
		if (!a_nodes.isEmpty())
		{
			HashSet<Integer> result = new HashSet<Integer>();

			while (!a_nodes.isEmpty())
			{
				int u = a_nodes.iterator().next(); // this is the way to get the first element form a set in java
				result.addAll(ReconstructCycle(a_nodes, path, u));
				a_nodes.removeAll(result); // here we take out all the nodes in the current cycle form the main set, just so we don't repeat the reconstruction more than once for each cycle
			}

			if (result.contains(s)) // if the start node is in the cycle, then we have to also check the path from the main node to the cycle
			{
				for (int i = 0; i < n; i++)
					 shortest[i] = 0;
			}
			else
			{
				for (int cyc : result)
					shortest[cyc] = 0;
			}
		}
	}

	private static HashSet<Integer> ReconstructCycle(HashSet<Integer> a_nodes, int[] path, int x) // helper function that gathers the cycle and returns it
	{
		int u = x; // this is to store the initial value that should end the cycle search
		int count = 0;
		do
		{
			if (count > path.length) // extra condition when we can't get to where we started from by backtracking
				break;
			count++;
			a_nodes.add(u);
			u = path[u];
		} while (u != x);

		return a_nodes;
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
        int s = scanner.nextInt() - 1;
        long distance[] = new long[n];
        int reachable[] = new int[n];
        int shortest[] = new int[n];
        for (int i = 0; i < n; i++) {
            distance[i] = Long.MAX_VALUE;
            reachable[i] = 0;
            shortest[i] = 1;
        }
        
        shortestPaths(adj, cost, s, distance, reachable, shortest); // Good job! (Max time used: 2.53/3.00, max memory used: 67809280/536870912.)
        
        for (int i = 0; i < n; i++) {
            if (reachable[i] == 0) {
                System.out.println('*');
            } else if (shortest[i] == 0) {
                System.out.println('-');
            } else {
                System.out.println(distance[i]);
            }
        }
    }
}
