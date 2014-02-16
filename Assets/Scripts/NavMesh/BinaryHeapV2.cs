using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BinaryHeapV2 {

	private List<float> mKeys;
	private List<int> mValues;
	
	public BinaryHeapV2()
	{
		mKeys = new List<float>();
		mValues = new List<int>();
	}
	
	// UNTESTED!!!!
	public void Add(float key, int val)
	{
		// add element to bottom of the array
		mKeys.Add(key);
		mValues.Add(val);
		
		int index = mKeys.Count - 1;
		

		//compare value to it's parent, swap if parent is larger
		while ( index > 0 && mKeys[(index+1)/2 - 1] > key )
		{
			//swap 
			//Debug.Log("Swapping " + mArray[index] + " with " + mArray[(index+1)/2 - 1]);
			mKeys[index] = mKeys[(index+1)/2 - 1];
			mValues[index] = mValues[(index+1)/2 - 1];
			
			mKeys[(index+1)/2 - 1] = key;
			mValues[(index+1)/2 - 1] = val;
			
			index = (index+1)/2 - 1; // set new index position
		}
	}
	
	public void ExtractMin()
	{
		// get the minimum value from the tree
		
		// move last element to the front
		mKeys[0] = mKeys[mKeys.Count-1];
		mValues[0] = mValues[mValues.Count-1];
		
		mKeys.RemoveAt(mKeys.Count-1);
		mValues.RemoveAt(mValues.Count-1);
		
		Heapify(0);
		
		//return node;
	}
	
	public void Heapify(int index)
	{
		int leftChildIndex = index*2 + 1;
		int rightChildIndex = index*2 + 2;
		int smallestIndex = index;
		
		if ( mValues.Count < leftChildIndex + 1 )
		{
			// No comparison to be made, return
			return;
		}
		
		if ( mKeys[leftChildIndex] < mKeys[smallestIndex] )
		{
			// If their is no right child, then the largest is by default the left
			// or if the 
			smallestIndex = leftChildIndex;
		}
		
		if ( mKeys.Count >= rightChildIndex + 1 )
		{
			// right child exists, check its value
			if ( mKeys[rightChildIndex] < mKeys[smallestIndex] )
				smallestIndex = rightChildIndex;
		}
		
		if ( smallestIndex	!= index ) // only heapify if we found a smaller value
		{
			float tempFloat = mKeys[index];
			int tempInt = mValues[index];
			
			mKeys[index] = mKeys[smallestIndex];
			mValues[index] = mValues[smallestIndex];
			
			mKeys[smallestIndex] = tempFloat;
			mValues[smallestIndex] = tempInt;
			
			Heapify( smallestIndex );	
		}
		
	}
	
	public bool ContainsValue(int val)
	{
		for ( int i=0; i < mValues.Count; i++ )
		{
			if ( mValues[i] == val )
				return true;
		}
		
		return false;
	}
	
	public int IndexOfValue(int val)
	{
		for ( int i=0; i < mValues.Count; i++ )
		{
			if ( mValues[i] == val )
				return i;
		}
		
		return -1; 
	}
	
	public int Values(int index) { return mValues[index]; }
	public float Keys(int index) { return mKeys[index]; }
	public int Count() { return mValues.Count; }
	public void RemoveAt(int index) { mValues.RemoveAt(index); mKeys.RemoveAt(index); } // Does this violate the heap structure?
	
	
}
