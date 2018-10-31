package _08_FastestRoute;

public class Vertex // class to set up NODE (VERTEX) properties. fields must all be public as we use them in another class
{
    public boolean known = false; // flags if a VERTEX is confirmed to be safe (Dijkstra)
    public long dist = Integer.MAX_VALUE; // initializes the node distances
    public int path = -1;
}
