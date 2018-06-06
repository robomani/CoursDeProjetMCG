using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct CardClass
{
    public string m_CardTypeName;
    public int m_NbrInDeck;
    public Material m_CardTypeSkin;
    public string m_ScriptToAttach;
}

public class PlayerController : MonoBehaviour
{
#region Variables


    [SerializeField]
    private int m_HP = 20;
    [SerializeField]
    private int m_MaxMana = 1;
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private Transform m_PlayerDeckPosition;
    [SerializeField]
    private Transform m_PlayerHandPosition;
    [SerializeField]
    private Transform m_PlayerGravePosition;
    [SerializeField]
    private GameObject m_ZoomCard;

    [SerializeField]
    private List<CardClass> m_CardTypes = new List<CardClass>();

    [SerializeField]
    private int m_RandomDeckSize = 65;
    [SerializeField]
    private bool m_RandomDeck = false;
    [SerializeField]
    private int m_MaxHandSize = 7;


    public GameObject[] m_Hand;
    public GameObject[] m_Deck;
    public GameObject[] m_Grave;

    private bool m_PlayerTurn = true;
    private int m_CountPlayerHand = 0;
    private int m_Mana = 1;
    private int[] m_CardNumberByType;
    private Ray m_RayPlayerHand;
    private Card m_LastCard = null;
    private Card m_SelectedCard = null;

    #endregion

    private void Start()
    {
        
        if (m_RandomDeck)
        {
            GenerateDeck(m_RandomDeckSize);
        }
        else
        {
            int countCardToAdd = 0;
            int[] temp = new int[m_CardTypes.Count];
            for (int i = 0; i < m_CardTypes.Count; i++)
            {
                countCardToAdd += m_CardTypes[i].m_NbrInDeck;
                temp[i] = m_CardTypes[i].m_NbrInDeck;
            }
            m_CardNumberByType = temp;
            GenerateDeck(countCardToAdd);
        }

        for (int i = 0; i < 7; i++)
        {
            DrawCard();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DrawCard();
        }

        m_RayPlayerHand = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;
        if (m_PlayerTurn)
        {
            Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);
            if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f,LayerMask.GetMask("Card")))
            {
                if (m_LastCard != null && m_LastCard != HitInfo.transform.GetComponent<Card>() && m_LastCard != m_SelectedCard)
                {
                    m_LastCard.UnIlluminate();
                }


                if (Input.GetButtonDown("Select"))
                {
                    if (m_SelectedCard != HitInfo.transform.GetComponent<Card>())
                    {
                        if (m_SelectedCard != null)
                        {
                            m_SelectedCard.UnIlluminate();
                        }
                        m_SelectedCard = HitInfo.transform.GetComponent<Card>();
                        m_SelectedCard.SelectedColor();
                    }
                    else
                    {
                        m_SelectedCard.UnIlluminate();
                        m_SelectedCard = null;
                    }
                    
                }

                if (Input.GetButtonDown("Zoom"))
                {
                    m_ZoomCard.SetActive(true);
                    m_ZoomCard.GetComponent<Renderer>().material = HitInfo.transform.GetComponent<Renderer>().material;
                    m_ZoomCard.GetComponent<Card>().UnIlluminate();
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
        }
    }

    public void Cast(int i_ManaCost)
    {
        m_Mana -= i_ManaCost;
    }

    public void RaiseMaxMana()
    {
        if (m_Mana > 0)
        {
            m_Mana = 0;
            m_MaxMana++;
        }
    }

    public void DrawCard()
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
            m_Deck[i] = null;
            StartCoroutine(PlayerDrawCardMove(m_Hand[indexLibre].transform, temp));
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
            m_SelectedCard.transform.rotation = m_PlayerGravePosition.rotation;
            m_SelectedCard.transform.position = m_PlayerGravePosition.position;
            m_Hand[m_SelectedCard.m_Position] = null;
            m_SelectedCard = null;

            DrawCard();
        }
       
    }

    public GameObject AddCardToDeck( int i_Counter, int i_Position)
    {

        GameObject deckTemp = Instantiate(m_CardPrefab, m_PlayerDeckPosition.position, m_PlayerDeckPosition.rotation, m_PlayerDeckPosition);
        deckTemp.AddComponent(Type.GetType(m_CardTypes[i_Counter].m_ScriptToAttach));
        deckTemp.GetComponent<Card>().m_CardName = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().m_Position = i_Position;
        deckTemp.name = m_CardTypes[i_Counter].m_CardTypeName;
        if (!m_RandomDeck)
        {
            m_CardNumberByType[i_Counter] -=  1;
        }
        return deckTemp;
    }

    public void ShuffleGraveInDeck()
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
                m_Grave[randTemp].transform.rotation = m_PlayerDeckPosition.rotation;
                m_Grave[randTemp].transform.position = m_PlayerDeckPosition.position;
                m_Grave[randTemp] = null;
                nbrCardToShuffle--;
            }
        }
    }

    public void GenerateDeck(int i_DeckSize = 65)
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

        m_Deck = tempDeck;
    }

    private IEnumerator PlayerDrawCardMove(Transform i_Card , Vector3 i_EndPos)
    {
        float currentTime = 0f;
        float timeToDraw = 1.5f;

        yield return new WaitForSeconds(0.2f);
        float value;

        while (currentTime != timeToDraw)
        {
            currentTime += Time.deltaTime;
            if (currentTime > timeToDraw)
            {
                currentTime = timeToDraw;
            }

            value = currentTime / timeToDraw;
            i_Card.position = Vector3.Lerp(m_PlayerDeckPosition.position, i_EndPos, value);
            i_Card.rotation = Quaternion.Slerp(m_PlayerDeckPosition.rotation, m_PlayerHandPosition.rotation, value);
            yield return new WaitForEndOfFrame();
        }
        currentTime = 0f;
    }
}
