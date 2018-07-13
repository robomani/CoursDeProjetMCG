using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{

    Creature,
    Building,
    Spell,
    Component
}

[System.Serializable]
public struct CardTemplate
{
    [SerializeField]
    private string m_CardName;
    [SerializeField]
    private CardType m_TemplateCardType;
    [SerializeField]
    private int m_CastCost;
    [SerializeField]
    private int m_CastRange;
    [SerializeField]
    private int m_ActivateCost;
    [SerializeField]
    private int m_Attack;
    [SerializeField]
    private int m_AttackRange;
    [Tooltip("Number of armor point bypased each attack")]
    [SerializeField]
    private int m_ArmorPercing;
    [SerializeField]
    private int m_Hp;
    [Tooltip("Number turn of shadow in the tile occupied when the card take damage")]
    [SerializeField]
    private int m_ShadowOnLifeLost;
    [SerializeField]
    private int m_Movement;
    [Tooltip("Number of armor point added alfter move (Disapear at the start of the next turn of the contrôler)")]
    [SerializeField]
    private int m_ArmorAlfterMove;
    [SerializeField]
    private int m_Armor;
    [Tooltip("Number spell effect the card ignore in a turn (Benefic spell also trigger the ignore)")]
    [SerializeField]
    private int m_NumberOfSpellToIgnore;
    [Tooltip("Same as movement range but for the trade effect")]
    [SerializeField]
    private int m_TradeRange;
    [Tooltip("Reduce the effective trade range by this number on any trade effect targeted at this card (Negate the trade if the reduction drop the range to 0 or lower)")]
    [SerializeField]
    private int m_TradeResist;
    [Tooltip("Augment the effective trade range by this number on any trade effect used by this card (No use against cards with no trade resist)")]
    [SerializeField]
    private int m_TradeCostReduction;
    [Tooltip("Grant a free movement of this number of tile alfter a successful trade initiated by this card")]
    [SerializeField]
    private int m_MoveAlfterTrade;
    [Tooltip("Can select a tile in this range to trade with the target tile instead of the tile occupied by the card that initiated the trade")]
    [SerializeField]
    private int m_TradeAdjacent;
    [Tooltip("Number of armor point added alfter a trade on or by this card")]
    [SerializeField]
    private int m_ArmorAlfterTrade;
    [Tooltip("Number of turn of shadow added to the new tile of the card alfter a trade on or by this card")]
    [SerializeField]
    private int m_ShadowAlfterTrade;
    [Tooltip("Number of tile the target of an attack initiated by this card is pushed (a spell push in the direction selected when cast)")]
    [SerializeField]
    private int m_PushRange;
    [Tooltip("Decrase the effective push range by this number on any push effect used on this card (Negate the push if the reduction drop the range to 0 or lower)")]
    [SerializeField]
    private int m_PushResist;
    [Tooltip("Can move up to this range following a card pushed by this card")]
    [SerializeField]
    private int m_MoveAlfterPush;
    [Tooltip("Can push up to this number of tile lateraly before pushing foward")]
    [SerializeField]
    private int m_LateralPush;
    [Tooltip("Number of armor point added alfter a push on this card")]
    [SerializeField]
    private int m_ArmorAlfterGettingPushed;
    [Tooltip("Create this number of turn of shadow on any tile leaved by a card pushed by this card")]
    [SerializeField]
    private int m_ShadowAlfterPush;
    [Tooltip("Create this number of turn of shadow on any tile occupied by this card when activated or casted")]
    [SerializeField]
    private int m_ShadowTime;
    [Tooltip("Can teleport to any tile under this range if the tile has shadow and is empty")]
    [SerializeField]
    private int m_ShadowTeleportRange;
    [Tooltip("Can see the content of any tile with shadow in this range of the card (Does not dispel the shadow)")]
    [SerializeField]
    private int m_ShadowSightRange;
    [Tooltip("Can see the content of any tile with shadow in this range of the card (Does not dispel the shadow)")]
    [SerializeField]
    private bool m_IndirectAttack;

    public bool IndirectAttack
    {
        get { return m_IndirectAttack; }
    }

    public string CardName
    {
        get { return m_CardName; }
    }

    public CardType TemplateCardType
    {
        get { return m_TemplateCardType; }
    }

    public int CastCost
    {
        get { return m_CastCost; }
    }

    public int CastRange
    {
        get { return m_CastRange; }
    }

    public int ActivateCost
    {
        get { return m_ActivateCost; }
    }

    public int Attack
    {
        get { return m_Attack; }
    }

    public int AttackRange
    {
        get { return m_AttackRange; }
    }

    public int ArmorPercing
    {
        get { return m_ArmorPercing; }
    }

    public int Hp
    {
        get { return m_Hp; }
    }

    public int ShadowOnLifeLost
    {
        get { return m_ShadowOnLifeLost; }
    }

    public int Movement
    {
        get { return m_Movement; }
    }

    public int ArmorAlfterMove
    {
        get { return m_ArmorAlfterMove; }
    }

    public int Armor
    {
        get { return m_Armor; }
    }

    public int NumberOfSpellToIgnore
    {
        get { return m_NumberOfSpellToIgnore; }
    }

    public int TradeRange
    {
        get { return m_TradeRange; }
    }

    public int TradeResist
    {
        get { return m_TradeResist; }
    }

    public int TradeCostReduction
    {
        get { return m_TradeCostReduction; }
    }

    public int MoveAlfterTrade
    {
        get { return m_MoveAlfterTrade; }
    }

    public int TradeAdjacent
    {
        get { return m_TradeAdjacent; }
    }

    public int ArmorAlfterTrade
    {
        get { return m_ArmorAlfterTrade; }
    }

    public int ShadowAlfterTrade
    {
        get { return m_ShadowAlfterTrade; }
    }

    public int PushRange
    {
        get { return m_PushRange; }
    }

    public int PushResist
    {
        get { return m_PushResist; }
    }

    public int MoveAlfterPush
    {
        get { return m_MoveAlfterPush; }
    }

    public int LateralPush
    {
        get { return m_LateralPush; }
    }

    public int ArmorAlfterGettingPushed
    {
        get { return m_ArmorAlfterGettingPushed; }
    }

    public int ShadowAlfterPush
    {
        get { return m_ShadowAlfterPush; }
    }

    public int ShadowTime
    {
        get { return m_ShadowTime; }
    }

    public int ShadowTeleportRange
    {
        get { return m_ShadowTeleportRange; }
    }

    public int ShadowSightRange
    {
        get { return m_ShadowSightRange; }
    }
}

[CreateAssetMenu(menuName = "ScriptableObject/CardData", fileName = "new Card Data", order = 1)]
public class CardsData : ScriptableObject
{
    [SerializeField]
    private CardTemplate[] m_CardTemplates;

    public CardTemplate[] CardTemplates
    {
        get { return m_CardTemplates; }
    }
}
