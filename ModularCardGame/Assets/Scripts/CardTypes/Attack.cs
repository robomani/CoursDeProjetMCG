using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{

    public Attack(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    public override void AddComponentCard(Card i_Card)
    {
        base.AddComponentCard(i_Card);
        /*
        if (i_Card.m_CardName == "Hp" || i_Card.m_CardName == "Movement")
        {
            m_Renderer.material.color = Color.black;
            if (i_Card.m_CardName == "Hp")
            {
                m_CardName = "Armor";
                m_Hp--;
                m_Attack--;
                m_CastCost--;
                m_Armor++;
            }
            else
            {
                m_CardName = "Push";
                m_Mouvement--;
                m_Attack--;
                m_CastCost--;
                m_PushRange++;
            }
        }
        */
    }
}
