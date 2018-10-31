#Uses python3
""" PATHS IN GRAPHS 2: EXCERCISE 1 - Given an directed graph with positive edge weights and with n vertices and m edges as well as two vertices u and v, compute the weight of a shortest path between u and v (that is, the minimum total weight of a path from u to v)."""

import sys


def distance(adj, cost, s, t):  #
    """ Dijkstra algorithm implemented with a list as a priority queue data structure  """
    n = len(adj)
    dist = [2147483647 for _ in range(n)]  # storage for the graph with values that are on purpose bigger than allowed
    priority_queue = [(i, 2147483647) for i in range(n)]  # this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly

    dist[s] = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
    priority_queue[s] = (s, 0)
    priority_queue.sort(key=lambda e: e[1])  # each time we update the priority queue, we have to sort the list, this is not necessary if we start from 0

    while priority_queue:
        u = priority_queue.pop(0)[0]  # we need a handle for this element to update it later on

        for i in range(len(adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
            v = adj[u][i]  # v stores the index of the node on the other end, as usual

            if dist[v] > dist[u] + cost[u][i]:  # for direct neighbours we RELAX THE EDGES if possible
                idx = priority_queue.index((v, dist[v]))  # for direct neighbours we RELAX THE EDGES if possible
                dist[v] = dist[u] + cost[u][i]

                priority_queue[idx] = (v, dist[u] + cost[u][i])  # here we Change Priority by changing old to new values and sorting the whole list
                priority_queue.sort(key=lambda e: e[1])

    if dist[t] == 2147483647:
        return -1
    else:
        return dist[t]


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

    print(distance(adj, cost, s, t))  # Good job! (Max time used: 0.86/10.00, max memory used: 43376640/536870912.)
