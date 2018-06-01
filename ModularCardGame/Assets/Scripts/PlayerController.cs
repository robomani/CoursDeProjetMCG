using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
#region Variables


    [SerializeField]
    private int m_HP = 20;
    [SerializeField]
    private int m_MaxMana = 1;
    [SerializeField]
    private int m_NBRSoldat = 10;
    [SerializeField]
    private Material m_SoldatSkin;
    [SerializeField]
    private int m_NBRMage = 5;
    [SerializeField]
    private Material m_MageSkin;
    [SerializeField]
    private int m_NBRArbaletrier = 5;
    [SerializeField]
    private Material m_ArbaletrierSkin;
    [SerializeField]
    private int m_NBRSort = 10;
    [SerializeField]
    private Material m_SortSkin;
    [SerializeField]
    private int m_NBRForge = 2;
    [SerializeField]
    private Material m_ForgeSkin;
    [SerializeField]
    private int m_NBRMur = 5;
    [SerializeField]
    private Material m_MurSkin;
    [SerializeField]
    private int m_NBRTour = 3;
    [SerializeField]
    private Material m_TourSkin;
    [SerializeField]
    private int m_NBRHp = 7;
    [SerializeField]
    private Material m_HPSkin;
    [SerializeField]
    private int m_NBRAttaque = 7;
    [SerializeField]
    private Material m_AttaqueSkin;
    [SerializeField]
    private int m_NBRMouvement = 3;
    [SerializeField]
    private Material m_MouvementSkin;
    [SerializeField]
    private int m_NBRPousse = 2;
    [SerializeField]
    private Material m_PousseSkin;
    [SerializeField]
    private int m_NBREchange = 2;
    [SerializeField]
    private Material m_EchangeSkin;
    [SerializeField]
    private int m_NBRArmure = 2;
    [SerializeField]
    private Material m_ArmureSkin;
    [SerializeField]
    private int m_NBROmbre = 2;
    [SerializeField]
    private Material m_OmbreSkin;
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private Transform m_PlayerDeckPosition;
    [SerializeField]
    private Transform m_PlayerHandPosition;
    [SerializeField]
    private Transform m_PlayerGravePosition;

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
    private int m_SelectedCardIndex;
    private Ray m_RayPlayerHand;
    private Card m_LastCard = new Card("",-1);

