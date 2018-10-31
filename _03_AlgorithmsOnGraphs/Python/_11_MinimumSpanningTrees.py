""" Example showing Spanning Trees on an undirected graph G(V, E) with positive edges, based on the '_11_SpanningTrees_1.JPG' """

# TREE is an undirected graph that is connected and ACYCLIC
# TREE with 'n' vertices has n-1 edges
# Any connected undirected graph G(V,E) with |E| = |V|-1 is a TREE
# An undirected graph is a tree if there is an unique path between any pair of its vertices


class SpanningTrees:
    def __init__(self, adjList, costList):
        self.adj = adjList  # ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES:
        self.cost = costList  # list for storing the initial uncorrected costs
        self.distance = 0  # total distance of the minimum path

        self.labels = {0: "A", 1: "B", 2: "C", 3: "D", 4: "E", 5: "F"}  # just to present nodes the same way as in lectures
        self.nodes = set([frozenset([self.labels[i]]) for i in range(len(self.adj))])  # initializing the initial sets for kruskal as : A, B, C, D, E, F
        self.x = set()  # final output showing the picked edges

        self.priority_queue = [(i, 2147483647) for i in range(len(self.adj))]  # this queue is to keep track of the min. distances, we still need arrays as we can't address elements in sets directly
        self.dist = [2147483647 for _ in range(len(self.adj))]  # initializing distances for prim


    # I. Kruskal algorithm - doesn't require a start point, as it sorts and picks the shortest edges independently from vertices
    def kruskal(self):
        edges = self.make_edges()
        edges.sort(key=lambda e: e[0])  # Kruskal algorithm sorts all edges and then processes them independently

        for idx in range(len(edges)):
            u = self.find(self.labels[edges[idx][1]])
            v = self.find(self.labels[edges[idx][2]])
            if not u == v:  # if the start and the end nodes are not in the same set, then we merge them, if they are, joining would mean a CYCLE which we can't allow
                self.nodes.add(u.union(v))
                self.nodes.remove(u)
                self.nodes.remove(v)
                self.x.add(self.labels[edges[idx][1]] + self.labels[edges[idx][2]])
                self.distance += edges[idx][0]

        self.display_values()

        return self.distance

    def find(self, node):
        """ helper function that will return the set, in which the desired node currently is """
        for item in self.nodes:
            if node in item:
                return item
        return set([])


    def make_edges(self):  # helper function to convert adjacency list into single edges
        result = []
        duplicates = set()

        for i in range(len(self.adj)):
            for j in range(len(self.adj[i])):
                if not (str(i) + str(self.adj[i][j])) in duplicates:  # here we check if we are not taking any duplicates, we only need an edge once
                    duplicates.add(str(self.adj[i][j])+ str(i))
                    result.append([])
                    current = result[-1]
                    current.append(self.cost[i][j])
                    current.append(i)
                    current.append(self.adj[i][j])
        return result

    def display_values(self):  # testing function that prints all node properties
        print("the following edges were picked as the shortest (only True for kruskal):\n" + str(self.x))


    # 2. Prim's algorithm, implemented with a Priority Queue (modified SortedSet) - it requires a start point and works the same way as Dijkstra
    def prim(self, s):
        result = 0
        self.dist[s] = 0
        self.priority_queue[s] = (s, 0)
        self.priority_queue.sort(key=lambda e: e[1])  # each time we update the priority queue, we have to sort the list, this is not necessary if we start from 0

        while self.priority_queue:
            u = self.priority_queue.pop(0)[0]  # we need a handle for this element to update it later on

            for i in range(len(self.adj[u])):  # in this case we use indices inside the lists in order to maintain the relation between 'adj' and 'cost'
                v = self.adj[u][i]  # v stores the index of the node on the other end, as usual

                if self.dist[v] > self.cost[u][i] and (v, self.dist[v]) in self.priority_queue:  # since we are only comparing single distances, we can't look back, so we only take into account
                    idx = self.priority_queue.index((v, self.dist[v]))  # for direct neighbours we RELAX THE EDGES if possible
                    self.dist[v] = self.cost[u][i]

                    self.priority_queue[idx] = (v, self.cost[u][i])  # here we Change Priority by changing old to new values and sorting the whole list
                    self.priority_queue.sort(key=lambda e: e[1])

            result += self.dist[u]

        return result


def run_tests():
    adj = [[1, 3, 4], [0, 2, 4, 5], [1, 5], [0, 4], [0, 1, 3, 5], [1, 2, 4]]
    adj_cost = [[4, 2, 1], [4, 8, 5, 6], [8, 1], [2, 3], [1, 5, 3, 9], [6, 1, 9]]

    print("\nKruskal's Algorithm:")
    graph_kruskal = SpanningTrees(adj, adj_cost)
    route_kruskal = graph_kruskal.kruskal()
    print(route_kruskal)

    print("\nPrim's Algorithm:")
    graph_prim = SpanningTrees(adj, adj_cost)
    route_prim = graph_prim.prim(2)  # we start from the same node is the same as node C in the picture
    print(route_prim)


run_tests()
