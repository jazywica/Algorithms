#Uses python3
""" PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v)."""

import sys


def distance(adj, cost, s, t):  #
    """ Dijkstra algorithm implemented with two simple arrays instead for a priority queue data structure """
    dist = [2147483647 for _ in range(len(adj))]  # storage for the graph with values that are on purpose bigger than allowed
    known = [False for _ in range(len(adj))]  # we use max. integer values while 'dist' was declared as long
    dist[s] = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

    while True:
        u = extract_min(dist, known)
        if u == -1 or all(known):  # first condition checks for non-connected nodes and the second if all nodes have been verified
            break

        for i in range(len(adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
            v = adj[u][i]  # v stores the index of the node on the other end, as usual

            if dist[v] > dist[u] + cost[u][i]:  # for direct neighbours we RELAX THE EDGES if possible
                dist[v] = dist[u] + cost[u][i]
        known[u] = True  # this is how we can CHANGE THE PRIORITY

    if dist[t] == 2147483647:
        return -1
    else:
        return dist[t]


def extract_min(dist, known):  # helper function that returns the element u index which has the smallest distance
    smallest = 2147483647
    index = -1
    for i in range(len(dist)):
        if dist[i] < smallest and not known[i]:
            smallest = dist[i]
            index = i
    return index  # we always compare the distances, but we return the index


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(zip(data[0:(3 * m):3], data[1:(3 * m):3]), data[2:(3 * m):3]))
    data = data[3 * m:]
    adj = [[] for _ in range(n)]
    cost = [[] for _ in range(n)]
    for ((a, b), w) in edges:
        adj[a - 1].append(b - 1)
        cost[a - 1].append(w)
    s, t = data[0] - 1, data[1] - 1

    print(distance(adj, cost, s, t))  # Good job! (Max time used: 0.42/10.00, max memory used: 43372544/536870912.)
