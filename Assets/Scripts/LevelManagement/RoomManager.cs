
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
/*
[SerializeField]
public class loadedRoom
{
    public Room room;
    public int number;
    public loadedRoom(Room _room, int _number)
    {
        room = _room;
        number = _number;
    }
}*/
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance = null;

    public Room currentRoom;
    public Vector3 respawnPos;

    private void Awake()
    {
        //Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameoverManager.Instance.go.SetActive(false);
        //Debug.Log("OnSceneLoaded: " + scene.name);
        if (respawnPos != Vector3.zero)
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.transform.position = respawnPos;
                Camera.main.transform.position = new Vector3(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y, Camera.main.transform.position.z);
            }
       
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 1f;
            if(GameObject.Find("Prompt00") != null)
            GameObject.Find("Prompt00").SetActive(false);
        }
    }

    /* public GameObject[] path0;
     public GameObject[] path1;
     public GameObject[] path2;
     [HideInInspector]
     public GameObject[][] rooms;

     public List<loadedRoom> loadedRooms = new List<loadedRoom>();

     public int chosenPath = 0;
     //public int currentRoom = 0;
     // Start is called before the first frame update
     void Start()
     {
         rooms = new GameObject[][] { path0, path1, path2 };

         Spawn(0);
     }

     // Update is called once per frame
     void Update()
     {

     }

     public loadedRoom GetLoadedRoom(int index)
     {
         for (int i = 0; i < loadedRooms.Count; i++)
         {
             if (loadedRooms[i].number == index)
             {
                 return loadedRooms[i];
             }
         }
         return null;
     }

     public void DeSpawn(int index)
     {
         loadedRoom get = GetLoadedRoom(index);
         if (get != null)
         {
             loadedRooms.Remove(get);
             Destroy(get.room.gameObject);
         }
     }

     public void Spawn(int index)
     {
         if (rooms[chosenPath].Length > index)
         {
             DeSpawn(index);
             GameObject prefab = rooms[chosenPath][index];
             Room prefabRoom = prefab.GetComponent<Room>();

             GameObject newLevel = Instantiate(prefab, prefabRoom.roomOffset, Quaternion.identity, transform);
             Room newRoom = newLevel.GetComponent<Room>();
             newRoom.manager = this;
             newRoom.roomNumber = index;
             loadedRooms.Add(new loadedRoom(newRoom, index));

         }
     }*/
}
