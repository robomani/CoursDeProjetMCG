using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool m_IsValid = false;
    public bool m_IsOccupied = false;
    public int PosX;
    public int PosY;

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
    }
}
