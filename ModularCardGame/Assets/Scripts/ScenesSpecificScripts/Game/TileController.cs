using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool m_IsValid = false;
    public bool m_IsValidTarget = false;
    public Card m_OccupiedBy = null;
    public int PosX;
    public int PosY;
    public GameObject m_AttackRangeVisual;
    public GameObject m_Avatar = null;
    public GameObject m_ShadowEffect;

    private Renderer m_Renderer;
    private Color m_Color;
    private Color m_AvatarColor;
    private int m_ShadowTime;
    private Card.Players m_Owner;
    private Vector3 m_baseShadowPosition;
    private Vector3 m_currentShadowPosition;

    private void Start()
    {
        if (m_ShadowEffect != null)
        {
            m_baseShadowPosition = m_ShadowEffect.transform.position;
            m_currentShadowPosition = m_baseShadowPosition;
        }
        m_Renderer = gameObject.GetComponent<Renderer>();
        m_Color = m_Renderer.material.color;
        if (m_Avatar != null)
        {
            m_AvatarColor = m_Avatar.GetComponent<Renderer>().material.color;
        }

        GameController.ChangeTurn += ShadowTick;
    }

    private void OnDestroy()
    {
        GameController.ChangeTurn -= ShadowTick;
    }

    public void Illuminate()
    {
        m_Renderer.material.SetColor("_Color", Color.green);
        m_IsValid = true;
    }

    public void EnemyIlluminate()
    {
        m_Renderer.material.SetColor("_Color", Color.red);
        m_IsValid = true;
    }

    public void UnIlluminate()
    {
        m_Renderer.material.SetColor("_Color", m_Color);
        m_IsValid = false;
        m_IsValidTarget = false;
        m_AttackRangeVisual.SetActive(false);
        if (m_Avatar != null)
        {
            m_Avatar.GetComponent<Renderer>().material.SetColor("_Color", m_AvatarColor);
        }
    }

    public void InAttackRange()
    {
        m_AttackRangeVisual.SetActive(true);
        m_IsValidTarget = true;
        if (m_Avatar != null)
        {
            m_Avatar.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public void ClearTile()
    {
        m_OccupiedBy = null;
        Debug.Log("Clear tile");
    }

    public void SetShadow(int i_Time, Card.Players i_Owner)
    {
        m_Owner = i_Owner;
        if (m_ShadowEffect != null)
        {
            m_ShadowEffect.SetActive(true);
            if (m_Owner == Card.Players.Player)
            {
                Vector3 temp = m_baseShadowPosition;
                temp.y = m_baseShadowPosition.y - 2.75f;
                temp.x = m_baseShadowPosition.x - 0.5f;
                m_currentShadowPosition = temp;
            }
            else
            {
                m_currentShadowPosition = m_baseShadowPosition;
            }

            m_ShadowEffect.transform.position = m_currentShadowPosition;
        }
        if (i_Time > m_ShadowTime)
        {
            m_ShadowTime = i_Time;
        }

        if (m_OccupiedBy != null && m_OccupiedBy.m_Owner != m_Owner)
        {
            ClearShadow();
        }
    }

    public void ClearShadow()
    {
        if (m_ShadowEffect != null)
        {
            m_ShadowEffect.SetActive(false);
        }
        m_ShadowTime = 0;
    }

    private void ShadowTick()
    {
        if (m_ShadowTime > 0)
        {
            m_ShadowTime--;
        }

        if (m_ShadowEffect != null && m_ShadowEffect.activeSelf && m_ShadowTime <= 0)
        {
            m_ShadowEffect.SetActive(false);
        }
    }
}
