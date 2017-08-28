#include "WitnessGraph.h"

WitnessGraph::WitnessGraph(int row, int column):row(row),column(column),StartPoint(nullptr),EndPoint(nullptr) {
	map = new Node*[row+1];
	for (int i = 0; i <= row; i++) {
		map[i] = new Node[column+1];
	}
	for (int i = 0; i <= row; i++) {
		for (int j = 0; j <= column; j++) {
			map[i][j].set(i, j);
			NodeInitOfEdge(i,j);
		}
	}
	SquareMap = new Square*[row];
	for (int i = 0; i < row; i++) {
		SquareMap[i] = new Square[column];
	}
	for (int i = 0; i < row; i++) {
		for (int j = 0; j < column; j++) {
			SquareMap[i][j].SetXY(i, j);
			SquareInit(i, j);
		}
	}
}

WitnessGraph::~WitnessGraph()
{
	for (int i = 0; i <= row; i++) delete[] map[i];
	delete[] map;
	for (int i = 0; i < row; i++) delete[] SquareMap[i];
	delete[] SquareMap;
}

void WitnessGraph::SetStartPoint(int x,int y)
{
	StartPoint = &map[x][y];
}

void WitnessGraph::SetEndPoint(int x, int y)
{
	EndPoint = &map[x][y];
}

bool WitnessGraph::solve()
{
	visitedNode = new int*[row+1];
	for (int i = 0; i <= row; i++) {
		visitedNode[i] = new int[column+1];
		for (int j = 0; j <= column; j++) visitedNode[i][j] = 0;
	}
	Clear();
//	while (solution.empty() == false) solution.pop();

	bool flag = dfs(StartPoint);
	
	for (int i = 0; i <= row; i++) {
		delete[] visitedNode[i];
	}
	delete[] visitedNode;
	return flag;
}

bool WitnessGraph::dfs(Node * N)
{
	
	visitedNode[N->X()][N->Y()] = 1;
	if (N == EndPoint) {
		visitedNode[N->X()][N->Y()] = 0;
		N->UnSetNextNode();
		return test();
	}
	auto it = N->begin();
	while (it != N->end()) {
		if (visitedNode[(*it)->X()][(*it)->Y()] == 0) {
			N->SetNextNode(**it);
			bool flag = dfs(*it);
			if (flag) return true;
		}
		it++;
	}
	visitedNode[N->X()][N->Y()] = 0;
	N->UnSetNextNode();
	return false;
}

bool WitnessGraph::testNode()
{
	for (auto it = NodeMustPass.begin(); it != NodeMustPass.end(); it++) {
		if (visitedNode[(*it)->X()][(*it)->Y()] == 0) return false;
	}
	return true;
}

bool WitnessGraph::testLine() {
	auto it = LineMustPassRight.begin();
	int x, y;
	while (it != LineMustPassRight.end()) {
		x = (*it)->X();
		y = (*it)->Y();
		if (!testLinePassRight(x,y)) return false;
		it++;
	}
	it = LineMustPassDown.begin();
	while (it != LineMustPassDown.end()) {
		x = (*it)->X();
		y = (*it)->Y();
		if (!testLinePassDown(x, y)) return false;
		it++;
	}
	return true;
}

bool WitnessGraph::testLinePassRight(int x,int y) {
	bool flag1, flag2;
	if (map[x][y].Next() == nullptr) flag1 = false;
	else flag1 = map[x][y].Next()->X() == x&&map[x][y].Next()->Y() == y + 1;
	if (map[x][y + 1].Next() == nullptr) flag2 = false;
	else flag2 = map[x][y + 1].Next()->X() == x&&map[x][y + 1].Next()->Y() == y;
	return flag1 || flag2;
}

bool WitnessGraph::testLinePassDown(int x, int y) {
	bool flag1, flag2;
	if (map[x][y].Next() == nullptr) flag1 = false;
	else flag1 = map[x][y].Next()->X() == x+1&&map[x][y].Next()->Y() == y;
	if (map[x + 1][y].Next() == nullptr) flag2 = false;
	else flag2 = map[x+1][y].Next()->X() == x&&map[x+1][y].Next()->Y() == y;
	return flag1 || flag2;
}

bool WitnessGraph::testLinePass(int x, int y, int x1, int y1) {
	bool flag1, flag2;
	if (map[x][y].Next() == nullptr) flag1 = false;
	else flag1 = map[x][y].Next()->X() == x1 && map[x][y].Next()->Y() == y1;
	if (map[x1][y1].Next() == nullptr) flag2 = false;
	else flag2 = map[x1][y1].Next()->X() == x&&map[x1][y1].Next()->Y() == y;
	return flag1 || flag2;
}

bool WitnessGraph::testSquare()
{
	s = new int*[row];
	for (int i = 0; i < row; i++) {
		s[i] = new int[column];
	}
	int color = 1;
	for (int i = 0; i < row; i++) {
		for (int j = 0; j < column; j++) {
			s[i][j] = 0;
		}
	}
	for (int i = 0; i < row; i++) {
		for (int j = 0; j < column; j++) {
			if(s[i][j]==0) Sep(i, j, color++);
		}
	}
	bool flag = testHex();
	for (int i = 0; i < row; i++) delete[] s[i];
	delete[] s;
	return flag;
}

