using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CapTimer : MonoBehaviour
{
    public float lastTime = 0f;
    public float duration = 10f;

}

public class ClosedCaptions : MonoBehaviour {

    public static TextMeshProUGUI[] captionsList = new TextMeshProUGUI[20];

    public static void SpawnCaption(string _caption){

        int listInsert = 0;
        
        for(int x = 0; x < captionsList.Length; x++)
        {
            if(captionsList[x] == null)
            {
                listInsert = x;
                break;
            }
        }

        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        Color col = new Color(0, 0, 0, 0.5f);
        i.color = col;

        panel.transform.SetParent(GameObject.Find("HUD").transform, false);

        int panelHeight = 40;
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(500, panelHeight);

        float heightOffset = Screen.height / 20;

        Vector2 pos = new Vector2(Screen.width / 2, (Screen.height - heightOffset) - ((panelHeight * 1f) * listInsert));
        panel.transform.position = pos;

        GameObject txtObj = new GameObject();
        txtObj.transform.parent = panel.transform;
        txtObj.name = "caption";    

        TextMeshProUGUI captionObj = txtObj.AddComponent<TextMeshProUGUI>();
        CapTimer capTime = txtObj.AddComponent<CapTimer>();
        capTime.duration = 1f;

        captionObj.text = "[" + _caption + "]";
        captionObj.fontSize = 24;
        captionObj.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 100);
        captionObj.horizontalAlignment = HorizontalAlignmentOptions.Center;
        captionObj.verticalAlignment = VerticalAlignmentOptions.Middle;    
        

        Debug.Log(_caption);
        captionObj.transform.position = pos;

        captionsList[listInsert] = captionObj;

        Destroy(panel, 1f);
    }
}

