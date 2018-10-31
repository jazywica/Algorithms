package _01_UndirectedGraphs;

import java.util.ArrayList;
import java.util.Arrays;


public class UGraph // class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests
{
	private ArrayList<Integer>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
	private Vertex[] nodes; // array for storing the graph
	private int counter; // field for the 'group' function
	private int clock = 1; // field for the pre and post visiting functions
	private ArrayList<Integer> order = new ArrayList<Integer>(); // EXTRA ITEM for storing the order that NODES were looked at. It has to be instantiated here as it is an object and we don't put it in the constructor

	public UGraph(ArrayList<Integer>[] inputList) // inside the constructor we commence all the preparatory work
	{
		adj = inputList;
		nodes = new Vertex[inputList.length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = new Vertex();
	}


	private void Explore(int v) // recursive DFS method that looks around the given VERTEX and finds all other connected VERTICES (direct and indirect)
	{
		nodes[v].visited = true; // before we do anything we first mark the current NODE as 'visited'
		nodes[v].group = counter; // at this point we can use the property called 'group' that will segregate all the reachable nodes into one group
		PreVisit(v); // we first introduce the pre-visit method before we start the recursion

		for (int w : adj[v]) // 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
		{
			if (!nodes[w].visited) // if we haven't explored the vertex yet, then we step into it
			{
				order.add(w);
				Explore(w); // here we call 'Explore' recursively (DFS) before we even finish with looking at all the neighbors
			}
		}

		PostVisit(v); // we 'wrap up' the post-order as we go out of recursion
	}


	// INTERVALS can be either nested (one contained in the other) or disjoint (non-overlapping). Interleaved (overlapping over part of their lengths) cases are not possible
	// - Case 1 (nested): explore v while we are exploring u, we can not finish exploring u until we are done with v, therefore post(u) > post(v)
	// - Case 2 (disjoint): explore v after we finished exploring u, therefore post(u) < pre(v)
	private void PreVisit(int v) { nodes[v].previsit = clock; clock += 1; } // two functions that should keep track of what the DFS is doing inside the GRAPH
	private void PostVisit(int v) { nodes[v].postvisit = clock; clock += 1; }


	private void GroupComponents() // 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|)
	{
		counter = 1; // 'counter = 1' initialises the first group of connected nodes
		for (int v = 0; v < nodes.length; v++)
		{
			if (!nodes[v].visited)
			{
				order.add(v);
				Explore(v);
				counter += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
			}
		}

		DisplayValues();
	}


	private void DisplayValues() // testing function that prints all node properties
	{
		for (int i = 0; i < nodes.length; i++)
			System.out.printf("node index: %1$s, belongs to group: %2$s, visited order: %3$s, pre: %4$s, post: %5$s" + "\r\n", i, nodes[i].group, order.get(i), nodes[i].previsit, nodes[i].postvisit);
	}


	public static void main(String[] args)
	{
		ArrayList<Integer>[] adjLst = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(4)), 
			new ArrayList<Integer>(), 
			new ArrayList<Integer>(Arrays.asList( 5, 6 )), 
			new ArrayList<Integer>(Arrays.asList( 5 )), 
			new ArrayList<Integer>(Arrays.asList( 0 )), 
			new ArrayList<Integer>(Arrays.asList( 2, 3, 6, 7 )), 
			new ArrayList<Integer>(Arrays.asList( 2, 5 )), 
			new ArrayList<Integer>(Arrays.asList( 5 ))};

		UGraph graph1 = new UGraph(adjLst);
		graph1.Explore(2);

		UGraph graph2 = new UGraph(adjLst);
		graph2.GroupComponents();
	}
}