void WitnessGraph::Sep(int i, int j, int color)
{
	int x, y;
	bool flag;
	s[i][j] = color;
	auto it = SquareMap[i][j].begin();
	while (it != SquareMap[i][j].end()) {
		x = (*it)->X();
		y = (*it)->Y();
		if (s[x][y] == 0) {
			if (x == i&&y == j + 1) flag = testLinePassDown(x, y);
			if (x == i&&y == j - 1) flag = testLinePassDown(i, j);
			if(x == i-1&&y == j) flag = testLinePassRight(i, j);
			if (x == i + 1 && y == j) flag = testLinePassRight(x, y);
		    if(!flag) Sep((*it)->X(), (*it)->Y(), color);
		} 
		it++;
	}
}

bool WitnessGraph::test() {
	return testNode()&&testLine()&&testSquare();
}

void WitnessGraph::AddLineRight(int x, int y) {
	ResetLineRight(x, y);
	LineMustPassRight.push_back(&map[x][y]);
}

void WitnessGraph::AddLineDown(int x, int y) {
	ResetLineDown(x, y);
	LineMustPassDown.push_back(&map[x][y]); 
}

void WitnessGraph::RemoveLineRight(int x, int y)
{
	LineMustPassRight.remove(&map[x][y]);
	map[x][y].remove(map[x][y+1]);
	map[x][y+1].remove(map[x][y]);
	if (x == row || y == column || x == 0) return;
	SquareMap[x][y].RemoveAdjSquare(SquareMap[x - 1][y]);
	SquareMap[x-1][y].RemoveAdjSquare(SquareMap[x][y]);
}

void WitnessGraph::RemoveLineDown(int x, int y) {
	LineMustPassDown.remove(&map[x][y]);
	map[x][y].remove(map[x+1][y]);
	map[x+1][y].remove(map[x][y]);
	if (x == row || y == column || y==0) return;
	SquareMap[x][y].RemoveAdjSquare(SquareMap[x][y-1]);
	SquareMap[x][y-1].RemoveAdjSquare(SquareMap[x][y]);
}

void WitnessGraph::ResetLineRight(int x, int y) {
	RemoveLineRight(x, y);
	NodeRemove(x, y, x, y + 1);
	NodeAdd(x, y, x, y + 1);
	NodeRemove(x, y + 1, x, y);
	NodeAdd(x, y + 1, x, y);
	if (x == row || y == column || x == 0) return;
	SquareMap[x][y].RemoveAdjSquare(SquareMap[x - 1][y]);
	SquareMap[x - 1][y].RemoveAdjSquare(SquareMap[x][y]);
	SquareMap[x][y].AddAdjSquare(SquareMap[x - 1][y]);
	SquareMap[x - 1][y].AddAdjSquare(SquareMap[x][y]);
}

void WitnessGraph::ResetLineDown(int x, int y) {
	RemoveLineDown(x, y);
	NodeRemove(x, y, x + 1, y);
	NodeAdd(x, y, x + 1, y);
	NodeRemove(x + 1, y, x, y);
	NodeAdd(x + 1, y, x, y);
	if (x == row || y == column || y == 0) return;
	SquareMap[x][y].RemoveAdjSquare(SquareMap[x][y - 1]);
	SquareMap[x][y - 1].RemoveAdjSquare(SquareMap[x][y]);
	SquareMap[x][y].AddAdjSquare(SquareMap[x][y - 1]);
	SquareMap[x][y - 1].AddAdjSquare(SquareMap[x][y]);
}

void WitnessGraph::Clear() {
	for (int i = 0; i <= row; i++) {
		for (int j = 0; j <= column; j++) {
			map[i][j].UnSetNextNode();
		}
	}
}

bool WitnessGraph::testHex() {
	int oct[10][10];
	int squ[10][10];
	for (int i = 0; i < 10; i++) {
		for (int j = 0; j < 10; j++) {
			oct[i][j] = squ[i][j] = 0;
		}
	}
	for (int i = 0; i < row; i++)
	{
		for (int j = 0; j < column; j++)
		{
			if (SquareMap[i][j].Squ() != -1) squ[s[i][j]][SquareMap[i][j].Squ()]++;
			if (SquareMap[i][j].Oct() != -1) oct[s[i][j]][SquareMap[i][j].Oct()]++;
		}
	}
	for (int i = 0; i < 10; i++)
	{
		int flag = 0;
		for (int j = 0; j < 10; j++)
		{
			if (oct[i][j] != 0 && oct[i][j] != 2) return false;
			if (squ[i][j] != 0) flag++;
			if (flag > 1) return false;
		}
	}
	return true;
}

void WitnessGraph::NodeInitOfEdge(int x,int y)
{
	if (x != 0) map[x][y].add(map[x - 1][y]);
	if (x != row) map[x][y].add(map[x + 1][y]);
	if (y != 0) map[x][y].add(map[x][y - 1]);
	if (y != column) map[x][y].add(map[x][y + 1]);
}

void WitnessGraph::SquareInit(int x, int y) {
	if (x != 0) SquareMap[x][y].AddAdjSquare(SquareMap[x - 1][y]);
	if (x != row - 1) SquareMap[x][y].AddAdjSquare(SquareMap[x + 1][y]);
	if (y != 0) SquareMap[x][y].AddAdjSquare(SquareMap[x][y - 1]);
	if (y != column - 1) SquareMap[x][y].AddAdjSquare(SquareMap[x][y + 1]);
}

void Node::add(Node & n)
{
	adj.push_back(&n);
}

void Node::remove(Node & n)
{
	adj.remove(&n);
}

