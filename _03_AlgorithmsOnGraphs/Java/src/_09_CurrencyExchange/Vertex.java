package _09_CurrencyExchange;

public class Vertex {
	public boolean known = false; // flags if a VERTEX is visited
    public long dist = Integer.MAX_VALUE; // initializes the node distances
    public int path = -1; // stores the previous node for the fastest path
    public int path_bfs = -1; // stores the previous node for the shortest path
}
