using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;

[System.Serializable]
public struct CardClass
{
    public string m_CardTypeName;
    public int m_NbrInDeck;
    public Material m_CardTypeSkin;
    public string m_ScriptToAttach;
}

public class GameController : MonoBehaviour
{
    #region Variables

    #region Player Base Stats
    [Tooltip("Nombre de vie de base du joueur")]
    [SerializeField]
    private int m_PlayerHp = 20;
    public int PlayerHp
    {
        get { return m_PlayerHp; }
        set { m_PlayerHp = value; }
    }

    [Tooltip("Le texte qui affiche le nombre de vie du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_PlayerHpText;

    [Tooltip("Nombre de Mana Maximum du joueur")]
    [SerializeField]
    private int m_PlayerMaxMana = 1;
    public int PlayerMaxMana
    {
        get { return m_PlayerMaxMana; }
    }

    [Tooltip("Le texte qui affiche le nombre de mana du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_PlayerManaText;
    #endregion

    #region AI Base Stats
    [Tooltip("Nombre de vie de base de l'IA")]
    [SerializeField]
    private int m_AIHp = 20;
    public int HpAI
    {
        get { return m_AIHp; }
    }

    [Tooltip("Le texte qui affiche le nombre de vie de l'IA")]
    [SerializeField]
    private TextMeshProUGUI m_HpAIText;  

    [Tooltip("Nombre de Mana Maximum de l'IA")]
    [SerializeField]
    private int m_AIMaxMana = 1;
    public int AIMaxMana
    {
        get { return m_AIMaxMana; }
        set { m_AIMaxMana = value; }
    }

    [Tooltip("Le texte qui affiche le nombre de mana de l'IA")]
    [SerializeField]
    private TextMeshProUGUI m_ManaAIText;

    //---------------------------------------------------------------------------------------------------------
    [Tooltip("Détermine si l'IA utilise le mode CHEAT!")]
    [SerializeField]
    private bool m_AICheatMode;
    #endregion

    #region Card Move Speed
    [Tooltip("Vitesse de mouvement des cartes des deck aux mains")]
    [SerializeField]
    private float m_DrawTime = 1.5f;
    public float DrawTime
    {
        get { return m_DrawTime; }
    }

    [Tooltip("Vitesse de mouvement des cartes des mains aux cimetières")]
    [SerializeField]
    private float m_DiscardTime = 1.5f;
    public float DiscardTime
    {
        get { return m_DiscardTime; }
    }

    [Tooltip("Vitesse de mouvement des cartes des cimetières aux decks")]
    [SerializeField]
    private float m_ShuffleTime = 1.5f;
    public float ShuffleTime
    {
        get { return m_ShuffleTime; }
    }
    #endregion

    [Tooltip("Le préfab des cartes à instancier")]
    [SerializeField]
    private GameObject m_CardPrefab;

    #region Destinations des cartes
    [Tooltip("Le transform de la position du deck du joueur")]
    [SerializeField]
    private Transform m_PlayerDeckPosition;

    [Tooltip("Le transform de la position de la main du joueur")]
    [SerializeField]
    private Transform m_PlayerHandPosition;

    [Tooltip("Le transform de la position du cimetière du joueur")]
    [SerializeField]
    private Transform m_PlayerGravePosition;
    public Transform PlayerGravePosition
    {
        get { return m_PlayerGravePosition; }
    }

    [Tooltip("Le transform de la rotation des cartes du joueur sur une case")]
    [SerializeField]
    private Transform m_PlayerTileRotation;
    public Transform PlayerTileRotation
    {
        get { return m_PlayerTileRotation; }
    }

    [Tooltip("Le transform de la position du deck de l'IA")]
    [SerializeField]
    private Transform m_AIDeckPosition;

    [Tooltip("Le transform de la position de la main de l'IA")]
    [SerializeField]
    private Transform m_AIHandPosition;

    [Tooltip("Le transform de la position du cimetière de l'IA")]
    [SerializeField]
    private Transform m_AIGravePosition;
    public Transform AIGravePosition
    {
        get { return m_AIGravePosition; }
    }

    [Tooltip("Le transform de la rotation des cartes de l'IA sur une case")]
    [SerializeField]
    private Transform m_AITileRotation;
    public Transform AITileRotation
    {
        get { return m_AITileRotation; }
    }
    #endregion

    [Tooltip("La carte qui prend l'apparence de la carte sur laquelle le joueur zoom")]
    [SerializeField]
    private GameObject m_ZoomCard;
    public GameObject ZoomCard
    {
        get { return m_ZoomCard; }
    }

    #region Reference des boutons
    [Tooltip("Le bouton qui permet de discarter la carte selectionnée")]
    [SerializeField]
    private GameObject m_BtnDiscard;
    public GameObject BtnDiscard
    {
        get { return m_BtnDiscard; }
    }

    [Tooltip("Le bouton qui augmente la mana maximum du joueur")]
    [SerializeField]
    private GameObject m_BtnGainMaxMana;

    [Tooltip("Le bouton qui augmente la mana maximum du joueur")]
    [SerializeField]
    private GameObject m_BtnPowerDrawCard;

    [Tooltip("Le bouton qui fini le tour du joueur")]
    [SerializeField]
    private GameObject m_BtnEndTurn;
    #endregion

    [Tooltip("Liste des types de cartes à placer dans les decks")]
    [SerializeField]
    private List<CardClass> m_CardTypes = new List<CardClass>();

    #region Deck Aléatoire
    [Tooltip("Nombre de carte dans le deck si le deck est crée aléatoirement")]
    [SerializeField]
    private int m_RandomDeckSize = 65;

    [Tooltip("Si actif crée le deck avec une composition aléatoire parmit toutes les cartes possibles et avec un nombre de carte choisie")]
    [SerializeField]
    private bool m_RandomDeck = false;
    #endregion

    [Tooltip("Nombre de carte simultané maximum dans la mains d'un joueurs")]
    [SerializeField]
    private int m_MaxHandSize = 7;

    [Tooltip("Lien avec le script du Gameboard")]
    [SerializeField]
    private BoardGenerator m_Board;
    public BoardGenerator Board { get { return m_Board; } }

    [SerializeField]
    private TextMeshPro m_ResultText;

    [SerializeField]
    private CardsData m_CardData;
    [SerializeField]
    private TileController m_AltarAI;
    public TileController AltarAI
    {
        get { return m_AltarAI; }
    }
    [SerializeField]
    private TileController m_AltarPlayer;
    public TileController AltarPlayer
    {
        get { return m_AltarPlayer; }
    }

    private GameObject[] m_PlayerHand;
    private GameObject[] m_PlayerDeck;
    private GameObject[] m_PlayerGrave;
    public GameObject[] PlayerGrave
    {
        get { return m_PlayerGrave; }
        set { m_PlayerGrave = value;}
    }

    private GameObject[] m_AIHand;
    public GameObject[] AIHand
    {
        get { return m_AIHand; }
    }
    private GameObject[] m_AIDeck;
    private List<GameObject> m_AIInPlay = null;
    private GameObject[] m_AIGrave;
    public GameObject[] AIGrave
    {
        get { return m_AIGrave; }
        set { m_AIGrave = value;}
    }

    private bool m_PlayerAvatarActive = false;

    private Card.Players m_TurnOwner = Card.Players.Player;
    public Card.Players TurnOwner
    {
        get { return m_TurnOwner; }
    }

    private int m_PlayerMana = 1;
    public int PlayerMana
    {
        get { return m_PlayerMana; }
        set
        {
            m_PlayerMana = value;
            if (m_PlayerMana < 0)
            {
                m_PlayerMana = 0;
            }
            UpdateTexts();
        }
    }

    private int m_AIMana = 1;
    public int AIMana
    {
        get { return m_AIMana; }
        set
        {
            m_AIMana = value;
            if (m_AIMana < 0)
            {
                m_AIMana = 0;
            }
            UpdateTexts();
        }
    }

    private bool m_TurnAI = false;
    public bool TurnAI
    {
        get { return m_TurnAI; }
    }

    public bool m_IsCheating;

    private int[] m_CardNumberByType;
    private int[] m_AICardNumberByType;

    public bool GameEnded;

    public Camera m_MainCamera;
    public Camera m_ZoomCamera;

    [SerializeField]
    private AIController m_AIController;
    #endregion

    public static Action ChangeTurn;

    private void Start()
    {
        AudioManager.Instance.GameStart();
        int nbrCardToAdd = Initialise();

        m_RandomDeck = GameManager.Instance.GetIfRandomDeck();
        m_RandomDeckSize = GameManager.Instance.RandomDeckSize();

        m_BtnGainMaxMana.SetActive(true);
        m_BtnPowerDrawCard.SetActive(true);

        if (m_RandomDeck)
        {
            m_PlayerDeck = GenerateDeck(m_RandomDeckSize);
            m_AIDeck = GenerateAIDeck(m_RandomDeckSize);
        }
        else
        {
            m_PlayerDeck = GenerateDeck(nbrCardToAdd);
            m_AIDeck = GenerateAIDeck(nbrCardToAdd);
        }
        

        for (int i = 0; i < 7; i++)
        {
            DrawCard();
            AIDrawCard();
        }

        CheckPlayableCard();
    }

    //Initialise l'array du nombre de chaque type de carte à ajouter dans le deck.
    //Retourne le nombre total de carte à ajouter dans le deck si l'on respecte le nombre de carte de chaque type
    private int Initialise()
    {
        int countCardToAdd = 0;
        int[] temp = new int[m_CardTypes.Count];
        int[] tempAI = new int[m_CardTypes.Count];
        for (int i = 0; i < m_CardTypes.Count; i++)
        {
            countCardToAdd += m_CardTypes[i].m_NbrInDeck;
            temp[i] = m_CardTypes[i].m_NbrInDeck;
            tempAI[i] = m_CardTypes[i].m_NbrInDeck;
        }
        m_CardNumberByType = temp;
        m_AICardNumberByType = tempAI;
        UpdateTexts();

        return countCardToAdd;
    }

    public void HurtPlayer(int i_Damage)
    {
#if UNITY_CHEAT
        if (!m_IsCheating)
        {
#endif
            m_PlayerHp -= i_Damage;
#if UNITY_CHEAT
        }
#endif
        AnimationManager.Instance.PlayerHurt();
    }

    public void HurtEnemy(int i_Damage)
    {
        m_AIHp -= i_Damage;
        AnimationManager.Instance.EnemyHurt();
    }

    public void EndTurn()
    {
        if (m_TurnAI)
        {
            m_TurnAI = false;
        }

        if (m_TurnOwner == Card.Players.Player)
        {
            m_TurnOwner = Card.Players.AI;
            m_AIMana = m_AIMaxMana;
            AIDrawCard();
            m_TurnAI = true;
            if (m_AICheatMode)
            {
                m_AIController.AICheatTurn();          
            }
            else
            {
                m_AIController.AITurn();
            }

            if (ChangeTurn != null)
            {
                ChangeTurn();
            }
        }
        else
	    {
            m_BtnEndTurn.SetActive(true);
            m_TurnOwner = Card.Players.Player;
            m_PlayerMana = m_PlayerMaxMana;
            DrawCard();
            CheckPlayableCard();
            if (ChangeTurn != null)
            {
                ChangeTurn();
            }
            m_PlayerMana = m_PlayerMaxMana;
        }
    }

    public void ChangePlayerMana(int i_Change)
    {
        if (m_TurnOwner == Card.Players.Player)
        {
            if (i_Change < 0)
            {
                AnimationManager.Instance.PlayerCast();
            }
            m_PlayerMana += i_Change;
            CheckPlayableCard();
        } 
    }

    public void ActivatePlayerAvatar()
    {
        m_PlayerAvatarActive = !m_PlayerAvatarActive;
        if (!m_PlayerAvatarActive)
        {
            m_BtnGainMaxMana.SetActive(true);
            m_BtnPowerDrawCard.SetActive(true);
        }
    }

    public void ShowNormalCards(Card i_SelectedCard = null)
    {
        for (int i = 0; i < 7; i++)
        {
            if (m_PlayerHand[i] != null && m_PlayerHand[i].GetComponent<Card>() != i_SelectedCard)
            {
                if (m_PlayerHand[i].GetComponent<Card>().m_Playable)
                {
                    m_PlayerHand[i].GetComponent<Card>().UnIlluminate();
                }
                else
                {
                    m_PlayerHand[i].GetComponent<Card>().UnplayableCard();
                }
            }
        }

        for (int i = 0; i < 15; i++)
        {
            if (m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy != i_SelectedCard)
            {
                if (m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy.m_Playable)
                {
                    m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy.UnIlluminate();
                }
                else
                {
                    m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy.UnplayableCard();
                }
            }
        }
    }

    public void PlayerStartTurn()
    {
        m_PlayerMana = m_PlayerMaxMana;
        m_TurnOwner = Card.Players.Player;
        CheckPlayableCard();
    }

    public void UpdateTexts()
    {
        m_PlayerHpText.text = m_PlayerHp.ToString();
        m_PlayerManaText.text = m_PlayerMana.ToString() + "/" + m_PlayerMaxMana.ToString();
        m_HpAIText.text = m_AIHp.ToString();
        m_ManaAIText.text = m_AIMana.ToString() + "/" + m_AIMaxMana.ToString();

        if (m_BtnPowerDrawCard.activeSelf)
        {
            m_BtnPowerDrawCard.GetComponentInChildren<TextMeshProUGUI>().text = "Draw Cost : " + m_PlayerHand.Count(s => s != null).ToString();
        }
    }

    public void PowerDrawCard()
    {
        if (m_PlayerMana >= m_PlayerHand.Count(s => s != null))
        {
            if (m_PlayerHand.Count(s => s == null) > 0)
            {
                AudioManager.Instance.AvatarSound();
                Cast(m_PlayerHand.Count(s => s != null));
                DrawCard();
                ActivatePlayerAvatar();
                CheckPlayableCard();
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if (m_PlayerHand[i] != null)
                    {
                        if (m_PlayerHand[i].GetComponent<Card>().m_Playable)
                        {
                            m_PlayerHand[i].GetComponent<Card>().Flash(Color.yellow, 1f);
                        }
                        else
                        {
                            m_PlayerHand[i].GetComponent<Card>().UnplayableCard();
                        }
                    }
                }
            }
        }
    }

    public void ShowValidTiles(Card i_Card = null)
    {
        if (i_Card.CardType == CardType.Component)
        {
            for (int i = 0; i < 7; i++)
            {
                if (m_PlayerHand[i] != null && m_PlayerHand[i].GetComponent<Card>() != i_Card)
                {
                    m_PlayerHand[i].GetComponent<Card>().ValidTarget();
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy != null)
                {
                    m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy.ValidTarget();
                }
            }
        }
        if (i_Card.CardType == CardType.Spell)
        {
            int x = 0;
            do
            {
                for (int i = 0; i < 3; i++)
                {
                    m_Board.m_Tiles[i + (x * 3)].GetComponent<TileController>().Illuminate();
                }
                x++;
            } while (x < i_Card.CastRange);
        }
        else if (i_Card.CastRange > -1 && i_Card.State == Card.States.InHand)
        {
            int x = 0;
            do
            {           
                for (int i = 0; i < 3; i++)
                {
                    if (m_Board.m_Tiles[i + (x * 3)].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Board.m_Tiles[i + (x * 3)].GetComponent<TileController>().Illuminate();
                    }    
                }
                x++;
            } while (x < i_Card.CastRange);
        }
        else if (i_Card.State == Card.States.InPlay && (i_Card.Movement > 0 || i_Card.AttackRange > 0))
        {
            TileController selected = i_Card.m_TileOccupied;
            int range = i_Card.Movement;
            int tempRange, tempRangeY;
            int attackRange = i_Card.AttackRange;
            int tempAttackRange , tempAttackRangeY;
            bool indirectAttack = i_Card.IndirectAttack;
            bool blockLeft = false, blockRight = false, blockUp = false, blockDown = false;

            for (int y = 0; y < 3; y++)
            {
                tempAttackRangeY = attackRange - y;
                tempRangeY = range - y;

                if (selected.PosY - y >= 0 && selected.PosY - y != selected.PosY)
                {
                    if (tempAttackRangeY >= 0 && (!blockDown || indirectAttack))
                    {
                        m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY - y].GetComponent<TileController>().InAttackRange();
                    }

                    if (m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY - y].GetComponent<TileController>().m_OccupiedBy)
                    {
                        blockDown = true;
                    }

                    if (tempRangeY >= 0 && !blockDown)
                    {
                        m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY - y].GetComponent<TileController>().Illuminate();
                    }
                }

                if (selected.PosY + y < 3 && selected.PosY + y != selected.PosY)
                {
                    if (tempAttackRangeY >= 0 && (!blockUp || indirectAttack))
                    {
                        m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY + y].GetComponent<TileController>().InAttackRange();
                    }

                    if (m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY + y].GetComponent<TileController>().m_OccupiedBy)
                    {
                        blockUp = true;
                    }

                    if (tempRangeY >= 0 && !blockUp)
                    {
                        m_Board.m_Tiles[((selected.PosX) * m_Board.m_Row) + selected.PosY + y].GetComponent<TileController>().Illuminate();
                    } 
                }
            }

