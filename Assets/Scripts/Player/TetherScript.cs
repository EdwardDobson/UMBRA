using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherScript : MonoBehaviour
{
    public Transform ghostTransform;
    public Transform playerTransform;

    bool isVisible = false;

    [SerializeField] GameObject node1;
    [SerializeField] GameObject node2;
    [SerializeField] GameObject line;

    [SerializeField] Vector3 playerOffset;
    [SerializeField] Vector3 ghostOffset;

    SpriteRenderer /*end1, end2,*/ middle;
    ParticleSystem psLine, ps1, ps2;
    ParticleSystem.EmissionModule emLine, em1, em2;
    Transform cameraTransform;
    float vertExtent;
    float horzExtent;

    float proximity;
    public bool tetherBroken = false;




    // Start is called before the first frame update
    void Start()
    {
       // end1 = node1.GetComponent<SpriteRenderer>();
        //end2 = node2.GetComponent<SpriteRenderer>();
        middle = line.GetComponent<SpriteRenderer>();

        psLine = line.GetComponent<ParticleSystem>();
        ps1 = node1.GetComponent<ParticleSystem>();
        ps2 = node2.GetComponent<ParticleSystem>();

        emLine = psLine.emission;
        em1 = ps1.emission;
        em2 = ps2.emission;

        cameraTransform = Camera.main.transform;

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }


    public void ShowTether(Transform ghost, Transform player)
    {
        isVisible = true;
        playerTransform = player;
        ghostTransform = ghost;
        tetherBroken = false;
    }

    public void BreakTether()
    {
        ghostTransform = node1.transform;
        tetherBroken = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            //node1.SetActive(true);
            //node2.SetActive(true);
            line.SetActive(true);

            if (tetherBroken)
            {
                if (Vector3.Distance(node1.transform.position, node2.transform.position) <= 1f)
                {
                    isVisible = false;
                    tetherBroken = false;
                    //node1.SetActive(false);
                    //node2.SetActive(false);
                    em1.rateOverTimeMultiplier = 0;
                    em2.rateOverTimeMultiplier = 0;
                    line.SetActive(false);
                    playerTransform = null;
                    ghostTransform = null;
                    return;
                }

                ghostTransform.position = Vector3.Lerp(node1.transform.position, node2.transform.position, Time.deltaTime * 10f);
            }


            Vector3 playerPos2D = new Vector3(playerTransform.position.x, playerTransform.position.y, 0) + playerOffset;
            Vector3 ghostPos2D = new Vector3(ghostTransform.position.x, ghostTransform.position.y, 0) + ghostOffset;



            float proximityX = Mathf.Min(Mathf.Abs(cameraTransform.position.x - ghostTransform.position.x) / horzExtent, 1);
            float proximityY = Mathf.Min(Mathf.Abs(cameraTransform.position.y - ghostTransform.position.y) / vertExtent, 1);


            proximity = Mathf.Max(proximityX, proximityY);
            proximity = proximity * proximity * proximity;

            proximity = Mathf.Max(proximity, 0.1f);
            //if(Vector3.Distance(playerPos2D, ghostPos2D) < 2f)
            //{
            //proximity = Mathf.Max(Vector3.Distance(playerPos2D, ghostPos2D), proximity);
            //proximity = Vector3.Distance(playerPos2D, ghostPos2D);
            //if (proximity < 0.2f)
            //{
            //    proximity = 0f;
            //}
            //}


            Color color = new Color(1, 1, 1, proximity);
            //end1.color = color;
            //end2.color = color;
            middle.color = color;
            int density = (int)(proximity * 500);

            //var lineMain = psLine.emission;
            //var p1Main = ps1.emission;
            //var p2Main = ps2.emission;
            emLine.rateOverTimeMultiplier = density * 3;
            em1.rateOverTimeMultiplier = density;
            em2.rateOverTimeMultiplier = density;

            if (!tetherBroken)
            {
                node1.transform.position = ghostPos2D;
                //node1.transform.up = ghostPos2D - playerPos2D;
            }
            node2.transform.position = playerPos2D;
                //node2.transform.up = node1.transform.up;
            
            line.transform.position = playerPos2D;
            line.transform.up = ghostPos2D - playerPos2D;
            line.transform.localScale = new Vector3(1, Vector3.Distance(playerPos2D, ghostPos2D), 1);


        }
        else
        {
            //node1.SetActive(false);
            //node2.SetActive(false);
            line.SetActive(false);
        }
    }
}
