using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int m_HP = 20;

    private int m_Mana = 1;
    private int m_MaxMana = 1;
    public Card[] m_Hand;
    public Card[] m_Deck;
    public Card[] m_Grave;


    public void Cast(int i_ManaCost)
    {
        m_Mana -= i_ManaCost;
    }

    public void RaiseMaxMana()
    {
        if (m_Mana > 0)
        {
            m_Mana = 0;
            m_MaxMana++;
        }
    }

    public void DrawCard()
    {

    }

    public void DiscardCard()
    {

    }
}
