using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector3 spawnPos;
   // public Vector3 roomOffset;
    //public Vector3 spawnOffset;
    //[HideInInspector]
    //public bool loaded = false;
    //[HideInInspector]
    //public int roomNumber;
    //[HideInInspector]
    //public RoomManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        spawnPos = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void Unload()
    //{
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (loaded) return;
        if(collision.tag == "Player")
        {
            //collision.transform.root.GetChild(0).GetComponent<PlayerController>().currentRoom = this;
            RoomManager.Instance.currentRoom = this;

            //loaded = true;
            //manager.Spawn(roomNumber + 1);
            //manager.DeSpawn(roomNumber - 2);

        }
    }
}
