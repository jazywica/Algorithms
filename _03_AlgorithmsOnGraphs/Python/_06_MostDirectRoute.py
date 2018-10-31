""" Simple example showing a BFS algorithm on a directed graph's G(V, E), based on the '_06_BreadthFirstSearch.JPG' """

from collections import deque

# LENGTH of the path L(P) in the number of edges in the path
# DISTANCE between two vertices is the length of the shortest path between them


class MostDirectRoute:
    def __init__(self, input_list):  # inside the constructor we commence all the preparatory work
        self.adj = input_list
        self.dist = []  # stores the distances by nodes. since the nodes are just numbers from 0 to n-1, the best data structure is an array
        self.prev = []  # stores the previous node, which is the node that the current node was discovered from - for the shortest path algorithm
        self.q = deque()  # queue to store nodes in BFS
        self.n = len(self.adj)


    def bfs(self, s):  # Breadth-First Search for exploring a graph
        layers = [[]]  # extra item for grouping the nodes into the layers

        for i in range(self.n):
            self.dist.append(self.n)  # the biggest possible distance is the number of nodes - 1

        self.dist[s] = 0  # zero distance to the starter node itself
        self.q.append(s)  # the starting node is the only one that gets into the queue prior to BFS

        while (self.q):
            u = self.q.popleft()
            for v in self.adj[u]:
                if self.dist[v] == self.n:
                    self.q.append(v)
                    self.dist[v] = self.dist[u] + 1
                    if len(layers) < self.dist[u] + 2:  # extra item. the inner lists must be initialized before we can use them
                        layers.append([])
                    layers[self.dist[u] + 1].append(v)

        self.print_group(layers)
        print()
        self.print_distance()


    def bfs_shortest_path(self, s, t):  # Breadth-First Search for looking for the shortest path in a graph by storing the nodes that a certain node was discovered from
        for i in range(self.n):
            self.dist.append(self.n)  # the biggest possible distance is the number of nodes - 1
            self.prev.append(None)

        self.dist[s] = 0  # zero distance to the starter node itself
        self.q.append(s)  # the starting node is the only one that gets into the queue prior to bfs

        while self.q:
            u = self.q.popleft()
            for v in self.adj[u]:
                if self.dist[v] == self.n:
                    self.q.append(v)
                    self.dist[v] = self.dist[u] + 1
                    self.prev[v] = u  # here we store the 'previous node, so we can backtrack it and find the shortest path

        return self.reconstruct_path(s, t)

    def reconstruct_path(self, s, t):  # we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
        result = deque()
        while t != s:
            result.appendleft(t)
            t = self.prev[t]
        result.appendleft(s)  # the loop above stops when reaching the start node, so if we want, we can include it here
        return result

    def print_group(self, group):  # we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
        for i in range(len(group)):
            print("to layer {0} belong nodes: ".format(i), end=' ')
            for j in group[i]:
                print(str(j) + " ", end=' ')
            print()

    def print_distance(self):  # we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
        for i in range(len(self.dist)):
            print("the node {0} is at layer: {1}".format(i, self.dist[i]))


def run_tests():
    adj_list = [[2], [2, 3], [1, 4, 5], [1], [7], [], [2], [5]]

    bfs_1 = MostDirectRoute(adj_list)
    bfs_1.bfs(0)
    print()

    bfs_2 = MostDirectRoute(adj_list)
    shortest_path = bfs_2.bfs_shortest_path(0, 7)
    print(shortest_path)

run_tests()
