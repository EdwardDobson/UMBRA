using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;


public class TerrainGridPoint
{
    public static Vector2 mapCenter = new Vector2(-55, 40) + new Vector2(-0.07121f, 0.53778f); // first one is centre of world, second vector is to align with the tilemap grid because someone decided not to have everything centered at 0,0 and instead centered at random floats with decimals :/
    public static Vector2 mapBounds = new Vector2(512f, 512f);

    public static List<TerrainGridPoint> terrainGrid = new List<TerrainGridPoint>();
    public static List<TerrainGridPoint> allNodes = new List<TerrainGridPoint>(); 
    public static float minNodeSize = 0.5f;

    public Vector2 nodeSize = new Vector2(200, 200);
    public Vector2 nodePosition = Vector2.zero;

    public TerrainGridPoint parent = null;
    public TerrainGridPoint[] childNodes;
    public List<TerrainGridPoint> neighbourNodes = new List<TerrainGridPoint>();

    public bool floor = false;
    public bool impassible = false;

    public TerrainGridPoint(Vector2 _pos, Vector2 _size)
    {
        nodePosition = _pos;
        nodeSize = _size;
    }
    public TerrainGridPoint(Vector2 _pos, Vector2 _size, TerrainGridPoint _parent)
    {
        nodePosition = _pos;
        nodeSize = _size;
        parent = _parent;
    }

    public static void CreateGrid()
    {

        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        TerrainGridPoint rootNode = new TerrainGridPoint(mapCenter, mapBounds);

        stopWatch.Start();

        rootNode.CreateNodes();

        stopWatch.Restart();
        CheckForTerrain();

        stopWatch.Stop();

        return;
    }

    public void CreateNodes()
    {

        if ((!(nodeSize.x / 2 <= minNodeSize)))
        {
            float halfXSize = nodeSize.x / 4;
            float halfYSize = nodeSize.y / 4;

            Vector2 childSize = nodeSize / 2;
            //TopLeft
            Vector2 p1 = new Vector2(nodePosition.x - halfXSize, nodePosition.y + halfYSize);
            //TopRight
            Vector2 p2 = new Vector2(nodePosition.x + halfXSize, nodePosition.y + halfYSize);
            //BottomLeft
            Vector2 p3 = new Vector2(nodePosition.x - halfXSize, nodePosition.y - halfYSize);
            //BottomRight
            Vector2 p4 = new Vector2(nodePosition.x + halfXSize, nodePosition.y - halfYSize);


            TerrainGridPoint n1 = new TerrainGridPoint(p1, nodeSize / 2, this);
            TerrainGridPoint n2 = new TerrainGridPoint(p2, nodeSize / 2, this);
            TerrainGridPoint n3 = new TerrainGridPoint(p3, nodeSize / 2, this);
            TerrainGridPoint n4 = new TerrainGridPoint(p4, nodeSize / 2, this);

            childNodes = new TerrainGridPoint[4];

            childNodes[0] = n1;
            childNodes[1] = n2;
            childNodes[2] = n3;
            childNodes[3] = n4;


            for (int i = 0; i < childNodes.Length; i++)
            {
                childNodes[i].CreateNodes();
            }
            allNodes.Add(this);
        }
        else
        {
            childNodes = new TerrainGridPoint[0];
            allNodes.Add(this);
            terrainGrid.Add(this);
        }
    }

    //Dont think this is needed anymore and it uses a lot of cpu
    //public static void FindNeighbours()
    //{
    //    //For every node
    //    List<Task> tasks = new List<Task>();
    //    for (int i = 0; i < terrainGrid.Count; i++)
    //    {
    //        tasks.Add(Task.Factory.StartNew(new System.Func<object, bool>(FindNeighboursThreadForward), i));
    //    }
    //    Task.WaitAll(tasks.ToArray());


    //    List<Task> tasks2 = new List<Task>();
    //    for (int i = terrainGrid.Count - 1; i > 0; --i)
    //    {
    //        tasks2.Add(Task.Factory.StartNew(new System.Func<object, bool>(FindNeighboursThreadBack), i));
    //    }
    //    Task.WaitAll(tasks2.ToArray());
    //}

    //public static bool FindNeighboursThreadBack(object _i)
    //{
    //    int count = terrainGrid.Count;

    //    TerrainGridPoint iNode = terrainGrid[(int)_i];

    //    //Compare with every other node
    //    for (int j = (int)_i - 1; j > 0; --j)
    //    {
    //        if (j < 0)
    //        {
    //            break;
    //        }

    //        TerrainGridPoint jNode = terrainGrid[j];

