using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    float m_speed;
    Vector2 m_direction;
    Side m_side;
    bool m_isPossessed;
    float m_damage;
    public float KnockbackAmount;
    public SFXScriptable hitSound;
    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        transform.right = m_direction * m_speed;
    }
    void Update()
    {
        m_rb2d.velocity = m_direction * m_speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Terrain")
        {
            Destroy(gameObject);
        }
        if (!m_isPossessed)
        {
            if (m_side == Side.Ghost)
            {
                if (collision.name.Contains("Ghost"))
                {
                    Destroy(gameObject);
                }
            }
            if (m_side == Side.Player)
            {
                if (collision.name.Contains("Player"))
                {
                    AudioManager.Instance.PlaySFX(hitSound.audioClips[0], hitSound.audioCaption);
                    collision.GetComponent<RecieveDamage>().TakeDamage(m_damage, transform.position, KnockbackAmount);
                    if (collision.GetComponent<BuffManager>() != null)
                    collision.GetComponent<BuffManager>().AddEffect(1, 2, 2, true, EffectType.Debuff, EffectAttribute.Speed);
                    Destroy(gameObject);
                }
            }
            if (m_side == Side.Both)
            {
                if (collision.tag == "Player")
                {
                    Destroy(gameObject);
                }
            }
        }
        if (m_isPossessed)
        {
            if (collision.tag == "Enemy")
            {
                if(collision.gameObject != transform.parent.gameObject)
                {
                    AudioManager.Instance.PlaySFX(hitSound.audioClips[0], hitSound.audioCaption);
                    collision.GetComponent<RecieveDamage>().TakeDamage(m_damage, transform.position, KnockbackAmount);
                    Destroy(gameObject);
                }
            }
        }
    }
    public void SetVariables(float _speed, Vector2 _direction, Side _side, bool _isPossessed, float _damage)
    {
        m_speed = _speed;
        m_direction = _direction;
        m_side = _side;
        m_isPossessed = _isPossessed;
        m_damage = _damage;
    }
}
