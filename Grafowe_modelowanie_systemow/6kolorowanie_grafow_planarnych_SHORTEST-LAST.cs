using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6planar
{
        class Vertex
    {
        public int VertexValue { get; set; } // assigned value
        public int VertexColor { get; set; } // assigned color
        public bool InGraph { get; set; }

        public List<int> vertexEdgeAndNeighborList;

        public Vertex(int value)
        {
            this.VertexValue = value;
            this.vertexEdgeAndNeighborList = new List<int>();
            this.VertexColor = 0;
            this.InGraph = true;
        }
        public int GetVertexDegree()
        {
            return this.vertexEdgeAndNeighborList.Count;
        }
        public void AddNeighbor(int neighborVertex)
        {
            this.vertexEdgeAndNeighborList.Add(neighborVertex);
        }

        public void PrintVertex()
        {
            Console.Write("Vertex: " + this.VertexValue + " : " + "Color: " + this.VertexColor);
            Console.Write(" : Neighbor list: ");
            foreach (var ele in this.vertexEdgeAndNeighborList)
            {
                Console.Write(ele + " ");
            }
            Console.WriteLine();
        }

    }
    class Graph
    {
        public int GraphVertexCount { get; set; }
        public Vertex[] vertexes;
        public int[] colorOrderSL;
        // vertexes are numbered from 0 to n-1
        // where n: GraphVertexCount

        public Graph(int howManyVerticies)
        {
            this.GraphVertexCount = howManyVerticies;
            this.vertexes = new Vertex[GraphVertexCount];
            for (int i = 0; i < GraphVertexCount; i++)
            {
                vertexes[i] = new Vertex(i);
            }
            // populate vertex array
            this.colorOrderSL = new int[this.GraphVertexCount];
        }

        public void AddNeighbors(int vertex, int neighbor)
        {
            vertexes[vertex].AddNeighbor(neighbor);
        }

        // indexer
        public Vertex this[int index]
        {
            get
            {
                if (index >= 0 && index < GraphVertexCount)
                {
                    return vertexes[index];
                }
                return new Vertex(-1);
                // non-existant vertex
            }
            set
            {
                if (index >= 0 && index < GraphVertexCount)
                {
                    vertexes[index] = value;
                }
            }
        }
        public void GenerateColorOrderSL()
        {
            // DEBUG // 
 //           this.PrintGraph();
            int[] neighborCount = new int[this.GraphVertexCount];
            for (int i = 0; i < this.GraphVertexCount; i++)
            {
                neighborCount[i] = this.vertexes[i].GetVertexDegree();
            }
 //           Console.Write("Current neighbor count: ");
 //           foreach (var ele in neighborCount)
 //           {
//                Console.Write(ele + " ");
 //           }
//            Console.WriteLine();
            for (int i = 0; i < GraphVertexCount; i++)
            {
                //int min = neighborCount.Min();  /// TO_DO: get minimum, but ONLY FROM the ones that are still InGraph==TRUE
                int min = int.MaxValue;
                int minIndex = -1;
                for (int k = 0; k < this.GraphVertexCount; k++)
                {
                    if (min > neighborCount[k] && this.vertexes[k].InGraph == true)
                    {
                        min = neighborCount[k];
                        minIndex = k;
                    }
                }
//                Console.WriteLine("Selected value: " + min + "at index: " + minIndex);
                this.colorOrderSL[GraphVertexCount - 1 - i] = minIndex;
                this.vertexes[minIndex].InGraph = false; // remove from further iterations
                // populate starting with the last slot 
                // (lowest deg() go to the end of array)
                // coloring starts with largest deg()
                foreach (var neighbor in vertexes[minIndex].vertexEdgeAndNeighborList)
                {
                    neighborCount[neighbor]--;
                }
  //              Console.Write("Current neighbor count: ");
  //              foreach (var ele in neighborCount)
  //              {
  //                  Console.Write(ele + " ");
  //              }
  //              Console.WriteLine();
            }
            // DEBUG //
  //          this.PrintGraph();
        }

        public void ColorGraph()
        {
            foreach (var number in colorOrderSL)
            {
                // see what are the neighbors colored with 
                // use the next available color
                int maxColors = 7; // use no more than 6 colors, use 0 slot in array for "no color assigned"
                bool[] six_colors = new bool[maxColors];
                for (int i = 0; i < maxColors; i++)
                {
                    six_colors[i] = false; // color not used
                }
                foreach (var neighbor in vertexes[number].vertexEdgeAndNeighborList)
                {
                    six_colors[vertexes[neighbor].VertexColor] = true;
                } // mark colors used by neighbors
                six_colors[0] = true; // WA for when all other verticies have a color, then this generates a false color 1
                int lowestFreeColor = 0;
                for (int i = 0; i < maxColors; i++)
                {
                    if (six_colors[i] == false)
                    {
                        if (i == 0) // neighbor has no color assigned
                        {
                            lowestFreeColor = 1;
                        }
                        else
                        {
                            lowestFreeColor = i;
                        }
                        break;
                    }
                }
                vertexes[number].VertexColor = lowestFreeColor;
  //              Console.WriteLine("Vertex number: " + vertexes[number].VertexValue);
    //            Console.Write("Available colors: ");
//                for (int i = 0; i < maxColors; i++)
  //              {
    //                Console.Write(i + " " + six_colors[i] + ",");
//                }
  //              Console.WriteLine();
    //            Console.WriteLine("Assigned color: " + lowestFreeColor);
//                Console.WriteLine("---------------");
            }
        }

        public void PopulateGraph()
        {
            // populate graph with data
            // {0,1},{1,2},{2,0}
            string currentInput = Console.ReadLine();
            string[] graphData = currentInput.Split(',');
            int j = 1;
            for (int i = 0; i < graphData.Length; i += 2, j += 2)
            {
                int firstNumber = int.Parse(graphData[i].Substring(1)); // {1
                int secondNumber = int.Parse(graphData[j].Substring(0, graphData[j].Length - 1)); // 2}
                this.AddNeighbors(firstNumber, secondNumber);
                this.AddNeighbors(secondNumber, firstNumber);
            }
        }

        public void PrintColoring()
        {
            foreach (var vertex in this.vertexes)
            {
                Console.WriteLine(vertex.VertexValue + " " + vertex.VertexColor);
            }
        }

        public void PrintGraph()
        {
            Console.WriteLine("=============");
            Console.WriteLine("Printing Graph:");
            foreach (var ele in this.vertexes)
            {
                ele.PrintVertex();
            }
            Console.WriteLine("Coloring order:");
            foreach (var ele in this.colorOrderSL)
            {
                Console.Write(ele + " ");
            }
            Console.WriteLine("=============");
        }
    }    
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfIterations = int.Parse(Console.ReadLine());
            for (int ii = 0; ii < numberOfIterations; ii++)
            {
                //read-in graph data 
                //Graph with 3 nodes and 3 edges:
                string[] currentInput = Console.ReadLine().Split();
                int nodes = -1;
                int edges = -1;
                foreach (var ele in currentInput)
                {
                    try
                    {
                        if (nodes >= 0)
                        {
                            edges = int.Parse(ele);
                        }
                        else
                        {
                            nodes = int.Parse(ele);
                        }
                    }
                    catch
                    {
                        // do nothing if ele not a number
                    }
                }
                // create graph
                Graph currentGraph = new Graph(nodes);
                // populate graph with data
                // {0,1},{1,2},{2,0}
                currentGraph.PopulateGraph();
                // run coloring algorithm
                    // move smallest deg(v) from graph to list until graph empty
                    // color elements in list order starting from the last added element
                currentGraph.GenerateColorOrderSL();
                currentGraph.ColorGraph();
                // print results
                currentGraph.PrintColoring();
            }
        }
    }
}