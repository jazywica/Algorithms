package _08_FastestRoute;

import java.util.Arrays;
import java.util.Collections;
import java.util.ArrayList;
import java.util.Stack;

// EDGE RELAXATION:
// Observation: any sub-path of an optimal path is also optimal, which takes us to the following property: If S-> ..-> u-> t is the shortest path from S to then d(S, t) = d(S, u) + w(u, t)
// dist[v] will be an upper bound on the actual distance from S to v, unlike in BFS, this value will most likely be updated many times during the procedure
// the EDGE RELAXATION for an edge (u, v) checks whether going from S to v through u improves the current value of dist, it pertains to the last edge before t at a given stage


public class FastestRoute
{
	private ArrayList<Integer>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
	private ArrayList<Integer>[] cost; // list for storing the initial uncorrected costs
	private Vertex[] nodes; // array for storing the graph

	private FastestRoute(ArrayList<Integer>[] adjList, ArrayList<Integer>[] costList)
	{
		adj = adjList;
		cost = costList;
		nodes = new Vertex[adjList.length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = new Vertex();
	}

	private Stack<Long> NaiveFastestRoute(int s, int t) // based on iterative correction of distances from origin until there is nothing to update anymore
	{
		nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
		boolean is_changed = false;

		do{
			is_changed = false;
			for (int u = 0; u < adj.length; u++)
			{
				for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
				{
					int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

					if (nodes[u].dist == Integer.MAX_VALUE) // if the starting node u hasn't been discovered yet, we just move on
						continue;
					else if (nodes[v].dist > nodes[u].dist + cost[u].get(i)) // for visited neighbours we RELAX THE EDGES if possible
					{
						nodes[v].dist = nodes[u].dist + cost[u].get(i);
						nodes[v].path = u;
						is_changed = true; // this variable will become true each time we change something
					}
				}
			}
		} while (is_changed); // the 'do' loop stops as soon as there is an iteration with no further updates

		DisplayValues();

		return ReconstructPath(s, t, nodes);
	}

	private Stack<Long> Dijkstra(int s, int t) // based on a fact that the smallest distance to all available nodes is enough to move onto next node, the other nodes can only be bigger
	{
		nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

		while (true)
		{
			int u = extractMin(nodes);
			if (u == -1 || Arrays.asList(nodes).stream().allMatch(x -> x.known == true))
				break;

			for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
			{
				int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

				if (nodes[v].dist > nodes[u].dist + cost[u].get(i)) // for direct neighbours we RELAX THE EDGES if possible
				{
					nodes[v].dist = nodes[u].dist + cost[u].get(i);
					nodes[v].path = u;
				}
			}
			nodes[u].known = true; // this is how we can CHANGE THE PRIORITY
		}

		DisplayValues();

		return ReconstructPath(s, t, nodes);
	}
	
	private int extractMin(Vertex[] nodes) // helper function that returns the element u index which has the smallest distance
	{
		long smallest = Integer.MAX_VALUE;
		int index = -1;
		for (int i = 0; i < nodes.length; i++)
		{
			if (nodes[i].dist < smallest && nodes[i].known == false)
			{
				smallest = nodes[i].dist;
				index = i;
			}
		}
		return index; // we always compare the distances, but we return the index
	}

	private Stack<Long> ReconstructPath(int s, int u, Vertex[] nodes) // we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
	{
		Stack<Long> result = new Stack<Long>();
		while (u != s)
		{
			result.push((long) u);
			u = nodes[u].path;
		}
		result.push((long) s); // the loop above stops when reaching the start node, so if we want, we can include it here
		Collections.reverse(result);
		return result;
	}

	private void DisplayValues() // testing function that prints all node properties
	{
		for (int i = 0; i < nodes.length; i++)
			System.out.printf("node index: %1$s, distance to the source: %2$s, received from: %3$s" + "\r\n", i, nodes[i].dist, nodes[i].path);
	}


	public static void main(String[] args) 
	{
		ArrayList<Integer>[] adj_small = new ArrayList[]{
			new ArrayList<Integer>(Arrays.asList(1, 2)),
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(0))
		};
		ArrayList<Integer>[] cost_small = new ArrayList[]{
			new ArrayList<Integer>(Arrays.asList(1, 5)),
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(2))
		};

		FastestRoute graph_small = new FastestRoute(adj_small, cost_small);
		Stack<Long> route_small = graph_small.NaiveFastestRoute(0, 2);
		for (Long x : route_small)
			System.out.print(x + " ");
		System.out.println();

		FastestRoute graph_small_2 = new FastestRoute(adj_small, cost_small);
		Stack<Long> route_small_2 = graph_small_2.Dijkstra(0, 2);
		for (Long x : route_small_2)
			System.out.print(x + " ");
		System.out.println();


		ArrayList<Integer>[] adj_big = new ArrayList[]{
			new ArrayList<Integer>(Arrays.asList(1, 2)),
			new ArrayList<Integer>(Arrays.asList(2, 3)),
			new ArrayList<Integer>(Arrays.asList(4, 5)),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>()
		};
		ArrayList<Integer>[] cost_big = new ArrayList[]{
			new ArrayList<Integer>(Arrays.asList(9, 5)),
			new ArrayList<Integer>(Arrays.asList(2, 2)),
			new ArrayList<Integer>(Arrays.asList(9, 8)),
			new ArrayList<Integer>(Arrays.asList(5)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(Arrays.asList(4)),
			new ArrayList<Integer>()
		};

		FastestRoute graph_big = new FastestRoute(adj_big, cost_big);
		Stack<Long> route_big = graph_big.NaiveFastestRoute(0, 5);
		for (Long x : route_big)
			System.out.print(x + " ");
		System.out.println();

		FastestRoute graph_big_2 = new FastestRoute(adj_big, cost_big);
		Stack<Long> route_big_2 = graph_big_2.Dijkstra(0, 5);
		for (Long x : route_big_2)
			System.out.print(x + " ");
	}
}

