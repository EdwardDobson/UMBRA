using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestDummy : MonoBehaviour
{
    public RecieveDamage damage;
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = ""+(int)damage.damageBuffer;
    }
}
