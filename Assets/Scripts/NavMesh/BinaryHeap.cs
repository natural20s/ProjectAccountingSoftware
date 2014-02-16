using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BinaryHeap {
	
	private List<BTreeNode> mArray;
	
	public BinaryHeap()
	{
		mArray = new List<BTreeNode>();
	}
	
	// UNTESTED!!!!
	public void Add(float key, int val)
	{
		BTreeNode newNode = new BTreeNode(key, val);
		
		
		// add element to bottom of the array
		mArray.Add(newNode);
		
		int index = mArray.Count - 1;
		

		//compare value to it's parent, swap if parent is larger
		while ( index > 0 && mArray[(index+1)/2 - 1].mKey > key )
		{
			//swap 
			mArray[index] = mArray[(index+1)/2 - 1];
			mArray[(index+1)/2 - 1] = newNode;
			
			index = (index+1)/2 - 1; // set new index position
		}
	}
	
	public BTreeNode ExtractMin()
	{
		// get the minimum value from the tree
		BTreeNode node = mArray[0];
		
		// move last element to the front
		mArray[0] = mArray[mArray.Count-1];
		mArray.RemoveAt(mArray.Count-1);
		
		Heapify(0);
		
		return node;
	}
	
	public void Heapify(int index)
	{
		int leftChildIndex = index*2 + 1;
		int rightChildIndex = index*2 + 2;
		int smallestIndex = index;
		
		if ( mArray.Count < leftChildIndex + 1 )
		{
			// No comparison to be made, return
			return;
		}
		
		if ( mArray[leftChildIndex].mKey < mArray[smallestIndex].mKey )
		{
			// If their is no right child, then the largest is by default the left
			// or if the 
			smallestIndex = leftChildIndex;
		}
		
		if ( mArray.Count >= rightChildIndex + 1 )
		{
			// right child exists, check its value
			if ( mArray[rightChildIndex].mKey < mArray[smallestIndex].mKey )
				smallestIndex = rightChildIndex;
		}
		
		if ( smallestIndex	!= index ) // only heapify if we found a smaller value
		{
			BTreeNode temp = mArray[index];
			mArray[index] = mArray[smallestIndex];
			mArray[smallestIndex] = temp;
			
			Heapify( smallestIndex );	
		}
		
	}
	
	public bool ContainsValue(int val)
	{
		for ( int i=0; i < mArray.Count; ++i )
		{
			if ( mArray[i].mValue == val )
				return true;
		}
		
		return false;
	}
	
	public int IndexOfValue(int val)
	{
		for ( int i=0; i < mArray.Count; ++i )
		{
			if ( mArray[i].mValue == val )
				return i;
		}
		
		return -1; 
	}
	
	public int Values(int index) { return mArray[index].mValue; }
	public float Keys(int index) { return mArray[index].mKey; }
	public int Count() { return mArray.Count; }
	public void RemoveAt(int index) { mArray.RemoveAt(index); } // Does this violate the heap structure?
	
	public string DisplayTree()
	{
		string values = "";
		for ( int i=0; i < mArray.Count; ++i )
		{
			values += mArray[i].mKey + ", ";
		}
		
		return values;
	}
		
	
	public struct BTreeNode
	{
		public float mKey;
		public int mValue;
			
		public BTreeNode (float key, int val)
		{
			mKey = key;
			mValue = val;
		}	
	}
	
	
}
