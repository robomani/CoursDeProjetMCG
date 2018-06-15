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
    private CardType TemplateCardType;
    [SerializeField]
    private int CastCost;
    [SerializeField]
    private int CastRange;
    [SerializeField]
    private int ActivateCost;
    [SerializeField]
    private int Attack;
    [Tooltip("Number of armor point bypased each attack")]
    [SerializeField]
    private int ArmorPercing;
    [SerializeField]
    private int AttackRange;
    [SerializeField]
    private int Hp;
    [Tooltip("Number turn of shadow in the tile occupied when the card take damage")]
    [SerializeField]
    private int ShadowOnLifeLost;
    [SerializeField]
    private int Movement;
    [Tooltip("Number of armor point added alfter move (Disapear at the start of the next turn of the contrôler)")]
    [SerializeField]
    private int ArmorAlfterMove;
    [SerializeField]
    private int Armor;
    [Tooltip("Number spell effect the card ignore in a turn (Benefic spell also trigger the ignore)")]
    [SerializeField]
    private int NumberOfSpellToIgnore;
    [Tooltip("Same as movement range but for the trade effect")]
    [SerializeField]
    private int TradeRange;
    [Tooltip("Reduce the effective trade range by this number on any trade effect targeted at this card (Negate the trade if the reduction drop the range to 0 or lower)")]
    [SerializeField]
    private int TradeResist;
    [Tooltip("Augment the effective trade range by this number on any trade effect used by this card (No use against cards with no trade resist)")]
    [SerializeField]
    private int TradeCostReduction;
    [Tooltip("Grant a free movement of this number of tile alfter a successful trade initiated by this card")]
    [SerializeField]
    private int MoveAlfterTrade;
    [Tooltip("Can select a tile in this range to trade with the target tile instead of the tile occupied by the card that initiated the trade")]
    [SerializeField]
    private int TradeAdjacent;
    [Tooltip("Number of armor point added alfter a trade on or by this card")]
    [SerializeField]
    private int ArmorAlfterTrade;
    [Tooltip("Number of turn of shadow added to the new tile of the card alfter a trade on or by this card")]
    [SerializeField]
    private int ShadowAlfterTrade;
    [Tooltip("Number of tile the target of an attack initiated by this card is pushed (a spell push in the direction selected when cast)")]
    [SerializeField]
    private int PushRange;
    [Tooltip("Decrase the effective push range by this number on any push effect used on this card (Negate the push if the reduction drop the range to 0 or lower)")]
    [SerializeField]
    private int PushResist;
    [Tooltip("Can move up to this range following a card pushed by this card")]
    [SerializeField]
    private int MoveAlfterPush;
    [Tooltip("Can push up to this number of tile lateraly before pushing foward")]
    [SerializeField]
    private int LateralPush;
    [Tooltip("Number of armor point added alfter a push on this card")]
    [SerializeField]
    private int ArmorAlfterGettingPushed;
    [Tooltip("Create this number of turn of shadow on any tile leaved by a card pushed by this card")]
    [SerializeField]
    private int ShadowAlfterPush;
    [Tooltip("Create this number of turn of shadow on any tile occupied by this card when activated or casted")]
    [SerializeField]
    private int ShadowTime;
    [Tooltip("Can teleport to any tile under this range if the tile has shadow and is empty")]
    [SerializeField]
    private int ShadowTeleportRange;
    [Tooltip("Can see the content of any tile with shadow in this range of the card (Does not dispel the shadow)")]
    [SerializeField]
    private int ShadowSightRange;

}

