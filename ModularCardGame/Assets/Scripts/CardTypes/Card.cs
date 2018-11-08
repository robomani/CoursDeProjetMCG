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
        InVoid,
    }

    public enum Players
    {
        Player,
        AI,
        None,
        Wild,
    }



    public int m_Position;

    public TileController m_TileOccupied;
    public GameController m_Game;

    #region Card Stats
    protected int m_CastCost = 1;
    protected int m_CastRange = 1;
    protected int m_ActivateCost = 1;
    protected int m_Attack = 1;
    protected int m_AttackRange = 1;
    protected int m_ArmorPercing;
    protected int m_Hp = 1;
    protected int m_ShadowOnLifeLost;
    protected int m_Mouvement = 0;
    protected int m_ArmorAlfterMove;
    protected int m_Armor;
    protected int m_NumberOfSpellToIgnore;
    protected int m_TradeRange;
    protected int m_TradeResist;
    protected int m_TradeCostReduction;
    protected int m_MoveAlfterTrade;
    protected int m_TradeAdjacent;
    protected int m_ArmorAlfterTrade;
    protected int m_ShadowAlfterTrade;
    protected int m_PushRange;
    protected int m_PushResist;
    protected int m_MoveAlfterPush;
    protected int m_LateralPush;
    protected int m_ArmorAlfterGettingPushed;
    protected int m_ShadowAlfterPush;
    protected int m_ShadowTime;
    protected int m_ShadowTeleportRange;
    protected int m_ShadowSightRange;
    protected bool m_IndirectAttack;
    protected CardType m_CardType;
    #endregion

    public string m_CardName;
    public bool m_Playable = true;
    public bool m_ValidTarget = false;
    public float m_MoveTime = 1.5f;


    public Players m_Owner = Players.AI ;
    public TextMeshPro m_CostText;
    public TextMeshPro m_AttackText;
    public TextMeshPro m_HpText;
    public TextMeshPro m_MoveText;
    public GameObject m_CharacterPrefab;
    public SpriteRenderer m_Star;
    public TextMeshPro m_HabilityList;

    protected States m_State = States.InDeck;
    protected Transform m_SpawnPoint;
    protected Transform m_GravePosition;
    protected GameObject m_Character;
    protected Renderer m_Renderer;
    #region Getters
    public Transform SpawnPoint
    {
        get { return m_SpawnPoint; }
    }

    public States State
    {
        get { return m_State; }
    }

    public int CastingCost
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
        get {
            AnimAttack();
            return m_Attack; }
    }

    public int AttackRange
    {
        get { return m_AttackRange; }
    }

    public int ArmorPercing
    {
        get { return m_ArmorPercing; }
    }

    public int HP
    {
        get { return m_Hp; }
    }

    public int ShadowOnLifeLost
    {
        get { return m_ShadowOnLifeLost; }
    }

    public int Movement
    {
        get { return m_Mouvement; }
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

    public int TradeAdjecent
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

    public bool IndirectAttack
    {
        get { return m_IndirectAttack; }
    }

    public CardType CardType
    {
        get { return m_CardType; }
    }
    #endregion

    private int m_TemporaryArmor = 0;

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

        Transform[] tempTransform = GetComponentsInChildren<Transform>();
        foreach (Transform TR in tempTransform)
        {
            if (TR.name == "SpawnPoint")
            {
                m_SpawnPoint = TR;
            }    
        }

        SpriteRenderer[] tempSprite = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer SR in tempSprite)
        {
            if (SR.name == "Star")
            {
                m_Star = SR;
            }
        }

        TextMeshPro[] tempTMP = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro TMP in tempTMP)
        {
            if (TMP.name == "HabilityList")
            {
                m_HabilityList = TMP;
            }
        }
    }

    protected void Start()
    {
        UpdateStatsOnCard();
        GameController.ChangeTurn += TurnTick;
    }

    private void OnDestroy()
    {
        GameController.ChangeTurn -= TurnTick;
    }

    public void InitCard(CardsData i_CardData , Transform i_Grave = null)
    {
        if (i_Grave)
        {
            m_GravePosition = i_Grave;
        }
        
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

    protected void UpdateStatsOnCard()
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
        if (m_ArmorPercing > 0 || m_ShadowOnLifeLost > 0 || m_ArmorAlfterMove > 0 || m_Armor > 0 || m_NumberOfSpellToIgnore > 0 || m_TradeRange > 0 
            || m_TradeResist > 0 || m_TradeCostReduction > 0 || m_MoveAlfterTrade > 0 || m_TradeAdjacent > 0 || m_ArmorAlfterTrade > 0 || m_ShadowAlfterTrade > 0
            || m_PushRange > 0 || m_PushResist > 0 || m_MoveAlfterPush > 0 || m_LateralPush > 0 || m_ArmorAlfterGettingPushed > 0 || m_ShadowAlfterPush > 0
            || m_ShadowTime > 0 || m_ShadowTeleportRange > 0 || m_ShadowSightRange > 0 || m_IndirectAttack == true)
        {
            if (m_Star != null)
            {
                m_Star.enabled = true;
            }
        }
        else
        {
            if (m_Star != null)
            {
                m_Star.enabled = false;
            } 
        }
    }

    public virtual void CopyCard(Card i_Card)
    {
        string HabilityListText = "";

        m_CastCost = i_Card.m_CastCost;
        m_CastRange = i_Card.m_CastRange;

        m_ActivateCost = i_Card.m_ActivateCost;

        m_Attack = i_Card.m_Attack;
        m_AttackRange = i_Card.m_AttackRange;
        m_ArmorPercing = i_Card.m_ArmorPercing;
        if (m_ArmorPercing > 0)
        {
            HabilityListText += "Armor Percing : " + m_ArmorPercing + "  \n";
        }

        m_Hp = i_Card.m_Hp;
        m_ShadowOnLifeLost = i_Card.m_ShadowOnLifeLost;
        if (m_ShadowOnLifeLost > 0)
        {
            HabilityListText += "Shadow On life lost : " + m_ShadowOnLifeLost + "  \n";
        }

        m_Mouvement = i_Card.m_Mouvement;
        m_ArmorAlfterMove = i_Card.m_ArmorAlfterMove;
        if (m_ArmorAlfterMove > 0)
        {
            HabilityListText += "Armor Alfter Move : " + m_ArmorAlfterMove + "  \n";
        }

        m_Armor = i_Card.m_Armor;
        if (m_Armor > 0)
        {
            HabilityListText += "Armor : " + m_Armor + "  \n";
        }

        m_NumberOfSpellToIgnore = i_Card.m_NumberOfSpellToIgnore;
        if (m_NumberOfSpellToIgnore > 0)
        {
            HabilityListText += "Number Of Spell To Ignore : " + m_NumberOfSpellToIgnore + "  \n";
        }

        m_TradeRange = i_Card.m_TradeRange;
        if (m_TradeRange > 0)
        {
            HabilityListText += "Trade Range : " + m_TradeRange + "  \n";
        }

        m_TradeResist = i_Card.m_TradeResist;
        if (m_TradeResist > 0)
        {
            HabilityListText += "Trade Resist : " + m_TradeResist + "  \n";
        }

        m_TradeCostReduction = i_Card.m_TradeCostReduction;
        if (m_TradeCostReduction > 0)
        {
            HabilityListText += "Trade Cost Reduction : " + m_TradeCostReduction + "  \n";
        }

        m_MoveAlfterTrade = i_Card.m_MoveAlfterTrade;
        if (m_MoveAlfterTrade > 0)
        {
            HabilityListText += "Move Alfter Trade : " + m_MoveAlfterTrade + "  \n";
        }

        m_TradeAdjacent = i_Card.m_TradeAdjacent;
        if (m_TradeAdjacent > 0)
        {
            HabilityListText += "Trade Adjacent : " + m_TradeAdjacent + "  \n";
        }

        m_ArmorAlfterTrade = i_Card.m_ArmorAlfterTrade;
        if (m_ArmorAlfterTrade > 0)
        {
            HabilityListText += "Armor Alfter Trade : " + m_ArmorAlfterTrade + "  \n";
        }

        m_ShadowAlfterTrade = i_Card.m_ShadowAlfterTrade;
        if (m_ShadowAlfterTrade > 0)
        {
            HabilityListText += "Shadow Alfter Trade : " + m_ShadowAlfterTrade + "  \n";
        }

        m_PushRange = i_Card.m_PushRange;
        if (m_PushRange > 0)
        {
            HabilityListText += "Push Range : " + m_PushRange + "  \n";
        }

        m_PushResist = i_Card.m_PushResist;
        if (m_PushResist > 0)
        {
            HabilityListText += "Push Resist : " + m_PushResist + "  \n";
        }

        m_MoveAlfterPush = i_Card.m_MoveAlfterPush;
        if (m_MoveAlfterPush > 0)
        {
            HabilityListText += "Move Alfter Push : " + m_MoveAlfterPush + "  \n";
        }

        m_LateralPush = i_Card.m_LateralPush;
        if (m_LateralPush > 0)
        {
            HabilityListText += "Lateral Push : " + m_LateralPush + "  \n";
        }

        m_ArmorAlfterGettingPushed = i_Card.m_ArmorAlfterGettingPushed;
        if (m_ArmorAlfterGettingPushed > 0)
        {
            HabilityListText += "Armor Alfter Getting Pushed : " + m_ArmorAlfterGettingPushed + "  \n";
        }

        m_ShadowAlfterPush = i_Card.m_ShadowAlfterPush;
        if (m_ShadowAlfterPush > 0)
        {
            HabilityListText += "Shadow Alfter Push : " + m_ShadowAlfterPush + "  \n";
        }

        m_ShadowTime = i_Card.m_ShadowTime;
        if (m_ShadowTime > 0)
        {
            HabilityListText += "Shadow Time : " + m_ShadowTime + " \n";
        }

        m_ShadowTeleportRange = i_Card.m_ShadowTeleportRange;
        if (m_ShadowTeleportRange > 0)
        {
            HabilityListText += "Shadow Teleport Range : " + m_ShadowTeleportRange + "  \n";
        }

        m_ShadowSightRange = i_Card.m_ShadowSightRange;
        if (m_ShadowSightRange > 0)
        {
            HabilityListText += "Shadow Sight Range : " + m_ShadowSightRange + "  \n";
        }

        m_IndirectAttack = i_Card.m_IndirectAttack;
        if (m_IndirectAttack == true)
        {
            HabilityListText += "Indirect Attack  \n";
        }

        m_CardType = i_Card.m_CardType;

        if (m_HabilityList != null)
        {
            m_HabilityList.text = HabilityListText;
        }

        UpdateStatsOnCard();
    }

    public bool LoseHp(int i_Damage, int i_ArmorPercing = 0)
    {
        m_Hp -= (i_Damage - Mathf.Max((m_Armor - i_ArmorPercing),0));
        if (m_TemporaryArmor > 0)
        {
            m_Armor -= m_TemporaryArmor;
        }
        if (m_Hp <= 0)
        {
            AnimHurt(true);
            return true;
        }
        if ((i_Damage - Mathf.Max((m_Armor - i_ArmorPercing), 0)) > 0)
        {
            AnimHurt(false);
        }
        AudioManager.Instance.PlayPingSound();
        UpdateStatsOnCard();
        return false;
    }

    public virtual void AddComponentCard(Card i_Card)
    {
        if (m_State == States.InHand)
        {
            m_CastCost += i_Card.m_CastCost;
            m_ActivateCost += i_Card.m_ActivateCost;
        }
        else
        {
            m_ActivateCost += Mathf.Max(Mathf.CeilToInt((i_Card.m_ActivateCost + i_Card.m_CastCost) / 2), 1);
        }

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

        if (m_ShadowTime > 0 && m_TileOccupied)
        {
            m_TileOccupied.SetShadow(m_ShadowTime, m_Owner);
        }
        UpdateStatsOnCard();
    }

    public virtual void ZeroCard()
    {

        m_CastCost = 0;
        m_ActivateCost = 0;

        m_Attack = 0;
        m_AttackRange = 0;
        m_ArmorPercing = 0;

        m_Hp = 0;
        m_ShadowOnLifeLost = 0;

        m_Mouvement = 0;
        m_ArmorAlfterMove = 0;

        m_Armor = 0;
        m_NumberOfSpellToIgnore = 0;

        m_TradeRange = 0;
        m_TradeResist = 0;
        m_TradeCostReduction = 0;
        m_MoveAlfterTrade = 0;
        m_TradeAdjacent = 0;
        m_ArmorAlfterTrade = 0;
        m_ShadowAlfterTrade = 0;
        m_PushRange = 0;
        m_PushResist = 0;
        m_MoveAlfterPush = 0;
        m_LateralPush = 0;
        m_ArmorAlfterGettingPushed = 0;
        m_ShadowAlfterPush = 0;
        m_ShadowTime = 0;
        m_ShadowTeleportRange = 0;
        m_ShadowSightRange = 0;

        m_Renderer.GetComponent<Material>().color = Color.black;

        UpdateStatsOnCard();
    }

    public virtual void SubstractComponentCard(Card i_Card)
    {
        if (m_State == States.InHand)
        {
            m_CastCost -= i_Card.m_CastCost;
            if (m_CastCost < 0)
            {
                m_CastCost = 0;
            }
            m_ActivateCost -= i_Card.m_ActivateCost;
            if (m_ActivateCost < 1)
            {
                m_ActivateCost = 1;
            }
        }
        else
        {
            m_ActivateCost -= Mathf.Max(Mathf.CeilToInt((i_Card.m_ActivateCost + i_Card.m_CastCost) / 2), 1);
            if (m_ActivateCost < 1)
            {
                m_ActivateCost = 1;
            }
        }

        m_Attack -= i_Card.m_Attack;
        if (m_Attack < 0)
        {
            m_Attack = 0;
        }
        m_AttackRange -= i_Card.m_AttackRange;
        if (m_AttackRange < 1)
        {
            m_AttackRange = 1;
        }
        m_ArmorPercing -= i_Card.m_ArmorPercing;
        if (m_ArmorPercing < 1)
        {
            m_ArmorPercing = 1;
        }

        m_Hp -= i_Card.m_Hp;
        if (m_Hp < 0)
        {
            m_Hp = 0;
        }
        m_ShadowOnLifeLost -= i_Card.m_ShadowOnLifeLost;
        if (m_ShadowOnLifeLost < 0)
        {
            m_ShadowOnLifeLost = 0;
        }

        m_Mouvement -= i_Card.m_Mouvement;
        if (m_Mouvement < 0)
        {
            m_Mouvement = 0;
        }
        m_ArmorAlfterMove -= i_Card.m_ArmorAlfterMove;
        if (m_ArmorAlfterMove < 0)
        {
            m_ArmorAlfterMove = 0;
        }

        m_Armor -= i_Card.m_Armor;
        if (m_Armor < 0)
        {
            m_Armor = 0;
        }
        m_NumberOfSpellToIgnore -= i_Card.m_NumberOfSpellToIgnore;
        if (m_NumberOfSpellToIgnore < 0)
        {
            m_NumberOfSpellToIgnore = 0;
        }

        m_TradeRange -= i_Card.m_TradeRange;
        if (m_TradeRange < 0)
        {
            m_TradeRange = 0;
        }
        m_TradeResist -= i_Card.m_TradeResist;
        if (m_TradeResist < 0)
        {
            m_TradeResist = 0;
        }
        m_TradeCostReduction -= i_Card.m_TradeCostReduction;
        if (m_TradeCostReduction < 0)
        {
            m_TradeCostReduction = 0;
        }
        m_MoveAlfterTrade -= i_Card.m_MoveAlfterTrade;
        if (m_MoveAlfterTrade < 0)
        {
            m_MoveAlfterTrade = 0;
        }
        m_TradeAdjacent -= i_Card.m_TradeAdjacent;
        if (m_TradeAdjacent < 0)
        {
            m_TradeAdjacent = 0;
        }
        m_ArmorAlfterTrade -= i_Card.m_ArmorAlfterTrade;
        if (m_ArmorAlfterTrade < 0)
        {
            m_ArmorAlfterTrade = 0;
        }
        m_ShadowAlfterTrade -= i_Card.m_ShadowAlfterTrade;
        if (m_ShadowAlfterTrade < 0)
        {
            m_ShadowAlfterTrade = 0;
        }
        m_PushRange -= i_Card.m_PushRange;
        if (m_PushRange < 0)
        {
            m_PushRange = 0;
        }
        m_PushResist -= i_Card.m_PushResist;
        if (m_PushResist < 0)
        {
            m_PushResist = 0;
        }
        m_MoveAlfterPush -= i_Card.m_MoveAlfterPush;
        if (m_MoveAlfterPush < 0)
        {
            m_MoveAlfterPush = 0;
        }
        m_LateralPush -= i_Card.m_LateralPush;
        if (m_LateralPush < 0)
        {
            m_LateralPush = 0;
        }
        m_ArmorAlfterGettingPushed -= i_Card.m_ArmorAlfterGettingPushed;
        if (m_ArmorAlfterGettingPushed < 0)
        {
            m_ArmorAlfterGettingPushed = 0;
        }
        m_ShadowAlfterPush -= i_Card.m_ShadowAlfterPush;
        if (m_ShadowAlfterPush < 0)
        {
            m_ShadowAlfterPush = 0;
        }
        m_ShadowTime -= i_Card.m_ShadowTime;
        if (m_ShadowTime < 0)
        {
            m_ShadowTime = 0;
        }
        m_ShadowTeleportRange -= i_Card.m_ShadowTeleportRange;
        if (m_ShadowTeleportRange < 0)
        {
            m_ShadowTeleportRange = 0;
        }
        m_ShadowSightRange -= i_Card.m_ShadowSightRange;
        if (m_ShadowSightRange < 0)
        {
            m_ShadowSightRange = 0;
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

    public void ChangeState(States i_State)
    {     
        if (i_State != m_State)
        {
            m_State = i_State;
            if (m_State == States.InPlay)
            {
                SpawnCharacter();
            }
            else
            {
                DestroyCharacter();
            }
            if (m_State == States.InGrave)
            {
                CardMove(m_GravePosition.position, m_GravePosition.rotation);
            }
        }
    }

    public void AddTemporaryArmor(int i_ArmorAmount)
    {
        m_Armor += i_ArmorAmount;
        m_TemporaryArmor += i_ArmorAmount;
    }

    public void Flash(Color i_Color, float i_Time)
    {
        StartCoroutine(FLashCoroutine(i_Color, i_Time));    
    }

    private IEnumerator FLashCoroutine(Color i_Color, float i_Time)
    {
        Debug.Log("Flash Coroutine start");
        float time = 0;
        while (time <= i_Time)
        {

            m_Renderer.material.SetColor("_Color", Color.Lerp(Color.white, i_Color, Mathf.PingPong(Time.time * i_Time, i_Time/1.5f)));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    protected void SpawnCharacter()
    {
        if (m_Character == null && m_CharacterPrefab != null)
        {
            m_Character = Instantiate(m_CharacterPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation, transform);
        }
        else if(m_Character != null)
        {
            Debug.Log("This card already has a character instance");
        }
        else
        {
            Debug.Log("This card already no character prefab to Instantiate");
        }
    }

    protected void DestroyCharacter()
    {
        if (m_Character != null)
        {
            Destroy(m_Character);
            m_Character = null;
        }
    }

    protected void AnimAttack()
    {
        if (m_Character != null && m_Character.GetComponent<Animator>())
        {
            m_Character.GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    protected void AnimHurt(bool i_Die)
    {
        if (m_Character != null && m_Character.GetComponent<Animator>())
        {
            m_Character.GetComponent<Animator>().SetTrigger("Hurt");
            if (i_Die)
            {
                AnimDie();
            }
            else
            {
                AnimIdle();
            }
        }
    }

    public IEnumerator CardMove(Vector3 i_EndPos, Quaternion i_EndRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float currentTime = 0f;
        AnimWalk();     

        yield return new WaitForSeconds(0.2f);
        float value = 0.0f;
        m_MoveTime = 1.5f;
        LookFoward CharPos = null;

        if (transform.GetComponentInChildren<LookFoward>())
        {
            CharPos = transform.GetComponentInChildren<LookFoward>();
        }
        while (currentTime != m_MoveTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > m_MoveTime)
            {
                currentTime = m_MoveTime;
            }

            value = currentTime / m_MoveTime;
            transform.position = Vector3.Lerp(startPos, i_EndPos, value);
            transform.rotation = Quaternion.Slerp(startRot, i_EndRot, value);
            if (CharPos != null)
            {
                CharPos.UpdatePos(transform.GetComponent<Card>().SpawnPoint.position);
            }
            yield return new WaitForEndOfFrame();
        }

        if (CharPos != null)
        {
            CharPos.UpdatePos(transform.GetComponent<Card>().SpawnPoint.position);
        }
        currentTime = 0f;

        if (transform != null && transform.GetComponent<Card>())
        {
            transform.GetComponent<Card>().AnimIdle();
        }
    }

    public void AttackCard(Card i_Target)
    {
        if (i_Target.LoseHp(m_Attack))
        {
            if (m_Game.TurnAI)
            {
                m_Game.AIMana -= m_ActivateCost;
            }
            else
            {
                m_Game.ChangePlayerMana(-m_ActivateCost);
            }

            if (i_Target.m_Owner == Card.Players.Player)
            {
                m_Game.PlayerGrave[System.Array.IndexOf(m_Game.PlayerGrave, null)] = i_Target.gameObject;
            }
            else
            {
                m_Game.AIGrave[System.Array.IndexOf(m_Game.AIGrave, null)] = i_Target.gameObject;
                m_Game.ShowNormalTiles(this);
            }

            i_Target.m_TileOccupied.ClearTile();
            i_Target.m_TileOccupied = null;
            i_Target.ChangeState(Card.States.InGrave);
        }
        else
        {
            if (m_Game.TurnAI)
            {
                m_Game.AIMana -= m_ActivateCost;
            }
            else
            {
                m_Game.ChangePlayerMana(-m_ActivateCost);
            }
        }
    }

    protected void AnimDie()
    {
        if (m_Owner == Players.AI)
        {
            if (m_CardType == CardType.Creature)
            {
                GameManager.Instance.AddOrRemoveCreatureToAI(false);
            }
            else if (m_CardType == CardType.Building)
            {
                GameManager.Instance.AddOrRemoveBuildingToAI(false);
            }
        }

        if (m_Character != null && m_Character.GetComponent<Animator>())
        {
            m_Character.GetComponent<Animator>().SetTrigger("Die");
        }  
    }

    public void AnimWalk(Transform i_Target = null)
    {
        if (m_Character != null && m_Character.GetComponent<Animator>())
        {
            if (i_Target != null && gameObject.GetComponent<LookFoward>())
            {
                gameObject.GetComponent<LookFoward>().NewTarget(i_Target);
            }
            m_Character.GetComponent<Animator>().SetTrigger("Walk");
        }
    }

    public void AnimIdle()
    {
        if (m_Character != null && m_Character.GetComponent<Animator>())
        {
            m_Character.GetComponent<Animator>().SetTrigger("Idle");
        }
    }

    private void TurnTick()
    {
        if (m_ShadowTime > 0 && m_State == States.InPlay)
        {
            m_TileOccupied.SetShadow(m_ShadowTime, m_Owner);
        }
    }
}
