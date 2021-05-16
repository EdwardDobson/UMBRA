using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class EnemyAI : MonoBehaviour
{
    public enum EPathType
    {
        followPlayer,
        patrol,
        stationary
    }

    AStar aStar;
    [SerializeField]
    EPathType currentPathType;
    [SerializeField]
    List<GameObject> patrolPointObjects = new List<GameObject>();
    List<Vector2> patrolPointVec = new List<Vector2>();
    List<Vector2> movementPath = new List<Vector2>();

    [SerializeField]
    float playerTrackDistance = 10f;

    public int maxPathfindRange;
    float lastTime = 0;
    float delay = 0.2f;

    public Vector2 currentPos;
    public Vector2 previousPos;

    [SerializeField]
    public EEnemyType enemyType;

    

    //For use in editor
    public void AddPatrolPoint()
    {
        GameObject go = new GameObject();
        go.transform.position = new Vector2(0, 0);
        go.name = "patrol point " + patrolPointObjects.Count.ToString();
        patrolPointObjects.Add(go);
    }

    public void DeletePatrolPoint(int _index)
    {
        DestroyImmediate(patrolPointObjects[_index].gameObject);

        int oldSize = patrolPointObjects.Count;
        patrolPointObjects.RemoveAt(_index);
        if (patrolPointObjects.Count == oldSize)
        {
            patrolPointObjects.RemoveAt(_index);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aStar = new AStar();
        if(maxPathfindRange <= 0)
        {
            maxPathfindRange = 50;
        }
        aStar.maxPathfindRange = maxPathfindRange;

        patrolPointVec.Add(transform.position);
        for (int i = 0; i < patrolPointObjects.Count; i++)
        {
            if (patrolPointObjects[i] != null)
            {
                patrolPointVec.Add(patrolPointObjects[i].transform.position);
                Debug.LogError("patrol point list error with " + gameObject.name);
            }
        }  

        switch (currentPathType)
        {
            case EPathType.followPlayer:
                if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < playerTrackDistance)
                {
                    MovementPathFunc(gameObject.transform.position, PlayerController.Instance.gameObject.transform.position);
                }
                break;

            case EPathType.patrol:
                if (patrolPointVec.Count > 0)
                {
                    MovementPathFunc(gameObject.transform.position, patrolPointVec[0]);
                }
                break;
        }
        previousPos = transform.position;
        currentPos = transform.position;
    }

    //Put pathfinding on a separate thread?
    public void Update()
    {

        if (!GetComponent<BaseEnemy>().GetPossessedState() || GetComponent<BaseEnemy>() == null)
        {

            //To temporarily turn off pathfinding if the player doesn't exist.
            bool stationarySwitch = false;
            if (!PlayerController.Instance.enabled)
            {
                currentPathType = EPathType.stationary;
                stationarySwitch = true;
            }

            switch (currentPathType)
            {
                case EPathType.followPlayer:
                    if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < playerTrackDistance)
                    {
                        if (!(PlayerController.Instance.transform.position == PlayerController.Instance.previousPosition))
                        {
                            if (Time.time - lastTime > delay)
                            {
                                lastTime = Time.time;
                                MovementPathFunc(gameObject.transform.position, PlayerController.Instance.gameObject.transform.position);
                            }
                        }
                        FollowPath();
                    }
                    break;
                case EPathType.patrol:
                    if (patrolPointVec.Count > 0)
                    {
                        if (movementPath.Count <= 0)
                        {
                            // if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), patrolPointVec[0]) <= aStar.moveStep)
                            //{
                            if (Vector2.Distance(transform.position, patrolPointVec[0]) >= 0.1)
                            {
                                transform.position = Vector2.MoveTowards(transform.position, patrolPointVec[0], Time.deltaTime);
                            }
                            else
                            {
                                patrolPointVec.Add(patrolPointVec[0]);
                                patrolPointVec.RemoveAt(0);
                                MovementPathFunc(gameObject.transform.position, patrolPointVec[0]);
                            }
                        }
                        FollowPath();
                    }
                    break;
                case EPathType.stationary:
                    break;
            }
            transform.position = new Vector2(transform.position.x, transform.position.y);


            if (stationarySwitch)
            {
                stationarySwitch = false;
                currentPathType = EPathType.followPlayer;
            }

        }
    }

    void MovementPathFunc(Vector2 _start, Vector2 _end)
    {

        movementPath = aStar.UpdatePathfinding(_start, _end, enemyType);

        /*
        movementPath = Task.Run(() =>
        {
            return aStar.UpdatePathfinding(_start, _end, enemyType);
        }).Result;
        */

        return;
    }

    void FollowPath()
    {
        if (movementPath.Count > 0)
        {
            previousPos = currentPos;
            transform.position = Vector2.MoveTowards(transform.position, movementPath[0], Time.deltaTime); //Move toward the next point in the path
            currentPos = transform.position;
            if (Vector2.Distance(transform.position, movementPath[0]) <= AStar.moveStep)
            {
                movementPath.RemoveAt(0);
            }
        }
        else
        {
            if (currentPathType == EPathType.followPlayer)
            {
                Vector2 playerPos = PlayerController.Instance.transform.position;

                if (Vector2.Distance(transform.position, playerPos) <= 1.5)
                {
                    if (Vector2.Distance(transform.position, playerPos) >= 0.5)
                    {
                        previousPos = currentPos;
                        transform.position = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime);
                        currentPos = transform.position;
                    }
                }
            }

        }
    }
}
