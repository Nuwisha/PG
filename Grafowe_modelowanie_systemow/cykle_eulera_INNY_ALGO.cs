/*
 *EXAMPLE DATA
 *
 *
2
n=6,m=9
{0,1} {0,3} {1,2} {1,3} {1,5} {2,5} {3,4} {3,5} {4,5}
n=4,m=4
{0,1} {0,3} {1,2} {2,3}
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace euler
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfIterations = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfIterations; i++)
            {
                // parse general graph data
                string currentInput;
                currentInput = Console.ReadLine(); // n=6,m=9
                int comaIndex = currentInput.IndexOf(',');
                int numberOfVerticies = int.Parse(currentInput.Substring(2, comaIndex - 2));
                int numberOfEdges = int.Parse(currentInput.Substring(comaIndex + 3));

                // create graph and load edge/vertex data
                Graph newGraph = new Graph(numberOfVerticies);
                newGraph.PopulateGraph();

                // run Euler Cycle detection
                /*
                 stos = {} ce = {}
                 stos.push(dowolny v)
                 while stos != {}:
                   v: stos.top
                   if deg(v) != 0:
                     u - neighbor with lowest index
                     G = (V(G), E(G) \ {{u,v}} \\ usun te krawedz
                     stos.push(u)
                    else (deg(v) == 0)
                     stos.pop(v)
                     ce.push(v)                  
                 */

                Stack mainStack = new Stack();
                Stack endStack = new Stack();

                mainStack.Push(newGraph[0]);
                while (mainStack.GetElementCount() != 0)
                {
                    Vertex v = mainStack.Top();
                    if (v.GetVertexDegree() != 0)
                    {
                        Vertex u = newGraph[v.LowestIndexNeighbor()];
                        mainStack.Push(u);
                        newGraph.RemoveEdge(v.VertexNumber, u.VertexNumber);
                    }
                    else if (v.GetVertexDegree() == 0)
                    {
                        Vertex toPush = mainStack.Pop();
                        endStack.Push(toPush);
                    }
                    else
                    {
                        Console.WriteLine("Done nothing");
                        Console.ReadLine();
                    }
                }

                // print vertexs in cyclic order
                endStack.PrintElements();
            }
        }
    }
    class Stack
    {
        public List<Vertex> vertexesInTheStack;

        public Stack()
        {
            vertexesInTheStack = new List<Vertex>();
        }

        public Vertex Pop()
        {
            if (vertexesInTheStack.Count == 0)
            {
                return new Vertex(-1); // return non-existent vertex
            }
            Vertex result = vertexesInTheStack.ElementAt(vertexesInTheStack.Count - 1);
            vertexesInTheStack.RemoveAt(vertexesInTheStack.Count - 1);
            return result;
        }

        public Vertex Top()
        {
            if (vertexesInTheStack.Count == 0)
            {
                return new Vertex(-1); // return non-existent vertex
            }
            Vertex result = vertexesInTheStack.ElementAt(vertexesInTheStack.Count - 1);
            return result;
        }

        public void Push(Vertex vertex)
        {
            vertexesInTheStack.Add(vertex);
        }

        public int GetElementCount()
        {
            return vertexesInTheStack.Count;
        }

        public void PrintElements()
        {
            foreach (var element in vertexesInTheStack)
            {
                Console.Write(element.VertexNumber + " ");
            }
            Console.WriteLine();
        }
    }
    class Vertex
    {
        public int VertexNumber { get; set; }

        public List<int> vertexNeighbors;

        public Vertex(int number)
        {
            this.VertexNumber = number;
            vertexNeighbors = new List<int>();
        }

        // indexer for Neighbors
        public int this[int index]
        {
            get
            {
                if (index >= 0 && index < vertexNeighbors.Count)
                {
                    return vertexNeighbors.ElementAt(index);
                }
                return -1; // non-existent vertex
            }
            set
            {
                if (index >= 0 && index < vertexNeighbors.Count)
                {
                    vertexNeighbors[index] = value;
                }
            }
        }

        public void AddNeighbor(int neighborNumber)
        {
            this.vertexNeighbors.Add(neighborNumber);
        }

        public int LowestIndexNeighbor()
        {
            int mini;
            mini = this.vertexNeighbors[0];
            foreach (var ele in this.vertexNeighbors)
            {
                if (ele < mini)
                {
                    mini = ele;
                }
            }
            return mini;
        }

        public void RemoveNeighbor(int neighboNumber)
        {
            for (int i = 0; i < vertexNeighbors.Count; i++)
            {
                if (vertexNeighbors[i] == neighboNumber)
                {
                    vertexNeighbors.RemoveAt(i);
                }
            }
        }

        public int GetVertexDegree()
        {
            return vertexNeighbors.Count;
        }

        public void PrintVertex()
        {
            Console.Write("Vertex: " + this.VertexNumber + ": ");
            foreach (var ele in vertexNeighbors)
            {
                Console.Write(ele + " ");
            }
            Console.WriteLine();
        }

    }
    class Graph
    {
        // public List<Vertex> Vertexes;
        public int VertexCount { get; set; } // number of vertexes in graph
        public Vertex[] vertexes; // vertex array 

        public Graph(int vertexNo)
        {
            //Vertexes = new List<Vertex>();
            VertexCount = vertexNo;
            vertexes = new Vertex[VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                vertexes[i] = new Vertex(i); // populate all vertexes with neighbour lists ON
                // vertexes are numbered from 0 to NoOfVertexes - 1
            }
        }
        public void AddNeighbor(int vertex, int neighbor)
        {
            vertexes[vertex].AddNeighbor(neighbor);
        }
        // could use an indexer ?
        public Vertex this[int index]
        {
            get
            {
                if (index >= 0 && index < VertexCount)
                {
                    return vertexes[index];
                }
                return new Vertex(-1); // return non-existant vertex
            }
            set
            {
                if (index >= 0 && index < VertexCount)
                {
                    vertexes[index] = value;
                }
            }
        }

        // remove edge
        public void RemoveEdge(int u, int v)
        {
            vertexes[u].RemoveNeighbor(v);
            vertexes[v].RemoveNeighbor(u);
        }

        // get data as described in excercise
        public void PopulateGraph()
        {
            string currentInput;
            // parse edge and vertecies data
            // {0,1} {0,3} {1,2} {1,3} {1,5} {2,5} {3,4} {3,5} {4,5}
            currentInput = Console.ReadLine();
            string[] inputElements = currentInput.Split();
            for (int i = 0; i < inputElements.Length - 1; i++) // {0,1}
            {
                int comaIndex = inputElements[i].IndexOf(',');
                int endBracketIndex = inputElements[i].IndexOf('}');
                int firstVertex = int.Parse(inputElements[i].Substring(1, comaIndex - 1));
                int secondVertex = int.Parse(inputElements[i].Substring(comaIndex + 1, endBracketIndex - comaIndex - 1));
                this.AddNeighbor(firstVertex, secondVertex);
                this.AddNeighbor(secondVertex, firstVertex);
            }
        }

        public void PrintGraph()
        {
            Console.WriteLine("Printing current Graph:");
            foreach (var element in vertexes)
            {
                Console.Write(element.VertexNumber + ": ");
                foreach (var ele in element.vertexNeighbors)
                {
                    Console.Write(ele);
                }
                Console.Write(" :: " + element.vertexNeighbors.Count);
                Console.WriteLine();
            }
            Console.WriteLine("===================");
        }

    }
}
