using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapParentDetection : MonoBehaviour
{
    GameObject m_parent;
    SwitchForm m_form;
    private void Start()
    {
        m_parent = transform.parent.gameObject;
        m_form = m_parent.GetComponent<TrapBaseClass>().GetSwitchForm();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_parent.GetComponent<TrapBaseClass>().GetJustTrap())
        {
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Player && !m_form.IsInGhostForm())
            {
                if (collision.gameObject.name.Contains("Player"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(true);
            }
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Ghost && m_form.IsInGhostForm())
            {
                if (collision.gameObject.name.Contains("Ghost"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(true);
            }
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Both)
            {
                if (collision.gameObject.tag.Contains("Player"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_parent.GetComponent<TrapBaseClass>().GetJustTrap())
        {
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Player && !m_form.IsInGhostForm())
            {
                if (collision.gameObject.name.Contains("Player"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(false);
            }
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Ghost && m_form.IsInGhostForm())
            {
                if (collision.gameObject.name.Contains("Ghost"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(false);
            }
            if (m_parent.GetComponent<TrapBaseClass>().GetSide() == Side.Both)
            {
                if (collision.gameObject.tag.Contains("Player"))
                    m_parent.GetComponent<TrapBaseClass>().SetDeployTrap(false);
            }
        }
    }
}
