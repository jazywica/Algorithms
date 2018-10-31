package _03_DirectedGraphs;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Arrays;

// CYCLES - a 'cycle' in a graph G is a sequence of vertices v1,v2,v2..vn so that (v1, v2), (v2, v3)....,(vn-1, vn),(vn, v1) area all edges. Which means that vertices are connected in a loop
// *Any graph that contains a cycle v1,...vn. can not be LINEARLY ORDERED, because if a start vertex vk comes first then vk comes before vk-1 and it is a contradiction
// DIRECTLY ACYCLIC GRAPH (DAG) is a a graph with no cycles and therefore can be linearly ordered
// DAG has two characteristic components: SOURCE (vertex with no incoming edges) and SINK (vertex with no outgoing edges)


public class DGraph // class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests
{
	private ArrayList<Integer>[] adj; // ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
	private Vertex[] nodes; // array for storing the graph
	private int counter; // field for the 'group' function
	private int clock = 1; // field for the pre and post visiting functions
	private int cycleValue; // field for storing a potential cycle inside a group
	private boolean cyclic; // flags if we have found a a cycle in a graph
	private LinkedList<Integer> order = new LinkedList<Integer>(); // stack for the TOPOLOGICAL SORT


	public DGraph(ArrayList<Integer>[] inputList) // inside the constructor we commence all the preparatory work
	{
		adj = inputList;
		nodes = new Vertex[inputList.length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
		for (int i = 0; i < nodes.length; i++)
			nodes[i] = new Vertex();
	}


	private void Explore(int v) // recursive DFS method that looks around the given VERTEX and finds all other connected VERTICES (direct and indirect)
	{
		if (adj[v].contains(cycleValue)) { cyclic = true; return; } // part of the 'isCyclic()' procedure which checks if the initial node value reappears

		nodes[v].visited = true;
		nodes[v].group = counter;
		PreVisit(v);

		for (int w : adj[v])
		{
			if (!nodes[w].visited)
				Explore(w); // here we call 'Explore' recursively (DFS) before we even finish with looking at all the neighbors
		}

		PostVisit(v);
		order.addFirst(v); // here we push the values on a stack for the TOPOLOGICAL SORT while we are backtracking
	}


	private void PreVisit(int v) { nodes[v].previsit = clock; clock += 1; } // two functions that should keep track of what the DFS is doing inside the GRAPH
	private void PostVisit(int v) { nodes[v].postvisit = clock; clock += 1; }


	private void GroupComponents() // 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|)
	{
		counter = 1; // 'counter = 1' initialises the first group of connected nodes
		for (int v = 0; v < nodes.length; v++)
		{
			if (!nodes[v].visited)
			{
				Explore(v);
				counter += 1; // when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter
			}
		}

		DisplayValues();
	}


	private boolean IsCyclic() // function stores the value we are starting with and runs 'Explore' to check if it is present in all connected components
	{
		for (int v = 0; v < nodes.length; v++)
		{
			cycleValue = v;
			if (!nodes[v].visited)
				Explore(v);
		}

		if (cyclic == true)
			return true;
		else
			return false;
	}
	
	
	private LinkedList<Integer> TopologicalSort() // function uses STACK, as we start placing the elements from the end to the front
	{
		for (int v = 0; v < nodes.length; v++)
		{
			if (!nodes[v].visited)
				Explore(v);
		}
		return order;
	}


	private void DisplayValues() // testing function that prints all node properties
	{
		for (int i = 0; i < nodes.length; i++)
			System.out.printf("node index: %1$s, belongs to group: %2$s, pre: %3$s, post: %4$s" + "\r\n", i, nodes[i].group, nodes[i].previsit, nodes[i].postvisit);
	}


	public static void main(String[] args)
	{
		ArrayList<Integer>[] adjDag = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1, 2)),
			new ArrayList<Integer>(Arrays.asList(2, 3, 5, 6)),
			new ArrayList<Integer>(Arrays.asList(4, 5, 6)),
			new ArrayList<Integer>(Arrays.asList(5)),
			new ArrayList<Integer>(Arrays.asList(6)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(7)),
			new ArrayList<Integer>()
		};
		
		ArrayList<Integer>[] adjCyclic = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(3)),
			new ArrayList<Integer>(Arrays.asList(4)),
			new ArrayList<Integer>(Arrays.asList(5)),
			new ArrayList<Integer>(Arrays.asList(2))
		};

		DGraph dag_1 = new DGraph(adjDag);
		DGraph cyclic_1 = new DGraph(adjCyclic);
		dag_1.GroupComponents(); // grouping components works just the same way as with undirected graph
		System.out.println();
		cyclic_1.GroupComponents();
		System.out.println();

		DGraph dag_2 = new DGraph(adjDag);
		DGraph cyclic_2 = new DGraph(adjCyclic);
		System.out.println(dag_2.IsCyclic()); // recognises what is cyclic and what is acyclic
		System.out.println(cyclic_2.IsCyclic());
		System.out.println();

		DGraph dag_3 = new DGraph(adjDag);
		LinkedList<Integer> topo = dag_3.TopologicalSort();
		for (int x : topo) // the product of a topological sort may not be the same in all cases, all depends on the order of elements in the adjacency lists
			System.out.print(x + " ");
	}
}
