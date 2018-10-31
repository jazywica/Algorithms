#Uses python3
""" MINIMUM SPANNING TREES: EXCERCISE 2 - Given n points on a plane and an integer k, compute the largest possible value of d such that the given points can be partitioned into k non-empty subsets in such a way that the distance between any two points from different subsets is at least d """

import sys
import math


def clustering(x, y, k):  # Recall that the length of a segment with endpoints (x1; y1) and (x2; y2) is equal to: SQRT((x1 - x2)^2 + (y1 - y2)^2)
    """ Used Kruskal's algorithm, implemented with a Disjoint Set (Set of Frozen Sets) """
    result = 0.
    edges = make_edges(x, y)
    nodes = set([frozenset([i]) for i in range(len(x))])  # we first initialize the set with single sets containing only one element - this is only possible with frozen sets, as they are immutable

    edges.sort(key=lambda e: e[0])  # Kruskal algorithm sorts all edges and then processes them independently

    for idx in range(len(edges)):
        u = find(nodes, edges[idx][1])
        v = find(nodes, edges[idx][2])
        if not u == v:  # if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
            if len(nodes) > k:
                nodes.add(u.union(v))
                nodes.remove(u)
                nodes.remove(v)
                result += edges[idx][0]
            else:  # if we already have the desired amount of clusters we wait for the first edge that is between two different sets - this is our answer
                return edges[idx][0]
    return -1


def make_edges(x, y):
    """ helper function that will join the input arrays into one list with three elements per edge: distance, from, to """
    result = []
    for i in range(0, len(x) - 1):
        for j in range(i + 1, len(y)):
            result.append([])
            current = result[-1]
            current.append(calculate_distance(x[i], y[i], x[j], y[j]))
            current.append(i)
            current.append(j)
    return result


def calculate_distance(x1, y1, x2, y2):
    return math.sqrt(math.pow(x1 - x2, 2) + math.pow(y1 - y2, 2))


def find(nodes, seq):
    """ helper function that will return the set, in which the desired node currently is """
    for item in nodes:
        if seq in item:
            return item
    return set([])


if __name__ == '__main__':
    input = sys.stdin.read()
    data = list(map(int, input.split()))
    n = data[0]
    data = data[1:]
    x = data[0:2 * n:2]  # storage for 'x' coordinates
    y = data[1:2 * n:2]  # storage for 'y' coordinates
    data = data[2 * n:]
    k = data[0]  # number k of clusters (non-empty subsets) which we are going to split our dataset into

    print("{0:.9f}".format(clustering(x, y, k)))  # Good job! (Max time used: 0.06/10.00, max memory used: 13172736/536870912.)    
