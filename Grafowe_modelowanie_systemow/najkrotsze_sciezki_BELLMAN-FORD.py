def getGraphsFromInput():
        numberOfGraphs = int(input())
        for i in range(numberOfGraphs):
            numberOfVerticies = int(input())
            lol = []
            for j in range(numberOfVerticies):
                tempL = list(input().split(' '))
                tempL = tempL[:-1]
                tempL = [int(x) for x in tempL]
                lol.append(tempL)
            yield (lol, numberOfVerticies)


def iterateOverSingleGraphFromInput(graph, verticies):
    distances = [] # from s 
    for i in range(verticies):
        distances.append(float('inf'))
    distances[0] = 0
    
    for i in range(verticies - 1):
        for j in range(len(graph)):  # u
            for k in range(len(graph[j])):  # v
                if graph[j][k] == 0:
                    pass # there is no connection in this place, don't evaluate
                elif distances[k] > distances[j] + graph[j][k]:
                    distances[k] = distances[j] + graph[j][k]
        
        for ele in distances:
            if ele == float('inf'):
                ele = 0
            print(str(ele), end = " ")
        print('\n', end = '')
    print('\n', end= '')

    # verify for negative cycles!

def runInputBasedProgram():
    generator = getGraphsFromInput()
    while True:
        try:
            graph, verticies = generator.__next__()
            iterateOverSingleGraphFromInput(graph,verticies)
        except StopIteration:
            break

runInputBasedProgram()
