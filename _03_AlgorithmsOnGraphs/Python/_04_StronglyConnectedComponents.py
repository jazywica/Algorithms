""" Simple example showing Connectivity in DIRECTED GRAPHS, based on the 'connected_components.JPG' """

from collections import deque

# CONNECTION - Two vertices 'v' and 'w' in a directed graph are connected if you can reach 'v' from 'w' and can reach 'w' from 'v'
# STRONGLY CONNECTED COMPONENTS - a partition of a directed graph, where two vertices are connected if and only if they are in the same component. Once you leave the SCC you can't come back
# METAGRAPH - is a way to show how the SCC connect to one another, so it always becomes a DAG because the SCC has been 'compressed' into partitions, so there are no cycles anymore


class Vertex:
    """ class to set up NODE (VERTEX) properties """
    def __init__(self):
        self.visited = False  # flags if a VERTEX was visited or not
        self.group = None  # keeps track of CONNECTED COMPONENTS within the graph
        self.previsit = None  # these two are for storing the order in which DFS went in and out each NODE
        self.postvisit = None


class Graph:
    """ class members are dynamic because we are testing a single instance of a class, if they were static the primary values would last through other tests """
    def __init__(self, inputList, inputList_rev):  # inside the constructor we commence all the preparatory work
        self.adj = inputList
        self.adj_rev = inputList_rev
        self.nodes = [Vertex() for _ in range(len(inputList))]  # after we instantiate the 'nodes' we have to populate it with the 'Vertex' object to be ready
        self.nodes_rev = [Vertex() for _ in range(len(inputList))]
        self.post_value = deque()  # order of the post-visit VERTICES we are going to follow
        self.post_index = deque()
        self.groups = []  # stores a list of SCC grouped also into lists
        self.g = []  # container for elements in each group
        self.counter = 0  # field for the 'group' function
        self.clock = 1  # field for the pre and post visiting functions

    def explore(self, v):  # the standard 'explore' function is just to aid the DFS procedure of the REVERSED GRAPH
        self.nodes_rev[v].visited = True
        self.nodes_rev[v].group = self.counter
        self.pre_visit(v)

        for w in self.adj_rev[v]:
            if not self.nodes_rev[w].visited:
                self.explore(w)

        self.post_value.appendleft(self.clock)  # here we note the post-order as we are backtracking
        self.post_index.appendleft(v)
        self.post_visit(v)

    def pre_visit(self, v):  # these two functions here are supposed to keep track of what the DFS is doing inside the GRAPH
        self.nodes_rev[v].previsit = self.clock
        self.clock += 1

    def post_visit(self, v):
        self.nodes_rev[v].postvisit = self.clock
        self.clock += 1

    def depth_first_search(self):  # 'DFS' in this case is only cover the REVERSED graph search for a SINK VERTEX
        counter = 1
        for v in range(0, len(self.nodes_rev), 1):
            if not self.nodes_rev[v].visited:
                self.explore(v)
                counter += 1  # when we finished with our recursive calls and we are on the way out, we increment the counter for the group
        self.displaying_values()

    def connected_components(self):  # in order to get the SCCs right, we have to run 'explore' procedures on the postorder values, so each time we go through the SCC we start from a SINK VERTEX
        self.depth_first_search()  # before we start anything, we need to run DFS on the REVERSED Graph in order to find the SINK VERTICES, for the SCC procedure to work
        counter = 0

        for v in self.post_index:  # here we go by the whole list of recorded post-visit values that we obtained from DFS
            if not self.nodes[v].visited:
                self.explore_components(v)
                counter += 1
                self.groups.append(list(self.g))  # we have to use the 'To.List()' method to pass the list as a VALUE, otherwise we will clear it in the next line and therefore won't change anything
                self.g.clear()

        self.print_group()
        return counter

    def explore_components(self, v):  # version of the 'explore' procedure for SCC
        self.nodes[v].visited = True

        for w in self.adj[v]:
            if not self.nodes[w].visited:
                self.explore_components(w)

        self.g.append(v)  # here we append all the elements of the group while backtracking

    def displaying_values(self):  # testing procedure for displaying the details of the REVERSED DFS
        for i in range(0, len(self.nodes), 1):
            print("node index: {0}, belongs to group: {1}, pre: {2}, post: {3}".format( i, self.nodes_rev[i].group, self.nodes_rev[i].previsit, self.nodes_rev[i].postvisit))

        for i in self.post_value:
            print(i, end=' ')
        print()
        for i in self.post_index:
            print(i, end=' ')
        print()
        print()

    def print_group(self):
        for i in self.groups:
            for j in i:
                print(j, end=' ')
            print()

    @staticmethod
    def reverse_graph(lst):
        reversed_list = [[] for _ in range(len(lst))]

        for i in range(len(lst)):
            for element in lst[i]:
                reversed_list[element].append(i)

        return reversed_list


def run_tests():
    adj_list = [[1], [2, 3], [0, 1, 4, 6], [0], [0, 2], [3], [], [3, 4, 5]]
    adj_list_rev = Graph.reverse_graph(adj_list)

    dag = Graph(adj_list, adj_list_rev)
    print(dag.connected_components())

run_tests()
