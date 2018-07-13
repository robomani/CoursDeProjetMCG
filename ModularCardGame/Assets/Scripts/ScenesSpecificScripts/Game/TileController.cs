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

    private Renderer m_Renderer;
    private Color m_Color;
    private Color m_AvatarColor;

    private void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
        m_Color = m_Renderer.material.color;
        if (m_Avatar != null)
        {
            m_AvatarColor = m_Avatar.GetComponent<Renderer>().material.color;
        }
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
}
