package _11_MinimumSpanningTrees;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.TreeSet;

public class SpanningTrees
{
	private ArrayList<Integer>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
	private ArrayList<Integer>[] cost; // list for storing the initial uncorrected costs
	private int distance = 0; // total distance of the minimum path

	private HashSet<HashSet<String>> nodes = new HashSet<HashSet<String>>(); // Containers for Kruskal
	@SuppressWarnings("serial")
	private Map<Integer, String> labels = new HashMap<Integer, String>() {{ put(0, "A"); put(1, "B"); put(2, "C"); put(3, "D"); put(4, "E"); put(5, "F");}}; // just to present nodes the same way as in lectures
	private HashSet<String> X = new HashSet<String>(); // final output showing the picked edges

	private long[] dist; // containers for Prim
	private TreeSet<long[]> priorityQueue = new TreeSet<long[]>(new CustomComparer()); // this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly
	
	private SpanningTrees(ArrayList<Integer>[] adjList, ArrayList<Integer>[] costList)
	{
		adj = adjList;
		cost = costList;
		dist = new long[adj.length];
		for (int i = 0; i < adj.length; i++)
		{
			nodes.add(new HashSet<String>(Arrays.asList(labels.get(i)))); // initializing the initial sets for Kruskal as : A, B, C, D, E, F
			dist[i] = Integer.MAX_VALUE; // initializing distances for Prim
		}
	}

	
	// I. Kruskal algorithm - doesn't require a start point, as it sorts and picks the shortest edges independently from vertices
	private int Kruskal()
	{
		ArrayList<int[]> edges = MakeEdges();
		Collections.sort(edges, (a, b) -> Double.compare(a[0], b[0])); // Kruskal algorithm sorts all edges and then processes them independently

		for (int idx = 0; idx < edges.size(); idx++)
		{
			HashSet<String> u = Find(labels.get(edges.get(idx)[1]));
			HashSet<String> v = Find(labels.get(edges.get(idx)[2]));
			
			if (!u.equals(v)) // if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
			{
				nodes.remove(u);
				nodes.remove(v);
				
				HashSet<String> combined = new HashSet<String>(u);
				combined.addAll(v);
				nodes.add(combined);
				X.add(labels.get(edges.get(idx)[1]) + labels.get(edges.get(idx)[2]));
				distance += edges.get(idx)[0];
			}
		}

		DisplayValues();

		return distance;
	}


	private HashSet<String> Find(String seq)
	{
		for (HashSet<String> item : nodes)
		{
			if (item.contains(seq))
				return item;
		}
		return new HashSet<String>();
	}


	private ArrayList<int[]> MakeEdges() // helper function to convert adjacency list into single edges
	{
		ArrayList<int[]> result = new ArrayList<int[]>();
		HashSet<String> duplicates = new HashSet<String>();

		for (int i = 0; i < adj.length; i++) {
			for (int j = 0; j < adj[i].size(); j++) {
				if (!duplicates.contains(String.valueOf(i) + adj[i].get(j).toString())) // here we check if we are not taking any duplicates, we only need an edge once
				{
					duplicates.add(adj[i].get(j).toString() + String.valueOf(i));
					result.add(new int[3]);
					int[] current = result.get(result.size() - 1);
					current[0] = cost[i].get(j);
					current[1] = i;
					current[2] = adj[i].get(j);
				}
			}
		}
		return result;
	}

	private void DisplayValues() // testing function that prints all node properties
	{
		System.out.println("the following edges were picked as the shortest (only true for Kruskal):");
		for (String item : X)
			System.out.print(item + " ");
		System.out.println();
	}
	
	
	// 2. Prim's algorithm, implemented with a Priority Queue (modified SortedSet) - it requires a start point and works the same way as Dijkstra
	private long Prim(int s)
	{
		long result = 0;
		dist[s] = 0;

		for (int i = 0; i < adj.length; i++)
		{
			if (i == s)
				priorityQueue.add(new long[] {(int)i, 0}); // since we can't address the element of a set, we can put it in while populating the collection
			else
				priorityQueue.add(new long[] {(int)i, Integer.MAX_VALUE});
		}

		
		while (!priorityQueue.isEmpty())
		{
			long[] U = priorityQueue.first(); // we need a handle for this element to update it later on
			priorityQueue.remove(U);
			int u = (int)U[0];

			for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
			{
				int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

				if (dist[v] > cost[u].get(i) && priorityQueue.contains(new long[] {v, dist[v]})) // since we are only comparing single distances, we can't look back, so we only take into account
				{
					long oldDist = dist[v]; // for direct neighbours we RELAX THE EDGES if possible
					dist[v] = cost[u].get(i);

					priorityQueue.remove(new long[] {(int)v, oldDist}); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
					priorityQueue.add(new long[] {(int)v, cost[u].get(i)});
				}
			}
			result += dist[u];
		}

		return result;
	}
	

	public static void main(String[] args) {
		ArrayList<Integer>[] adj = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1, 3, 4)),
			new ArrayList<Integer>(Arrays.asList(0, 2, 4, 5)),
			new ArrayList<Integer>(Arrays.asList(1, 5)),
			new ArrayList<Integer>(Arrays.asList(0, 4)),
			new ArrayList<Integer>(Arrays.asList(0, 1, 3, 5)),
			new ArrayList<Integer>(Arrays.asList(1, 2, 4))
		};
		ArrayList<Integer>[] adj_cost = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(4, 2, 1)),
			new ArrayList<Integer>(Arrays.asList(4, 8, 5, 6)),
			new ArrayList<Integer>(Arrays.asList(8, 1)),
			new ArrayList<Integer>(Arrays.asList(2, 3)),
			new ArrayList<Integer>(Arrays.asList(1, 5, 3, 9)),
			new ArrayList<Integer>(Arrays.asList(6, 1, 9))
		};

		System.out.println("\nKruskal's Algorithm:");
		SpanningTrees graph_kruskal = new SpanningTrees(adj, adj_cost);
		int route_kruskal = graph_kruskal.Kruskal();
		System.out.println(route_kruskal);

		System.out.println("\nPrim's Algorithm:");
		SpanningTrees graph_prim = new SpanningTrees(adj, adj_cost);
		long route_prim = graph_prim.Prim(2); // we start from the same node is the same as node C in the picture
		System.out.println(route_prim);
	}
}
