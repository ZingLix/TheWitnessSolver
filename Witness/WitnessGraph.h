#pragma once
#include <list>
#include <stack>

class Node {
public:
	Node() :x(0), y(0), nextNode(nullptr) {}
	Node(int row, int column) :x(row), y(column),nextNode(nullptr) {}
	void add(Node& n);
	void remove(Node& n);
	void set(int X, int Y) {
		x = X;
		y = Y;
	}
	int X() { return x; }
	int Y() { return y; }
	auto begin() { return adj.begin(); }
	auto end() { return adj.end(); }
	void SetNextNode(Node &N) { nextNode = &N; }
	void UnSetNextNode() { nextNode = nullptr; }
	Node *Next() { return nextNode; }

private:
	int x, y;
	std::list<Node*> adj;
	Node *nextNode;
};

class Square {
public:
	Square(int x,int y):x(x),y(y),oct(-1),squ(-1){}
	Square():x(0), y(0), oct(-1), squ(-1){}
	void SetXY(int x, int y) { this->x = x; this->y = y; }
	void SetOct(int x) { oct = x; }
	void SetSqu(int x) { squ = x; }
	void AddAdjSquare(Square &S) { adj.push_back(&S); }
	void RemoveAdjSquare(Square &S) { adj.remove(&S); }
	auto begin() { return adj.begin(); }
	auto end() { return adj.end(); }
	int X() { return x; }
	int Y() { return y; }
	int Oct() { return oct; }
	int Squ() { return squ; }

private:
	int x, y;
	int oct, squ;
	std::list<Square *> adj;
};

class WitnessGraph final{
public:
	WitnessGraph() = delete;
	WitnessGraph(int row, int column);
	~WitnessGraph();
	void SetStartPoint(int x,int y);
	void SetEndPoint(int x, int y);
	bool solve();
	//std::stack<Node *>& ans();
	int Row() { return this->row; }
	int Column() { return this->column; }
	bool isStartPointSet() { return StartPoint != nullptr; }
	bool isEndPointSet() { return EndPoint != nullptr; }
	int GetStartPointX() { return StartPoint->X(); }
	int GetStartPointY() { return StartPoint->Y(); }
	int GetEndPointX() { return EndPoint->X(); }
	int GetEndPointY() { return EndPoint->Y(); }
	void SetPtrToStart() { ptr = StartPoint; }
	void PtrToNext() { ptr = ptr->Next(); }
	int PtrX() { return ptr==nullptr? -1 : ptr->X(); }
	int PtrY() { return ptr == nullptr ? -1 : ptr->Y(); }
	void AddLineRight(int x, int y);
	void AddLineDown(int x, int y);
	void RemoveLineRight(int x, int y);
	void RemoveLineDown(int x, int y);
	void ResetLineRight(int x, int y);
	void ResetLineDown(int x, int y);
	void NodeAdd(int x, int y,int x1,int x2) {
		map[x][y].add(map[x1][x2]);
	}
	void NodeRemove(int x, int y, int x1, int y1) { map[x][y].remove(map[x1][y1]); }
	void Clear();
	void SetOct(int i, int j, int color) { SquareMap[i][j].SetOct(color); }
	void SetSqu(int i, int j, int color) { SquareMap[i][j].SetSqu(color); }
	int GetOct(int i, int j) { return SquareMap[i][j].Oct(); }
	int GetSqu(int i, int j) { return SquareMap[i][j].Squ(); }

private:
	void NodeInitOfEdge(int x,int y);
	void SquareInit(int x, int y);
	bool dfs(Node *N);
	bool test();
	bool testNode();
	bool testLine();
	bool testLinePassRight(int x,int y);
	bool testLinePassDown(int x,int y);
	bool testLinePass(int x, int y, int x1, int y1);
	bool testSquare();
//	bool testOct();
	bool testHex();
	void Sep(int i,int j,int color);

private:
	int row, column;
	Node **map;
	Square **SquareMap;
	Node *StartPoint;
	Node *EndPoint;
	std::list<Node *> NodeMustPass;
	std::list<Node *> LineMustPassRight;
	std::list<Node *> LineMustPassDown;
	int **visitedNode;
	Node *ptr;
	int **s;
};

