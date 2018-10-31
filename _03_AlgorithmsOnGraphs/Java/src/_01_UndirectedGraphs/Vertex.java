package _01_UndirectedGraphs;

public class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
{
	public boolean visited = false; // flags if a VERTEX was visited or not
	public Integer group = null; // keeps track of CONNECTED COMPONENTS within the graph
	public Integer previsit = null; // these two are for storing the order in which DFS went in and out each NODE
	public Integer postvisit = null;
}
