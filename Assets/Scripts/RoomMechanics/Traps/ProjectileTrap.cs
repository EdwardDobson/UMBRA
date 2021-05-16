using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrap : TrapBaseClass
{
    [SerializeField]
    GameObject m_projectilePrefab;
    Transform m_firePoint;
    public float ProjectileSpeed;
    public float ProjectileAliveDuration;
    public Vector2 ProjectileDirection;

    private void Start()
    {
        m_firePoint = transform.GetChild(0);
        TrapSetUp();
    }
    void Update()
    {
        if (FireTrap)
        {
            Fire();
        }
        BaseLoop();
    }
    void Fire()
    {
        GameObject clone = Instantiate(m_projectilePrefab, m_firePoint);
        clone.transform.position = m_firePoint.position;
        clone.GetComponent<Projectile>().SetVariables(ProjectileSpeed, ProjectileDirection, FormToHit, IsPossessed, TrapDamage);
        Destroy(clone, ProjectileAliveDuration);
        FireTrap = false;
    }
}
