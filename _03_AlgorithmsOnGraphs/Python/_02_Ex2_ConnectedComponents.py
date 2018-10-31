#Uses python3
""" DECOMPOSITION OF GRAPHS 1: EXCERCISE 2 - Given an undirected graph with 𝑛 vertices and 𝑚 edges, compute the number of connected components """

import sys


def number_of_components(adj):
    result = 0  # this is for counting the number of groups, we only need to return the last value
    nodes = [False for _ in range(n)]  # the storage for node status has to be initialized with unvisited values

    for v in range(len(nodes)):  # we used 'for' loop to have the INDEXES available as an argument for 'Explore' method
        if not nodes[v]:  # if we haven't explored the vertex yet, then we step into it
            explore(adj, nodes, v)
            result += 1  # when we are finished with our recursive calls we know that all connected components have been discovered, we increment the counter
    return result


def explore(adj, nodes, v):
    """ recursive DFS method that looks around the given VERTEX and find all other connected VERTICES (direct and indirect) """
    nodes[v] = True
    for w in adj[v]:  # 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
        if not nodes[w]:
            explore(adj, nodes, w)


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

    print(number_of_components(adj))  # Good job! (Max time used: 0.03/5.00, max memory used: 8151040/536870912.)
