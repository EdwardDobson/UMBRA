using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXObject", menuName = "ScriptableObjects/SFXScriptableObject", order = 1)]
public class SFXScriptable : ScriptableObject
{
    public List<AudioClip> audioClips;

    public string audioCaption;
}
