using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : MonoBehaviour
{
    public string m_CardName;
    public int m_Position;

    private Renderer m_Renderer;



    public Card(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    public void Illuminate()
    {
        m_Renderer.material.SetColor("_Color", Color.green);
    }

    public void SelectedColor()
    {
        m_Renderer.material.SetColor("_Color", Color.cyan);
    }

    public void UnIlluminate()
    {
        m_Renderer.material.SetColor("_Color", Color.white);
    }
}
