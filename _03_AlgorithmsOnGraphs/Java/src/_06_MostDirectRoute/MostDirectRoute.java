package _06_MostDirectRoute;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Collections;
import java.util.Arrays;
import java.util.Stack;


public class MostDirectRoute
{
	private ArrayList<Integer>[] adj;
	private int[] dist; // stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
	private LinkedList<Integer> q; // queue to store nodes in BFS, note that this is not the fastest way of implementing a queue
	private Integer[] prev; // stores the previous node, which is the node that the current node was discovered from - for the shortest path algorithm
	private int n;

	public MostDirectRoute(ArrayList<Integer>[] inputList) // inside the constructor we commence all the preparatory work
	{
		adj = inputList;
		dist = new int[adj.length];
		prev = new Integer[adj.length];
		q = new LinkedList<Integer>();
		n = adj.length;
	}


	private void BFS(int s) // Breadth-First Search for exploring a graph
	{
		ArrayList<ArrayList<Integer>> layers = new ArrayList<ArrayList<Integer>>(Arrays.asList(new ArrayList<Integer>(s))); // extra item for grouping the nodes into the layers

		for (int i = 0; i < n; i++)
			dist[i] = n; // the biggest possible distance is the number of nodes - 1

		dist[s] = 0; // zero distance to the starter node itself
		q.offer(s); // the starting node is the only one that gets into the queue prior to BFS

		while (!q.isEmpty())
		{
			int u = q.poll();
			for (int v : adj[u])
			{
				if (dist[v] == n)
				{
					q.offer(v);
					dist[v] = dist[u] + 1;
					if (layers.size() < dist[u] + 2) // extra item. the inner lists must be initialized before we can use them
						layers.add(new ArrayList<Integer>());
					layers.get(dist[u] + 1).add(v);
				}
			}
		}

		PrintGroup(layers);
		System.out.println();
		PrintDistance(dist);
	}


	private Stack<Integer> BFS_ShortestPath(int s, int t) // Breadth-First Search for looking for the shortest path in a graph by storing the nodes that the current node was discovered from
	{
		for (int i = 0; i < n; i++)
		{
			dist[i] = n; // the biggest possible distance is the number of nodes - 1
			prev[i] = null;
		}

		dist[s] = 0; // zero distance to the starter node itself
		q.offer(s); // the starting node is the only one that gets into the queue prior to BFS

		while (!q.isEmpty())
		{
			int u = q.poll();
			for (int v : adj[u])
			{
				if (dist[v] == n)
				{
					q.offer(v);
					dist[v] = dist[u] + 1;
					prev[v] = u; // here we store the 'previous node, so we can backtrack it and find the shortest path
				}
			}
		}

		return ReconstructPath(s, t, prev);
	}


	private static Stack<Integer> ReconstructPath(int s, int u, Integer[] prev) // we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
	{
		Stack<Integer> result = new Stack<Integer>();
		while (u != s)
		{
			result.push(u);
			u = (int)prev[u];
		}
		result.push(s); // the loop above stops when reaching the start node, so if we want, we can include it here
		
		Collections.reverse(result);
		return result;
	}
	
	
	private static void PrintGroup(ArrayList<ArrayList<Integer>> group) // we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
	{
		for (int i = 0; i < group.size(); i++)
		{
			System.out.printf("to layer %1$s belong nodes: ", i);
			for (int j : group.get(i))
				System.out.print(j + " ");
			System.out.println();
		}
	}


	private static void PrintDistance(int[] distance) // we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
	{
		for (int i = 0; i < distance.length; i++)
			System.out.printf("the node %1$s is at layer: %2$s" + "\r\n", i, distance[i]);
	}


	public static void main(String[] args) 
	{
		ArrayList<Integer>[] adjList = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>(Arrays.asList(2, 3)),
			new ArrayList<Integer>(Arrays.asList(1, 4, 5)),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(Arrays.asList(7)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>(Arrays.asList(5))
		};

		MostDirectRoute bfs_1 = new MostDirectRoute(adjList);
		bfs_1.BFS(0);
		System.out.println();

		MostDirectRoute bfs_2 = new MostDirectRoute(adjList);
		Stack<Integer> shortestPath = bfs_2.BFS_ShortestPath(0, 7);
		for (int x : shortestPath)
			System.out.print(x + " ");
	}
}
