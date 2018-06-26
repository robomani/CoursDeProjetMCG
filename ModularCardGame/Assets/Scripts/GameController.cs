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

    [Tooltip("Le texte qui affiche le nombre de vie du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_PlayerHpText;

    [Tooltip("Nombre de Mana Maximum du joueur")]
    [SerializeField]
    private int m_PlayerMaxMana = 1;

    [Tooltip("Le texte qui affiche le nombre de mana du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_PlayerManaText;
    #endregion

    #region AI Base Stats
    [Tooltip("Nombre de vie de base de l'IA")]
    [SerializeField]
    private int m_HpAI = 20;

    [Tooltip("Le texte qui affiche le nombre de vie de l'IA")]
    [SerializeField]
    private TextMeshProUGUI m_HpAIText;  

    [Tooltip("Nombre de Mana Maximum de l'IA")]
    [SerializeField]
    private int m_MaxManaAI = 1;

    [Tooltip("Le texte qui affiche le nombre de mana de l'IA")]
    [SerializeField]
    private TextMeshProUGUI m_ManaAIText;
    #endregion

    #region Card Move Speed
    [Tooltip("Vitesse de mouvement des cartes des deck aux mains")]
    [SerializeField]

    private float m_DrawTime = 1.5f;
    [Tooltip("Vitesse de mouvement des cartes des mains aux cimetières")]
    [SerializeField]
    private float m_DiscardTime = 1.5f;

    [Tooltip("Vitesse de mouvement des cartes des cimetières aux decks")]
    [SerializeField]
    private float m_ShuffleTime = 1.5f;
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

    [Tooltip("Le transform de la rotation des cartes du joueur sur une case")]
    [SerializeField]
    private Transform m_PlayerTileRotation;

    [Tooltip("Le transform de la position du deck de l'IA")]
    [SerializeField]
    private Transform m_AIDeckPosition;

    [Tooltip("Le transform de la position de la main de l'IA")]
    [SerializeField]
    private Transform m_AIHandPosition;

    [Tooltip("Le transform de la position du cimetière de l'IA")]
    [SerializeField]
    private Transform m_AIGravePosition;

    [Tooltip("Le transform de la rotation des cartes de l'IA sur une case")]
    [SerializeField]
    private Transform m_AITileRotation;
    #endregion

    [Tooltip("La carte qui prend l'apparence de la carte sur laquelle le joueur zoom")]
    [SerializeField]
    private GameObject m_ZoomCard;

    #region Reference des boutons
    [Tooltip("Le bouton qui permet de discarter la carte selectionnée")]
    [SerializeField]
    private GameObject m_BtnDiscard;

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

    [SerializeField]
    private TextMeshPro m_ResultText;

    [SerializeField]
    private CardsData m_CardData;
    [SerializeField]
    private TileController m_AltarAI;
    [SerializeField]
    private TileController m_AltarPlayer;

    private GameObject[] m_PlayerHand;
    private GameObject[] m_PlayerDeck;
    private GameObject[] m_PlayerGrave;

    private GameObject[] m_AIHand;
    private GameObject[] m_AIDeck;
    private List<GameObject> m_AIInPlay;
    private GameObject[] m_AIGrave;

    private bool m_PlayerAvatarActive = false;
    private bool m_WaitForMulligan = true;
    private bool m_TurnAI = false;
    private Card.Players m_TurnOwner = Card.Players.Player;
    private int m_PlayerMana = 1;
    private int m_AIMana = 1;
    private int[] m_CardNumberByType;
    private int[] m_AICardNumberByType;
    private Ray m_RayPlayerHand;
    private Card m_LastCard = null;
    private Card m_SelectedCard = null;
    private Vector3 m_TempPosition;

    #endregion

    private void Start()
    {
        int nbrCardToAdd = Initialise();

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


    private void Update()
    {
        if (m_PlayerHp <= 0)
        {
            m_ResultText.gameObject.SetActive(true);
            m_ResultText.text = "You Lost";
        }
        else if(m_HpAI <= 0)
        {
            m_ResultText.gameObject.SetActive(true);
            m_ResultText.text = "You Won";
        }
        m_RayPlayerHand = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;
        RaycastHit TileHitInfo;
        if (m_TurnOwner == Card.Players.Player && !m_WaitForMulligan)
        {
            Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);
            if (Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar AI")))
            {
                if (Input.GetButtonDown("Select") && TileHitInfo.transform.GetComponentInParent<TileController>().m_IsValidTarget && m_SelectedCard != null)
                {
                    ChangePlayerMana(-m_SelectedCard.m_ActivateCost);
                    m_HpAI -= m_SelectedCard.m_Attack;
                    m_SelectedCard.UnIlluminate();
                    m_SelectedCard = null;
                    ShowNormalTiles();
                }
            }
            else if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f,LayerMask.GetMask("Card")))
            {
                if (m_LastCard != null && m_LastCard != HitInfo.transform.GetComponent<Card>() && m_LastCard != m_SelectedCard && m_LastCard.m_Owner == m_TurnOwner && m_LastCard.m_Playable)
                {
                    m_LastCard.UnIlluminate();
                }

                if (Input.GetButtonDown("Select") && m_SelectedCard != null && m_SelectedCard.m_CardType == CardType.Component && HitInfo.transform.GetComponent<Card>().m_ValidTarget)
                {
                    ChangePlayerMana(-m_SelectedCard.m_CastCost);
                    HitInfo.transform.GetComponent<Card>().AddComponentCard(m_SelectedCard);
                    m_BtnDiscard.SetActive(false);
                    Destroy(m_SelectedCard.gameObject);
                    m_SelectedCard = null;
                    ShowNormalTiles();
                    CheckPlayableCard();
                }
                else if (Input.GetButtonDown("Select") && (HitInfo.transform.GetComponent<Card>().m_State == Card.States.InHand || HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay) && HitInfo.transform.GetComponent<Card>().m_Owner == m_TurnOwner && HitInfo.transform.GetComponent<Card>().m_Playable)
                {
                    if (m_SelectedCard != HitInfo.transform.GetComponent<Card>())
                    {
                        if (m_SelectedCard != null)
                        {
                            m_SelectedCard.UnIlluminate();
                        }
                        m_SelectedCard = HitInfo.transform.GetComponent<Card>();
                        
                        if (m_SelectedCard.m_State == Card.States.InHand)
                        {
                            m_BtnDiscard.SetActive(true);
                            Vector3 temp = m_SelectedCard.transform.position;
                            temp.y += 1f;
                            temp.x -= 2.5f;
                            temp.z -= 0.75f;
                            m_BtnDiscard.transform.position = temp;
                        }
                        ShowNormalTiles();
                        m_SelectedCard.SelectedColor();
                        ShowValidTiles(m_SelectedCard);    
                    }
                    else
                    {
                        m_SelectedCard.UnIlluminate();
                        ShowNormalTiles();
                        m_SelectedCard = null;
                        m_BtnDiscard.SetActive(false);
                    }
                    
                }
                else if (Input.GetButtonDown("Select") && HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay && HitInfo.transform.GetComponent<Card>().m_ValidTarget)
                {
                    AttackCard(m_SelectedCard, HitInfo.transform.GetComponent<Card>());
                }


                if (Input.GetButtonDown("Zoom") && (HitInfo.transform.GetComponent<Card>().m_State == Card.States.InHand || HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay))
                {
                    m_ZoomCard.SetActive(true);
                    m_ZoomCard.GetComponent<Renderer>().material = HitInfo.transform.GetComponent<Renderer>().material;
                    m_ZoomCard.GetComponent<Card>().CopyCard(HitInfo.transform.GetComponent<Card>());
                }

                if(m_SelectedCard != HitInfo.transform.GetComponent<Card>() && HitInfo.transform.GetComponent<Card>() != m_ZoomCard.GetComponent<Card>() && HitInfo.transform.GetComponent<Card>().m_Owner == m_TurnOwner && HitInfo.transform.GetComponent<Card>().m_Playable)
                {
                    if (m_SelectedCard != null && m_SelectedCard.m_CardType == CardType.Component)
                    {

                    }
                    else
                    {
                        HitInfo.transform.GetComponent<Card>().Illuminate();
                    }  
                }

                if (m_SelectedCard == null && ((HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay && HitInfo.transform.GetComponent<Card>().m_Owner == Card.Players.AI) || HitInfo.transform.GetComponent<Card>().m_Owner == Card.Players.Player))
                {
                    ShowValidTiles(HitInfo.transform.GetComponent<Card>());
                }
                
                              
                m_LastCard = HitInfo.transform.GetComponent<Card>();

            }
            else
            {
                if (m_LastCard != null && m_LastCard != m_SelectedCard && m_LastCard.m_Owner == m_TurnOwner && m_LastCard.m_Playable)
                {
                    m_LastCard.UnIlluminate();
                    m_LastCard.m_CardName = "";
                }

                if (m_SelectedCard == null)
                {
                    ShowNormalTiles();
                }

                if (Input.GetButtonDown("Zoom"))
                {
                    m_ZoomCard.SetActive(false);
                }
            }

            if (Input.GetButtonDown("Select") &&  m_SelectedCard != null && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Tiles"))  && TileHitInfo.transform.GetComponent<TileController>().m_IsValid )
            {
                m_TempPosition = TileHitInfo.transform.position;
                m_TempPosition.y += 1;
                StartCoroutine(CardMove(m_SelectedCard.transform, m_SelectedCard.transform.position, m_TempPosition, m_SelectedCard.transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                if (m_SelectedCard.m_State == Card.States.InHand)
                {
                    m_SelectedCard.m_State = Card.States.InPlay;
                    m_PlayerHand[m_SelectedCard.m_Position] = null;
                    m_SelectedCard.m_Position = -1;
                    ChangePlayerMana(-m_SelectedCard.m_CastCost);
                }
                else
                {
                    m_SelectedCard.m_TileOccupied.m_OccupiedBy = null;
                    ChangePlayerMana(-m_SelectedCard.m_ActivateCost);  
                }   
                m_SelectedCard.m_TileOccupied = TileHitInfo.transform.GetComponent<TileController>();
                TileHitInfo.transform.GetComponent<TileController>().m_OccupiedBy = m_SelectedCard;
                m_BtnDiscard.SetActive(false);
                m_SelectedCard = null;
                ShowNormalTiles();
                CheckPlayableCard();
            }

            if (Input.GetButtonDown("Select") && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar Player")) && m_PlayerMana > 0)
            {
                ActivatePlayerAvatar();
            }

            if (Input.GetButtonDown("Cheat"))
            { 
                DrawCard();
                ChangePlayerMana(10);
                m_PlayerHp += 1;
            }
        }
        else if (m_TurnOwner == Card.Players.AI && !m_WaitForMulligan)
        {
            
        }

        if (m_SelectedCard != null)
        {
            ShowValidTiles(m_SelectedCard);
        }
        UpdateTexts();
    }


    private void AttackCard(Card i_Attacker, Card i_Target)
    {
        if (i_Target.LoseHp(i_Attacker.m_Attack))
        {
            if (i_Target.m_Owner == Card.Players.Player)
            {
                StartCoroutine(CardMove(i_Target.transform, i_Target.transform.position, m_PlayerGravePosition.position, i_Target.transform.rotation, m_PlayerGravePosition.rotation, m_DiscardTime));
                m_AIMana -= i_Attacker.m_ActivateCost;
                m_PlayerGrave[System.Array.IndexOf(m_PlayerGrave, null)] = i_Target.gameObject;
            }
            else
            {
                StartCoroutine(CardMove(i_Target.transform, i_Target.transform.position, m_AIGravePosition.position, i_Target.transform.rotation, m_AIGravePosition.rotation, m_DiscardTime));
                ChangePlayerMana(-i_Attacker.m_ActivateCost);
                m_AIGrave[System.Array.IndexOf(m_AIGrave, null)] = i_Target.gameObject;
                m_SelectedCard = null;
                ShowNormalTiles();

            }

            i_Target.m_TileOccupied.ClearTile();
            i_Target.m_TileOccupied = null;
            i_Target.m_State = Card.States.InGrave;
        }
        else
        {
            if (m_TurnAI)
            {
                m_AIMana -= i_Attacker.m_ActivateCost;
            }
            else
            {
                ChangePlayerMana(-i_Attacker.m_ActivateCost);
            }
        }
    }

    private IEnumerator AiTurn()
    {
        yield return new WaitForSeconds(m_DrawTime + 0.3f);
        for (int i = 0; i < m_AIHand.Length; i++)
        {
            if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().m_CardType == CardType.Creature)
            {
                for (int x = 14; x > 11; x--)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                        m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                        StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        m_AIHand[i].GetComponent<Card>().m_State = Card.States.InPlay;
                        m_AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;

                    }
                }

            }
            else if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().m_CardType == CardType.Building)
            {
                for (int x = 9; x < 15; x++)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                        m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                        StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        m_AIHand[i].GetComponent<Card>().m_State = Card.States.InPlay;
                        m_AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;
                    }
                }
            }
            else if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().m_CardType == CardType.Component)
            {
                for (int x = 0; x < 15; x++)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI)
                    {
                        if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_CardType != CardType.Building || !m_AIHand[i].GetComponent<Attack>())
                        {
                            StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                            yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                            m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.AddComponentCard(m_AIHand[i].GetComponent<Card>());
                            Destroy(m_AIHand[i]);
                            m_AIHand[i] = null;
                            yield return new WaitForSeconds(0.5f);
                            break;
                        }
                        
                    }
                }
            }
        }

        for (int x = 0; x < 15; x++)
        {
            if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_CardType == CardType.Creature)
            {
                //Attack une colonne plus proche du joueur si possible
                if (m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));           
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack l'avatar du joueur si possible
                else if (x - 3 < 0)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    TileController PlayerAvatar = m_AltarPlayer;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerAvatar.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerAvatar.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    m_PlayerHp -= AiCard.m_Attack;
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une colonne plus loin du joueur si possible
                else if (m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x + 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    // return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une ligne plus haut si possible
                else if (x%3 != 0 &&  m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une ligne plus bas si possible
                else if ((x + 1) % 3 != 0 && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Bouge vers le joueur si possible
                else if (x-3 >= 0 && m_Board.m_Tiles[x-3].GetComponent<TileController>().m_OccupiedBy == null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_CardType == CardType.Creature)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy = AiCard;
                    m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = null;
                    AiCard.m_TileOccupied = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>();
                    AiCard = null;
                    yield return new WaitForSeconds(0.5f);
                    break;
                }


            }
        }
        m_TurnAI = false;
        EndTurn();
    }

    public void EndTurn()
    {
        if (m_TurnAI)
        {

        }
        else if (m_TurnOwner == Card.Players.Player)
        {
            m_TurnOwner = Card.Players.AI;
            m_AIMana = m_MaxManaAI;
            AIDrawCard();
            m_TurnAI = true;
            StartCoroutine(AiTurn());
        }
        else
	    {
            m_BtnEndTurn.SetActive(true);
            m_TurnOwner = Card.Players.Player;
            m_PlayerMana = m_PlayerMaxMana;
            DrawCard();
            CheckPlayableCard();
        }
    }

    public void ChangePlayerMana(int i_Change)
    {
        m_PlayerMana += i_Change;
        CheckPlayableCard();
    }

    private void ActivatePlayerAvatar()
    {
        m_PlayerAvatarActive = !m_PlayerAvatarActive;
        if (m_PlayerAvatarActive)
        {
            m_BtnGainMaxMana.SetActive(false);
            m_BtnPowerDrawCard.SetActive(false);
        }
        else
        {
            m_BtnGainMaxMana.SetActive(true);
            m_BtnPowerDrawCard.SetActive(true);
        }
    }

    private void ShowNormalCards()
    {
        for (int i = 0; i < 7; i++)
        {
            if (m_PlayerHand[i] != null && m_PlayerHand[i].GetComponent<Card>() != m_SelectedCard)
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
            if (m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[i].GetComponent<TileController>().m_OccupiedBy != m_SelectedCard)
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

    private void UpdateTexts()
    {
        m_PlayerHpText.text = m_PlayerHp.ToString();
        m_PlayerManaText.text = m_PlayerMana.ToString() + "/" + m_PlayerMaxMana.ToString();
        m_HpAIText.text = m_HpAI.ToString();
        m_ManaAIText.text = m_AIMana.ToString() + "/" + m_MaxManaAI.ToString();

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
                Cast(m_PlayerHand.Count(s => s != null));
                DrawCard();
                ActivatePlayerAvatar();
                CheckPlayableCard();
            }
            else
            {
                //TODO:: Start a coroutine that flash the Cards in hand red
            }
        }
        else
        {
            //TODO:: Start a coroutine that flash the mana counter of the player and the PowerDrawButon
        }

    }

    public void StopWaitingForMulligan()
    {
        m_WaitForMulligan = false;
    }

    private void ShowValidTiles(Card i_Card)
    {
        if (i_Card.m_CardType == CardType.Component)
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
        else if (i_Card.m_CastRange > -1 && i_Card.m_State == Card.States.InHand)
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
            } while (x < i_Card.m_CastRange);
        }
        else if (i_Card.m_State == Card.States.InPlay && (i_Card.m_Mouvement > 0 || i_Card.m_AttackRange > 0))
        {
            TileController tile = null;
            TileController selected = i_Card.m_TileOccupied;
            int range = i_Card.m_Mouvement;
            int AttackRange = i_Card.m_AttackRange;


            for (int i = 0; i < m_Board.m_Tiles.Length; i++)
            {
                tile = m_Board.m_Tiles[i].GetComponent<TileController>();
                
                if ((selected.PosX + range >=  tile.PosX && selected.PosX - range <= tile.PosX) && (selected.PosY + range >= tile.PosY && selected.PosY - range <= tile.PosY))
                {
                    if (tile != selected && tile.m_OccupiedBy == null)
                    {
                        if (i_Card.m_Owner == Card.Players.Player)
                        {
                            tile.Illuminate();
                        }
                        else
                        {
                            tile.EnemyIlluminate();
                        }
                    }                    
                }
                if ((selected.PosX + AttackRange >= tile.PosX && selected.PosX - AttackRange <= tile.PosX) && (selected.PosY + AttackRange >= tile.PosY && selected.PosY - AttackRange <= tile.PosY))
                {
                    if (i_Card.m_IndirectAttack)
                    {
                        if (tile != selected && tile.m_OccupiedBy != null && tile.m_OccupiedBy.m_Owner != i_Card.m_Owner)
                        {
                            tile.m_OccupiedBy.ValidTarget();

                        }
                        tile.InAttackRange();
                    }
                    else if(selected.PosX == tile.PosX || selected.PosY == tile.PosY)
                    {
                        if (tile != selected && tile.m_OccupiedBy != null && tile.m_OccupiedBy.m_Owner != i_Card.m_Owner)
                        {
                            tile.m_OccupiedBy.ValidTarget();

                        }
                        tile.InAttackRange();
                    }
                }
            }
            if(selected.PosX + AttackRange >= 5 && i_Card.m_Owner == Card.Players.Player)
            {
                m_AltarAI.InAttackRange();
            }
            else if (selected.PosX - AttackRange < 0 && i_Card.m_Owner == Card.Players.AI)
            {
                m_AltarPlayer.InAttackRange();
            }
        }
    }

    private void ShowNormalTiles()
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

        ShowNormalCards();
    }

    public void Cast(int i_ManaCost)
    {
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

    public void RaiseMaxPlayerMana()
    {
        if (m_PlayerMana > 0)
        {
            m_PlayerMana = 0;
            m_PlayerMaxMana++;
            m_PlayerManaText.text = m_PlayerMana.ToString() + "/" + m_PlayerMaxMana.ToString();
        }

        CheckPlayableCard();
    }

    private void DrawCard()
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
            m_PlayerDeck[i].GetComponent<Card>().m_State = Card.States.InHand;
            m_PlayerDeck[i] = null;
            StartCoroutine(CardMove(m_PlayerHand[indexLibre].transform, m_PlayerDeckPosition.position, temp, m_PlayerDeckPosition.rotation, m_PlayerHandPosition.rotation, m_DrawTime));
        }
        else
        {
            Debug.Log("Main pleine");
        } 
    }

    private void AIDrawCard()
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
            m_AIDeck[i].GetComponent<Card>().m_State = Card.States.InHand;
            m_AIDeck[i] = null;
            StartCoroutine(CardMove(m_AIHand[indexLibre].transform, m_AIDeckPosition.position, temp, m_AIDeckPosition.rotation, m_AIHandPosition.rotation, m_DrawTime));
        }
        else
        {
            Debug.Log("Main pleine");
        }
    }

    public void DiscardCard()
    {
        if (m_SelectedCard != null)
        {
            m_PlayerGrave[System.Array.IndexOf(m_PlayerGrave, null)] = m_SelectedCard.gameObject;
            StartCoroutine(CardMove(m_SelectedCard.transform, m_SelectedCard.transform.position, m_PlayerGravePosition.position, m_SelectedCard.transform.rotation, m_PlayerGravePosition.rotation, m_DiscardTime));
            m_SelectedCard.m_State = Card.States.InGrave;
            m_PlayerHand[m_SelectedCard.m_Position] = null;
            m_SelectedCard = null;
            m_ZoomCard.SetActive(false);
            DrawCard();
            ShowNormalTiles();
        }
        CheckPlayableCard();
    }

    public void MulliganHand()
    {
        for (int i = 0; i < m_PlayerHand.Length; i++)
        {
            m_SelectedCard = m_PlayerHand[i].GetComponent<Card>();
            DiscardCard();
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
        deckTemp.GetComponent<Card>().m_State = Card.States.InDeck;
        deckTemp.GetComponent<Card>().m_Owner  = i_Player;
        deckTemp.name = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().InitCard(m_CardData);
        if (!m_RandomDeck)
        {
            m_CardNumberByType[i_Counter] -=  1;
        }
        return deckTemp;
    }

    private void CheckPlayableCard()
    {
        for (int i = 0; i < 7; i++)
        {
            if (m_PlayerHand[i] != null && m_PlayerHand[i].GetComponent<Card>().m_CastCost > m_PlayerMana)
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
                if (temp.m_OccupiedBy.m_ActivateCost > m_PlayerMana)
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

    private void ShuffleGraveInDeck()
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
                m_PlayerGrave[randTemp].GetComponent<Card>().m_State = Card.States.InDeck;
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
                case 1:
                    cardToAdd = 5;
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

        yield return new WaitForSeconds(0.2f);
        float value;

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
            yield return new WaitForEndOfFrame();
        }
        currentTime = 0f;
    }
}
