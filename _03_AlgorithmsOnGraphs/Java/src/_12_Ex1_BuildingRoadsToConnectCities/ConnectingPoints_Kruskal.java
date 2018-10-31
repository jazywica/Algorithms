package _12_Ex1_BuildingRoadsToConnectCities;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;
import java.util.Scanner;


public class ConnectingPoints_Kruskal {
	private static double minimumDistance(int[] x, int[] y) {
		double result = 0;
		ArrayList<double[]> edges = MakeEdges(x, y);
		HashSet<HashSet<Integer>> nodes = new HashSet<HashSet<Integer>>();
		for (int i = 0; i < x.length; i++) // we first initialize the set with single sets containing only one element
			nodes.add(new HashSet<Integer>(Arrays.asList(i)));

		Collections.sort(edges, (a, b) -> Double.compare(a[0], b[0])); // Kruskal algorithm sorts all edges and then processes them independently

		for (int idx = 0; idx < edges.size(); idx++) {
			HashSet<Integer> u = Find(nodes, edges.get(idx)[1]);
			HashSet<Integer> v = Find(nodes, edges.get(idx)[2]);

			if (!u.equals(v)) // if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
			{
				nodes.remove(u);
				nodes.remove(v);
				
				HashSet<Integer> combined = new HashSet<Integer>(u);
				combined.addAll(v);
				nodes.add(combined);
				
				result += edges.get(idx)[0];
			}
		}

		return result;
	}


	private static ArrayList<double[]> MakeEdges(int[] x, int[] y) // helper function that will join the input arrays into one list with three elements per edge: distance, from, to
	{
		ArrayList<double[]> result = new ArrayList<double[]>();

		for (int i = 0; i < x.length - 1; i++) {
			for (int j = i + 1; j < y.length; j++) {
				result.add(new double[3]);
				double[] current = result.get(result.size() - 1);
				current[0] = CalculateDistance(x[i], y[i], x[j], y[j]);
				current[1] = (int)i;
				current[2] = (int)j;
			}
		}
		return result;
	}


	private static double CalculateDistance(int x1, int y1, int x2, int y2)
	{
		return Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
	}


	private static HashSet<Integer> Find(HashSet<HashSet<Integer>> nodes, double node) // helper function that will return the set, in which the desired node currently is
	{
		for (HashSet<Integer> item : nodes)
		{
			if (item.contains((int)node))
				return item;
		}
		return new HashSet<Integer>();
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
        
        System.out.println(minimumDistance(x, y)); // Good job! (Max time used: 0.94/3.00, max memory used: 40833024/536870912.)
    }
}
