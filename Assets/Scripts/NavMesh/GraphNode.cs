using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphNode 
{

	private int mIndex;
	private Vector3 mPosition;
	public DynArray mEdges;
	//public List<GraphEdge> fastEdges = new List<GraphEdge>();
	
	// Constructor for a graph node.
	// Paramters: int - index of this node
	//			  Vector3 - the position in the world space of this node
	public GraphNode(int ndx, Vector3 pos)
	{
		mIndex = ndx;
		mPosition = pos;
		mEdges = new DynArray(new GraphEdge(0, 1) );
	}
	
	public void AddEdge(GraphEdge edge)
	{
		mEdges.Add(edge);
		//fastEdges.Add(edge);
	}
	
	
	
	// Setters
	public void SetIndex(int ndx) { mIndex = ndx; }
	public void SetPosition(Vector3 pos) { mPosition = pos; }
	
	// Getters
	public int GetIndex() { return mIndex; }
	public Vector3 GetPosition() { return mPosition; }
	
}


