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

    public enum Players
    {
        Player,
        AI
    }

    public int m_Position;

    public TileController m_TileOccupied;

    #region Card Stats
    public int m_CastCost = 1;
    public int m_CastRange = 1;

    public int m_ActivateCost = 1;

    public int m_Attack = 1;
    public int m_AttackRange = 1;

    public int m_ArmorPercing;

    public int m_Hp = 1;

    public int m_ShadowOnLifeLost;

    public int m_Mouvement = 0;
    public int m_ArmorAlfterMove;

    public int m_Armor;
    public int m_NumberOfSpellToIgnore;

    public int m_TradeRange;
    public int m_TradeResist;
    public int m_TradeCostReduction;
    public int m_MoveAlfterTrade;
    public int m_TradeAdjacent;
    public int m_ArmorAlfterTrade;
    public int m_ShadowAlfterTrade;
    public int m_PushRange;
    public int m_PushResist;
    public int m_MoveAlfterPush;
    public int m_LateralPush;
    public int m_ArmorAlfterGettingPushed;
    public int m_ShadowAlfterPush;
    public int m_ShadowTime;
    public int m_ShadowTeleportRange;
    public int m_ShadowSightRange;
    public bool m_IndirectAttack;

    #endregion

    public string m_CardName;
    public bool m_Playable = true;
    public bool m_ValidTarget = false;
    public States m_State = States.InDeck;
    public Players m_Owner = Players.AI ;
    protected Renderer m_Renderer;

    protected virtual void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    public void InitCard(CardsData i_CardData)
    {
        for (int i = 0; i < i_CardData.CardTemplates.Length; i++)
        {
            if (i_CardData.CardTemplates[i].CardName == m_CardName)
            {
                CardTemplate temp = i_CardData.CardTemplates[i];
                m_CastCost = temp.CastCost;
                m_CastRange = temp.CastRange;

                m_ActivateCost = temp.ActivateCost;

                m_Attack = temp.Attack;
                m_AttackRange = temp.AttackRange;
                m_ArmorPercing = temp.ArmorPercing;

                m_Hp = temp.Hp;
                m_ShadowOnLifeLost = temp.ShadowOnLifeLost;

                m_Mouvement = temp.Movement;
                m_ArmorAlfterMove = temp.ArmorAlfterMove;

                m_Armor = temp.Armor;
                m_NumberOfSpellToIgnore = temp.NumberOfSpellToIgnore;

                m_TradeRange = temp.TradeRange;
                m_TradeResist = temp.TradeResist;
                m_TradeCostReduction = temp.TradeCostReduction;
                m_MoveAlfterTrade = temp.MoveAlfterTrade;
                m_TradeAdjacent = temp.TradeAdjacent;
                m_ArmorAlfterTrade = temp.ArmorAlfterTrade;
                m_ShadowAlfterTrade = temp.ShadowAlfterTrade;
                m_PushRange = temp.PushRange;
                m_PushResist = temp.PushResist;
                m_MoveAlfterPush = temp.MoveAlfterPush;
                m_LateralPush = temp.LateralPush;
                m_ArmorAlfterGettingPushed = temp.ArmorAlfterGettingPushed;
                m_ShadowAlfterPush = temp.ShadowAlfterPush;
                m_ShadowTime = temp.ShadowTime;
                m_ShadowTeleportRange = temp.ShadowTeleportRange;
                m_ShadowSightRange = temp.ShadowSightRange;
                m_IndirectAttack = temp.IndirectAttack;
                break;
            }
        }
    }

    public bool LoseHp(int i_Damage)
    {
        
        m_Hp -= i_Damage;
        Debug.Log(m_Hp);
        if (m_Hp <= 0)
        {
            return true;
            Debug.Log("To destroy");
        }
        return false;
    }

    public void Illuminate()
    {
        m_Renderer.material.SetColor("_Color", Color.green);
    }

    public void SelectedColor()
    {
        m_Renderer.material.SetColor("_Color", Color.cyan);
    }

    public void UnplayableCard()
    {
        m_Renderer.material.SetColor("_Color", Color.grey);
        m_Playable = false;
    }

    public void ValidTarget()
    {
        m_Renderer.material.SetColor("_Color", Color.red);
        m_ValidTarget = true;
    }

    public void UnIlluminate()
    {
        m_Renderer.material.SetColor("_Color", Color.white);
        m_Playable = true;
        m_ValidTarget = false;
    }
}
