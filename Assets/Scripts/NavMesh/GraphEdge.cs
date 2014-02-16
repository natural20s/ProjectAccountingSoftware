using UnityEngine;
using System.Collections;

public class GraphEdge {

	private int mFromIndex;
	private int mToIndex;
	
	public GraphEdge(int frm, int to)
	{
		mFromIndex = frm;
		mToIndex= to;
	}
	
	public void SetFromIndex(int frm) { mFromIndex = frm; }
	public void SetToIndex(int to) { mToIndex = to; }
	
	// sets both indices, from index first, to index second
	public void SetEdge(int frm, int to)
	{
		mFromIndex = frm;
		mToIndex = to;
	}
	
	public int GetFromIndex() { return mFromIndex; }
	public int GetToIndex() { return mToIndex; }
}


// This struct will (hopefully in the future) represent a dynamic array class
// with limited functionality. For now, it is a dynamic array struct for GraphEdges
public struct DynArray
{
	public GraphEdge[] mArray;
	private int mCapacity;
	private int mSize;
	
	public DynArray(GraphEdge type)
	{
		mArray = new GraphEdge[8];	
		mCapacity = 8;
		mSize = 0;
	}
	
	public void Add(GraphEdge newObj)
	{
		if ( mSize >= mArray.Length )
		{
			GraphEdge[] temp = new GraphEdge[mCapacity << 1];
			
			for ( int i = 0; i < (mCapacity << 1); ++i ) 
			{
				temp[i] = mArray[i];	
			}
			mCapacity = mCapacity << 1;
			mArray = new GraphEdge[mCapacity];
			mArray = temp;
		}
		
		mArray[mSize] = newObj;
		++mSize;
	}
	
	public GraphEdge Loc(int i) { return mArray[i]; }
	
	public int GetSize() { return mSize; }
}