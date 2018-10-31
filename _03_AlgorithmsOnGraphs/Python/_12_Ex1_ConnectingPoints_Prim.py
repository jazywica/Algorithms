#Uses python3
""" MINIMUM SPANNING TREES: EXCERCISE 1 - Given n points on a plane, connect them with segments of minimum total length such that there is a path between any two points """

import sys
import math


def minimum_distance(x, y):  # Recall that the length of a segment with endpoints (x1; y1) and (x2; y2) is equal to: SQRT((x1 - x2)^2 + (y1 - y2)^2)
    """ used Prim's algorithm, implemented with a Priority Queue (Sorting a List constantly as there are no standard sorted collections) """
    result = 0.
    n = len(x)
    adj = [[] for _ in range(n)]
    cost = [[] for _ in range(n)]
    priority_queue = [(i, 2147483647) for i in range(n)]  # this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly
    dist = [2147483647 for _ in range(n)]

    dist[0] = 0
    priority_queue[0] = (0, 0)
    priority_queue.sort(key=lambda e: e[1])  # each time we update the priority queue, we have to sort the list, this is not necessary if we start from 0
    make_edges(x, y, adj, cost)

    while priority_queue:
        u = priority_queue.pop(0)[0]  # we need a handle for this element to update it later on

        for i in range(len(adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
            v = adj[u][i]  # v stores the index of the node on the other end, as usual

            if dist[v] > cost[u][i] and (v, dist[v]) in priority_queue: # since we are only comparing single distances, we can't look back, so we only take into account
                idx = priority_queue.index((v, dist[v]))  # for direct neighbours we RELAX THE EDGES if possible
                dist[v] = cost[u][i]

                priority_queue[idx] = (v, cost[u][i])  # here we Change Priority by changing old to new values and sorting the whole list
                priority_queue.sort(key=lambda e: e[1])

        result += dist[u]

    return result


def calculate_distance(x1, y1, x2, y2):
    return math.sqrt(math.pow(x1 - x2, 2) + math.pow(y1 - y2, 2))


def make_edges(x, y, adj, cost):
    """ helper function that will join the coordinate arrays into UNDIRECTED adjacency and cost lists """
    for i in range(0, len(x) - 1):
        for j in range(i + 1, len(y)):
            distance = calculate_distance(x[i], y[i], x[j], y[j])
            adj[i].append(j)
            cost[i].append(distance)
            adj[j].append(i)
            cost[j].append(distance)


if __name__ == '__main__':
    input = sys.stdin.read()
    data = list(map(int, input.split()))
    n = data[0]
    x = data[1::2]  # storage for 'x' coordinates
    y = data[2::2]  # storage for 'y' coordinates

    print("{0:.9f}".format(minimum_distance(x, y)))  # Good job! (Max time used: 0.42/10.00, max memory used: 9097216/536870912.)
