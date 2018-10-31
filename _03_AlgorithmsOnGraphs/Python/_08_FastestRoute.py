""" Simple example with Naive and dijkstra algorithm on a directed graph G(V, E), based on the '_08_FastestRoute.JPG' """

from collections import deque

# EDGE RELAXATION:
# Observation: any sub-path of an optimal path is also optimal, which takes us to the following property: If S-> ..-> u-> t is the shortest path from S to then d(S, t) = d(S, u) + w(u, t)
# dist[v] will be an upper bound on the actual distance from S to v, unlike in BFS, this value will most likely be updated many times during the procedure
# the EDGE RELAXATION for an edge (u, v) checks whether going from S to v through u improves the current value of dist, it pertains to the last edge before t at a given stage


class Vertex:
    """ class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class """
    def __init__(self):
        self.known = False  # flags if a VERTEX is confirmed to be safe (dijkstra)
        self.dist = 2147483647  # initializes the node distances
        self.path = -1


class FastestRoute:
    def __init__(self, adjList, costList):
        self.adj = adjList  # ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        self.cost = costList  # list for storing the initial uncorrected costs
        self.nodes = [Vertex() for _ in range(len(adjList))]  # after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values

    def naive_fastest_route(self, s, t):  # based on iterative correction of distances from origin until there is nothing to update anymore
        self.nodes[s].dist = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
        is_changed = True

        while(is_changed):  # the 'do' loop stops as soon as there is an iteration with no further updates
            is_changed = False
            for u in range(len(self.adj)):
                for i in range(len(self.adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    v = self.adj[u][i]  # v stores the index of the node on the other end, as usual

                    if self.nodes[u].dist == 2147483647:  # if the starting node u hasn't been discovered yet, we just move on
                        continue
                    elif self.nodes[v].dist > self.nodes[u].dist + self.cost[u][i]:  # for visited neighbours we RELAX THE EDGES if possible
                        self.nodes[v].dist = self.nodes[u].dist + self.cost[u][i]
                        self.nodes[v].path = u
                        is_changed = True  # this variable will become True each time we change something

        self.display_values()
        return self.reconstruct_path(s, t)


    def dijkstra(self, s, t):  # based on a fact that the smallest distance to all available nodes is enough to move onto next node, the other nodes can only be bigger
        self.nodes[s].dist = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

        while True:
            u = self.extract_min()
            if u == -1 or all(map(lambda x: x.known, self.nodes)):  # first condition checks for non-connected nodes and the second if all nodes have been verified
                break

            for i in range(len(self.adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                v = self.adj[u][i]  # v stores the index of the node on the other end, as usual

                if self.nodes[v].dist > self.nodes[u].dist + self.cost[u][i]:  # for direct neighbours we RELAX THE EDGES if possible
                    self.nodes[v].dist = self.nodes[u].dist + self.cost[u][i]
                    self.nodes[v].path = u
            self.nodes[u].known = True  # this is how we can CHANGE THE PRIORITY

        self.display_values()
        return self.reconstruct_path(s, t)

    def extract_min(self):  # helper function that returns the element u index which has the smallest distance
        smallest = 2147483647
        index = -1
        for i in range(0, len(self.nodes), 1):
            if self.nodes[i].dist < smallest and self.nodes[i].known == False:
                smallest = self.nodes[i].dist
                index = i
        return index  # we always compare the distances, but we return the index

    def reconstruct_path(self, s, t):  # we have to pass instance members as parameters because they are not allowed inside STATIC METHODS
        result = deque()
        while t != s:
            result.appendleft(t)
            t = self.nodes[t].path
        result.appendleft(s)  # the loop above stops when reaching the start node, so if we want, we can include it here
        return result

    def display_values(self):  # testing function that prints all node properties
        for i in range(0, len(self.nodes), 1):
            print("node index: {0}, distance to the source: {1}, received from: {2}".format( i, self.nodes[i].dist, self.nodes[i].path))


def run_tests():
    adj_small = [[1, 2], [2], [], [0]]  # first case from the exercises
    cost_small = [[1, 5], [2], [], [2]]

    graph_small = FastestRoute(adj_small, cost_small)
    route_small = graph_small.naive_fastest_route(0, 2)
    print(route_small)
    print()

    graph_small_2 = FastestRoute(adj_small, cost_small)
    route_small_2 = graph_small_2.dijkstra(0, 2)
    print(route_small_2)
    print()


    adj_big = [[1, 2], [2, 3], [4, 5], [1], [], [1], [2], []]
    cost_big = [[9, 5], [2, 2], [9, 8], [5], [], [1], [4], []]

    graph_big = FastestRoute(adj_big, cost_big)
    route_big = graph_big.naive_fastest_route(0, 5)
    print(route_big)
    print()

    graph_big_2 = FastestRoute(adj_big, cost_big)
    route_big_2 = graph_big_2.dijkstra(0, 5)
    print(route_big_2)
    print()

run_tests()
