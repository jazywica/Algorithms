package _09_CurrencyExchange;

import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;
import java.util.ArrayList;
import java.util.LinkedList;

//ARBITRARY IN CURRENCY EXHCANGE
//Conversions use multiplications of pair rates: x*y = 2^(log2(x)) * 2^(log2(y)) = 2^(log2(x) + log2(y)) but we can use a simple log property and turn it to a sum of numbers instead
//to maximize result 4 * 1 * 0.5 = 2 = 2^1 we use a sum of logarithms: log2(4)+log2(1)+log2(0.5) = 2 + 0 + -1 = 1 which is the exponent of the result we got from numbers
//to minimize a sum we can do the following: min = - SUM(log(xi)) or better: SUM(-log(xi)), so we have to convert each conversion rate to: -log2(x)

//Assume that a cycle ci -> cj -> ck -> ci has negative weight. This means that -(log cij + log cjk + log cki) < 0 and hence log cij + log cjk + log cki > 0.
//This, in turn, means that: rij rjk rki = 2log cij 2log cjk2log cki = 2log cij+log cjk+log cki > 1 - this means that we take out more than we put in.
//Negative cycles have a (logarithmic) negative sum, which is easy to detect, as it endlessly decreases the numbers within it. 


public class Bellman_Ford {
	private ArrayList<Integer>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
	private ArrayList<Integer>[] cost; // list for storing the initial uncorrected costs
	private Vertex[] nodes; // array for storing the graph
	private LinkedList<Integer> q; // queue to store nodes in BFS
	private HashSet<Integer> A_nodes; // storage for the Arbitrage, all the nodes affected by the cycle
	private HashSet<Integer> cycle; // stores only the cycle

