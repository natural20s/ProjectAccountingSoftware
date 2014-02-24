using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// false implies use XY, left handed coordinate system (+z is away from camera)
//#define USE_XZ 

public class NavGraphConstructor : MonoBehaviour 
{
	private static NavGraphConstructor instance = null;
	public static NavGraphConstructor Instance { get { return instance; } }
	
	public Vector3[] NavigationGraph;
	public Vector3[] BHeap;
	
	public List<GraphNode> Nodes;
	public List<GraphEdge> Edges;
	
	public Vector3 searchStart;
	public Vector3 searchEnd;
	
	public Vector3 startPoint;
	
	public int xCastDistance = 10;
	public int zCastDistance = 10;
	public float incrementDistX = 1.0f;	
	public float incrementDistZ = 1.0f;
	public float rayCastHeight = 10.0f;
	public int mNumOfNodes = 0;
	//public int maxNodesToAdd = 50;
	
	public GameObject NodeObj; 
	public GameObject StartNode;
	public GameObject EndNode;
	
	public Vector3[] pathPoints;
	
	public bool DisplayEdges = false;
	
	private List<UnityEngine.GameObject> PathNodes = new List<UnityEngine.GameObject>();
	
	private Transform mPlayer;
	
	void Awake()
	{
		instance = this;
		FloodFill2(startPoint);
	}
	
	// Use this for initialization
	void Start () 
	{
		
		//DisplayPath( AStarSearch3(searchStart, searchEnd) );
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player)
		{
			mPlayer = player.transform;
		}

		for (int idx = 0; idx < Edges.Count; ++idx)
		{
			if (Edges[idx].GetFromIndex() == 0 || Edges[idx].GetToIndex() == 0)
			{
				//Debug.Log ("Index to 0: " + idx + " going from " + NavigationGraph[Edges[idx].GetFromIndex()] + " to " + NavigationGraph[Edges[idx].GetToIndex()]);
			}
		}
	}
	
	void Update()
	{
		if (DisplayEdges)
		{
			foreach ( GraphEdge n in Edges )
			{
				Debug.DrawLine(NavigationGraph[n.GetFromIndex()], NavigationGraph[n.GetToIndex()], Color.black);
			}
		}
	}
	
	
	public void SetParametersAndFindPath(string startX, string startZ, string endX, string endZ, string incDist)
	{
		
		float.TryParse(startX, out searchStart.x);
		float.TryParse(endX, out searchEnd.x);
#if USE_XZ
		float.TryParse(startZ, out searchStart.z);
		float.TryParse(endZ, out searchEnd.z);
#else
		float.TryParse(startZ, out searchStart.y);
		float.TryParse(endZ, out searchEnd.y);
#endif

		
		DisplayPath( AStar(searchStart, searchEnd) );
	}
	
	// TODO
	// comments
	public void SetParametersAndCreateGraph(string incDist, string xCastDist, string zCastDist)
	{
		Debug.LogError("ToDo: Need to hook up a variable for incrementDistZ");
		float.TryParse(incDist, out incrementDistX);
		//float.TryParse(incDistZ, out blah
		int.TryParse(xCastDist, out xCastDistance);
		int.TryParse(zCastDist, out zCastDistance);
		
		FloodFill2(startPoint);
	}
	
	
	public Vector3[] FindPathToPlayer(Vector3 startLocation)
	{	
		searchStart = FindClosestPoint(startLocation);
		searchEnd = FindClosestPoint(mPlayer.position);

		//AStar(searchStart, searchEnd);
		DisplayPath( AStar(searchStart, searchEnd) );
		return NodePathToPoint();
	}
	
	public Vector3[] FindPathToLocation(Vector3 startLocation, Vector3 endLocation)
	{
		searchStart = FindClosestPoint(startLocation);
		searchEnd = FindClosestPoint(endLocation);
#if USE_XZ
		searchStart.y = 0f;
		searchEnd.y = 0f;
#else
		searchStart.z = 0f;
		searchEnd.z = 0f;
#endif
		//AStar(searchStart, searchEnd);
		DisplayPath( AStar(searchStart, searchEnd) );
		return NodePathToPoint();
	}
	
	// Finds the node in the nav graph closest to the desired position
	public Vector3 FindClosestPoint(Vector3 desired)
	{
		Vector3 closestPoint = Nodes[0].GetPosition();
		foreach(GraphNode node in Nodes)
		{
			if ( (node.GetPosition() - desired).sqrMagnitude < (closestPoint - desired).sqrMagnitude )
			{
				closestPoint = node.GetPosition();
			}
		}
#if USE_XZ
		closestPoint.y = 0f;
#else
		closestPoint.z = 0f;
#endif
		return closestPoint;
	}
	
	// Makes the desired vector a valid position on the nav mesh
	public Vector3 SnapToMesh(Vector3 desired)
	{
		Vector3 fixedPoint = Vector3.zero;
		
		fixedPoint.x = (int)desired.x - (int)desired.x%incrementDistX;


#if USE_XZ
		fixedPoint.y = 0;
		fixedPoint.z = (int)desired.z - (int)desired.z%incrementDistZ;
#else
		fixedPoint.y = (int)desired.y - (int)desired.y%incrementDistZ;
		fixedPoint.z = 0;
#endif

		return fixedPoint;
	}
	
	
	public void FloodFill2(Vector3 startPoint)
	{
		// clear our nodes and edges, we'll be finding new ones
		Nodes = new List<GraphNode>();
		Edges = new List<GraphEdge>();
		
		mNumOfNodes = 0; // this serves as the size of the array
		GraphNode seed = new GraphNode(mNumOfNodes, startPoint); // make startPoint into the first GraphNode
		++mNumOfNodes;

		// initialize our queue and add the first node
		Queue<GraphNode> q = new Queue<GraphNode>();
		q.Enqueue(seed);
		
		// We will keep track of all the valid nodes in our Nodes array
		Nodes.Add(seed);
		
		// note: incrementDist is already a float
		// we add 1 to the cast distances because in our loops we start at 0
		NavigationGraph = new Vector3[(int)( (xCastDistance+2)*(zCastDistance+2) )]; 
		
		//DateTime start = DateTime.Now;
		
		GraphNode currentNode = new GraphNode(0, startPoint);
		for ( int xPos = 0; xPos < xCastDistance; ++xPos )
		{
			for ( int zPos = 0; zPos < zCastDistance; ++zPos )
			{	
				currentNode = null;
				
				// Search for the x and z value we are currently processing in our list of known positions (Nodes) 
				// to make sure we don't process an invalid position
				for ( int n = 0; n < Nodes.Count; ++n )
				{
					if ( (xPos*incrementDistX + startPoint.x) == Nodes[n].GetPosition().x && 
#if USE_XZ
					    (zPos*incrementDistX + startPoint.z) == Nodes[n].GetPosition().z )
					    
#else
						(zPos*incrementDistZ + startPoint.y) == Nodes[n].GetPosition().y )
#endif // USE_XZ
					{
						currentNode = Nodes[n];
						break;
					}
				}
				
				if ( currentNode != null )
				{
					float xPoint = startPoint.x;
#if USE_XZ
					float zPoint = startPoint.z;
#else
					float zPoint = startPoint.y;
#endif
					// determine the next point we want to check for
					Vector3 neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX + incrementDistX, 
					                                    rayCastHeight, zPoint + zPos*incrementDistZ);
					AddNode(neighborPoint, ref currentNode, ref q);
					
					
					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX - incrementDistX, 
					                            rayCastHeight, zPoint + zPos*incrementDistZ);
					AddNode(neighborPoint, ref currentNode, ref q);
		
					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX, 
					                            rayCastHeight, zPoint + zPos*incrementDistZ + incrementDistZ);
					AddNode(neighborPoint, ref currentNode, ref q);
		
					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX, 
					                            rayCastHeight, zPoint + zPos*incrementDistZ - incrementDistZ);
					AddNode(neighborPoint, ref currentNode, ref q);
					
					// diagonals
