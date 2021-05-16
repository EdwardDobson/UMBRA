using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace AGPUI
{
    public class DialogBoxes
    {
        static TextMeshProUGUI m_buttonPrompt;
        static GameObject m_displayTip;
        static TextMeshProUGUI m_storyText;
        static GameObject m_storyBoxBackground;
        static ControlBindings m_bindings;
        /// <summary>
        /// Set up to gather all of the text objects
        /// </summary>
        public static void DialogBoxesSetup()
        {
            m_buttonPrompt = GameObject.Find("HUD").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            m_displayTip = GameObject.Find("HUD").transform.GetChild(2).gameObject;
            m_storyText = GameObject.Find("HUD").transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
            m_storyBoxBackground = GameObject.Find("HUD").transform.GetChild(3).gameObject;
            m_bindings = GameObject.Find("GameManager").GetComponent<ControlBindings>();
        }
        /// <summary>
        /// Displays the button prompt above the player
        /// </summary>
        public static void DisplayButtonPrompt(Vector2 _startPos, float _offset, string _prompt)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(_startPos);
            pos = new Vector3(pos.x, pos.y + _offset);
            m_buttonPrompt.transform.position = pos;
            m_buttonPrompt.text = "Press " + _prompt;
        }
        /// <summary>
        /// Hides button prompts
        /// </summary>
        public static void HideButtonPrompt()
        {
            m_buttonPrompt.text = "";
        }
        /// <summary>
        /// Displays tips above a sign
        /// </summary>
        public static void DisplayTip(Vector2 _placedPos, string _tipText)
        {
            m_displayTip.SetActive(true);
            m_displayTip.transform.position = _placedPos;
            m_displayTip.GetComponent<TextMeshProUGUI>().text = _tipText;
        }
        /// <summary>
        /// Hides Display Tip
        /// </summary>
        public static void HideDisplayTip()
        {
            m_displayTip.SetActive(false);
        }
        /// <summary>
        /// Displays fixed story and closed captions
        /// </summary>
        public static void DisplayStory(StoryPiece _storyPiece)
        {
            if (_storyPiece.Sentences[0] != null)
            {
                m_storyBoxBackground.SetActive(true);
                Time.timeScale = 0;
                if (m_storyText.text.Length < _storyPiece.Sentences[0].Length)
                    m_storyText.text += _storyPiece.Sentences[0] + "\n\nPress " + m_bindings.Interact + " to continue";
                if (Input.GetKeyDown(m_bindings.Interact) || Input.GetButtonDown("ButtonA"))
                {
                    _storyPiece.Sentences.Remove(_storyPiece.Sentences[0]);
                    m_storyText.text = "";
                }
            }
            if (_storyPiece.Sentences.Count <= 0)
                HideStory(_storyPiece);
        }
        /// <summary>
        /// Hides the story popup
        /// </summary>
        public static void HideStory(StoryPiece _storyPiece)
        {
            m_storyText.text = "";
            Time.timeScale = 1;
            m_storyBoxBackground.SetActive(false);
            if(!GameObject.Find("StoryManager").GetComponent<StoryManager>().FinishedStories.Contains(_storyPiece.ID))
            GameObject.Find("StoryManager").GetComponent<StoryManager>().FinishedStories.Add(_storyPiece.ID);
        }
    }
}

