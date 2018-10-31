#Uses python3
""" DECOMPOSITION OF GRAPHS 2: EXCERCISE 3 - Compute the number of strongly connected components of a given directed graph with 𝑛 vertices and 𝑚 edges """

import sys
from collections import deque

sys.setrecursionlimit(200000)


def number_of_strongly_connected_components(adj):  # we have to run 'explore' procedures on the postorder values, so each time we go through a SCC we start from a SINK VERTEX
    nodes = [False for _ in range(n)]  # the storage for node status has to be initialized with unvisited values
    nodes_rev = [False for _ in range(n)]
    post_index = deque()  # stack for storing the list of reversed 'post' indexes
    adj_rev = [[] for _ in range(len(adj))]  # before we use a LIST of LISTS we have to initialize it a such  # initializing the reversed list

    for i in range(len(adj)):  # reversing the order of the graph edges for DFS
        for element in adj[i]:
            adj_rev[element].append(i)

    for v in range(0, len(nodes_rev), 1):  # 'DFS' procedure scans the REVERSED graph for SINK VERTEXES in SCCs
        if not nodes_rev[v]:
            explore(adj_rev, nodes_rev, post_index, v, True)

    result = 0  # we are going to use the 'counter' at this point to group the SCCs

    for v in post_index:  # here we go by the whole list of recorded post-visit values that we obtained from DFS and explore the real graph
        if not nodes[v]:
            explore(adj, nodes, post_index, v, False)
            result += 1  # when we finished with our recursive calls and we are on the way out, we increment the SCC counter

    return result


def explore(adj, nodes, post_index, v, store_index):  # multipurpose 'Explode' function for both the standard and reversed graph
    nodes[v] = True  # before we do anything we first mark the NODE we are at as 'visited'

    for w in adj[v]:
        if not nodes[w]:
            explore(adj, nodes, post_index, w, store_index)

    if store_index:
        post_index.appendleft(v)  # we store the post-order values as we go out of RECURSION only when we scan the REVERSED GRAPH


if __name__ == '__main__':
    input = sys.stdin.read()
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    adj = [[] for _ in range(n)]  # before we use a LIST of LISTS we have to initialize it a such
    for (a, b) in edges:
        adj[a - 1].append(b - 1)

    print(number_of_strongly_connected_components(adj))  # Good job! (Max time used: 0.08/5.00, max memory used: 11563008/536870912.)
