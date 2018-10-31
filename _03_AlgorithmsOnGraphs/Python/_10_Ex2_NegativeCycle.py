#Uses python3
""" PATHS IN GRAPHS 2: EXCERCISE 2 - Given an directed graph with possibly negative edge weights and with n vertices and m edges, check whether it contains a cycle of negative weight. """

import sys


def negative_cycle(adj, cost):
    n = len(adj)
    dist = [0 for _ in range(n)] # since there is no start point we may as well initialize everything with 0, as the negative cycle, it will decrease the values to negative anyway

    for iter in range(n):  # theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
        is_changed = False  # extra flag to close the procedure quicker if an iteration doesn't change anything
        for u in range(0, n, 1):  # here we start running and optimizing all edges in a graph
            for i in range(len(adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                v = adj[u][i]  # v stores the index of the node on the other end, as usual

                if dist[v] > dist[u] + cost[u][i]:  # for visited neighbours we RELAX THE EDGES if possible
                    dist[v] = dist[u] + cost[u][i]
                    is_changed = True

                    if iter == n - 1:  # if there is a cycle, it will be detected on the |V|-1 iteration
                        return 1

        if is_changed == False:  # here we check after each iteration if there has been a change, we return if there wasn't
            return 0

    return 0  # if no cycles have been found we just return an empty list


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

    print(negative_cycle(adj, cost))  # Good job! (Max time used: 3.62/10.00, max memory used: 10387456/536870912.)
