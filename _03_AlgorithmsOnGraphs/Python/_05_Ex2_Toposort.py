#Uses python3
""" DECOMPOSITION OF GRAPHS 2: EXCERCISE 2 - Compute a topological ordering of a given directed acyclic graph (DAG) with 𝑛 vertices and 𝑚 edges """

import sys
from collections import deque


def toposort(adj):  # we are going to put the elements on a STACK, as we start placing the elements from the end to the start
    order = deque()  # this is for storing the TOPOLOGICAL SORT
    nodes = [False for _ in range(n)]  # the storage for node status has to be initialized with unvisited values
    for v in range(len(nodes)):
        if not nodes[v]:
            dfs(adj, nodes, order, v)
    return order


def dfs(adj, nodes, order, v):  # helper function that will look around the given VERTEX and find all other connected VERTICES (direct and indirect)
    nodes[v] = True
    for w in adj[v]:
        if not nodes[w]:
            dfs(adj, nodes, order, w)
    order.appendleft(v)  # this is where we normally store the 'post-order' value - where we exit the recursion


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    adj = [[] for _ in range(n)]  # before we use a LIST of LISTS we have to initialize it a such
    for (a, b) in edges:
        adj[a - 1].append(b - 1)

    order = toposort(adj)  # Good job! (Max time used: 0.44/10.00, max memory used: 39608320/536870912.)

    for x in order:  # displays the topologically sorted list
        print(x + 1, end=' ')
