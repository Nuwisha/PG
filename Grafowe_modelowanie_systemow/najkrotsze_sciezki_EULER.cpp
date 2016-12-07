#include <iostream>
#include <string>
#include <limits>

using namespace std;

int main()
{
	int numberOfTests; // how many times to run code (#of Test Cases)
	int numberOfCities; // how many cities per Test Case 

	string* cityNames; // array for: city -> name
	int*** cityNeighbors; // 3d array for: city -> neighbor array -> neighbor data
	int** distances; // 2d array for: city -> distance + visited (2 cell array)
	int* howManyNeighbors; // array for: city -> how many neighbors (for for loops)

	string startCity;
	string endCity;
	string cityName;
	int numberOfNeighbors;
	int numberOfPaths;

	cin >> numberOfTests;
	for (int ii = 0; ii < numberOfTests; ii++){
		cin >> numberOfCities;
		cityNames = new string[numberOfCities];
		cityNeighbors = new int**[numberOfCities];
		distances = new int*[numberOfCities];
		howManyNeighbors = new int[numberOfCities];
		// LOAD CITIES DATA
		for (int jj = 0; jj < numberOfCities; jj++){
			distances[jj] = new int[2]; // [0]:distance [1]: isVisited -> 0: no, 1: yes
			cin >> cityName;
			cityNames[jj] = cityName;
			cin >> numberOfNeighbors;
			cityNeighbors[jj] = new int*[numberOfNeighbors];
			howManyNeighbors[jj] = numberOfNeighbors;
			for (int kk = 0; kk < numberOfNeighbors; kk++){
				cityNeighbors[jj][kk] = new int[2];
				cin >> cityNeighbors[jj][kk][0]; 
				cityNeighbors[jj][kk][0]--; // acount for cities numbered from 1 not 0
				cin >> cityNeighbors[jj][kk][1]; // distance to neighbor city
			}
		}
		// LOAD PATH DATA AND RUN DIJKSTRA ON THEM
		cin >> numberOfPaths;
		for (int nn = 0; nn < numberOfPaths; nn++){
			cin >> startCity;
			cin >> endCity;
			int startIndex = 0;
			int endIndex = 0;
			int lowestDistance;
			int lowestDistanceIndex;
			for (int ll = 0; ll < numberOfCities; ll++){
				if (cityNames[ll] == startCity) {
					startIndex = ll;
				}
				if (cityNames[ll] == endCity) {
					endIndex = ll;
				}
				// reset all distances and visited
				distances[ll][0] = numeric_limits<int>::max(); // set distance as 'infinity'
				distances[ll][1] = 0; // nothing visited yet
			}
			distances[startIndex][0] = 0; // set source
			for (int tt = 0; tt < numberOfCities; tt++){ // go through all verticies
				lowestDistance = numeric_limits<int>::max();
				for (int uu = 0; uu < numberOfCities; uu++){ // find lowest value, not visited vertix
					if (distances[uu][1] == 0 && distances[uu][0] < lowestDistance){
						lowestDistance = distances[uu][0];
						lowestDistanceIndex = uu;
					}
				}
				for (int mm = 0; mm < howManyNeighbors[lowestDistanceIndex]; mm++){ // for each neighbor
					int neighborIndex = cityNeighbors[lowestDistanceIndex][mm][0];
					int neighborDistance = cityNeighbors[lowestDistanceIndex][mm][1];
					if (distances[neighborIndex][0] > (distances[lowestDistanceIndex][0] + neighborDistance)){
						distances[neighborIndex][0] = (distances[lowestDistanceIndex][0] + neighborDistance); // update to shorter
					}
				}
				distances[lowestDistanceIndex][1] = 1; // mark city as visited
			}
			cout << distances[endIndex][0] << endl;
		}
		// clean up the memory
		delete[] cityNames;
		for (int i = 0; i < numberOfCities; i++){
			delete[] distances[i];
			for (int j = 0; j < howManyNeighbors[i]; j++){
				delete cityNeighbors[i][j];
			}
		}
		delete[] cityNeighbors;
		delete[] distances;
		delete[] howManyNeighbors;
	}
	return 0;
}