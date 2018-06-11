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

    public TileController m_TileOccupied;

    #region Card Stats
    public int m_CastCost = 1;
    public int m_CastRange = 1;

    public int m_ActivateCost = 1;

    public int m_Attack = 1;
    public int m_AttackRange = 1;

    public int m_Hp = 1;
    public int m_Mouvement = 0;
    #endregion

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
