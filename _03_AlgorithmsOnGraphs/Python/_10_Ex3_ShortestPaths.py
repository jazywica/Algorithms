#Uses python3
""" PATHS IN GRAPHS 2: EXCERCISE 3 - Given an directed graph with possibly negative edge weights and with n vertices and m edges as well as its vertex s, compute the length of shortest paths from s to all other vertices of the graph. """

import sys
from collections import deque


def shortest_paths(adj, cost, s, distance, reachable, shortest):
    n = len(adj)
    a_nodes = set([])  # stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set
    path = [-1 for _ in range(n)]
    visited = [False for _ in range(n)]
    current = []  # list for excluding non-reachable nodes
    q = deque([s])

    # 1. Checking connectivity with BFS
    while q:  # Breadth-First Search for looking for all reachable bodes
        u = q.popleft()
        reachable[u] = 1  # all reachable nodes are going to be enqueued sooner all later, including the first element
        current.append(u)
        for v in adj[u]:
            if not visited[v]:
                q.append(v)
                visited[v] = True

    # 2. Detecting negative cycles - there must be 'n' iterations over all reachable nodes
    distance[s] = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
    for iter in range(0, n, 1):
        is_changed = False  # extra flag to close the procedure quicker, if an iteration doesn't change anything
        for u in current:
            for i in range(len(adj[u])):
                v = adj[u][i]  # v stores the index of the node on the other end of the edge, as usual

                if distance[u] == 9223372036854775807:  # if the starting node u hasn't been discovered yet, we just move on
                    continue

                elif distance[v] > distance[u] + cost[u][i]:  # for visited neighbours we RELAX THE EDGES if possible
                    distance[v] = distance[u] + cost[u][i]
                    path[v] = u  # here we store the fastest path, we can't do it in the BFS above, as we are going to backtrack the cycle, as it was discovered
                    is_changed = True

                    if iter == n - 1:
                        a_nodes.add(u)
                        for k in adj[u]:
                            a_nodes.add(k)
        if is_changed == False:  # here we check after each iteration if there has been a change, we exit function if it hasn't
            return

    # 3. Track all the cycles from the discovered nodes - we are going to remove all nodes present in the current cycle from 'a_nodes' on the fly
    if a_nodes:
        result = set([])

        while a_nodes:
            u = a_nodes.pop()
            current_cycle = reconstruct_cycle(a_nodes, path, u)
            result = result.union(current_cycle)
            a_nodes = a_nodes.difference(result)  # here we take out all the nodes in the current cycle form the main set, just so we don't repeat the reconstruction more than once for each cycle

        if s in result:  # if the start node is in the cycle, then we have to also check the path from the main node to the cycle
            for i in range(n):
                shortest[i] = 0
        else:
            for cyc in result:
                shortest[cyc] = 0


def reconstruct_cycle(a_nodes, path, x):  # helper function that gathers the cycle and returns it
    u = x  # this is to store the initial value that should end the cycle search
    count = 0
    while count < len(path):  # condition when we can't get to where we started from by backtracking
        a_nodes.add(u)
        u = path[u]
        count +=1
        if u == x:  # extra condition when we can't get to where we started from by backtracking
            break
    return a_nodes


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
    s = data[0]
    s -= 1
    distance = [10**19] * n
    reachable = [0] * n
    shortest = [1] * n

    shortest_paths(adj, cost, s, distance, reachable, shortest)  # Good job! (Max time used: 4.69/10.00, max memory used: 12058624/536870912.)

    for x in range(n):
        if reachable[x] == 0:
            print('*')
        elif shortest[x] == 0:
            print('-')
        else:
            print(distance[x])