            for (int x = 1; x <= Mathf.Max(range, attackRange); x++)
            {
                tempAttackRange = attackRange - x;
                tempRange = range - x;
                
                if (selected.PosX - x >= 0)
                {
                    if (tempAttackRange >= 0 && (indirectAttack || !blockLeft))
                    {
                        m_Board.m_Tiles[((selected.PosX - x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().InAttackRange();
                    }

                    if (m_Board.m_Tiles[((selected.PosX - x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().m_OccupiedBy)
                    {
                        blockLeft = true;
                    }

                    if (tempRange >= 0 && !blockLeft)
                    {
                        m_Board.m_Tiles[((selected.PosX - x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().Illuminate();
                    }
                    

                    if (indirectAttack)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            tempAttackRangeY = tempAttackRange - y;
                            tempRangeY = tempRange - y;

                            if (selected.PosY - y >= 0)
                            {
                                if (tempAttackRangeY >= 0)
                                {
                                    m_Board.m_Tiles[((selected.PosX - x) * m_Board.m_Row) + selected.PosY - y].GetComponent<TileController>().InAttackRange();
                                }
                            }

                            if (selected.PosY + y < 3)
                            {
                                if (tempAttackRangeY >= 0)
                                {
                                    m_Board.m_Tiles[((selected.PosX - x) * m_Board.m_Row) + selected.PosY + y].GetComponent<TileController>().InAttackRange();
                                }
                            }
                        }
                    }
                }

                if (selected.PosX + x <= m_Board.m_Column)
                {
                    if (selected.PosX + x >= m_Board.m_Column && tempAttackRange >= 0)
                    {
                        
                    }
                    else if (selected.PosX + x <= m_Board.m_Column && tempAttackRange >= 0 && (indirectAttack || !blockRight))
                    {
                        m_Board.m_Tiles[((selected.PosX + x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().InAttackRange();
                    }

                    if (selected.PosX + x < m_Board.m_Column && m_Board.m_Tiles[((selected.PosX + x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().m_OccupiedBy)
                    {
                        blockRight = true;
                    }

                    if (selected.PosX + x < m_Board.m_Column && tempRange >= 0 && !blockRight)
                    {
                        m_Board.m_Tiles[((selected.PosX + x) * m_Board.m_Row) + selected.PosY].GetComponent<TileController>().Illuminate();
                    }
                    

                    if (selected.PosX + x < m_Board.m_Column)
                    {
                        if (indirectAttack)
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                tempAttackRangeY = tempAttackRange - y;
                                tempRangeY = tempRange - y;

                                if (selected.PosY - y >= 0)
                                {
                                    if (tempAttackRangeY >= 0)
                                    {
                                        m_Board.m_Tiles[((selected.PosX + x) * m_Board.m_Row) + selected.PosY - y].GetComponent<TileController>().InAttackRange();
                                    }
                                }

                                if (selected.PosY + y < 3)
                                {
                                    if (tempAttackRangeY >= 0)
                                    {
                                        m_Board.m_Tiles[((selected.PosX + x) * m_Board.m_Row) + selected.PosY + y].GetComponent<TileController>().InAttackRange();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            if(selected.PosX + attackRange >= 5 && i_Card.m_Owner == Card.Players.Player && (!blockRight || indirectAttack))
            {
                m_AltarAI.InAttackRange();
            }
            else if (selected.PosX - attackRange < 0 && i_Card.m_Owner == Card.Players.AI)
            {
                m_AltarPlayer.InAttackRange();
            }
        }
    }

    public void ShowNormalTiles(Card i_SelectedCard)
    {
        for (int x = 0; x < m_Board.m_Tiles.Length; x++)
        {
            m_Board.m_Tiles[x].GetComponent<TileController>().UnIlluminate();
            if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null)
            {
                m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.UnIlluminate();
            }
        }
        m_AltarAI.UnIlluminate();
        m_AltarPlayer.UnIlluminate();

        ShowNormalCards(i_SelectedCard);
    }

    public void Cast(int i_ManaCost)
    {
        AnimationManager.Instance.PlayerCast();
        if (m_TurnOwner == Card.Players.Player)
        {
            ChangePlayerMana(-i_ManaCost);
        }
        else
        {
            m_AIMana -= i_ManaCost;
        }
        m_PlayerManaText.text = m_PlayerMana.ToString() + "/" + m_PlayerMaxMana.ToString();

        CheckPlayableCard();
    }

    public void UpdateAIManaText()
    {
        m_ManaAIText.text = m_AIMana.ToString() + "/" + m_AIMaxMana.ToString();
    }

    public void RaiseMaxPlayerMana()
    {
        AnimationManager.Instance.PlayerCast();
        if (m_PlayerMana >= m_PlayerMaxMana)
        {
            m_PlayerMana = 0;
            m_PlayerMaxMana++;
            m_PlayerManaText.text = m_PlayerMana.ToString() + "/" + m_PlayerMaxMana.ToString();
        }
        AudioManager.Instance.AvatarSound();
        CheckPlayableCard();
    }

    public void DrawCard()
    {
        Vector3 temp = new Vector3();
        int indexLibre = System.Array.IndexOf(m_PlayerHand, null);
        if (indexLibre > -1)
        {
            int i = 0;
            while (m_PlayerDeck[i] == null)
            {
                i++;
                if (i >= m_PlayerDeck.Length)
                {
                    ShuffleGraveInDeck();
                    i = 0;
                    break;
                }  
            }
            m_PlayerHand[indexLibre] = m_PlayerDeck[i];
            temp = m_PlayerHandPosition.position;
            if (indexLibre == 0)
            {
            }
            else if (indexLibre % 2 == 0)
            {
                temp.z += (indexLibre / 2) * 1.7f;
            }
            else
            {
                temp.z -= ((indexLibre + 1) / 2) * 1.7f;
            }
            m_PlayerDeck[i].GetComponent<Card>().m_Position = indexLibre;
            m_PlayerDeck[i].GetComponent<Card>().ChangeState(Card.States.InHand);
            m_PlayerDeck[i] = null;
            StartCoroutine(CardMove(m_PlayerHand[indexLibre].transform, m_PlayerDeckPosition.position, temp, m_PlayerDeckPosition.rotation, m_PlayerHandPosition.rotation, m_DrawTime));
        }
        else
        {
            Debug.Log("Main pleine");
        } 
    }

    public void AIDrawCard()
    {
        Vector3 temp = new Vector3();

        int indexLibre = System.Array.IndexOf(m_AIHand, null);
        if (indexLibre > -1)
        {
            int i = 0;
            while (m_AIDeck[i] == null)
            {
                i++;
                if (i >= m_AIDeck.Length)
                {
                    ShuffleGraveInDeck();
                    i = 0;
                    break;
                }
            }
            m_AIHand[indexLibre] = m_AIDeck[i];
            temp = m_AIHandPosition.position;
            if (indexLibre == 0)
            {
            }
            else if (indexLibre % 2 == 0)
            {
                temp.z += (indexLibre / 2) * 1.7f;
            }
            else
            {
                temp.z -= ((indexLibre + 1) / 2) * 1.7f;
            }
            m_AIDeck[i].GetComponent<Card>().m_Position = indexLibre;
            m_AIDeck[i].GetComponent<Card>().ChangeState(Card.States.InHand);
            m_AIDeck[i] = null;
            StartCoroutine(CardMove(m_AIHand[indexLibre].transform, m_AIDeckPosition.position, temp, m_AIDeckPosition.rotation, m_AIHandPosition.rotation, m_DrawTime));
        }
        else
        {
            Debug.Log("Main pleine");
        }
    }
    
    public void AIDiscardCard(Card i_Card)
    { 
        m_AIGrave[System.Array.IndexOf(m_AIGrave, null)] = i_Card.gameObject;
        StartCoroutine(CardMove(i_Card.transform, i_Card.transform.position, m_AIGravePosition.position, i_Card.transform.rotation, m_AIGravePosition.rotation, m_DiscardTime));
        i_Card.ChangeState(Card.States.InGrave);
        m_AIHand[i_Card.m_Position] = null;
        m_ZoomCard.SetActive(false);
        m_AIMana--;
        AIDrawCard();       
    }

    public void DiscardCard(Card i_SelectedCard)
    {
        if (i_SelectedCard != null)
        {
            m_PlayerGrave[System.Array.IndexOf(m_PlayerGrave, null)] = i_SelectedCard.gameObject;
            i_SelectedCard.MoveCard(m_PlayerGravePosition.position, m_PlayerGravePosition.rotation);
            if (i_SelectedCard.State == Card.States.InHand)
            {
                m_PlayerHand[i_SelectedCard.m_Position] = null;
                ChangePlayerMana(-i_SelectedCard.CastingCost);
                DrawCard();
            }
            i_SelectedCard.ChangeState(Card.States.InGrave);
            ShowNormalTiles(i_SelectedCard);
        }
        CheckPlayableCard();
    }

    public void MulliganHand()
    {
        for (int i = 0; i < m_PlayerHand.Length; i++)
        {
            ChangePlayerMana(m_PlayerHand[i].GetComponent<Card>().CastingCost);
            DiscardCard(m_PlayerHand[i].GetComponent<Card>());
        }
    }

    private GameObject AddCardToDeck( int i_Counter, int i_Position, Card.Players i_Player )
    {
        GameObject deckTemp = null;
        if (i_Player == Card.Players.Player)
        {
            deckTemp = Instantiate(m_CardPrefab, m_PlayerDeckPosition.position, m_PlayerDeckPosition.rotation, m_PlayerDeckPosition);
        }
        else
        {
            deckTemp = Instantiate(m_CardPrefab, m_AIDeckPosition.position, m_AIDeckPosition.rotation, m_AIDeckPosition);
        }
        deckTemp.AddComponent(Type.GetType(m_CardTypes[i_Counter].m_ScriptToAttach));
        deckTemp.GetComponent<Card>().m_CardName = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().m_Position = i_Position;
        deckTemp.GetComponent<Card>().ChangeState(Card.States.InDeck);
        deckTemp.GetComponent<Card>().m_Owner  = i_Player;
        deckTemp.name = m_CardTypes[i_Counter].m_CardTypeName;
        if (i_Player == Card.Players.Player)
        {
            deckTemp.GetComponent<Card>().InitCard(m_CardData, m_PlayerGravePosition);
        }
        else
        {
            deckTemp.GetComponent<Card>().InitCard(m_CardData, m_AIGravePosition);
        }
        deckTemp.GetComponent<Card>().m_Game = this;
        if (!m_RandomDeck)
        {
            m_CardNumberByType[i_Counter] -=  1;
        }
        return deckTemp;
    }

    public void CheckPlayableCard()
    {
        for (int i = 0; i < 7; i++)
        {
            if (m_PlayerHand[i] != null && m_PlayerHand[i].GetComponent<Card>().CastingCost > m_PlayerMana)
            {              
                m_PlayerHand[i].GetComponent<Card>().UnplayableCard();
            }
            else if(m_PlayerHand[i] != null)
            {
                m_PlayerHand[i].GetComponent<Card>().UnIlluminate();
            }
        }
        TileController temp;
        for (int i = 0; i < 15; i++)
        {
            temp = m_Board.m_Tiles[i].GetComponent<TileController>();
            if (temp.m_OccupiedBy != null && temp.m_OccupiedBy.m_Owner == m_TurnOwner)
            {
                if (temp.m_OccupiedBy.ActivateCost > m_PlayerMana)
                {
                    temp.m_OccupiedBy.UnplayableCard();
                }
                else
                {
                    temp.m_OccupiedBy.UnIlluminate();
                }
            }
        }
    }

    public void ShuffleGraveInDeck()
    {   
        int i = 0;
        while (m_PlayerGrave[i] != null)
        {
            i++;
        }
        int nbrCardToShuffle = i;
        while (nbrCardToShuffle > 0)
        {
            int randTemp = UnityEngine.Random.Range(0, i);
            if (m_PlayerGrave[randTemp] != null)
            {
                m_PlayerDeck[i - nbrCardToShuffle] = m_PlayerGrave[randTemp];
                m_PlayerGrave[randTemp].GetComponent<Card>().ChangeState(Card.States.InDeck);
                StartCoroutine(CardMove(m_PlayerGrave[randTemp].transform, m_PlayerGravePosition.position, m_PlayerDeckPosition.position, m_PlayerGravePosition.rotation, m_PlayerDeckPosition.rotation, m_ShuffleTime));
                m_PlayerGrave[randTemp] = null;
                nbrCardToShuffle--;
            }
        }
    }

    private GameObject[] GenerateDeck(int i_DeckSize = 65)
    {
        GameObject[] tempDeck = new GameObject[i_DeckSize];
        m_PlayerGrave = new GameObject[i_DeckSize];
        m_PlayerHand = new GameObject[m_MaxHandSize];

        for (int i = 0; i < tempDeck.Length; i++)
        {
            float randTemp = UnityEngine.Random.Range(0,m_CardTypes.Count);
            if (m_CardNumberByType[(int)randTemp] > 0)
            {
                tempDeck[i] = AddCardToDeck((int)randTemp,i, Card.Players.Player);
                tempDeck[i].GetComponent<Renderer>().material = m_CardTypes[(int)randTemp].m_CardTypeSkin;
            }
            else
            {
                i--;
            }
        }
        return tempDeck;
    }

    private GameObject[] GenerateAIDeck(int i_DeckSize = 65)
    {
        GameObject[] tempAIDeck = new GameObject[i_DeckSize];
        m_AIGrave = new GameObject[i_DeckSize];
        m_AIHand = new GameObject[m_MaxHandSize];

        for (int i = 0; i < tempAIDeck.Length; i++)
        {
            float randTemp = UnityEngine.Random.Range(0, 3);
            int cardToAdd = 0;
            switch ((int)randTemp)
            {
                case 0:
                    cardToAdd = 0;
                    break;
                case 2:
                    cardToAdd = 7;
                    break;
                case 3:
                    cardToAdd = 8;
                    break;
                default:
                    break;
            }

            if (m_AICardNumberByType[cardToAdd] > 0)
            {
                tempAIDeck[i] = AddCardToDeck(cardToAdd, i, Card.Players.AI);
                tempAIDeck[i].GetComponent<Renderer>().material = m_CardTypes[cardToAdd].m_CardTypeSkin;
            }
            else
            {
                i--;
            }
        }

        return tempAIDeck;
    }

    public IEnumerator CardMove(Transform i_Card, Vector3 i_StartPos, Vector3 i_EndPos, Quaternion i_StartRot, Quaternion i_EndRot, float i_MoveTime)
    {
        float currentTime = 0f;
        if (i_Card.GetComponent<Card>())
        {
            i_Card.GetComponent<Card>().AnimWalk();
        }
        
        yield return new WaitForSeconds(0.2f);
        float value;

        LookFoward CharPos = null;

        if (i_Card.GetComponentInChildren<LookFoward>())
        {
            CharPos = i_Card.GetComponentInChildren<LookFoward>();
        }

        while (currentTime != i_MoveTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > i_MoveTime)
            {
                currentTime = i_MoveTime;
            }

            value = currentTime / i_MoveTime;
            i_Card.position = Vector3.Lerp(i_StartPos, i_EndPos, value);
            i_Card.rotation = Quaternion.Slerp(i_StartRot, i_EndRot, value);
            if (CharPos != null)
            {
                CharPos.UpdatePos(i_Card.GetComponent<Card>().SpawnPoint.position);
            }
            yield return new WaitForEndOfFrame();
        }

        if (CharPos != null)
        {
            CharPos.UpdatePos(i_Card.GetComponent<Card>().SpawnPoint.position);
        }
        currentTime = 0f;

        if (i_Card != null && i_Card.GetComponent<Card>())
        {
            i_Card.GetComponent<Card>().AnimIdle();
        }
    }

    public void GameEnd(bool i_PlayerWin)
    {
        m_ResultText.gameObject.SetActive(true);
        if (i_PlayerWin)
        {
            m_ResultText.text = "You Won";
        }
        else
        {
            m_ResultText.text = "You Lost";
        }
        GameEnded = true;
        m_BtnEndTurn.SetActive(false);
        m_BtnGainMaxMana.SetActive(false);
        m_BtnPowerDrawCard.SetActive(false);
    }

    public void Zoom(bool i_StartZoom, Card i_CardToZoom = null)
    {
        if (i_StartZoom)
        {
            m_ZoomCard.SetActive(true);
            m_ZoomCard.GetComponent<Renderer>().material = i_CardToZoom.transform.GetComponent<Renderer>().material;
            m_ZoomCard.GetComponent<Card>().CopyCard(i_CardToZoom);
            m_ZoomCard.AddComponent<Card>().UnIlluminate();
            m_ZoomCamera.enabled = true;
            m_MainCamera.enabled = false;
            ShowNormalCards();
        }
        else
        {
            m_ZoomCard.SetActive(false);
            m_MainCamera.enabled = true;
            m_ZoomCamera.enabled = false;
        }
    }

    public void SelectedCardAttackAIAvatar(Card i_SelectedCard)
    {
        ChangePlayerMana(-i_SelectedCard.ActivateCost);
        HurtEnemy(i_SelectedCard.Attack);
        i_SelectedCard.UnIlluminate();
        i_SelectedCard = null;
        ShowNormalTiles(i_SelectedCard);
    }

    public void SelectedComponentAddToCard(Card i_CardOver, Card i_SelectedCard)
    {
        ChangePlayerMana(-i_SelectedCard.CastingCost);
        i_CardOver.AddComponentCard(i_SelectedCard);
        m_BtnDiscard.SetActive(false);
        Destroy(i_SelectedCard.gameObject);
        ShowNormalTiles(i_SelectedCard);
        CheckPlayableCard();
    }

    public void showButtonDiscard(bool i_Activate, Card i_SelectedCard)
    {
        m_BtnDiscard.SetActive(i_Activate);
        Vector3 temp = i_SelectedCard.transform.position;
        temp.y += 1f;
        temp.x -= 2.5f;
        temp.z -= 0.75f;
        m_BtnDiscard.transform.position = temp;
    }

    public void SelectedCardToInPlayState(Card i_SelectedCard)
    {
        i_SelectedCard.ChangeState(Card.States.InPlay);
        m_PlayerHand[i_SelectedCard.m_Position] = null;
        i_SelectedCard.m_Position = -1;
        ChangePlayerMana(-i_SelectedCard.CastingCost);
    }

    public void SelectedCardToBoard(TileController i_Tile, Card i_SelectedCard)
    {
        AudioManager.Instance.AvatarSound();
        i_SelectedCard.m_TileOccupied = i_Tile;
        i_Tile.m_OccupiedBy = i_SelectedCard;
        m_BtnDiscard.SetActive(false);
        ShowNormalTiles(i_SelectedCard);
        CheckPlayableCard();
    }

    public void CastSpell(TileController i_Tile, Card i_SelectedCard)
    {
        AudioManager.Instance.AvatarSound();
        i_Tile.AddSpellEffects(i_SelectedCard);
        m_BtnDiscard.SetActive(false);
        DiscardCard(i_SelectedCard);
        ShowNormalTiles(i_SelectedCard);
        CheckPlayableCard();
    }
}
