using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Card
{
    public Movement(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    public override void AddComponentCard(Card i_Card)
    {
        base.AddComponentCard(i_Card);
        /*
        if (i_Card.m_CardName == "Hp" || i_Card.m_CardName == "Attack" || i_Card.m_CardName == m_CardName)
        {
            m_Renderer.material.color = Color.black;
            if (i_Card.m_CardName == "Hp")
            {
                m_CardName = "Shadow";
                m_Hp--;
                m_Mouvement--;
                m_CastCost--;
                m_ShadowTime++;
            }
            else if (i_Card.m_CardName == "Attack")
            {
                m_CardName = "Push";
                m_Mouvement--;
                m_Attack--;
                m_CastCost--;
                m_PushRange++;
            }
            else
            {
                m_CardName = "Trade";
                m_Mouvement -= 2;
                m_CastCost--;
                m_TradeRange++;
            }
        }
        */
    }
}
