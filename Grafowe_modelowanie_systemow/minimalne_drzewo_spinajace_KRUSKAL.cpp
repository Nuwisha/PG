#include <iostream>
#include <string>
#include <stdlib.h>

using namespace std;

struct Edge{
	int s_vertex, e_vertex, edge_val;
};

int main(){
	int numberOfTests;
	int numberOfVerticies;
	int numberOfEdges;
	string currentInput;
	bool* visited;
	int sumOfMinTree;
	Edge* temp;
	Edge** edges;
	bool * vertexEdges;
	int result;

	cin >> numberOfTests;
	for (int kk = 0; kk < numberOfTests; kk++){
		// grab No. of edges and verticies into string and then strip values
		// n=6,m=9
		result = 0;
		cin >> currentInput;
		int coma = currentInput.find(',');
		string n_verticies = currentInput.substr(2, (coma - 2));
		string n_edges = currentInput.substr((coma + 3));
		numberOfVerticies = atoi(n_verticies.c_str());
		numberOfEdges = atoi(n_edges.c_str());
		
		// init visited
		visited = new bool[numberOfVerticies];
		for (int jj = 0; jj < numberOfVerticies; jj++) {
				visited[jj] = false;
		}
		// init tracking od candidate edges
		vertexEdges = new bool[numberOfEdges];
		for (int hh = 0; hh < numberOfEdges; hh++){
			vertexEdges[hh] = false;
		}
		// init array of edges:
		edges = new Edge*[numberOfEdges];
		// load edges from input:
		// {0,1}1 {0,5}3 {1,2}9 {1,3}7 {1,5}5 {2,3}8 {3,4}5 {3,5}2 {4,5}4
		for (int mm = 0; mm < numberOfEdges; mm++){
			temp = new Edge();
			string input;
			cin >> input;
			int s_bracket = input.find('{');
			int e_bracket = input.find('}');
			int coma = input.find(',');
			string numA = input.substr((s_bracket + 1), (coma - 1));
			string numB = input.substr((coma + 1), (e_bracket - coma - 1));
			string numC = input.substr((e_bracket + 1));
			int num1 = atoi(numA.c_str());
			int num2 = atoi(numB.c_str());
			int num3 = atoi(numC.c_str());
			temp->s_vertex = num1;
			temp->e_vertex = num2;
			temp->edge_val = num3;
			edges[mm] = temp;
		}
		// selection sort edges by weight
		for (int oo = 0; oo < numberOfEdges; oo++){
			int pp = oo + 1;
			while (pp < numberOfEdges) {
				if (edges[oo]->edge_val > edges[pp]->edge_val){
					temp = edges[oo];
					edges[oo] = edges[pp];
					edges[pp] = temp;
				}
				pp++;
			}
		}
		// go through all vertices
		// for each vertex find all edges that have that vertex as s_ver or e_ver
		// make list of all those edges [selection sort has them sorted already by weight!]
		// verify if other vertex is marked as VISITED - TRUE - if so - reject edge
		// else: add edge weight to min. spanning tree
		// mark other vertex as visited
		// when done - retune sum of weights of min. spanning tree
		int currentVertex = 0;
		int otherVertex = 0;
		for (int yy = 0; yy < numberOfVerticies; yy++){
			// mark currentVertex as visited = added to Tree
			visited[currentVertex] = true;
			for (int tt = 0; tt < numberOfEdges; tt++){
				// mark all edges that are connected to current vertex - add them to que
				if (edges[tt]->e_vertex == currentVertex && !visited[edges[tt]->s_vertex]){
					vertexEdges[tt] = true;
				}
				else if (edges[tt]->e_vertex == currentVertex && visited[edges[tt]->s_vertex]){
					vertexEdges[tt] = false; // reject edge, other vertex already in tree
				}
				else if (edges[tt]->s_vertex == currentVertex && !visited[edges[tt]->e_vertex]){
					vertexEdges[tt] = true;
				}
				else if (edges[tt]->s_vertex == currentVertex && visited[edges[tt]->e_vertex]){
					vertexEdges[tt] = false;
				}
			}
			// select lowest cost edge (it's already sorted! so the 1st TRUE in vertexEdges with other edge not in tree (visited FALSE) is OK
			for (int aa = 0; aa < numberOfEdges; aa++) {
				if (vertexEdges[aa] == true) {
					result += edges[aa]->edge_val;
					vertexEdges[aa] = false;
					if (visited[edges[aa]->e_vertex]) {
						otherVertex = edges[aa]->s_vertex;
					}
					else {
						otherVertex = edges[aa]->e_vertex;
					}
					break;
				}
			}
			// chage currentVertex to other vertex from the edge of the lowest cost
			currentVertex = otherVertex;
			visited[otherVertex] = true;
		}
		cout << result << endl;

		// CLEAN UP - delete visited, delete edges. 
		delete[] vertexEdges;
		delete[] visited;
		for (int ee = 0; ee < numberOfEdges; ee++){
			delete edges[ee];
		}
		delete[] edges;
	}
	return 0;
}