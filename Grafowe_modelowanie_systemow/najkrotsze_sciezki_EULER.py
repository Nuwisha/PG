class city():
    def __init__(self):
        self.name = ""
        self.index = -1
        self.paths = {} # KEY target city (indexed by city index starting at 1) VALUE: int, cost to get to target (edge value)
        self.distanceFromSource = 0
        self.isVisited = False

    def getIndex(self):
        return self.index

    def setIndex(self, index):
        self.index = index

    def setVisited(self, boolean):
        self.isVisited = boolean

    def getVisited(self):
        return self.isVisited

    def setDistanceFromSource(self, distance):
        self.distanceFromSource = distance

    def getDistanceFromSource(self):
        return self.distanceFromSource

    def setCityAsSource(self):
        self.distanceFromSource = 0
        self.isVisited = False

    def resetCitySettings(self):
        self.distanceFromSource = float('inf')
        self.isVisited = False

    def addPath(self, destination, cost):
        self.paths[int(destination)] = int(cost)

    def getCost(self, destination):
        return self.paths[int(destination)]

    def getDestinations(self):
        return self.paths.keys()

    def setName(self, name): # might not need name at all if name stored in dictionary name -> index
        self.name = name

    def getName(self):
        return self.name

    def __str__(self):
        return "{} -> keys: {}, values: {}, distance: {}".format(self.name, self.paths.keys(), self.paths.values(), self.distanceFromSource)

class cityList():
    def __init__(self):
        self.cities = {} # dic of city objects -> key: city.index, value city object
        self.cityNames = {} # when adding a city to self.cities also set city.index as value HERE and city.name as KEY here
        self.start = None
        self.end = None

    def dijkstra(self, startName, endName):
        """
1. Start @ source and set distance from source to 0, other to infinity.
2. Pick lowest distance value from vertices not yet visited
3. update distances to their neighbours (if path from s->u->v is shorter than s->v)
4. repeat untill ALL verticies have been visited
        """
        self.start = self.cities[self.cityNames[startName]]
        self.end = self.cities[self.cityNames[endName]]
        for val in self.cities.values():
            val.resetCitySettings()
        self.start.setCityAsSource()
        while True:
            citiesToVisit = list(filter(lambda x: x.getVisited() == False, self.cities.values())) # get list of not visitied verticies
            #print("len(citiesToVisit) = {}".format(len(citiesToVisit)))
            if len(citiesToVisit) == 0:
                break
            lowestCostCity = min(citiesToVisit, key=lambda x: x.getDistanceFromSource())
            for neighbour in lowestCostCity.getDestinations():
                neighbourCost = self.cities[neighbour].getDistanceFromSource()   # distance neighbour already has
                alternativePath = lowestCostCity.getDistanceFromSource() + lowestCostCity.getCost(neighbour) # path from source to neighbour via current
                # neighbours distance from source VS current point + path to neighbour from current point
                if neighbourCost > alternativePath:
                    self.cities[neighbour].setDistanceFromSource(alternativePath)
                #self.debugPrintDistances()
            lowestCostCity.setVisited(True)
        return self.end.getDistanceFromSource()
        
    def loadCities(self):
        numberOfCities = int(input())
        for i in range(numberOfCities):
            newCity = city()
            cityName = input()
            cityIndex = i + 1
            newCity.setName(cityName)
            newCity.setIndex(cityIndex)
            numberOfNeighbours = int(input())
            for j in range(numberOfNeighbours):
                neighbourElements = list(input().split(' '))
                newCity.addPath(int(neighbourElements[0]), int(neighbourElements[1]))
            self.cityNames[cityName] = newCity.getIndex()
            self.cities[newCity.getIndex()] = newCity

    def findPaths(self):
        numberOfPaths = int(input())
        for i in range(numberOfPaths):
            targetList = input().split(' ')
            print(self.dijkstra(targetList[0], targetList[1]))

    def debugPrintDistances(self):
        for ele in self.cities.values():
            print(ele, end = '; ')
            print(ele.getDistanceFromSource())
                    


def runInputProgram():
    numberOfTests = int(input())
    for i in range(numberOfTests):
        currentCityList = cityList()
        currentCityList.loadCities()
        currentCityList.findPaths()
        if i < numberOfTests - 1:
            input() # read the delimiting newline
        
runInputProgram()