    //        if (Vector2.Distance(jNode.nodePosition, iNode.nodePosition) <= Mathf.Sqrt(minNodeSize * 2) + 0.5) //Check if boxes are touching
    //        { //If it is, add to neighbor list                                
    //            iNode.neighbourNodes.Add(jNode);
    //        }

    //    }

    //    return true;
    //}

    //public static bool FindNeighboursThreadForward(object _i)
    //{
    //    int count = terrainGrid.Count;

    //    TerrainGridPoint iNode = terrainGrid[(int)_i];

    //    //Compare with every other node
    //    for (int j = (int)_i + 1; j < count; j++)
    //    {
    //        if (j > count)
    //        {
    //            break;
    //        }

    //        TerrainGridPoint jNode = terrainGrid[j];

    //        if (Vector2.Distance(jNode.nodePosition, iNode.nodePosition) <= Mathf.Sqrt(minNodeSize * 2) + 0.5) //Check if boxes are touching
    //        { //If it is, add to neighbor list                                
    //            iNode.neighbourNodes.Add(jNode);
    //        }

    //    }

    //    return true;
    //}

    public static void CheckForTerrain()
    {
        foreach (var node in terrainGrid)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(node.nodePosition, new Vector2(minNodeSize, minNodeSize), 0);

            foreach (var hit in hits)
            {
                if (hit.tag.Contains("Floor"))
                {
                    node.floor = true;
                }
                else if (hit.tag.Contains("Impassable"))
                {
                    node.impassible = true;
                }
            }
        }
    }

    public static bool isVertInsideBox(Vector3 _point, TerrainGridPoint _node)
    {
        float minX = _node.nodePosition.x - (_node.nodeSize.x / 2);
        float maxX = _node.nodePosition.x + (_node.nodeSize.x / 2);
        float minY = _node.nodePosition.y - (_node.nodeSize.y / 2);
        float maxY = _node.nodePosition.y + (_node.nodeSize.y / 2);

        if (_point.x >= minX && _point.x <= maxX &&
            _point.y >= minY && _point.y <= maxY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3[] GetVerticies()
    {
        Vector3[] verts = new Vector3[4];

        float halfXSize = nodeSize.x / 2;
        float halfYSize = nodeSize.y / 2;

        //TopLeft
        Vector3 v1 = new Vector3(nodePosition.x - halfXSize, nodePosition.y + halfYSize);
        //TopRight
        Vector3 v2 = new Vector3(nodePosition.x + halfXSize, nodePosition.y + halfYSize);
        //BottomLeft
        Vector3 v3 = new Vector3(nodePosition.x - halfXSize, nodePosition.y - halfYSize);
        //BottomRight
        Vector3 v4 = new Vector3(nodePosition.x + halfXSize, nodePosition.y - halfYSize);

        verts[0] = v1;
        verts[1] = v2;
        verts[2] = v3;
        verts[3] = v4;

        return verts;
    }

    public static void findNodeGivenPoint(Vector3 _point, out TerrainGridPoint _nodeToReturn, out bool _wasNodeFound)
    {
        bool toBreak = false;
        _nodeToReturn = getRootNode(allNodes[0]);
        _wasNodeFound = false;
        while (!_wasNodeFound)
        {
            if (isVertInsideBox(_point, _nodeToReturn) && _nodeToReturn.childNodes.Length == 0)
            {
                break;
            }

            for (int i = 0; i < _nodeToReturn.childNodes.Length; i++)
            {
                //If the point is inside and its a leaf node
                if (isVertInsideBox(_point, _nodeToReturn.childNodes[i]) && _nodeToReturn.childNodes[i].childNodes.Length == 0)
                {
                    _nodeToReturn = _nodeToReturn.childNodes[i];
                    toBreak = true;
                    break;
                }
                else if (isVertInsideBox(_point, _nodeToReturn.childNodes[i]))
                {
                    _nodeToReturn = _nodeToReturn.childNodes[i];
                    break;
                }
            }
            if (toBreak)
            {
                _wasNodeFound = true;
                break;
            }
        }
    }

    public static TerrainGridPoint getRootNode(TerrainGridPoint _node)
    {
        while (!(_node.parent == null)) //while current node has a parent
        {
            _node = _node.parent;
        }
        return _node;
    }
}

public class GridPoint
{
    public Vector2 position = Vector2.zero;
    public float distanceFromStart = 0; //G - cost from start to this location 
    public float distanceFromDestination = 0; //H - estimated cost from this location to end
    public float totalCosts = 0; //F - total of G and H
    public GridPoint parent = null;

    public GridPoint(Vector2 _position)
    {
        position = _position;
    }    
}

public class AStar
{
    public static float moveStep = 0.5f;

    GridPoint currentPoint = null;

    Vector2 startPoint = Vector2.zero;
    Vector2 targetPoint = new Vector2(30, 10);

    List<GridPoint> openList = new List<GridPoint>();
    List<GridPoint> closedList = new List<GridPoint>();
    List<Vector2> pathToReturn = new List<Vector2>();

    public EEnemyType enemyType;

    public int maxPathfindRange;   

    //The function to call for finding a path
    public List<Vector2> UpdatePathfinding(Vector2 _start, Vector2 _target, EEnemyType _type)
    {
        enemyType = _type;
        startPoint = _start;
        targetPoint = _target;

        //If points are the same immidiately return for efficiency.
        if (Vector2.Distance(startPoint, targetPoint) < moveStep)
        {
            return pathToReturn;
        }


        TerrainGridPoint tempTerr;
        bool tempNodeFound = false;
        //Check if the target point is covered by terrain map.
        TerrainGridPoint.findNodeGivenPoint(_target, out tempTerr, out tempNodeFound);
        if (tempNodeFound)
        {
            GridPoint tempGridP = new GridPoint(targetPoint);
            if (!(IsValidPoint(tempGridP))) //Check if the target point is reachable.
            {
                return new List<Vector2> { _start }; //Dont move
            }
        } else
        {
            Debug.LogError("target point not on terrain map");
            return new List<Vector2> { _start }; //Dont move
        }


        pathToReturn.Clear();
        openList.Clear();
        closedList.Clear();

        //Create starting point
        GridPoint s = new GridPoint(startPoint);
        s.distanceFromStart = 0;
        s.distanceFromDestination = ComputeDistanceFromDestination(startPoint, targetPoint);
        s.totalCosts = s.distanceFromStart + s.distanceFromDestination;
        currentPoint = s;
        openList.Add(currentPoint);

        //work through openList
        while (openList.Count > 0)
        {
            //Find the point in the openList with the lowest total cost and make it the current node.
            float lowestTotalCosts = float.MaxValue;
            foreach (var p in openList)
            {
                if (p.totalCosts < lowestTotalCosts)
                {
                    lowestTotalCosts = p.totalCosts;
                    currentPoint = p;
                }
            }
            openList.Remove(currentPoint);
            closedList.Add(currentPoint);


            //If found target point
            if (Vector2.Distance(currentPoint.position, targetPoint) < moveStep * 1.5)
            {
                //Recursive
                AddToList(currentPoint);

                //remove point that the agent would be standing on.
                pathToReturn.RemoveAt(0);

                foreach (var i in pathToReturn)
                {
                    Debug.DrawLine(i + new Vector2(-0.5f, -0.5f), i - new Vector2(-0.5f, -0.5f), Color.blue, 100f);
                }

                return pathToReturn;
            }

            //Retrieve adjacent positions and calucate their costs
            List<GridPoint> adjacentPositions = GetAdjacentPositions(currentPoint.position);

            //If there are no adjacent positions
            if (adjacentPositions.Count <= 0)
            {
                openList.Clear();
                return pathToReturn;
            }

            //Check best in the adjacent positions
            foreach (var p in adjacentPositions)
            {
                //if it not in closed list
                if (closedList.FirstOrDefault(l => l.position == p.position) == null)
                {
                    //if its not in the open list
                    if (openList.FirstOrDefault(l => l.position == p.position) == null)
                    {
                        //If its diagonal, make the cost slightly bigger for path realism
                        if (IsDiagonal(currentPoint, p))
                        {
                            p.distanceFromStart = currentPoint.distanceFromStart + 1.4142f;
                        }
                        else
                        {
                            p.distanceFromStart = currentPoint.distanceFromStart + 1;
                        }

                        //calculate costs
                        p.distanceFromDestination = ComputeDistanceFromDestination(p.position, targetPoint);
                        p.totalCosts = p.distanceFromStart + p.distanceFromDestination;
                        p.parent = currentPoint;

                        openList.Insert(0, p);
                    }
                    //if its already in the open list from a different parent, check if the new one would be better
                    else
                    {
                        //Check if new costs would be better than current costs
                        if ((currentPoint.distanceFromStart) + p.distanceFromDestination < p.totalCosts)
                        {
                            //if they were, update them. includeing checking if they're diagonal.
                            if (IsDiagonal(currentPoint, p))
                            {
                                p.distanceFromStart = currentPoint.distanceFromStart + 1.4142f;
                            }
                            else
                            {
                                p.distanceFromStart = currentPoint.distanceFromStart + 1;
                            }

                            p.totalCosts = p.distanceFromStart + p.distanceFromDestination;
                            p.parent = currentPoint;
                        }
                    }
                }
            }
        }

        return pathToReturn;
    }

    //P1 is the current point. Checks if P2 is diagonal to P1 or not.
    private bool IsDiagonal(GridPoint _p1, GridPoint _p2)
    {
        if (_p1.position.x + moveStep == _p2.position.x && _p1.position.y + moveStep == _p2.position.y ||
            _p1.position.x + moveStep == _p2.position.x && _p1.position.y - moveStep == _p2.position.y ||
            _p1.position.x - moveStep == _p2.position.x && _p1.position.y + moveStep == _p2.position.y ||
            _p1.position.x - moveStep == _p2.position.x && _p1.position.y - moveStep == _p2.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<GridPoint> GetAdjacentPositions(Vector2 _currentPos)
    {
        List<GridPoint> positions = new List<GridPoint>();

        //diagonals 
        GridPoint p1 = new GridPoint(new Vector2(_currentPos.x + moveStep, _currentPos.y + moveStep));
        GridPoint p2 = new GridPoint(new Vector2(_currentPos.x + moveStep, _currentPos.y - moveStep));
        GridPoint p3 = new GridPoint(new Vector2(_currentPos.x - moveStep, _currentPos.y + moveStep));
        GridPoint p4 = new GridPoint(new Vector2(_currentPos.x - moveStep, _currentPos.y - moveStep));

        //not-diagonals
        GridPoint p5 = new GridPoint(new Vector2(_currentPos.x + moveStep, _currentPos.y));
        GridPoint p6 = new GridPoint(new Vector2(_currentPos.x - moveStep, _currentPos.y));
        GridPoint p7 = new GridPoint(new Vector2(_currentPos.x, _currentPos.y + moveStep));
        GridPoint p8 = new GridPoint(new Vector2(_currentPos.x, _currentPos.y - moveStep));

        if (IsValidPoint(p1))
        {
            positions.Add(p1);
        }
        if (IsValidPoint(p2))
        {
            positions.Add(p2);
        }
        if (IsValidPoint(p3))
        {
            positions.Add(p3);
        }
        if (IsValidPoint(p4))
        {
            positions.Add(p4);
        }
        if (IsValidPoint(p5))
        {
            positions.Add(p5);
        }
        if (IsValidPoint(p6))
        {
            positions.Add(p6);
        }
        if (IsValidPoint(p7))
        {
            positions.Add(p7);
        }
        if (IsValidPoint(p8))
        {
            positions.Add(p8);
        }

        return positions;
    }

    private bool IsValidPoint(GridPoint _point)
    {
        //Make sure the point is within invisible boundary to stop infinite search of flying enemies.
        if (Vector2.Distance(_point.position, startPoint) > maxPathfindRange)
        {
            return false;
        }

        TerrainGridPoint terrPoint;
        bool nodeFound = false;
        TerrainGridPoint.findNodeGivenPoint(_point.position, out terrPoint, out nodeFound);

        bool walkable = false;
        
        //Check if any of the objects are Terrain, if they are, point isn't walkable
        //if enemy is not flying
        if (enemyType == EEnemyType.Flying)
        {
            walkable = true;
        }
        else
        {
            if (terrPoint.floor)
            {
                walkable = true;
            }
        }

        //If its impassable, its not walkable
        if (terrPoint.impassible)
        {
            walkable = false;
        }
        

        //Make sure the point is within the bounds of the ground plane
        if (walkable)
        {
            Debug.DrawLine(_point.position + new Vector2(-0.5f, -0.5f), _point.position - new Vector2(-0.5f, -0.5f), Color.green, 100f);
            return true;
        }
        else
        {
            Debug.DrawLine(_point.position + new Vector2(-0.5f, -0.5f), _point.position - new Vector2(-0.5f, -0.5f), Color.red, 100f);
            return false;
        }

    }


    private int ComputeDistanceFromDestination(Vector3 _pointPos, Vector3 _targetPos)
    {
        return Mathf.RoundToInt(Mathf.Abs(_targetPos.x - _pointPos.x) + Mathf.Abs(_targetPos.y - _pointPos.y));
    }

    //Recursive function to get the final path
    private void AddToList(GridPoint _point)
    {
        pathToReturn.Insert(0, _point.position);
        //pathToReturn.Insert(i, new Vector3(closedList[i].position.x, ground.transform.position.y + 1, closedList[i].position.z));
        if (_point.parent == null)
        {
            return;
        }
        else
        {
            if(pathToReturn.Count >= 50)
            {
                Debug.LogError("path to return stack overflow override");
                return;
            }
            AddToList(_point.parent);
        }
    }

}