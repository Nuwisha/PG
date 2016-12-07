using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapDijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfRuns = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfRuns; i++)
            {
                string rawInput = Console.ReadLine();
                int numberOfCities = int.Parse(rawInput);
                Cities currentCities = new Cities(numberOfCities);
                currentCities.FillCityArray();
                int numberOfPaths = int.Parse(Console.ReadLine());
                for (int k = 0; k < numberOfPaths; k++)
                {
                    Console.WriteLine(currentCities.GetDistanceOverInputCities());
                }
                rawInput = Console.ReadLine(); // 1x new line of separation
            }

        }
    }

    class Cities
    {
        public DHeap BinaryHeap; // starting point, priority queue 
        public City[] cityArray; // end point, sure values
        public int NumberOfCities { get; set; }
        public Cities(int number)
        {
            NumberOfCities = number;
            NumberOfCities++; // city indexing starts at 1, ignore 0 slot or use as temp
            cityArray = new City[NumberOfCities];
            BinaryHeap = new DHeap(number);

        }
        public void FillCityArray()
        {
            cityArray[0] = new City();
            for (int i = 1; i < NumberOfCities; i++)
            {
                string name = Console.ReadLine();
                int neighbors_count = int.Parse(Console.ReadLine());
                cityArray[i] = new City(i, neighbors_count, name);
                // load info on neighbor/distance for current city
                for (int j = 0; j < neighbors_count; j++)
                {
                    string[] no_dist = Console.ReadLine().Split(' ');
                    cityArray[i].neighbors[j] = new Distance(int.Parse(no_dist[0]), int.Parse(no_dist[1]));
                }
                //this.BinaryHeap.pushElement(a);
                BinaryHeap.HighestHeapIndex++;
                BinaryHeap.heap[BinaryHeap.HighestHeapIndex] = cityArray[i];
                BinaryHeap.whereInHeap[i] = BinaryHeap.HighestHeapIndex;
            }
        }
        public void ResetDistances()
        {
            foreach (var ele in this.cityArray)
            {
                ele.ResetDistance();
            }
        }

        public int GetDistanceOverInputCities()
        {
            int result = 0;
            string[] path = Console.ReadLine().Split();
            string startingCity = path[0];
            string endCity = path[1];
            //int startIndex = this.cityArray.Where(x => x.Name == startingCity).ToArray()[0].CityIndexNumber;
            //int endIndex = cityArray.Where(x => x.Name == endCity).ToArray()[0].CityIndexNumber;
            int startIndex = 0;
            int endIndex = 0;
            for (int n = 1; n < this.NumberOfCities; n++)
            {
                if (this.cityArray[n].Name == startingCity)
                {
                    startIndex = n;
                }
                if (this.cityArray[n].Name == endCity)
                {
                    endIndex = n;
                }
            }
            this.ResetDistances();
            this.cityArray[startIndex].DistanceFromSource = 0;
            // reset heap 2/2
            this.BinaryHeap.buildHeap(); // this can be replaced with just moving source city to index 1 in heap // will REQ building heap 1x elsewhere
            for (int k = 1; k < this.NumberOfCities; k++) // do as many times as there are verticies in graph
            {
                City currentCity = this.BinaryHeap.popMinimumElement(); // get lowest distance element from MIN heap
                foreach (var element in currentCity.neighbors)
                {
                    if (this.cityArray[element.NeigborIndex].DistanceFromSource > currentCity.DistanceFromSource + element.DistanceToNeighbor)
                    {
                        this.cityArray[element.NeigborIndex].DistanceFromSource = currentCity.DistanceFromSource + element.DistanceToNeighbor;
                        this.BinaryHeap.restoreHeapPropertiesAfterDistanceUpdate(this.BinaryHeap.heap, this.BinaryHeap.whereInHeap[element.NeigborIndex]);
                    }
                }
            }
            result = cityArray[endIndex].DistanceFromSource;
            // reset heap 1/2
            this.BinaryHeap.HighestHeapIndex = this.BinaryHeap.TotalHeapSize - 1;

            return result;
        }
    }
    class City
    {
        public string Name { get; set; }
        public int HowManyNeighbors { get; set; }
        public int CityIndexNumber { get; set; }
        public int DistanceFromSource { get; set; }
        public Distance[] neighbors;
        public City(int selfindex = 0, int numberOfNeighbors = 0, string name = "")
        {
            this.Name = name;
            this.HowManyNeighbors = numberOfNeighbors;
            this.CityIndexNumber = selfindex;
            neighbors = new Distance[HowManyNeighbors];
            this.DistanceFromSource = int.MaxValue;
        }
        public void ResetDistance()
        {
            this.DistanceFromSource = int.MaxValue;
        }
    }
    class Distance
    {
        public int NeigborIndex { get; set; }
        public int DistanceToNeighbor { get; set; }
        public Distance(int neighbor, int distance)
        {
            this.NeigborIndex = neighbor;
            this.DistanceToNeighbor = distance;
        }
    }
    class DHeap
    {
        public int[] whereInHeap;
        public City[] heap;
        // root stored at index = 1
        public int HighestHeapIndex { get; set; }
        public int TotalHeapSize { get; set; } // this should be verticies + 1 to accomodate root@1
        public DHeap(int numberOfElements)
        {
            heap = new City[numberOfElements + 1];
            HighestHeapIndex = 0;
            TotalHeapSize = numberOfElements + 1;
            whereInHeap = new int[numberOfElements + 1]; // key: city index; points to: current index in heap array
        }
        public void Debug_PrintHeap()
        {
            int step = 1;
            Console.WriteLine("=============");
            Console.WriteLine("Heap table:");
            for (int i = 1; i < this.TotalHeapSize; i++)
            {
                if (heap[i].DistanceFromSource == int.MaxValue)
                {
                    Console.Write("max" + " ");
                }
                else
                {
                    Console.Write(heap[i].DistanceFromSource + " ");
                }
            }
            Console.WriteLine();
            for (int i = 1; i < HighestHeapIndex; i++)
            {
                if (heap[i].DistanceFromSource == int.MaxValue)
                {
                    Console.Write("max" + " ");
                }
                else
                {
                    Console.Write(heap[i].DistanceFromSource + " ");
                }
                if (i == step)
                {
                    Console.WriteLine();
                    step += (i + 1);
                }
            }
        }
        public int getLeftChildIndex(int i)
        {
            return 2 * i;
        }
        public int getRightChildIndex(int i)
        {
            return (2 * i) + 1;
        }
        public int getParentIndex(int i)
        {
            return i / 2;
        }
        public void restoreHeapProperties(City[] heap, int i)
        {
            int leftChild = getLeftChildIndex(i);
            int rightChild = getRightChildIndex(i);
            int min = i; // asume parent is the smallest element
            if (leftChild <= HighestHeapIndex && this.heap[i].DistanceFromSource > this.heap[leftChild].DistanceFromSource)
            {
                min = leftChild; // left child smaller than parent
            }
            if (rightChild <= HighestHeapIndex && this.heap[min].DistanceFromSource > this.heap[rightChild].DistanceFromSource)
            {
                min = rightChild; // right child smaller than parent
            }
            if (min != i)
            {
                City temp = this.heap[i];
                this.heap[i] = this.heap[min];
                this.heap[min] = temp;
                this.whereInHeap[this.heap[i].CityIndexNumber] = i;
                this.whereInHeap[this.heap[min].CityIndexNumber] = min;
                i = min;
                restoreHeapProperties(this.heap, i);
            }
        }
        public void restoreHeapPropertiesAfterDistanceUpdate(City[] heap, int i)
        {
            if (i <= HighestHeapIndex) // don't do visited, they're in the poped section of heap!
            {
                while (i > 1 && heap[i].DistanceFromSource < heap[getParentIndex(i)].DistanceFromSource)
                // swap with parent if updated node distance is smaller than parent's
                {
                    this.heap[0] = this.heap[i]; // temp
                    this.heap[i] = this.heap[getParentIndex(i)];
                    this.heap[getParentIndex(i)] = this.heap[0];
                    this.whereInHeap[this.heap[i].CityIndexNumber] = i;
                    this.whereInHeap[heap[getParentIndex(i)].CityIndexNumber] = getParentIndex(i);
                    i = getParentIndex(i);
                }
            }
        }
        public void buildHeap()
        {
            for (int i = HighestHeapIndex / 2; i >= 1; i--)
            {
                this.restoreHeapProperties(this.heap, i);
            }
        }
        public City popMinimumElement()
        {
            City minimum = this.heap[1];
            this.heap[1] = this.heap[HighestHeapIndex]; // swap with last element
            this.heap[HighestHeapIndex] = minimum;
            this.whereInHeap[this.heap[HighestHeapIndex].CityIndexNumber] = HighestHeapIndex;
            this.whereInHeap[this.heap[1].CityIndexNumber] = 1;
            HighestHeapIndex--;
            this.restoreHeapProperties(this.heap, 1);
            return minimum;
        }
        public void pushElement(City newElement)
        {
            HighestHeapIndex++;
            //this.resize();
            int i = HighestHeapIndex;
            this.heap[i] = newElement;
            while (i > 1 && newElement.DistanceFromSource < this.heap[getParentIndex(i)].DistanceFromSource)
            {
                City temp = this.heap[i];
                this.heap[i] = this.heap[getParentIndex(i)];
                this.heap[getParentIndex(i)] = temp;
                whereInHeap[this.heap[i].CityIndexNumber] = i;
                whereInHeap[this.heap[getParentIndex(i)].CityIndexNumber] = getParentIndex(i);
                i = getParentIndex(i);
            }
        }
        public void resize()
        {
            // this should never be used since the heap size will be that of all vertexes for current run
            // it's just for future practice
            if (HighestHeapIndex > TotalHeapSize)
            {
                int tempSize = TotalHeapSize;
                City[] temp = new City[TotalHeapSize];
                for (int i = 0; i < TotalHeapSize; i++)
                {
                    temp[i] = this.heap[i];
                }
                TotalHeapSize *= 2;
                this.heap = new City[TotalHeapSize];
                for (int i = 0; i < tempSize; i++)
                {
                    this.heap[i] = temp[i];
                }
            }
        }
    }
}
