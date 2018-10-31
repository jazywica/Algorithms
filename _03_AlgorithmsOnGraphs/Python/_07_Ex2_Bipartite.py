#Uses python3
""" PATHS IN GRAPHS 1: EXCERCISE 2 - Given an undirected graph with n vertices and m edges, check whether it is bipartite. """

import sys
from collections import deque


def bipartite(adj):
    n = len(adj)
    color = ["" for _ in range(n)]  # An alternative definition: a graph is bipartite if its vertices can be colored with two colors (black & white) such that the endpoints of each edge have different colors.
    q = deque()  # queue to store nodes in BFS

    color[0] = "white"  # we must always start from the first element available
    q.append(0)  # the starting node is the only one that gets into the queue prior to BFS

    while q:  # algorithm is based on the fact that we always enter the next level after we are finished with the current one
        u = q.popleft()
        for v in adj[u]:
            cur_color = color[u]

            if color[v] == "":  # for unvisited neighbours we enqueue and assign the other color
                q.append(v)
                color[v] = "black" if (cur_color == "white") else "white"
            else:  # for visited neighbours we check if the color is opposite
                if cur_color == color[v]:
                    return 0
    return 1


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    adj = [[] for _ in range(n)]
    for (a, b) in edges:
        adj[a - 1].append(b - 1)
        adj[b - 1].append(a - 1)

    print(bipartite(adj))  # Good job! (Max time used: 0.39/10.00, max memory used: 43913216/536870912.)
