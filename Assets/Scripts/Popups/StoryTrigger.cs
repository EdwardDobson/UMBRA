
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGPUI;
using System;

[Serializable]
public struct StoryPiece
{
    public int ID;
    public List<string> Sentences;
}
public class StoryTrigger : MonoBehaviour
{
    bool m_displayStory;
    public StoryPiece StoryPiece;
    private void Update()
    {
        if (m_displayStory && StoryPiece.Sentences.Count > 0)
            DialogBoxes.DisplayStory(StoryPiece);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            m_displayStory = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
            m_displayStory = false;
    }
}
