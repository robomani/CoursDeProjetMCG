using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : MonoBehaviour
{

    public enum States
    {
        InDeck,
        InHand,
        InPlay,
        InGrave
    }

    public int m_Position;
    public int m_Portee = 1;
    public int m_Mouvement = 0;
    public string m_CardName;
    public States m_State = States.InDeck;
    protected Renderer m_Renderer;

    protected virtual void Awake()
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
