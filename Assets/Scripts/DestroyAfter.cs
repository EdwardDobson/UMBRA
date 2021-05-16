using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{

    public float Duration = 1.5f;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= Duration)
        {
            Destroy(gameObject);
        }
    }
}
