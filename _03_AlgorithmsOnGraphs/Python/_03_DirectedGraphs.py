""" Simple example checking ACYCLICITY and performing TOPOLOGICAL SORT, based on the 'DirectedGraph.JPG' """

from collections import deque

# CYCLES - a 'cycle' in a graph G is a sequence of vertices v1,v2,v2..vn so that (v1, v2), (v2, v3)....,(vn-1, vn),(vn, v1) area all edges. Which means that vertices are connected in a loop
# *Any graph that contains a cycle v1,...vn. can not be LINEARLY ORDERED, because if a start vertex vk comes first then vk comes before vk-1 and it is a contradiction
# DIRECTLY ACYCLIC GRAPH (DAG) is a a graph with no cycles and therefore can be linearly ordered
# DAG has two characteristic components: SOURCE (vertex with no incoming edges) and SINK (vertex with no outgoing edges)


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
        self.cycle_value = 0 # field for storing a potential cycle inside a group
        self.cyclic = False  # flags if we have found a a cycle in a graph
        self.order = deque()  # stack for the TOPOLOGICAL SORT

    def explore(self, v):  # recursive DFS method that looks around the given VERTEX and finds all other connected VERTICES (direct and indirect)
        if self.cycle_value in self.adj[v]:  # part of the 'isCyclic()' procedure which checks if the value was present
            self.cyclic = True
            return

        self.nodes[v].visited = True
        self.nodes[v].group = self.counter
        self.pre_visit(v)

        for w in self.adj[v]:
            if not self.nodes[w].visited:
                self.explore(w)  # here we call 'explore' recursively (DFS) before we even finish with looking at all the neighbors

        self.post_visit(v)
        self.order.appendleft(v)  # here we push the values on a stack for the TOPOLOGICAL SORT while we are backtracking

    def pre_visit(self, v):  # two functions that should keep track of what the DFS is doing inside the GRAPH
        self.nodes[v].previsit = self.clock
        self.clock += 1

    def post_visit(self, v):
        self.nodes[v].postvisit = self.clock
        self.clock += 1

    def group_components(self):  # 'DFS' procedure displaying all the VERTICES in the GRAPH, split into groups. Runtime: O(|V| + |E|)
        counter = 1  # 'counter = 1' initializes the first group of connected nodes
        for v in range(0, len(self.nodes), 1):
            if not self.nodes[v].visited:
                self.explore(v)
                counter += 1  # when we finished with our recursive calls we know that all connected components have been discovered, we increment the counter

        self.display_values()

    def is_cyclic(self):  # function stores the value we are starting with and runs 'explore' to check if it is present in all connected components
        for v in range(0, len(self.nodes), 1):
            self.cycle_value = v
            if not self.nodes[v].visited:
                self.explore(v)

        if self.cyclic == True:
            return True
        else:
            return False

    def topological_sort(self):  # function uses STACK, as we start placing the elements from the end to the front
        for v in range(0, len(self.nodes), 1):
            if not self.nodes[v].visited:
                self.explore(v)
        return self.order

    def display_values(self):  # testing function that prints all node properties
        for i in range(0, len(self.nodes), 1):
            print("node index: {0}, belongs to group: {1}, pre: {2}, post: {3}".format(i, self.nodes[i].group, self.nodes[i].previsit, self.nodes[i].postvisit))


def run_tests():
    adj_dag = [[1, 2], [2, 3, 5, 6], [4, 5, 6], [5], [6], [], [7], []]
    adj_cyclic = [[1], [], [3], [4], [5], [2]]  # 0-> 1,  2-> 3-> 4-> 2

    dag_1 = Graph(adj_dag)
    cyclic_1 = Graph(adj_cyclic)
    dag_1.group_components()  # grouping components works just the same way as with undirected graph
    print()
    cyclic_1.group_components()
    print()

    dag_2 = Graph(adj_dag)
    cyclic_2 = Graph(adj_cyclic)
    print(dag_2.is_cyclic())  # recognizes what is cyclic and what is acyclic
    print(cyclic_2.is_cyclic())
    print()

    dag_3 = Graph(adj_dag)
    topo = dag_3.topological_sort()
    print(topo)  # the product of a topological sort may not be the same in all cases, all depends on the order of elements in the adjacency lists


run_tests()
