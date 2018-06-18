using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool m_IsValid = false;
    public Card m_OccupiedBy = null;
    public int PosX;
    public int PosY;
    public GameObject m_AttackRangeVisual;

    private Renderer m_Renderer;

    private void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
    }

    public void Illuminate()
    {
        m_Renderer.material.SetColor("_Color", Color.green);
        m_IsValid = true;
    }

    public void UnIlluminate()
    {
        m_Renderer.material.SetColor("_Color", Color.white);
        m_IsValid = false;
        m_AttackRangeVisual.SetActive(false);
    }

    public void InAttackRange()
    {
        m_AttackRangeVisual.SetActive(true);       
    }

    public void ClearTile()
    {
        m_OccupiedBy = null;
    }
}