//					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX + incrementDistX, rayCastHeight, zPoint + zPos*incrementDistZ - incrementDistZ);
//					AddNode(neighborPoint, ref currentNode, ref q);
//					
//					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX - incrementDistX, rayCastHeight, zPoint + zPos*incrementDistZ - incrementDistZ);
//					AddNode(neighborPoint, ref currentNode, ref q);
//					
//					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX - incrementDistX, rayCastHeight, zPoint + zPos*incrementDistZ + incrementDistZ);
//					AddNode(neighborPoint, ref currentNode, ref q);
//					
//					neighborPoint = GetNeighborPoint(xPoint + xPos*incrementDistX + incrementDistX, rayCastHeight, zPoint + zPos*incrementDistZ + incrementDistZ);
//					AddNode(neighborPoint, ref currentNode, ref q);
					
				}
			}	 
		}
		//Debug.Log( "Flood fill 2: " + (DateTime.Now - start) );
	}
	
	public void AddNode(Vector3 neighborPoint, ref GraphNode currentNode, ref Queue<GraphNode> q )
	{
		RaycastHit hitInfo;
		Vector3 rayDirection = Vector3.zero;
#if USE_XZ
		rayDirection = new Vector3(0, -1, 0);
#else
		rayDirection = new Vector3(0, 0, 1);
#endif //USE_XZ
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		if ( Physics.Raycast(neighborPoint, rayDirection, out hitInfo, Mathf.Infinity, layerMask) )
		{
			if (hitInfo.transform.tag == "Ground")
			{
				GraphNode newNode = new GraphNode(mNumOfNodes, hitInfo.point); // make a new node for this point
				GraphEdge newEdge = new GraphEdge(currentNode.GetIndex(), newNode.GetIndex()); // creat a new edge
				
				int index = 0;
				bool nodeFound = false;
				while ( !nodeFound && index <= mNumOfNodes )
				{ 
					//Debug.Log (index + " out of " + NavigationGraph.Length + " thinks there's only" + mNumOfNodes);
					nodeFound = ( NavigationGraph[index] == hitInfo.point );
					
					++index;
				}
				
				
				if ( !nodeFound ) // if we have not found this node before, add it
				{
					Nodes.Add(newNode);
					NavigationGraph[mNumOfNodes] = hitInfo.point;
					++mNumOfNodes;
					
					q.Enqueue(newNode);	
				}
				else
				{
					newEdge.SetToIndex(index-1);
				}
				
				// If the raycast hit then we will always want to add the edge, since we want edges 
				// in both directions and there won't ever be duplicates.
	
				
				// check if there is a clear path to add an edge
				Vector3 heightOffset = Vector3.zero;
#if USE_XZ
				heightOffset = new Vector3(0, 0.5f, 0);
#else
				heightOffset = new Vector3(0, 0, -0.5f);
#endif // USE_XZ
				if ( !Physics.Linecast(currentNode.GetPosition() + heightOffset, newNode.GetPosition() + heightOffset, out hitInfo, layerMask) ) 
				{
					//if (currentNode.GetPosition() == Vector3.zero || newNode.GetPosition() == Vector3.zero)
					{
						//Debug.Log ("One of these nodes are zero " + currentNode.GetPosition() + " " + newNode.GetPosition());
					}

					Edges.Add(newEdge);	
					currentNode.AddEdge(newEdge);
				}
			}
			else
			{
				Debug.Log ("Hit object " + hitInfo.transform.name);
			}
		}
	}

	public Vector3 GetNeighborPoint(float xPoint, float raycastHeight, float zPoint) 
	{
#if USE_XZ
		return new Vector3(xPoint, raycastHeight, zPoint);
#else
		return new Vector3(xPoint, zPoint, -raycastHeight);
#endif //USE_XZ
	}

	public List<GraphEdge> AStar(Vector3 start, Vector3 end)
	{
		List<float> gCosts = new List<float>();
		List<float> fCosts = new List<float>();
		
		List<GraphEdge> spt = new List<GraphEdge>(); // shortest path tree
		List<GraphEdge> searchFrontier = new List<GraphEdge>();
		
		for (int i = 0; i < Nodes.Count; ++i)
		{
			gCosts.Add(9999);
			fCosts.Add(9999);
			searchFrontier.Add(new GraphEdge(-1, -1));
		}
		
		BinaryHeap pq = new BinaryHeap();
		
		int index = 0;
		while (pq.Count() == 0 && index < Nodes.Count)
		{
			// check if we've found the node we want to start at
#if USE_XZ
			if (Nodes[index].GetPosition().x == start.x && Nodes[index].GetPosition().z == start.z)
#else // USE_ZY
			if (Nodes[index].GetPosition().x == start.x && Nodes[index].GetPosition().y == start.y)
#endif //USE_XZ
			{
				pq.Add(0, index); // add it to our pq
				gCosts[index] = 0; // set its g cost
				fCosts[index] = 0; // set its f cost
				searchFrontier[index] = new GraphEdge(index, index); // add it to frontier
			}
			++index;
		}
		
		Vector2 endPosV2;
#if USE_XZ 
		endPosV2 = new Vector2(end.x, end.z);
#else //USE_XY
		endPosV2 = new Vector2(end.x, end.y);
#endif
		while (pq.Count() > 0)
		{
			int closestNode = pq.Values(0);
			pq.RemoveAt(0);
			
			if (!spt.Contains(searchFrontier[closestNode]))
				spt.Add(searchFrontier[closestNode]); // if I use insert here, then later I can index it instead of searching for it
			
			Vector2 curPositionV2 = new Vector2(Nodes[closestNode].GetPosition().x, 
#if USE_XZ 
	        Nodes[closestNode].GetPosition().z);
#else
			Nodes[closestNode].GetPosition().y);
#endif
			if (curPositionV2 == endPosV2)
			{
				return spt;
			}
			
			GraphNode curNode = Nodes[closestNode];
			
			for (int i = 0; i < curNode.mEdges.GetSize(); ++i)
			{
				// index of the node that this edge is pointing to 
				int edgeToIndex = curNode.mEdges.Loc(i).GetToIndex();
				
				float g = gCosts[curNode.GetIndex()] + (curNode.GetPosition() - Nodes[edgeToIndex].GetPosition()).sqrMagnitude;
				float h = (end - Nodes[edgeToIndex].GetPosition()).sqrMagnitude;
				float f = g + h;
				
				// this is where I would index for the edge
				//if (searchFrontier[edgeToIndex].GetToIndex() == -1 &&
				 //   searchFrontier[edgeToIndex].GetFromIndex() == -1)
				bool onFrontier = false;
				for (int j = 0; j < searchFrontier.Count; +++j)
				{
					if (searchFrontier[j].GetToIndex() == edgeToIndex)
					{
						onFrontier = true;
						break;
					}
				}
				
				if (!onFrontier)
				{
					searchFrontier[edgeToIndex] = curNode.mEdges.Loc(i);
					
					gCosts[edgeToIndex] = g;
					fCosts[edgeToIndex] = f;
					
					if (pq.ContainsValue(edgeToIndex))
					{
						int pqIndex = pq.IndexOfValue(edgeToIndex);
						
						float oldFCost = pq.Keys(pqIndex);
						
						if (f < oldFCost) // if path is shorter
						{
							pq.RemoveAt(pqIndex);
							pq.Add(f, edgeToIndex);
						}
					}
					else
					{
						pq.Add(f, edgeToIndex);
					}
				}
				else 
				{
					int indexOfEdgeToIndex = pq.IndexOfValue(edgeToIndex);
					
					if (indexOfEdgeToIndex >= 0 && f < pq.Keys(indexOfEdgeToIndex))
					{
						pq.RemoveAt(indexOfEdgeToIndex);
						pq.Add(f, edgeToIndex);
					}
				}
			}
			
		}
		
		//Debug.LogWarning("Path to destination not found");
		spt = new List<GraphEdge>(); // clear the list to represent no path found
		return spt;
	}
	
	
	
	public void DisplayPath(List<GraphEdge> path)
	{
		// If there is already a path being displayed, destroy all of those nodes
		if ( PathNodes.Count > 0 )
		{
			for ( int i = 0; i < PathNodes.Count; i++ )
			{
				DestroyObject(PathNodes[i]);
			}
		}
		PathNodes = new List<UnityEngine.GameObject>();
		
		
		if ( path.Count == 0 ) // if the path count is 0 then a path was not found, so we will just display the beginning and end nodes
		{
			PathNodes.Add( (GameObject)Instantiate(StartNode, searchStart, Quaternion.identity) );
			PathNodes.Add( (GameObject)Instantiate(EndNode, searchEnd, Quaternion.identity) );
		}
		else
		{
			float pathCost = 0; // used to keep track of the path cost, this doesn't affect the function, more used for testing/debugging
			
			GraphEdge curEdge = path[path.Count - 1]; // start tracing the edges back from the destination
			
			PathNodes.Add( (GameObject)Instantiate(EndNode, Nodes[curEdge.GetToIndex()].GetPosition(), Quaternion.identity) );
			pathCost += (Nodes[curEdge.GetToIndex()].GetPosition() - Nodes[curEdge.GetFromIndex()].GetPosition()).sqrMagnitude;
			
			GraphEdge tempEdge;
			while ( curEdge.GetToIndex() != curEdge.GetFromIndex() ) // stop when the index this edge is pointing to is the beginning
			{
				// the curEdge changes at the end of the loop, so let's reset tempEdge at the beginning
				tempEdge = curEdge;
				
				// The first edge to the first node is (-1, -1), so when we search the Path for it, we loop out
				// instantiate a Node at the current edge, and do this over and over
				for ( int i = 0; i < path.Count; i++ )
				{
					// there should only be one edge coming from the previous node, so we search for it
					if ( path[i].GetToIndex() == curEdge.GetFromIndex() )
					{
						curEdge = path[i];
						break;	
					}
				}
				
				// if we found the start node, make a different colored node
				if ( curEdge.GetToIndex() == curEdge.GetFromIndex() )
				{
					PathNodes.Add( (GameObject)Instantiate(StartNode, Nodes[tempEdge.GetFromIndex()].GetPosition(), Quaternion.identity) );
				}
				else
				{
					// otherwise make a generic node object
					PathNodes.Add( (GameObject)Instantiate(NodeObj, Nodes[tempEdge.GetFromIndex()].GetPosition(), Quaternion.identity) );
				}
				pathCost += (Nodes[curEdge.GetToIndex()].GetPosition() - Nodes[curEdge.GetFromIndex()].GetPosition()).sqrMagnitude;
			}
		}
	}
	
	// Returns an array of positions representing the path to be taken
	public Vector3[] NodePathToPoint()
	{
		pathPoints = new Vector3[PathNodes.Count];
		
		for (int i = 0; i < pathPoints.Length; ++i)
		{
			pathPoints[pathPoints.Length -  1 - i] = PathNodes[i].transform.position;
		}
		
		return pathPoints;
	}
}
