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
    private int m_HpJoueur = 20;

    [Tooltip("Le texte qui affiche le nombre de vie du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_HpJoueurText;

    [Tooltip("Nombre de Mana Maximum du joueur")]
    [SerializeField]
    private int m_MaxManaJoueur = 1;

    [Tooltip("Le texte qui affiche le nombre de mana du joueur")]
    [SerializeField]
    private TextMeshProUGUI m_ManaJoueurText;
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

    private GameObject[] m_Hand;
    private GameObject[] m_Deck;
    private GameObject[] m_Grave;

    private bool m_PlayerAvatarActive = false;
    private bool m_WaitForMulligan = true;
    private bool m_PlayerTurn = true;
    private int m_PlayerMana = 1;
    private int m_AIMana = 1;
    private int[] m_CardNumberByType;
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
            m_Deck = GenerateDeck(m_RandomDeckSize);
        }
        else
        {
            m_Deck = GenerateDeck(nbrCardToAdd);
        }

        for (int i = 0; i < 7; i++)
        {
            DrawCard();
        }
        
    }

    //Initialise l'array du nombre de chaque type de carte à ajouter dans le deck.
    //Retourne le nombre total de carte à ajouter dans le deck si l'on respecte le nombre de carte de chaque type
    private int Initialise()
    {
        int countCardToAdd = 0;
        int[] temp = new int[m_CardTypes.Count];
        for (int i = 0; i < m_CardTypes.Count; i++)
        {
            countCardToAdd += m_CardTypes[i].m_NbrInDeck;
            temp[i] = m_CardTypes[i].m_NbrInDeck;
        }
        m_CardNumberByType = temp;

        UpdateTexts();

        return countCardToAdd;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DrawCard();
        }

        m_RayPlayerHand = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;
        RaycastHit TileHitInfo;
        if (m_PlayerTurn && !m_WaitForMulligan)
        {
            Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);
            if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f,LayerMask.GetMask("Card")))
            {
                if (m_LastCard != null && m_LastCard != HitInfo.transform.GetComponent<Card>() && m_LastCard != m_SelectedCard)
                {
                    m_LastCard.UnIlluminate();
                }


                if (Input.GetButtonDown("Select") && (HitInfo.transform.GetComponent<Card>().m_State == Card.States.InHand || HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay))
                {
                    if (m_SelectedCard != HitInfo.transform.GetComponent<Card>())
                    {
                        if (m_SelectedCard != null)
                        {
                            m_SelectedCard.UnIlluminate();
                        }
                        m_SelectedCard = HitInfo.transform.GetComponent<Card>();
                        m_SelectedCard.SelectedColor();
                        if (m_SelectedCard.m_State == Card.States.InHand)
                        {
                            m_BtnDiscard.SetActive(true);
                        }
                        Vector3 temp = m_SelectedCard.transform.position;
                        temp.y += 1f;
                        temp.x -= 2.5f;
                        temp.z -= 0.75f;
                        m_BtnDiscard.transform.position = temp;
                        ShowNormalTiles();
                        ShowValidTiles();
                    }
                    else
                    {
                        m_SelectedCard.UnIlluminate();
                        ShowNormalTiles();
                        m_SelectedCard = null;
                        m_BtnDiscard.SetActive(false);
                    }
                    
                }
                

                if (Input.GetButtonDown("Zoom") && (HitInfo.transform.GetComponent<Card>().m_State == Card.States.InHand || HitInfo.transform.GetComponent<Card>().m_State == Card.States.InPlay))
                {
                    m_ZoomCard.SetActive(true);
                    m_ZoomCard.GetComponent<Renderer>().material = HitInfo.transform.GetComponent<Renderer>().material;
                }

                if(m_SelectedCard != HitInfo.transform.GetComponent<Card>() && HitInfo.transform.GetComponent<Card>() != m_ZoomCard.GetComponent<Card>())
                {
                    HitInfo.transform.GetComponent<Card>().Illuminate();
                }
                              
                m_LastCard = HitInfo.transform.GetComponent<Card>();

            }
            else
            {
                if (m_LastCard != null && m_LastCard != m_SelectedCard)
                {
                    m_LastCard.UnIlluminate();
                    m_LastCard.m_CardName = "";
                }

                if (Input.GetButtonDown("Zoom"))
                {
                    m_ZoomCard.SetActive(false);
                }
            }

            if (Input.GetButtonDown("Select") &&  m_SelectedCard != null && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Tiles"))  && TileHitInfo.transform.GetComponent<TileController>().m_IsValid)
            {
                m_TempPosition = TileHitInfo.transform.position;
                m_TempPosition.y += 1;
                StartCoroutine(CardMove(m_SelectedCard.transform, m_SelectedCard.transform.position, m_TempPosition, m_SelectedCard.transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                m_SelectedCard.m_State = Card.States.InPlay;
                m_Hand[m_SelectedCard.m_Position] = null;
                m_SelectedCard.m_Position = -1;
                m_SelectedCard.m_TileOccupied = TileHitInfo.transform.GetComponent<TileController>();
                TileHitInfo.transform.GetComponent<TileController>().m_IsOccupied = true;
                m_BtnDiscard.SetActive(false);
                ShowNormalTiles();
            }

            if (Input.GetButtonDown("Select") && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar Player")) && m_PlayerMana > 0)
            {
                ActivatePlayerAvatar();
            }

            if (Input.GetButtonDown("Cheat"))
            { 
                DrawCard();
                m_PlayerMana += 10;
                m_HpJoueur += 1;
            }


        }

        UpdateTexts();
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



    private void UpdateTexts()
    {
        m_HpJoueurText.text = m_HpJoueur.ToString();
        m_ManaJoueurText.text = m_PlayerMana.ToString() + "/" + m_MaxManaJoueur.ToString();
        m_HpAIText.text = m_HpAI.ToString();
        m_ManaAIText.text = m_AIMana.ToString() + "/" + m_MaxManaAI.ToString();

        if (m_BtnPowerDrawCard.activeSelf)
        {
            m_BtnPowerDrawCard.GetComponentInChildren<TextMeshProUGUI>().text = "Draw Cost : " + m_Hand.Count(s => s != null).ToString();
        }
    }

    public void PowerDrawCard()
    {
        if (m_PlayerMana >= m_Hand.Count(s => s != null))
        {
            if (m_Hand.Count(s => s == null) > 0)
            {
                Cast(m_Hand.Count(s => s != null));
                DrawCard();
                ActivatePlayerAvatar();
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

    private void ShowValidTiles()
    {
        if (m_SelectedCard.m_CastRange > -1 && m_SelectedCard.m_State == Card.States.InHand)
        {
            int x = 0;
            do
            {           
                for (int i = 0; i < 3; i++)
                {
                    m_Board.m_Tiles[i + (x * 3)].GetComponent<TileController>().Illuminate();
                }
                x++;
            } while (x < m_SelectedCard.m_CastRange);
        }
        else if (m_SelectedCard.m_State == Card.States.InPlay && m_SelectedCard.m_Mouvement > 0)
        {

        }
    }

    private void ShowNormalTiles()
    {
        for (int x = 0; x < m_Board.m_Tiles.Length; x++)
        {
            m_Board.m_Tiles[x].GetComponent<TileController>().UnIlluminate();           
        }
    }

    public void Cast(int i_ManaCost)
    {
        if (m_PlayerTurn)
        {
            m_PlayerMana -= i_ManaCost;
        }
        else
        {
            m_AIMana -= i_ManaCost;
        }
        m_ManaJoueurText.text = m_PlayerMana.ToString() + "/" + m_MaxManaJoueur.ToString();
    }

    public void RaiseMaxPlayerMana()
    {
        if (m_PlayerMana > 0)
        {
            m_PlayerMana = 0;
            m_MaxManaJoueur++;
            m_ManaJoueurText.text = m_PlayerMana.ToString() + "/" + m_MaxManaJoueur.ToString();
        }
    }

    private void DrawCard()
    {
        Vector3 temp = new Vector3();
        int indexLibre = System.Array.IndexOf(m_Hand, null);
        if (indexLibre > -1)
        {
            int i = 0;
            while (m_Deck[i] == null)
            {
                i++;
                if (i >= m_Deck.Length)
                {
                    ShuffleGraveInDeck();
                    i = 0;
                    break;
                }  
            }
            m_Hand[indexLibre] = m_Deck[i];
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
            m_Deck[i].GetComponent<Card>().m_Position = indexLibre;
            m_Deck[i].GetComponent<Card>().m_State = Card.States.InHand;
            m_Deck[i] = null;
            StartCoroutine(CardMove(m_Hand[indexLibre].transform, m_PlayerDeckPosition.position, temp, m_PlayerDeckPosition.rotation, m_PlayerHandPosition.rotation, m_DrawTime));
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
            m_Grave[System.Array.IndexOf(m_Grave, null)] = m_SelectedCard.gameObject;
            StartCoroutine(CardMove(m_SelectedCard.transform, m_SelectedCard.transform.position, m_PlayerGravePosition.position, m_SelectedCard.transform.rotation, m_PlayerGravePosition.rotation, m_DiscardTime));
            m_SelectedCard.m_State = Card.States.InGrave;
            m_Hand[m_SelectedCard.m_Position] = null;
            m_SelectedCard = null;
            m_ZoomCard.SetActive(false);
            DrawCard();
        }
       
    }

    public void MulliganHand()
    {
        for (int i = 0; i < m_Hand.Length; i++)
        {
            m_SelectedCard = m_Hand[i].GetComponent<Card>();
            DiscardCard();
        }
        
    }

    private GameObject AddCardToDeck( int i_Counter, int i_Position)
    {

        GameObject deckTemp = Instantiate(m_CardPrefab, m_PlayerDeckPosition.position, m_PlayerDeckPosition.rotation, m_PlayerDeckPosition);
        deckTemp.AddComponent(Type.GetType(m_CardTypes[i_Counter].m_ScriptToAttach));
        deckTemp.GetComponent<Card>().m_CardName = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().m_Position = i_Position;
        deckTemp.GetComponent<Card>().m_State = Card.States.InDeck;
        deckTemp.name = m_CardTypes[i_Counter].m_CardTypeName;
        if (!m_RandomDeck)
        {
            m_CardNumberByType[i_Counter] -=  1;
        }
        return deckTemp;
    }

    private void ShuffleGraveInDeck()
    {   
        int i = 0;
        while (m_Grave[i] != null)
        {
            i++;
        }
        int nbrCardToShuffle = i;
        while (nbrCardToShuffle > 0)
        {
            int randTemp = UnityEngine.Random.Range(0, i);
            if (m_Grave[randTemp] != null)
            {
                m_Deck[i - nbrCardToShuffle] = m_Grave[randTemp];
                m_Grave[randTemp].GetComponent<Card>().m_State = Card.States.InDeck;
                StartCoroutine(CardMove(m_Grave[randTemp].transform, m_PlayerGravePosition.position, m_PlayerDeckPosition.position, m_PlayerGravePosition.rotation, m_PlayerDeckPosition.rotation, m_ShuffleTime));
                m_Grave[randTemp] = null;
                nbrCardToShuffle--;
            }
        }
    }

    private GameObject[] GenerateDeck(int i_DeckSize = 65)
    {
        GameObject[] tempDeck = new GameObject[i_DeckSize];
        m_Grave = new GameObject[i_DeckSize];
        m_Hand = new GameObject[m_MaxHandSize];

        for (int i = 0; i < tempDeck.Length; i++)
        {
            float randTemp = UnityEngine.Random.Range(0,m_CardTypes.Count);
            if (m_CardNumberByType[(int)randTemp] > 0)
            {
                tempDeck[i] = AddCardToDeck((int)randTemp,i);
                tempDeck[i].GetComponent<Renderer>().material = m_CardTypes[(int)randTemp].m_CardTypeSkin;
            }
            else
            {
                i--;
            }
        }

        return tempDeck;
    }

    private IEnumerator CardMove(Transform i_Card, Vector3 i_StartPos, Vector3 i_EndPos, Quaternion i_StartRot, Quaternion i_EndRot, float i_MoveTime)
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