	private Bellman_Ford(ArrayList<Integer>[] adjList, ArrayList<Integer>[] costList)
	{
		adj = adjList;
		cost = costList;
		nodes = new Vertex[adjList.length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = new Vertex();
	}


	private ArrayList<Integer> BellmanFord(int s, int t) // based on iterative correction of distances from origin until there is nothing to update anymore
	{
		nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

		for (int iter = 0; iter < adj.length - 1; iter++) // theoretically it is enough to run |V| - 1 iterations on all edges to be sure that the full optimization of distances has been made
		{
			for (int u = 0; u < adj.length; u++) // here we start running and optimizing all edges in a graph
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
					}
				}
			}
		}

		DisplayValues();

		return ReconstructPath(s, t);
	}

	private ArrayList<Integer> ReconstructPath(int s, int u)
	{
		ArrayList<Integer> result = new ArrayList<Integer>();
		while (u != s)
		{
			result.add(u);
			u = nodes[u].path;
		}
		result.add(s); // the loop above stops when reaching the start node, so if we want, we can include it here
		Collections.reverse(result);
		return result;
	}


	private ArrayList<Integer> NegativeCycles(int s, int t) // based on iterative correction of distances from origin to detect if there are negative cycles in the graph
	{
		nodes[s].dist = 0; // the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
		int n = adj.length;
		ArrayList<Integer> result = new ArrayList<Integer>();
		A_nodes = new HashSet<Integer>(); // stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set
		cycle = new HashSet<Integer>(); // stores only the cycle


		// 1. Bellman-Ford that collects all the affected by the cycle nodes
		for (int iter = 0; iter < n; iter++) // theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
		{
			for (int u = 0; u < n; u++) // here we start running and optimizing all edges in a graph
			{
				for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
				{
					int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

					if (nodes[u].dist == Integer.MAX_VALUE) // if the starting node u hasn't been discovered yet, we just move on
						continue;
					else if (nodes[v].dist > nodes[u].dist + cost[u].get(i)) // for visited neighbours we RELAX THE EDGES if possible
					{
						nodes[v].dist = nodes[u].dist + cost[u].get(i);
						nodes[v].path = u; // here we store the fastest path

						if (iter == n - 1)
						{
							cycle.add(u); // this should pick up the cycle itself
							A_nodes.add(u);
							for (int k : adj[u]) // this should pick up all affected nodes
								A_nodes.add(k);
						}
					}
				}
			}
		}
		
		// 2. BFS for checking if the end currency is reachable from the cycle
		q = new LinkedList<Integer>();
		for(int x : cycle)
			q.add(x); // here we convert the SET into a QUEUE for BFS

		while (!q.isEmpty())
		{
			int u = q.poll();
			for (int v : adj[u])
			{
				if (nodes[v].known == false)
				{
					q.offer(v);
					nodes[v].known = true;
					nodes[v].path_bfs = (int)u; // here we store the shortest path for comparison
				}
			}
		}

		result = ReconstructArbitrage(t);


		// 3. BFS for checking if the cycle is reachable from the start
		q = new LinkedList<Integer>(Arrays.asList(s));
		for (int i = 0; i < nodes.length; i++)
			nodes[i].known = false;

		while (!q.isEmpty())
		{
			int u = q.poll();
			if (u == result.get(result.size() - 1))
			{
				System.out.println("Yes, there is a path with an infinite cycle !!!\n"); // actually it is enough to get to the first element of the cycle
				break;
			}

			for (int v : adj[u])
			{
				if (nodes[v].known == false)
				{
					q.offer(v);
					nodes[v].known = true;
					nodes[v].path_bfs = (int)u; // here we store the shortest path for comparison
				}
			}
		}

		System.out.println("Values after BFS:");
		DisplayValues();

		return result;
	}
	
	private ArrayList<Integer> ReconstructArbitrage(int u)
	{
		ArrayList<Integer> result = new ArrayList<Integer>();
		while (u != -1)
		{
			if (cycle.contains(u))
			{
				result.add(u);
				Collections.reverse(result);
				return result;
			}
			result.add(u);
			u = nodes[u].path_bfs;
		}
		return new ArrayList<Integer>();
	}
		
	void DisplayValues() // testing function that prints shortest paths and compares fastest and shortest paths
	{
		for (int i = 0; i < nodes.length; i++)
			System.out.printf("node index: %1$s, distance to the source: %2$s, fastest path: %3$s, shortest path: %4$s" + "\r\n", i, nodes[i].dist, nodes[i].path, nodes[i].path_bfs);
	}

	
	static void ExchangeExample(double RUR_USD, double RUR_EUR, double EUR_USD)
	{
		System.out.printf("Single conversion to USD: %1$s, conversion via EUR: %2$.4f" + "\r\n", RUR_USD, RUR_EUR * EUR_USD); // this means that it is better to buy EUR first and then change it to USD
		double R_U = -Math.log(RUR_USD) / Math.log(2);
		double R_E = -Math.log(RUR_EUR) / Math.log(2);
		double E_U = -Math.log(EUR_USD) / Math.log(2);
		System.out.printf("Logarithmic values for single rates, conversion to USD: %1$.4f, to EUR: %2$.4f, to EUR: %3$.4f" + "\r\n", R_U, R_E, E_U);
		System.out.printf("Logarithmic sums of two alternative exchanges, direct conversion to USD: %1$.4f, to USD via EUR: %2$.4f" + "\r\n", R_U, R_E + E_U); // now we can add the sum
	}
	
	public static void main(String[] args) {
		//Currency Exchange Example: We can either directly exchange RUR to USD with 0.015 rate or exchange it via EUR with rates 0.013 and 1.16
		ExchangeExample(0.015, 0.013, 1.16); // testing numbers from the video
		System.out.println();


		// To solve the exchange problem we have to use the naive solution that scans all edges separately in order to handle negative edges (which we need to 
		//Dijkstra algorithm doesn't work for negative edges as it relies on a fact that the shortest path goes through vertices that are closer to the start and can not predict for negative edges
		ArrayList<Integer>[] adjDag = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1, 2)),
			new ArrayList<Integer>(Arrays.asList(2, 3)),
			new ArrayList<Integer>(Arrays.asList(3, 4)),
			new ArrayList<Integer>(Arrays.asList(4)),
			new ArrayList<Integer>()
		};
		ArrayList<Integer>[] adjDag_cost = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(4, 3)),
			new ArrayList<Integer>(Arrays.asList(-2, 4)),
			new ArrayList<Integer>(Arrays.asList(-3, 1)),
			new ArrayList<Integer>(Arrays.asList(2)),
			new ArrayList<Integer>()
		};

		Bellman_Ford graph = new Bellman_Ford(adjDag, adjDag_cost);
		ArrayList<Integer> route = graph.BellmanFord(0, 3);
		for (int x : route)
			System.out.print(x + " ");
		System.out.println();


		System.out.println("\nNegativeCycles:");
		ArrayList<Integer>[] adjCyclic = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1, 2)),
			new ArrayList<Integer>(Arrays.asList(2, 3, 7)),
			new ArrayList<Integer>(Arrays.asList(3, 4)),
			new ArrayList<Integer>(Arrays.asList(6)),
			new ArrayList<Integer>(Arrays.asList(3, 5)),
			new ArrayList<Integer>(Arrays.asList(2, 8)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(3)),
			new ArrayList<Integer>()
		};
		ArrayList<Integer>[] adjCyclic_cost = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(4, 3)),
			new ArrayList<Integer>(Arrays.asList(-2, 4, 4)),
			new ArrayList<Integer>(Arrays.asList(-3, 1)),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(Arrays.asList(2, 2)),
			new ArrayList<Integer>(Arrays.asList(-5, 1)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>()
		};

		Bellman_Ford graphCyclic = new Bellman_Ford(adjCyclic, adjCyclic_cost);
		ArrayList<Integer> routeCyclic = graphCyclic.NegativeCycles(0, 6);
		for (int x : routeCyclic)
			System.out.print(x + " ");

		System.out.println();
	}
}
