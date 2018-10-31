package _07_Ex2_IsGraphBipartite; // must be removed for submission

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Queue;
import java.util.Scanner;


public class Bipartite // PATHS IN GRAPHS 1: EXCERCISE 2 - Given an undirected graph with n vertices and m edges, check whether it is bipartite.
{
	private static int bipartite(ArrayList<Integer>[] adj) // An undirected graph is called bipartite if its vertices can be split into two parts such that each edge of the graph joins to vertices from different parts.
	{
		int n = adj.length;
		String[] color = new String[n]; // An alternative definition: a graph is bipartite if its vertices can be colored with two colors (black & white) such that the endpoints of each edge have different colors.
		Queue<Integer> q = new LinkedList<Integer>(); // queue to store nodes in BFS

		for (int i = 0; i < n; i++) // initialize the color array
			color[i] = "";

		color[0] = "white"; // we must always start from the first element available
		q.offer(0); // the starting node is the only one that gets into the queue prior to BFS

		while (!q.isEmpty()) // algorithm is based on the fact that we always enter the next level after we are finished with the current one
		{
			int u = q.poll();
			for (int v : adj[u])
			{
				String cur_color = color[u];

				if (color[v].equals("")) // for unvisited neighbours we enqueue and assign the other color
				{
					q.offer(v);
					color[v] = (cur_color.equals("white")) ? "black" : "white";
				}
				else // for visited neighbours we check if the color is opposite
				{
					if (color[v].equals(cur_color))
						return 0;
				}
			}
		}

		return 1;
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
        
        System.out.println(bipartite(adj)); // Good job! (Max time used: 2.10/3.00, max memory used: 198762496/536870912.)
    }
}
