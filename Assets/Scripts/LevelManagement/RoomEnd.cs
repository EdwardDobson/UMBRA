using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class RoomEnd : MonoBehaviour
{
    public bool Opened;
    SwitchForm m_form;
    public List<GameObject> Enemies = new List<GameObject>();
    SpriteRenderer m_renderer;
    BoxCollider2D m_boxCollider;
    public GameObject GameObjectToHide;
    public GameObject CameraFocus;
    private void Start()
    {
        m_form = GameObject.Find("Player").GetComponent<SwitchForm>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        for(int i = 0;  i < Enemies.Count; ++i)
        {
            if (Enemies[i] == null)
                Enemies.RemoveAt(i);
        }

        if (!m_form.IsInGhostForm())
        {
            if (Enemies.Count > 0)
            {
                Opened = false;
                if (GameObjectToHide != null)
                {
                    GameObjectToHide.GetComponent<BoxCollider2D>().enabled = true;
                    GameObjectToHide.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    m_renderer.enabled = true;
                    m_boxCollider.enabled = true;
                } 

            }
            else if (Enemies.Count <= 0)
            {
                Opened = true;
                if(GameObjectToHide != null)
                {
                    GameObjectToHide.GetComponent<BoxCollider2D>().enabled = false;
                    GameObjectToHide.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    m_renderer.enabled = false;
                    m_boxCollider.enabled = false;
                }


                if (CameraFocus != null)
                {
                    CameraFocus.SetActive(true);
                    CameraFocus = null;
                }
            }
        }
    }
}
