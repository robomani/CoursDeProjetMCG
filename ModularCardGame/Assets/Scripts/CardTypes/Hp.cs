using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : Card
{
    public Hp(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    public override void AddComponentCard(Card i_Card)
    {
        base.AddComponentCard(i_Card);

        if (i_Card.m_CardName == "Movement" || i_Card.m_CardName == "Attack")
        {
            m_Renderer.material.color = Color.black;
            if (i_Card.m_CardName == "Movement")
            {
                m_CardName = "Shadow";
                m_Hp--;
                m_Mouvement--;
                m_CastCost--;
                m_ShadowTime++;
            }
            else if (i_Card.m_CardName == "Attack")
            {
                m_CardName = "Armor";
                m_Hp--;
                m_Attack--;
                m_CastCost--;
                m_Armor++;
            }
        }
    }
}
