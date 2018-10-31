#Uses python3
""" PATHS IN GRAPHS 1: EXCERCISE 1 - Given an undirected graph with n vertices and m edges and two vertices u and v, compute the length of a shortest path between u and v """

import sys
from collections import deque


def distance(adj, s, t):
    n = len(adj)
    dist = [-1 for _ in range(n)]  # initialize the distance list with -1 as this is the expected 'no path' value
    q = deque()  # queue to store nodes in BFS

    dist[s] = 0  # zero distance to the starter node itself
    q.append(s)

    while q:
        u = q.popleft()
        for v in adj[u]:
            if dist[v] == -1:
                q.append(v)
                dist[v] = dist[u] + 1

    return dist[t]  # since distance 'dist' was initiated with -1, then it is enough to return the appropriate value from the array


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    adj = [[] for _ in range(n)]  # before we use a LIST of LISTS we have to initialize it a such
    for (a, b) in edges:
        adj[a - 1].append(b - 1)
        adj[b - 1].append(a - 1)
    s, t = data[2 * m] - 1, data[2 * m + 1] - 1

    print(distance(adj, s, t))  # Good job! (Max time used: 0.44/10.00, max memory used: 45162496/536870912.)
