package _04_StronglyConnectedComponents;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Arrays;

//CONNECTION - Two vertices 'v' and 'w' in a directed graph are connected if you can reach 'v' from 'w' and can reach 'w' from 'v'
//STRONGLY CONNECTED COMPONENTS - a partition of a directed graph, where two vertices are connected if and only if they are in the same component. Once you leave the SCC you can't come back
//METAGRAPH - is a way to show how the SCC connect to one another, so it always becomes a DAG because the SCC has been 'compressed' into partitions, so there are no cycles anymore


public class DGraphComponents {
	private ArrayList<Integer>[] adj;
	private ArrayList<Integer>[] adj_rev;
	private Vertex[] nodes;
	private Vertex[] nodes_rev;

	private LinkedList<Integer> postValue = new LinkedList<Integer>(); // order of the post-visit VERTICES we are going to follow
	private LinkedList<Integer> postIndex = new LinkedList<Integer>();
	private ArrayList<ArrayList<Integer>> groups = new ArrayList<ArrayList<Integer>>(); // stores a list of SCC grouped also into lists
	private ArrayList<Integer> g = new ArrayList<Integer>(); // container for elements in each group
	private int counter; // field for the 'group' function
	private int clock = 1; // field for the pre and post visiting functions


	public DGraphComponents(ArrayList<Integer>[] inputList, ArrayList<Integer>[] inputList_rev) // inside the constructor we commence all the preparatory work
	{
		adj = inputList;
		adj_rev = inputList_rev;
		nodes = new Vertex[inputList.length]; // after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects
		nodes_rev = new Vertex[inputList.length];
		for (int i = 0; i < nodes.length; i++)
		{
			nodes[i] = new Vertex();
			nodes_rev[i] = new Vertex();
		}
	}


	private void PreVisit(int v) { nodes_rev[v].previsit = clock; clock += 1; } // two functions that should keep track of what the DFS is doing inside the GRAPH
	private void PostVisit(int v) { nodes_rev[v].postvisit = clock; clock += 1; }


	private void Explore(int v) // the standard 'Explore' function is just to aid the DFS procedure of the REVERSED GRAPH
	{
		nodes_rev[v].visited = true;
		nodes_rev[v].group = counter;
		PreVisit(v);

		for (int w : adj_rev[v])
		{
			if (!nodes_rev[w].visited)
				Explore(w);
		}

		postValue.addFirst(clock); // here we note the post-order as we are backtracking
		postIndex.addFirst(v);
		PostVisit(v);
	}


	private void DepthFirstSearch() // 'DFS' procedure scans the REVERSED graph for SINK VERTEXES in SCCs
	{
		counter = 1;
		for (int v = 0; v < nodes_rev.length; v++)
		{
			if (!nodes_rev[v].visited)
			{
				Explore(v);
				counter += 1; // when we finished with our recursive calls and we are on the way out, we increment the counter for the group
			}
		}

		DisplayingValues();
	}


	private int ConnectedComponents() // in order to get the SCCs right, we have to run 'explore' procedures on the postorder values, so each time we go through the SCC we start from a SINK VERTEX
	{
		DepthFirstSearch(); // before we start anything, we need to run DFS on the REVERSED Graph in order to find the SINK VERTICES, for the SCC procedure to work
		counter = 0;

		for (int v : postIndex) // here we go by the whole list of recorded post-visit values that we obtained from DFS
		{
			if (!nodes[v].visited)
			{
				ExploreComponents(v);
				counter += 1;
				groups.add((ArrayList<Integer>) g.clone()); // we have to use the 'To.List()' method to pass the new list as a VALUE, otherwise we will clear it in the next line and therefore won't change anything
				g.clear();
			}
		}

		PrintGroup();
		return counter;
	}
	
	private void ExploreComponents(int v) // version of the 'Explore' procedure for SCC
	{
		nodes[v].visited = true;

		for (int w : adj[v])
		{
			if (!nodes[w].visited)
				ExploreComponents(w);
		}

		g.add(v); // here we append all the elements of the group while backtracking
	}


	private void DisplayingValues() // testing procedure for displaying the details of the REVERSED DFS
	{
		for (int i = 0; i < nodes.length; i++)
			System.out.printf("node index: %1$s, belongs to group: %2$s, pre: %3$s, post: %4$s" + "\r\n", i, nodes_rev[i].group, nodes_rev[i].previsit, nodes_rev[i].postvisit);

		for (int i : postValue)
			System.out.print(i + " ");

		System.out.println();

		for (int i : postIndex)
			System.out.print(i + " ");

		System.out.println();
		System.out.println();
	}


	private void PrintGroup()
	{
		for (ArrayList<Integer> i : groups)
		{
			for (int j : i)
				System.out.print(j + " ");
			System.out.println();
		}
	}

	private static ArrayList<Integer>[] ReverseGraph(ArrayList<Integer>[] lst)
	{
		ArrayList<Integer>[] reversedList = new ArrayList[lst.length];
		for (int i = 0; i < lst.length; i++)
			reversedList[i] = new ArrayList<Integer>();

		for (int i = 0; i < lst.length; i++)
			for (int element : lst[i])
				reversedList[element].add(i);

		return reversedList;
	}


	public static void main(String[] args)
	{
		ArrayList<Integer>[] adj = new ArrayList[] {
			new ArrayList<Integer>(Arrays.asList(1)),
			new ArrayList<Integer>(Arrays.asList(2, 3)),
			new ArrayList<Integer>(Arrays.asList(0, 1, 4, 6)),
			new ArrayList<Integer>(Arrays.asList(0)),
			new ArrayList<Integer>(Arrays.asList(0, 2)),
			new ArrayList<Integer>(Arrays.asList(3)),
			new ArrayList<Integer>(),
			new ArrayList<Integer>(Arrays.asList(3, 4, 5))
		};

		ArrayList<Integer>[] adj_rev = ReverseGraph(adj);
		DGraphComponents dag = new DGraphComponents(adj, adj_rev);
		System.out.println(dag.ConnectedComponents());
	}
}