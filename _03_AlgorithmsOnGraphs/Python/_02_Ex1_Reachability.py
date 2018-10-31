#Uses python3
""" DECOMPOSITION OF GRAPHS 1: EXCERCISE 1 - Given an undirected graph and two distinct vertices 𝑢 and 𝑣, check if there is a path between 𝑢 and 𝑣 """

import sys

is_found = False  # global flag to quickly get out of the RECURSION


def reach(adj, x, y):
    global is_found  # the global variable is already initiated in the main body below
    nodes = [False for _ in range(n)]  # the storage for node status has to be initialized with unvisited values
    return explore(adj, nodes, x, y)


def explore(adj, nodes, x, y):
    global is_found  # the global variable is already initiated in the main body below
    nodes[x] = True  # before we do anything we first mark the NODE we are at as 'visited'
    for w in adj[x]:
        if not nodes[w] and not is_found:  # if we haven't explored the vertex yet (false) AND THE CONNECTION IS NOT DISCOVERED YET, then we go further
            if w == y:  # here we check the main condition
                is_found = True  # here we set our variable as visited
                return 1  # now we start backtracking straight away
            explore(adj, nodes, w, y)  # here we call 'reach' recursively with a new neighbor and the old 'y' value

    if is_found:  # now we check if the connection was found and return the appropriate values
        return 1
    else:
        return 0


if __name__ == '__main__':
    input = sys.stdin.read()  # the 'read()' method requires CTRL+D to close the input stream
    data = list(map(int, input.split()))
    n, m = data[0:2]
    data = data[2:]
    edges = list(zip(data[0:(2 * m):2], data[1:(2 * m):2]))
    x, y = data[2 * m:]
    adj = [[] for _ in range(n)]  # before we use a LIST of LISTS we have to initialize it a such
    x, y = x - 1, y - 1
    for (a, b) in edges:
        adj[a - 1].append(b - 1)
        adj[b - 1].append(a - 1)

    print(reach(adj, x, y))  # Good job! (Max time used: 0.16/5.00, max memory used: 8101888/536870912.)
