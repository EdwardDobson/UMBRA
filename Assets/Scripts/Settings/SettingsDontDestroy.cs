using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDontDestroy : MonoBehaviour
{
    static SettingsDontDestroy m_instance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
