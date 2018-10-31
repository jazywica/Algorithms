""" Simple example showing an undirected graph's G(V, E) basic functionality, based on the 'UndirectedGraph.JPG' """


class Vertex:
    """ class to set up NODE (VERTEX) properties """
    def __init__(self):
        self.visited = False  # flags if a VERTEX was visited or not
        self.group = None  # keeps track of CONNECTED COMPONENTS within the graph
        self.previsit = None  # these two are for storing the order in which DFS went in and out each NODE
        self.postvisit = None


class Graph:
    """ class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests """
    def __init__(self, input_list):  # inside the constructor we commence all the preparatory work
        self.adj = input_list  # ADJACENCY LIST, which is a representation of our test graph containing EDGES (pointers) to other VERTICES
        self.nodes = [Vertex() for _ in range(len(input_list))]  # after we instantiate the 'nodes' we have to populate it with the 'Vertex' object to be ready
        self.counter = 0  # field for the 'group' function
        self.clock = 1  # field for the pre and post visiting functions
        self.order = []  # EXTRA ITEM for storing the order that NODES were looked at. It has to be instantiated here as it is an object and we don't put it in the constructor

    # INTERVALS can be either nested (one contained in the other) or disjoint (non-overlapping). Interleaved (overlapping over part of their lengths) cases are not possible
    # - Case 1 (nested): explore v while we are exploring u, we can no finished exploring u until we are done with v, therefore post(u) > post(v)
    # - Case 2 (disjoint): explore v after we finished exploring u, therefore post(u) < pre(v)
    def pre_visit(self, v):
        self.nodes[v].previsit = self.clock
        self.clock += 1  # two functions that should keep track of what the DFS is doing inside the GRAPH

    def post_visit(self, v):
        self.nodes[v].postvisit = self.clock
        self.clock += 1  # we note when a group opens and closes , so in this example everything between 7 and 16 is within a group that starts with '2' etc.

    def explore(self, v):
        """ recursive DFS method that looks around the given VERTEX and find all other connected VERTICES (direct and indirect) """
        self.nodes[v].visited = True  # before we do anything we first mark the current NODE as 'visited'
        self.nodes[v].group = self.counter  # at this point we can use the property called 'group' that will segregate all the reachable nodes into one group
        self.pre_visit(v)  # we first introduce the pre-visit method before we start the recursion

        for w in self.adj[v]:  # 'v' is a symbol of the current VERTEX and 'w' represents its neighbor
            if not self.nodes[w].visited:  # if we haven't explored the vertex yet, then we step into it
                self.order.append(w)
                self.explore(w)  # here we call 'Explore' recursively (DFS) before we even finish with looking at all the neighbors

        self.post_visit(v)  # we 'wrap up' the post-order as we go out of recursion

    def group_components(self):
        """ 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|) """
        self.counter = 1  # 'counter = 1' initializes the first group of connected nodes
        for v in range(len(self.nodes)):
            if not self.nodes[v].visited:
                self.order.append(v)
                self.explore(v)
                self.counter += 1  # when we finished with our recursive calls we know that all connected components have been discovered, so we increment the counter

        self.display_values()

    def display_values(self):  # testing function that prints all node properties
        for i in range(len(self.nodes)):  # use 'for' loop to have indexes available for display
            print("node index: {0}, belongs to group: {1}, visited order: {2}, pre: {3}, post: {4}".format(i, self.nodes[i].group, self.order[i], self.nodes[i].previsit, self.nodes[i].postvisit))
        

def run_tests():
    adj_list = [[4], [], [5, 6], [5], [0], [2, 3, 6, 7], [2, 5], [5]]

    graph1 = Graph(adj_list)
    graph1.explore(2)

    graph2 = Graph(adj_list)
    graph2.group_components()


run_tests()
