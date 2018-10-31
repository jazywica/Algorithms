package _10_Ex1_MinimumCostOfFlight;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.Scanner;
import java.util.TreeSet;

public class Dijkstra_PriorityQueue {
	static private class CustomComparer implements Comparator<long[]> // custom comparer for sorting elements in SortedSet by distance
    {
    	public final int compare(long[] left, long[] right)
    	{
    		int comp = (new Long(left[1])).compareTo(right[1]);
    		if (comp == 0)
    			return (new Long(left[0])).compareTo(right[0]); // this will prevent from deleting all nodes with the same initial value

    		return comp;
    	}
    }
	
	private static long distance(ArrayList<Integer>[] adj, ArrayList<Integer>[] cost, int s, int t) // Dijkstra algorithm implemented with two simple arrays as a queue data structure
	{
		long[] dist = new long[adj.length];
		TreeSet<long[]> priorityQueue = new TreeSet<long[]>(new CustomComparer()); // this queue is just to keep track of the minimum distances, we still need arrays as we can't address sets directly

		for (int i = 0; i < adj.length; i++)
		{
			dist[i] = Integer.MAX_VALUE;
			if (i == s)
				priorityQueue.add(new long[] {i, 0});
			else
				priorityQueue.add(new long[] {i, Integer.MAX_VALUE});
		}

		dist[s] = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

		while (!priorityQueue.isEmpty())
		{
			long[] U = priorityQueue.first(); // we need a handle for this element to update it later on
			priorityQueue.remove(U);
			int u = (int)U[0];

			for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
			{
				int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

				if (dist[v] > dist[u] + cost[u].get(i)) // for direct neighbours we RELAX THE EDGES if possible
				{
					long oldDist = dist[v];
					dist[v] = dist[u] + cost[u].get(i);

					priorityQueue.remove(new long[]{v, oldDist}); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
					priorityQueue.add(new long[] {v, dist[u] + cost[u].get(i)});
				}
			}
		
		}

		if (dist[t] == Integer.MAX_VALUE)
			return -1;
		else
			return dist[t];
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
        
        System.out.println(distance(adj, cost, x, y)); // Good job! (Max time used: 2.73/3.00, max memory used: 251482112/536870912.)
    }

}
