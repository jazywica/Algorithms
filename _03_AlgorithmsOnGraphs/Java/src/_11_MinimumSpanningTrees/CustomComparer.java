package _11_MinimumSpanningTrees;

import java.util.Comparator;


public class CustomComparer implements Comparator<long[]> // custom comparer for sorting elements in SortedSet by distance
{
	public final int compare(long[] left, long[] right)
	{
		int comp = (new Long(left[1])).compareTo(right[1]);
		if (comp == 0)
			return (new Long(left[0])).compareTo(right[0]); // this will prevent from deleting all nodes with the same initial value

		return comp;
	}
}
