using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Card : MonoBehaviour
{

    public enum States
    {
        InDeck,
        InHand,
        InPlay,
        InGrave,
        InVoid
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
    public CardType m_CardType;
    #endregion


    public string m_CardName;
    public bool m_Playable = true;
    public bool m_ValidTarget = false;
    public States m_State = States.InDeck;
    public Players m_Owner = Players.AI ;
    public TextMeshPro m_CostText;
    public TextMeshPro m_AttackText;
    public TextMeshPro m_HpText;
    public TextMeshPro m_MoveText;

    protected Renderer m_Renderer;

    protected virtual void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        TextMeshPro[] temp = gameObject.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro TM in temp)
        {
            if (TM.name == "CostText")
            {
                m_CostText = TM;
            }
            else if (TM.name == "HpText")
            {
                m_HpText = TM;
            }
            else if (TM.name == "AttackText")
            {
                m_AttackText = TM;
            }
            else if (TM.name == "MoveText")
            {
                m_MoveText = TM;
            }
        } 
    }

    protected void Start()
    {
        UpdateStatsOnCard();
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

                m_CardType = temp.TemplateCardType;
                break;
            }
        }
    }

    public void UpdateStatsOnCard()
    {
        if (m_CardType == CardType.Component)
        {
            m_CostText.text = m_CastCost.ToString();
            m_AttackText.text = "+" + m_Attack;
            m_HpText.text = "+" + m_Hp;
            m_MoveText.text = "+" + m_Mouvement;
        }
        else
        {
            m_CostText.text = m_CastCost + "|" + m_ActivateCost;
            m_AttackText.text = m_Attack.ToString();
            m_HpText.text = m_Hp.ToString();
            m_MoveText.text = m_Mouvement.ToString();
        }  
        
    }

    public void CopyCard(Card i_Card)
    {
        m_CastCost = i_Card.m_CastCost;
        m_CastRange = i_Card.m_CastRange;

        m_ActivateCost = i_Card.m_ActivateCost;

        m_Attack = i_Card.m_Attack;
        m_AttackRange = i_Card.m_AttackRange;
        m_ArmorPercing = i_Card.m_ArmorPercing;

        m_Hp = i_Card.m_Hp;
        m_ShadowOnLifeLost = i_Card.m_ShadowOnLifeLost;

        m_Mouvement = i_Card.m_Mouvement;
        m_ArmorAlfterMove = i_Card.m_ArmorAlfterMove;

        m_Armor = i_Card.m_Armor;
        m_NumberOfSpellToIgnore = i_Card.m_NumberOfSpellToIgnore;

        m_TradeRange = i_Card.m_TradeRange;
        m_TradeResist = i_Card.m_TradeResist;
        m_TradeCostReduction = i_Card.m_TradeCostReduction;
        m_MoveAlfterTrade = i_Card.m_MoveAlfterTrade;
        m_TradeAdjacent = i_Card.m_TradeAdjacent;
        m_ArmorAlfterTrade = i_Card.m_ArmorAlfterTrade;
        m_ShadowAlfterTrade = i_Card.m_ShadowAlfterTrade;
        m_PushRange = i_Card.m_PushRange;
        m_PushResist = i_Card.m_PushResist;
        m_MoveAlfterPush = i_Card.m_MoveAlfterPush;
        m_LateralPush = i_Card.m_LateralPush;
        m_ArmorAlfterGettingPushed = i_Card.m_ArmorAlfterGettingPushed;
        m_ShadowAlfterPush = i_Card.m_ShadowAlfterPush;
        m_ShadowTime = i_Card.m_ShadowTime;
        m_ShadowTeleportRange = i_Card.m_ShadowTeleportRange;
        m_ShadowSightRange = i_Card.m_ShadowSightRange;
        m_IndirectAttack = i_Card.m_IndirectAttack;

        m_CardType = i_Card.m_CardType;

        UpdateStatsOnCard();
    }

    public bool LoseHp(int i_Damage)
    {
        
        m_Hp -= i_Damage;
        if (m_Hp <= 0)
        {
            return true;
        }
        UpdateStatsOnCard();
        return false;
    }

    public void AddComponentCard(Card i_Card)
    {
        if (m_State == States.InHand)
        {
            m_CastCost += i_Card.m_CastCost;
            m_ActivateCost += i_Card.m_ActivateCost;

            m_Attack += i_Card.m_Attack;
            m_AttackRange += i_Card.m_AttackRange;
            m_ArmorPercing += i_Card.m_ArmorPercing;

            m_Hp += i_Card.m_Hp;
            m_ShadowOnLifeLost += i_Card.m_ShadowOnLifeLost;

            m_Mouvement += i_Card.m_Mouvement;
            m_ArmorAlfterMove += i_Card.m_ArmorAlfterMove;

            m_Armor += i_Card.m_Armor;
            m_NumberOfSpellToIgnore += i_Card.m_NumberOfSpellToIgnore;

            m_TradeRange += i_Card.m_TradeRange;
            m_TradeResist += i_Card.m_TradeResist;
            m_TradeCostReduction += i_Card.m_TradeCostReduction;
            m_MoveAlfterTrade += i_Card.m_MoveAlfterTrade;
            m_TradeAdjacent += i_Card.m_TradeAdjacent;
            m_ArmorAlfterTrade += i_Card.m_ArmorAlfterTrade;
            m_ShadowAlfterTrade += i_Card.m_ShadowAlfterTrade;
            m_PushRange += i_Card.m_PushRange;
            m_PushResist += i_Card.m_PushResist;
            m_MoveAlfterPush += i_Card.m_MoveAlfterPush;
            m_LateralPush += i_Card.m_LateralPush;
            m_ArmorAlfterGettingPushed += i_Card.m_ArmorAlfterGettingPushed;
            m_ShadowAlfterPush += i_Card.m_ShadowAlfterPush;
            m_ShadowTime += i_Card.m_ShadowTime;
            m_ShadowTeleportRange += i_Card.m_ShadowTeleportRange;
            m_ShadowSightRange += i_Card.m_ShadowSightRange;
        }
        else
        {      
            m_ActivateCost += Mathf.Max(Mathf.CeilToInt((i_Card.m_ActivateCost + i_Card.m_CastCost)/2),1);

            m_Attack += i_Card.m_Attack;
            m_AttackRange += i_Card.m_AttackRange;
            m_ArmorPercing += i_Card.m_ArmorPercing;

            m_Hp += i_Card.m_Hp;
            m_ShadowOnLifeLost += i_Card.m_ShadowOnLifeLost;

            m_Mouvement += i_Card.m_Mouvement;
            m_ArmorAlfterMove += i_Card.m_ArmorAlfterMove;

            m_Armor += i_Card.m_Armor;
            m_NumberOfSpellToIgnore += i_Card.m_NumberOfSpellToIgnore;

            m_TradeRange += i_Card.m_TradeRange;
            m_TradeResist += i_Card.m_TradeResist;
            m_TradeCostReduction += i_Card.m_TradeCostReduction;
            m_MoveAlfterTrade += i_Card.m_MoveAlfterTrade;
            m_TradeAdjacent += i_Card.m_TradeAdjacent;
            m_ArmorAlfterTrade += i_Card.m_ArmorAlfterTrade;
            m_ShadowAlfterTrade += i_Card.m_ShadowAlfterTrade;
            m_PushRange += i_Card.m_PushRange;
            m_PushResist += i_Card.m_PushResist;
            m_MoveAlfterPush += i_Card.m_MoveAlfterPush;
            m_LateralPush += i_Card.m_LateralPush;
            m_ArmorAlfterGettingPushed += i_Card.m_ArmorAlfterGettingPushed;
            m_ShadowAlfterPush += i_Card.m_ShadowAlfterPush;
            m_ShadowTime += i_Card.m_ShadowTime;
            m_ShadowTeleportRange += i_Card.m_ShadowTeleportRange;
            m_ShadowSightRange += i_Card.m_ShadowSightRange;
        }
        

        UpdateStatsOnCard();
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
