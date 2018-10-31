package _07_Ex1_MinNumberOfFlightSegments; // must be removed for submission

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Queue;
import java.util.Scanner;


public class BFS // PATHS IN GRAPHS 1: EXCERCISE 1 - Given an undirected graph with n vertices and m edges and two vertices u and v, compute the length of a shortest path between u and v
{
	private static int distance(ArrayList<Integer>[] adj, int s, int t)
	{
		int n = adj.length;
		int[] dist = new int[n]; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
		Queue<Integer> q = new LinkedList<Integer>(); // queue to store nodes in BFS

		for (int i = 0; i < n; i++)
			dist[i] = -1; // initialize the distance list with -1 as this is the expected 'no path' value

		dist[s] = 0; // zero distance to the starter node itself
		q.offer(s);

		while (!q.isEmpty())
		{
			int u = q.poll();
			for (int v : adj[u])
			{
				if (dist[v] == -1)
				{
					q.offer(v);
					dist[v] = dist[u] + 1;
				}
			}
		}

		return dist[t]; // since distance 'dist' was initiated with -1, then it is enough to return the appropriate value from the array
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
        
        System.out.println(distance(adj, x, y)); // Good job! (Max time used: 1.74/3.00, max memory used: 199524352/536870912.)
    }
}
