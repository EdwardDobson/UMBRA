using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public float cameraSpeed = 2;

    Transform focusPoint;
    Transform mainCamera;
    int collisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
        focusPoint = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (focusPoint)
        {
            Vector3 lerpPos = Vector3.MoveTowards(mainCamera.position, new Vector3(focusPoint.position.x, focusPoint.position.y, mainCamera.position.z), Time.fixedDeltaTime * cameraSpeed);//Vector3.Lerp(camera.position, focusPoint.position, 0.05f);
            //lerpPos.z = camera.position.z;
            mainCamera.position = lerpPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CameraZone" && collision.transform.childCount > 0)
        {
            focusPoint = collision.transform.GetChild(0);
            collisionCount += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CameraZone")
        {
            //focusPoint = transform;
            collisionCount -= 1;

            if(collisionCount <= 0)
            {
                focusPoint = transform;
            }
        }
    }
}