#endregion

    private void Start()
    {
        if (m_RandomDeck)
        {
            GenerateDeck(m_RandomDeckSize);
        }
        else
        {
            GenerateDeck(m_NBRSoldat + m_NBRMage + m_NBRArbaletrier + m_NBRSort + m_NBRForge + m_NBRMur + m_NBRTour + m_NBRHp + m_NBRAttaque + m_NBRMouvement + m_NBRPousse + m_NBREchange + m_NBRArmure + m_NBROmbre);
        }  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DrawCard();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            DiscardCard(m_LastCard.m_Position);
        }

        m_RayPlayerHand = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;
        if (m_PlayerTurn)
        {
            Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);
            if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f,LayerMask.GetMask("Card")))
            {
                if (m_LastCard.m_CardName != "" && m_LastCard.m_Position != HitInfo.transform.GetComponent<Card>().m_Position)
                {
                    m_LastCard.UnIlluminate();
                }
                Debug.Log(HitInfo.transform.name);
                m_SelectedCardIndex = HitInfo.transform.GetComponent<Card>().m_Position;
                HitInfo.transform.GetComponent<Card>().Illuminate();
                m_LastCard = HitInfo.transform.GetComponent<Card>();

            }
            else
            {
                m_LastCard.UnIlluminate();
                m_LastCard.m_CardName = "";
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
            }
            m_Hand[indexLibre] = m_Deck[i];
            Debug.Log(indexLibre);
            if (indexLibre == 0)
            {
                m_Deck[i].transform.Translate(m_PlayerHandPosition.position);
                m_Deck[i].GetComponent<Card>().m_Position = indexLibre;
                m_Deck[i] = null;
            }
            else if (indexLibre % 2 == 0)
            {
                temp = m_PlayerHandPosition.position;
                temp.z += (indexLibre / 2) * 1.7f;
                m_Deck[i].transform.Translate(temp);
                m_Deck[i].GetComponent<Card>().m_Position = indexLibre;
                m_Deck[i] = null;
            }
            else
            {
                temp = m_PlayerHandPosition.position;
                temp.z -= ((indexLibre + 1) / 2) * 1.7f;
                m_Deck[i].transform.Translate(temp);
                m_Deck[i].GetComponent<Card>().m_Position = indexLibre;
                m_Deck[i] = null;
            }
           
        }
        else
        {
            Debug.Log("Main pleine");
        }
        
    }

    public void DiscardCard(int i_CardNumber)
    {
        if (m_Hand[i_CardNumber].GetComponent<Card>())
        {
            m_Grave[System.Array.IndexOf(m_Grave, null)] = m_Hand[i_CardNumber];
            m_Hand[i_CardNumber].transform.Translate(m_PlayerGravePosition.position);
            m_Hand[i_CardNumber] = null;
        }
       
    }

    public GameObject AddCardToDeck( string i_Type,ref int r_Counter, int i_Position)
    {

        GameObject deckTemp = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeckPosition);
        deckTemp.GetComponent<Card>().m_CardName = i_Type;
        deckTemp.GetComponent<Card>().m_Position = i_Position;
        deckTemp.name = i_Type;
        if (!m_RandomDeck)
        {
            r_Counter--;
        }
        return deckTemp;
    }

    public void GenerateDeck(int i_DeckSize = 65)
    {
        GameObject[] tempDeck = new GameObject[i_DeckSize];
        m_Grave = new GameObject[i_DeckSize];
        m_Hand = new GameObject[m_MaxHandSize];

        for (int i = 0; i < tempDeck.Length; i++)
        {
            float randTemp = Random.Range(0, 14);
            if (randTemp == 0 && m_NBRSoldat > 0)
            {
                tempDeck[i] = AddCardToDeck("Soldat",ref m_NBRSoldat, i);
                tempDeck[i].GetComponent<Renderer>().material = m_SoldatSkin;
            }
            else if (randTemp == 1 && m_NBRMage > 0)
            {
                tempDeck[i] = AddCardToDeck("Mage", ref m_NBRMage, i);
                tempDeck[i].GetComponent<Renderer>().material = m_MageSkin;
            }
            else if (randTemp == 2 && m_NBRArbaletrier > 0)
            {
                tempDeck[i] = AddCardToDeck("Arbaletrier", ref m_NBRArbaletrier, i);
                tempDeck[i].GetComponent<Renderer>().material = m_ArbaletrierSkin;
            }
            else if (randTemp == 3 && m_NBRSort > 0)
            {
                tempDeck[i] = AddCardToDeck("Sort", ref m_NBRSort, i);
                tempDeck[i].GetComponent<Renderer>().material = m_SortSkin;
            }
            else if (randTemp == 4 && m_NBRForge > 0)
            {
                tempDeck[i] = AddCardToDeck("Forge", ref m_NBRForge, i);
                tempDeck[i].GetComponent<Renderer>().material = m_ForgeSkin;
            }
            else if (randTemp == 5 && m_NBRMur > 0)
            {
                tempDeck[i] = AddCardToDeck("Mur", ref m_NBRMur, i);
                tempDeck[i].GetComponent<Renderer>().material = m_MurSkin;
            }
            else if (randTemp == 6 && m_NBRTour > 0)
            {
                tempDeck[i] = AddCardToDeck("Tour", ref m_NBRTour, i);
                tempDeck[i].GetComponent<Renderer>().material = m_TourSkin;
            }
            else if (randTemp == 7 && m_NBRHp > 0)
            {
                tempDeck[i] = AddCardToDeck("Hp", ref m_NBRHp, i);
                tempDeck[i].GetComponent<Renderer>().material = m_HPSkin;
            }
            else if (randTemp == 8 && m_NBRAttaque > 0)
            {
                tempDeck[i] = AddCardToDeck("Attaque", ref m_NBRAttaque, i);
                tempDeck[i].GetComponent<Renderer>().material = m_AttaqueSkin;
            }
            else if (randTemp == 9 && m_NBRMouvement > 0)
            {
                tempDeck[i] = AddCardToDeck("Mouvement", ref m_NBRMouvement, i);
                tempDeck[i].GetComponent<Renderer>().material = m_MouvementSkin;
            }
            else if (randTemp == 10 && m_NBRPousse > 0)
            {
                tempDeck[i] = AddCardToDeck("Pousse", ref m_NBRPousse, i);
                tempDeck[i].GetComponent<Renderer>().material = m_PousseSkin;
            }
            else if (randTemp == 11 && m_NBREchange > 0)
            {
                tempDeck[i] = AddCardToDeck("Echange", ref m_NBREchange, i);
                tempDeck[i].GetComponent<Renderer>().material = m_EchangeSkin;
            }
            else if (randTemp == 12 && m_NBRArmure > 0)
            {
                tempDeck[i] = AddCardToDeck("Armure", ref m_NBRArmure, i);
                tempDeck[i].GetComponent<Renderer>().material = m_ArmureSkin;
            }
            else if (randTemp == 13 && m_NBROmbre > 0)
            {
                tempDeck[i] = AddCardToDeck("Ombre", ref m_NBROmbre, i);
                tempDeck[i].GetComponent<Renderer>().material = m_OmbreSkin;
            }
            else
            {
                i--;
            }
        }

        m_Deck = tempDeck;
    }
}
