using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public bool m_IsValid = false;
    public bool m_IsValidTarget = false;
    public Card m_OccupiedBy = null;
    public int PosX;
    public int PosY;
    public GameObject m_AttackRangeVisual;
    public GameObject m_Avatar = null;
    public GameObject m_ShadowEffect;
    public GameObject m_Borders;

    private Renderer m_Renderer;
    private Color m_Color;
    private Color m_AvatarColor;
    private int m_ShadowTime;
    private Card.Players m_Owner;
    private Vector3 m_baseShadowPosition;
    private Vector3 m_currentShadowPosition;


    #region SpellVariables
    private int m_SpellTurnDuration = 0;
    private int m_DamagePerTurn = 0;
    private int m_ArmorPerTurn = 0;
    private int m_ArmorPercing = 0;
    private int m_ShadowPerTurn = 0;
    private TileController m_TradeTarget;
    private TileController m_PushTarget;
    private Card.Players m_SpellOwner;
    private Card m_OccupiedTemp;
    #endregion

    private void Start()
    {
        if (m_ShadowEffect != null)
        {
            m_baseShadowPosition = m_ShadowEffect.transform.position;
            m_currentShadowPosition = m_baseShadowPosition;
        }
        m_Renderer = gameObject.GetComponent<Renderer>();
        m_Color = m_Renderer.material.color;
        if (m_Avatar != null)
        {
            m_AvatarColor = m_Avatar.GetComponent<Renderer>().material.color;
        }

        GameController.ChangeTurn += TurnTick;
    }

    private void OnDestroy()
    {
        GameController.ChangeTurn -= TurnTick;
    }

    public void Illuminate()
    {
        m_Borders.SetActive(true);
        m_IsValid = true;
    }

    public void EnemyIlluminate()
    {
        m_Renderer.material.SetColor("_Color", Color.red);
        m_IsValid = true;
    }

    public void UnIlluminate()
    {
        m_Borders.SetActive(false);
        m_IsValid = false;
        m_IsValidTarget = false;
        m_AttackRangeVisual.SetActive(false);
        if (m_OccupiedBy)
        {
            m_OccupiedBy.UnIlluminate();
        }
        if (m_Avatar != null)
        {
            m_Avatar.GetComponent<Renderer>().material.SetColor("_Color", m_AvatarColor);
        }
    }

    public void InAttackRange()
    {
        m_AttackRangeVisual.SetActive(true);
        m_IsValidTarget = true;
        if (m_OccupiedBy)
        {
            m_OccupiedBy.ValidTarget();
        }
        if (m_Avatar != null)
        {
            m_Avatar.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    public void ClearTile()
    {
        m_OccupiedBy = null;
        Debug.Log("Clear tile");
    }

    public void SetShadow(int i_Time, Card.Players i_Owner)
    {
        m_Owner = i_Owner;
        if (m_ShadowEffect != null)
        {
            m_ShadowEffect.SetActive(true);
            if (m_Owner == Card.Players.Player)
            {
                Vector3 temp = m_baseShadowPosition;
                temp.y = m_baseShadowPosition.y - 2.75f;
                temp.x = m_baseShadowPosition.x - 0.5f;
                m_currentShadowPosition = temp;
            }
            else
            {
                m_currentShadowPosition = m_baseShadowPosition;
            }

            m_ShadowEffect.transform.position = m_currentShadowPosition;
        }
        if (i_Time > m_ShadowTime)
        {
            m_ShadowTime = i_Time;
        }

        if (m_OccupiedBy != null && m_OccupiedBy.m_Owner != m_Owner)
        {
            ClearShadow();
        }
    }

    public void ClearShadow()
    {
        if (m_ShadowEffect != null)
        {
            m_ShadowEffect.SetActive(false);
        }
        m_ShadowTime = 0;
    }

    private void TurnTick()
    {
        if (--m_SpellTurnDuration <= 0)
        {
            m_TradeTarget = null;
            m_PushTarget = null;
            m_ShadowPerTurn = 0;
            m_DamagePerTurn = 0;
            m_ArmorPercing = 0;
            m_ArmorPerTurn = 0;
            m_SpellOwner = Card.Players.None;
            m_Renderer.material.SetColor("_Color", m_Color);
        }
        else
        {
            SpellEffects();
            m_Renderer.material.SetColor("_Color", Color.blue);
        } 

        if (m_ShadowTime > 0)
        {
            m_ShadowTime--;
        }

        if (m_ShadowEffect != null && m_ShadowEffect.activeSelf && m_ShadowTime <= 0)
        {
            m_ShadowEffect.SetActive(false);
        }

        
    }

    public void SpellEffects()
    {
        SetShadow(m_ShadowPerTurn, m_SpellOwner);

        SpellDamage();
        SpellTrade();
        SpellPush();
    }

    public void AddEffectToCard()
    {
        if (m_OccupiedBy && m_ArmorPerTurn > 0)
        {
            m_OccupiedBy.AddTemporaryArmor(m_ArmorPerTurn);
        }
    }

    private void SpellDamage()
    {
        if (m_OccupiedBy != null)
        {
            m_OccupiedBy.LoseHp(m_DamagePerTurn, m_ArmorPercing);
        }
    }

    private void SpellTrade()
    {
        if (m_TradeTarget != null && m_OccupiedBy != null)
        {
            m_OccupiedTemp = m_TradeTarget.m_OccupiedBy;
            m_TradeTarget.m_OccupiedBy = m_OccupiedBy;
            m_OccupiedBy = m_OccupiedTemp;
            m_OccupiedTemp = null;

            SpellDamage();
        }
    }

    private void SpellPush()
    {
        if (m_PushTarget != null && m_OccupiedBy != null)
        {
            if (m_PushTarget.m_OccupiedBy != null)
            {
                m_OccupiedBy.LoseHp(1);
                m_PushTarget.m_OccupiedBy.LoseHp(1);
            }
        }
    }

    public void AddSpellEffects(Card i_Spell)
    {
        if (m_SpellOwner == Card.Players.None || m_SpellOwner == i_Spell.m_Owner)
        {
            m_SpellOwner = i_Spell.m_Owner;
        }
        else
        {
            m_SpellOwner = Card.Players.Wild;
        }
        
        m_SpellTurnDuration = i_Spell.HP;
        m_DamagePerTurn = i_Spell.Attack;
        if (m_DamagePerTurn > 0)
        {
            m_ArmorPercing = i_Spell.Armor;
        }
        else
        {
            m_ArmorPerTurn = i_Spell.Armor;
        }
        m_ShadowPerTurn = i_Spell.ShadowTime;
    }

    public void AddTargetTradeTile(TileController i_Tile)
    {
        m_TradeTarget = i_Tile;
    }

    public void AddTargetPushTile(TileController i_Tile)
    {
        m_PushTarget = i_Tile;
    }
}
