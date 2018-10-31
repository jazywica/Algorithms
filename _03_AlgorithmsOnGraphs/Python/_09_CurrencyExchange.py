""" Simple example with Bellman-Ford algorithm and Negative Cycles on a directed graph G(V, E) and negative weights, based on the _09_CurrencyExchange.JPG """

from collections import deque
import math

# ARBITRARY IN CURRENCY EXHCANGE
# Conversions use multiplications of pair rates: x*y = 2^(log2(x)) * 2^(log2(y)) = 2^(log2(x) + log2(y)) but we can use a simple log property and turn it to a sum of numbers instead
# to maximize result 4 * 1 * 0.5 = 2 = 2^1 we use a sum of logarithms: log2(4)+log2(1)+log2(0.5) = 2 + 0 + -1 = 1 which is the exponent of the result we got from numbers
# to minimize a sum we can do the following: min = - SUM(log(xi)) or better: SUM(-log(xi)), so we have to convert each conversion rate to: -log2(x)

# Assume that a cycle ci -> cj -> ck -> ci has negative weight. This means that -(log cij + log cjk + log cki) < 0 and hence log cij + log cjk + log cki > 0.
# This, in turn, means that: rij rjk rki = 2log cij 2log cjk2log cki = 2log cij+log cjk+log cki > 1 - this means that we take out more than we put in.
# Negative cycles have a (logarithmic) negative sum, which is easy to detect, as it endlessly decreases the numbers within it.


class Vertex:
    """ class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class """
    def __init__(self):
        self.known = False  # flags if a VERTEX is visited
        self.dist = 2147483647  # initializes the node distances
        self.path = -1  # stores the previous node for the fastest path
        self.path_bfs = -1  # stores the previous node for the shortest path


