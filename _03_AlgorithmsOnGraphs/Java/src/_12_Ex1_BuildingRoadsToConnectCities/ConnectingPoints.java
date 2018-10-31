package _12_Ex1_BuildingRoadsToConnectCities; // must be removed for submission

import java.util.Scanner;
import java.util.TreeSet;
import java.util.ArrayList;
import java.util.Comparator;


public class ConnectingPoints // MINIMUM SPANNING TREES: EXCERCISE 1 - Given n points on a plane, connect them with segments of minimum total length such that there is a path between any two points.
{
    static private class CustomComparer implements Comparator<double[]> // custom comparer for sorting elements in SortedSet by distance
    {
    	public final int compare(double[] left, double[] right)
    	{
    		int comp = (new Double(left[1])).compareTo(right[1]);
    		if (comp == 0)
    			return (new Double(left[0])).compareTo(right[0]); // this will prevent from deleting all nodes with the same initial value

    		return comp;
    	}
    }
	
    private static double minimumDistance(int[] x, int[] y) {
        double result = 0.;
        int n = x.length;
		ArrayList<Integer>[] adj = (ArrayList<Integer>[])new ArrayList[n];
		ArrayList<Double>[] cost = (ArrayList<Double>[])new ArrayList[n];
		
		TreeSet<double[]> priorityQueue = new TreeSet<double[]>(new CustomComparer()); // this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly
		double[] dist = new double[n];

		for (int i = 0; i < adj.length; i++) {
			if (i == 0) {
				dist[i] = 0;
				priorityQueue.add(new double[] {(int)i, 0});
			}
			else {
				dist[i] = Integer.MAX_VALUE;
				priorityQueue.add(new double[] {(int)i, Integer.MAX_VALUE});
			}
			adj[i] = new ArrayList<Integer>();
			cost[i] = new ArrayList<Double>();
		}

		MakeEdges(x, y, adj, cost);

		while (!priorityQueue.isEmpty())
		{
			double[] U = priorityQueue.first(); // we need a handle for this element to update it later on
			priorityQueue.remove(U);
			int u = (int)U[0];

			for (int i = 0; i < adj[u].size(); i++) // in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
			{
				int v = adj[u].get(i); // v stores the index of the node on the other end, as usual

				if (dist[v] > cost[u].get(i) && priorityQueue.contains(new double[] {v, dist[v]})) // since we are only comparing single distances, we can't look back, so we only take into account
				{
					double oldDist = dist[v]; // for direct neighbours we RELAX THE EDGES if possible
					dist[v] = cost[u].get(i);

					priorityQueue.remove(new double[] {(int)v, oldDist}); // here we Change Priority by subtracting and adding again a new value, as this is the only way to get the things sorted
					priorityQueue.add(new double[] {(int)v, cost[u].get(i)});
				}
			}
			result += dist[u];
		}

		return result;
	}


	private static void MakeEdges(int[] x, int[] y, ArrayList<Integer>[] adj, ArrayList<Double>[] cost) // helper function that will join the coordinate arrays into adjacency and cost lists
	{
		for (int i = 0; i < x.length - 1; i++) {
			for (int j = i + 1; j < y.length; j++) {
				double distance = CalculateDistance(x[i], y[i], x[j], y[j]);
				adj[i].add((int)j);
				cost[i].add(distance);
				adj[j].add((int)i);
				cost[j].add(distance);
			}
		}
	}


	private static double CalculateDistance(int x1, int y1, int x2, int y2)
	{
		return Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
	}


    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int n = scanner.nextInt();
        int[] x = new int[n];
        int[] y = new int[n];
        for (int i = 0; i < n; i++) {
            x[i] = scanner.nextInt();
            y[i] = scanner.nextInt();
        }
        
        System.out.println(minimumDistance(x, y)); // Good job! (Max time used: 0.79/3.00, max memory used: 42463232/536870912.)
    }
}

