using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Card
{
    public Tower(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    protected override void Awake()
    {
        base.Awake();
        m_CharacterPrefab = (GameObject)Resources.Load("CharTower", typeof(GameObject));
        
    }

    public override void AddComponentCard(Card i_Card)
    {
        if (m_State == States.InHand)
        {
            m_CastCost += i_Card.CastingCost;
            m_ActivateCost += i_Card.ActivateCost;
        }
        else
        {
            m_ActivateCost += Mathf.Max(Mathf.CeilToInt((i_Card.ActivateCost + i_Card.CastingCost) / 2), 1);
        }

        m_Attack += i_Card.Attack;
        m_ArmorPercing += i_Card.ArmorPercing;

        m_Hp += i_Card.HP;
        m_ShadowOnLifeLost += i_Card.ShadowOnLifeLost;

        m_Armor += i_Card.Armor;
        m_NumberOfSpellToIgnore += i_Card.NumberOfSpellToIgnore;

        m_ShadowTime += i_Card.ShadowTime;
        m_ShadowSightRange += i_Card.ShadowSightRange;

        if (m_ShadowTime > 0 && m_TileOccupied)
        {
            m_TileOccupied.SetShadow(m_ShadowTime, m_Owner);
        }
        UpdateStatsOnCard();
    }
}