[CreateAssetMenu(menuName = "ScriptableObject/CardData", fileName = "new Card Data", order = 1)]
public class CardsData : ScriptableObject
{
    [SerializeField]
    private CardTemplate[] m_CardTemplates;
    /*
    #region Soldat
    [Header("Creature Soldat")]
    public int SoldatCastCost = 1;
    public int SoldatCastRange = 1;
    public int SoldatActivateCost = 1;
    public int SoldatAttack = 1;
    public int SoldatAttackRange = 1;
    public int SoldatHp = 1;
    public int SoldatMouvement = 1;
    #endregion

    #region Mage
    [Space]
    [Header("Creature Mage")]
    public int MageCastCost = 1;
    public int MageCastRange = 1;
    public int MageActivateCost = 1;
    public int MageAttack = 1;
    public int MageAttackRange = 1;
    public int MageHp = 1;
    public int MageMouvement = 1;
    #endregion

    #region Arbaletrier
    [Space]
    [Header("Creature Arbaletrier")]
    public int ArbaletrierCastCost = 1;
    public int ArbaletrierCastRange = 1;
    public int ArbaletrierActivateCost = 1;
    public int ArbaletrierAttack = 1;
    public int ArbaletrierAttackRange = 1;
    public int ArbaletrierHp = 1;
    public int ArbaletrierMouvement = 1;
    #endregion

    #region Sort
    [Space]
    [Header("Sort")]
    public int SortCastCost = 1;
    public int SortCastRange = 1;
    public int SortActivateCost = 1;
    public int SortAttack = 1;
    public int SortAttackRange = 1;
    public int SortHp = 1;
    public int SortMouvement = 1;
    #endregion

    #region Mur
    [Space]
    [Header("Batiment Mur")]
    public int MurCastCost = 1;
    public int MurCastRange = 1;
    public int MurActivateCost = 1;
    public int MurAttack = 1;
    public int MurAttackRange = 1;
    public int MurHp = 1;
    public int MurMouvement = 1;
    #endregion

    #region Tour
    [Space]
    [Header("Batiment Tour")]
    public int TourCastCost = 1;
    public int TourCastRange = 1;
    public int TourActivateCost = 1;
    public int TourAttack = 1;
    public int TourAttackRange = 1;
    public int TourHp = 1;
    public int TourMouvement = 1;
    #endregion

    #region Forge
    [Space]
    [Header("Batiment Forge")]
    public int ForgeCastCost = 1;
    public int ForgeCastRange = 1;
    public int ForgeActivateCost = 1;
    public int ForgeAttack = 1;
    public int ForgeAttackRange = 1;
    public int ForgeHp = 1;
    public int ForgeMouvement = 1;
    #endregion

    #region HP
    [Space]
    [Header("Composante HP")]
    public int HPCastCost = 1;
    public int HPCastRange = 1;
    public int HPActivateCost = 1;
    public int HPAttack = 1;
    public int HPAttackRange = 1;
    public int HPHp = 1;
    public int HPMouvement = 1;
    #endregion

    #region Attaque
    [Space]
    [Header("Composante Attaque")]
    public int AttaqueCastCost = 1;
    public int AttaqueCastRange = 1;
    public int AttaqueActivateCost = 1;
    public int AttaqueAttack = 1;
    public int AttaqueAttackRange = 1;
    public int AttaqueHp = 1;
    public int AttaqueMouvement = 1;
    #endregion

    #region Mouvement
    [Space]
    [Header("Composante Mouvement")]
    public int MouvementCastCost = 1;
    public int MouvementCastRange = 1;
    public int MouvementActivateCost = 1;
    public int MouvementAttack = 1;
    public int MouvementAttackRange = 1;
    public int MouvementHp = 1;
    public int MouvementMouvement = 1;
    #endregion

    #region Armure
    [Space]
    [Header("Composante Armure")]
    public int ArmureCastCost = 1;
    public int ArmureCastRange = 1;
    public int ArmureActivateCost = 1;
    public int ArmureAttack = 1;
    public int ArmureAttackRange = 1;
    public int ArmureHp = 1;
    public int ArmureMouvement = 1;
    #endregion

    #region Ombre
    [Space]
    [Header("Composante Ombre")]
    public int OmbreCastCost = 1;
    public int OmbreCastRange = 1;
    public int OmbreActivateCost = 1;
    public int OmbreAttack = 1;
    public int OmbreAttackRange = 1;
    public int OmbreHp = 1;
    public int OmbreMouvement = 1;
    #endregion

    #region Pousse
    [Space]
    [Header("Composante Pousse")]
    public int PousseCastCost = 1;
    public int PousseCastRange = 1;
    public int PousseActivateCost = 1;
    public int PousseAttack = 1;
    public int PousseAttackRange = 1;
    public int PousseHp = 1;
    public int PousseMouvement = 1;
    #endregion

    #region Echange
    [Space]
    [Header("Composante Echange")]
    public int EchangeCastCost = 1;
    public int EchangeCastRange = 1;
    public int EchangeActivateCost = 1;
    public int EchangeAttack = 1;
    public int EchangeAttackRange = 1;
    public int EchangeHp = 1;
    public int EchangeMouvement = 1;
    #endregion
    */
}
