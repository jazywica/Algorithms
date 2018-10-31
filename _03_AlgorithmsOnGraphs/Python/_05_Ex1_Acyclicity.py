#Uses python3
""" DECOMPOSITION OF GRAPHS 2: EXCERCISE 1 - Check whether a given directed graph with 𝑛 vertices and 𝑚 edges contains a cycle """

import sys

cyclic = False  # flags if we have found a a cycle in a graph


def acyclic(adj):
    global cyclic
    nodes = [False for _ in range(n)]  # the storage for node status has to be initialized with unvisited values
    for v in range(len(nodes)):
        cycle_value = v  # field for storing a potential cycle inside a group
        if not nodes[v]:
            explore(adj, nodes, cycle_value, v)
    if cyclic:
        return 1
    else:
        return 0


def explore(adj, nodes, cycle_value, v):  # helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
    global cyclic
    if cycle_value in adj[v]:  # checks if the initial node value reappears and returns immediately
        cyclic = True
        return
    nodes[v] = True
    for w in adj[v]:
        if not nodes[w]:
            explore(adj, nodes, cycle_value, w)


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    adj = [[] for _ in range(n)]  # before we use a LIST of LISTS we have to initialize it a such
    for (a, b) in edges:
        adj[a - 1].append(b - 1)

    print(acyclic(adj))  # Good job! (Max time used: 0.04/5.00, max memory used: 7946240/536870912.)