class Bellman_Ford:
    def __init__(self, adjList, costList):
        self.adj = adjList  # ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        self.cost = costList  # list for storing the initial uncorrected costs
        self.nodes = [Vertex() for _ in range(len(self.adj))]  # after we instantiate the 'nodes' we have to populate it with the 'Vertex' objects with default values
        self.cycle = set([])  # stores only the cycle


    def bellman_ford(self, s, t):  # based on iterative correction of distances from origin until there is nothing to update anymore
        self.nodes[s].dist = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly

        for iter in range(len(self.adj)):  # theoretically it is enough to run |V| - 1 iterations on all edges to be sure that the full optimization of distances has been made
            for u in range(len(self.adj)):  # here we start running and optimizing all edges in a graph
                for i in range(len(self.adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    v = self.adj[u][i]  # v stores the index of the node on the other end, as usual

                    if self.nodes[u].dist == 2147483647:  # if the starting node u hasn't been discovered yet, we just move on
                        continue
                    elif self.nodes[v].dist > self.nodes[u].dist + self.cost[u][i]:  # for visited neighbours we RELAX THE EDGES if possible
                        self.nodes[v].dist = self.nodes[u].dist + self.cost[u][i]
                        self.nodes[v].path = u

        self.display_values()
        return self.reconstruct_path(s, t)

    def reconstruct_path(self, s, u):
        result = deque()
        while u != s:
            result.appendleft(u)
            u = self.nodes[u].path
        result.appendleft(s)  # the loop above stops when reaching the start node, so if we want, we can include it here
        return result


    def negative_cycles(self, s, t):  # based on iterative correction of distances from origin to detect if there are negative cycles in the graph
        self.nodes[s].dist = 0  # the only non-max value to start with is going to be the start node, so when we start scanning edges we will start updating distances from the start implicitly
        n = len(self.adj)
        result = deque()
        A_nodes = set([])  # stores the set A of all affected nodes. since we only want to add a node once, it is best to use a set

        # 1. Bellman-Ford that collects all the affected by the cycle nodes
        for iter in range(n):  # theoretically it is enough to run |V| - 1 iterations, so if something changes on the |V|th then we detected a cycle
            for u in range(n):  # here we start running and optimizing all edges in a graph
                for i in range(len(self.adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                    v = self.adj[u][i]  # v stores the index of the node on the other end, as usual

                    if self.nodes[u].dist == 2147483647:  # if the starting node u hasn't been discovered yet, we just move on
                        continue
                    elif self.nodes[v].dist > self.nodes[u].dist + self.cost[u][i]:  # for visited neighbours we RELAX THE EDGES if possible
                        self.nodes[v].dist = self.nodes[u].dist + self.cost[u][i]
                        self.nodes[v].path = u  # here we store the fastest path

                        if iter == n - 1:
                            self.cycle.add(u)  # this should pick up the cycle itself
                            A_nodes.add(u)
                            for k in self.adj[u]:  # this should pick up all affected nodes
                                A_nodes.add(k)

        # 2. BFS for checking if the end currency is reachable from the cycle
        q = deque(list(self.cycle))  # here we convert the SET into a QUEUE for BFS
        while q:  # Breadth-First Search for looking for the shortest path in a graph by storing the nodes that the current node was discovered from
            u = q.popleft()
            for v in self.adj[u]:
                if self.nodes[v].known == False:
                    q.append(v)
                    self.nodes[v].known = True
                    self.nodes[v].path_bfs = u  # here we store the shortest path for comparison

        result = self.reconstruct_arbitrage(t)

        # 3. BFS for checking if the cycle is reachable from the start
        q = deque([s])
        for i in range(len(self.nodes)):
            self.nodes[i].known = False

        while q:
            u = q.popleft()
            if u == result[0]:
                print("Yes, there is a path with an infinite cycle !!!\n")  # actually it is enough to get to the first element of the cycle
                break

            for v in self.adj[u]:
                if not self.nodes[v].known:
                    q.append(v)
                    self.nodes[v].known = True
                    self.nodes[v].path_bfs = u  # here we store the shortest path for comparison

        print("Values after BFS:")
        self.display_values()
        return result

    def reconstruct_arbitrage(self, u):
        result = deque()
        while u != -1:
            if u in self.cycle:
                result.appendleft(u)
                return result
            result.appendleft(u)
            u = self.nodes[u].path_bfs
        return deque()


    def display_values(self):  # testing function that prints shortest paths and compares fastest and shortest paths
        for i in range(0, len(self.nodes), 1):
            print("node index: {0}, distance to the source: {1}, fastest path: {2}, shortest path: {3}".format(i, self.nodes[i].dist, self.nodes[i].path, self.nodes[i].path_bfs))


def run_tests():
    # Currency Exchange Example: We can either directly exchange RUR to USD with 0.015 rate or exchange it via EUR with rates 0.013 and 1.16
    def exchange_example(RUR_USD, RUR_EUR, EUR_USD):
        print("Single conversion to USD: {0}, conversion via EUR: {1}".format( RUR_USD, RUR_EUR * EUR_USD))  # this means that it is better to buy EUR first and then change it to USD
        R_U = -math.log(RUR_USD, 2)
        R_E = -math.log(RUR_EUR, 2)
        E_U = -math.log(EUR_USD, 2)
        print("Logarithmic values for single rates, conversion to USD: {0:0.4f}, to EUR: {1:0.4f}, to EUR: {2:0.4f}".format(R_U, R_E, E_U))
        print("Logarithmic sums of two alternative exchanges, direct conversion to USD: {0:0.4f}, to USD via EUR: {1:0.4f}".format(R_U, R_E + E_U))  # now we can add the sum

    exchange_example(0.015, 0.013, 1.16)  # testing numbers from the video
    print()


    # To solve the exchange problem we have to use the naive solution that scans all edges separately in order to handle negative edges (which we need to
    # Dijkstra algorithm doesn't work for negative edges as it relies on a fact that the shortest path goes through vertices that are closer to the start and can not predict for negative edges
    adj_dag = [[1, 2], [2, 3], [3, 4], [4], []]
    adj_dag_cost = [[4, 3], [-2, 4], [-3, 1], [2], []]

    graph = Bellman_Ford(adj_dag, adj_dag_cost)
    route = graph.bellman_ford(0, 3)
    print(route)

    print("\nNegative cycles:")
    adj_cyclic = [[1, 2], [2, 3, 7], [3, 4], [6], [3, 5], [2, 8], [], [3], []]
    adj_cyclic_cost = [[4, 3], [-2, 4, 4], [-3, 1], [1], [2, 2], [-5, 1], [], [1], []]

    graph_cyclic = Bellman_Ford(adj_cyclic, adj_cyclic_cost)
    routeCyclic = graph_cyclic.negative_cycles(0, 6)
    print(routeCyclic)


run_tests()
