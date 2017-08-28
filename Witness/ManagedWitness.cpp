#include "WitnessGraph.h"

public ref class ManagedWitnessGraph {
public:
	ManagedWitnessGraph(int x,int y):graph(new WitnessGraph(x,y)){}
	~ManagedWitnessGraph() { delete graph; }
	void SetStartPoint(int x,int y) {
		graph->SetStartPoint(x,y);
	}
	void SetEndPoint(int x, int y) {
		graph->SetEndPoint(x,y);
	}
	bool solve() {
		return graph->solve();
	}
	//std::stack<Node*> & ans() {
	//	return graph->ans();
	//}
	int column() {
		return graph->Column();
	}
	int row() {
		return graph->Row();
	}
	bool isStartPointSet() { return graph->isStartPointSet(); }
	bool isEndPointSet() { return graph->isEndPointSet(); }
	int GetStartPointX(){ return graph->GetStartPointX(); }
	int GetStartPointY() { return graph->GetStartPointY();}
	int GetEndPointX() { return graph->GetEndPointX();}
	int GetEndPointY() { return graph->GetEndPointY();}
	void SetPtrToStart() { graph->SetPtrToStart(); }
	void PtrToNext() { graph->PtrToNext(); }
	int PtrX() { return graph->PtrX(); }
	int PtrY() { return graph->PtrY(); }
	void AddLineRight(int x, int y) { graph->AddLineRight(x, y); }
	void AddLineDown(int x, int y) { graph->AddLineDown(x, y); }
	void RemoveLineRight(int x, int y) { graph->RemoveLineRight(x, y); }
	void RemoveLineDown(int x, int y) { graph->RemoveLineDown(x, y); }
	void ResetLineRight(int x, int y) { graph->ResetLineRight(x, y); }
	void ResetLineDown(int x, int y) { graph->ResetLineDown(x, y); }
	void NodeAdd(int x, int y, int x1, int x2) { graph->NodeAdd(x, y, x1, x2);}
	void NodeRemove(int x, int y, int x1, int y1) { graph->NodeRemove(x, y, x1, y1); }
	void SetOct(int i, int j, int color) { graph->SetOct(i, j, color); }
	void SetSqu(int i, int j, int color) { graph->SetSqu(i, j, color); }
	int GetOct(int i, int j) { return graph->GetOct(i, j); }
	int GetSqu(int i, int j) { return graph->GetSqu(i, j);}

private:
	WitnessGraph *graph;
};