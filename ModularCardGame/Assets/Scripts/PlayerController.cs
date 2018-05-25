using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int m_HP = 20;
    [SerializeField]
    private int m_MaxMana = 1;
    [SerializeField]
    private int m_NBRSolder = 10;
    [SerializeField]
    private int m_NBRMage = 5;
    [SerializeField]
    private int m_NBRArbaletrier = 5;
    [SerializeField]
    private int m_NBRSort = 10;
    [SerializeField]
    private int m_NBRForge = 2;
    [SerializeField]
    private int m_NBRMur = 5;
    [SerializeField]
    private int m_NBRTour = 3;
    [SerializeField]
    private int m_NBRHp = 7;
    [SerializeField]
    private int m_NBRAttaque = 7;
    [SerializeField]
    private int m_NBRMouvement = 3;
    [SerializeField]
    private int m_NBRPousse = 2;
    [SerializeField]
    private int m_NBREchange = 2;
    [SerializeField]
    private int m_NBRArmure = 2;
    [SerializeField]
    private int m_NBROmbre = 2;
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private Transform m_PlayerDeck;
    [SerializeField]
    private Transform m_PlayerHand;
    [SerializeField]
    private Transform m_PlayerGrave;

    [SerializeField]
    private int m_RandomDeckSize = 65;
    [SerializeField]
    private bool m_RandomDeck = false;

    private int m_Mana = 1;


    public Card[] m_Hand;
    public GameObject[] m_Deck;
    public Card[] m_Grave;


    private void Start()
    {
        if (m_RandomDeck)
        {
            GenerateDeck(m_RandomDeckSize);
        }
        else
        {
            GenerateDeck(m_NBRSolder + m_NBRMage + m_NBRArbaletrier + m_NBRSort + m_NBRForge + m_NBRMur + m_NBRTour + m_NBRHp + m_NBRAttaque + m_NBRMouvement + m_NBRPousse + m_NBREchange + m_NBRArmure + m_NBROmbre);
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

    }

    public void DiscardCard()
    {

    }

    public void GenerateDeck(int i_DeckSize = 65)
    {
        GameObject[] tempDeck = new GameObject[i_DeckSize];
        for (int i = 0; i < tempDeck.Length; i++)
        {
            float randTemp = Random.Range(0, 14);
            if (randTemp == 0 && m_NBRSolder > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Soldat";
                tempDeck[i].name = "Soldat";
                if (!m_RandomDeck)
                {
                    m_NBRSolder--;
                }                
            }
            else if (randTemp == 1 && m_NBRMage > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Mage";
                tempDeck[i].name = "Mage";
                if (!m_RandomDeck)
                {
                    m_NBRMage--;
                }
            }
            else if (randTemp == 2 && m_NBRArbaletrier > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Arbaletrier";
                tempDeck[i].name = "Arbaletrier";
                if (!m_RandomDeck)
                {
                    m_NBRArbaletrier--;
                }
            }
            else if (randTemp == 3 && m_NBRSort > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Sort";
                tempDeck[i].name = "Sort";
                if (!m_RandomDeck)
                {
                    m_NBRSort--;
                }
            }
            else if (randTemp == 4 && m_NBRForge > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Forge";
                tempDeck[i].name = "Forge";
                if (!m_RandomDeck)
                {
                    m_NBRForge--;
                }
            }
            else if (randTemp == 5 && m_NBRMur > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Mur";
                tempDeck[i].name = "Mur";
                if (!m_RandomDeck)
                {
                    m_NBRMur--;
                }
            }
            else if (randTemp == 6 && m_NBRTour > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Tour";
                tempDeck[i].name = "Tour";
                if (!m_RandomDeck)
                {
                    m_NBRTour--;
                }
            }
            else if (randTemp == 7 && m_NBRHp > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Hp";
                tempDeck[i].name = "Hp";
                if (!m_RandomDeck)
                {
                    m_NBRHp--;
                }
            }
            else if (randTemp == 8 && m_NBRAttaque > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Attaque";
                tempDeck[i].name = "Attaque";
                if (!m_RandomDeck)
                {
                    m_NBRAttaque--;
                }
            }
            else if (randTemp == 9 && m_NBRMouvement > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Mouvement";
                tempDeck[i].name = "Mouvement";
                if (!m_RandomDeck)
                {
                    m_NBRMouvement--;
                }
            }
            else if (randTemp == 10 && m_NBRPousse > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Pousse";
                tempDeck[i].name = "Pousse";
                if (!m_RandomDeck)
                {
                    m_NBRPousse--;
                }
            }
            else if (randTemp == 11 && m_NBREchange > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Echange";
                tempDeck[i].name = "Echange";
                if (!m_RandomDeck)
                {
                    m_NBREchange--;
                }
            }
            else if (randTemp == 12 && m_NBRArmure > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Armure";
                tempDeck[i].name = "Armure";
                if (!m_RandomDeck)
                {
                    m_NBRArmure--;
                }
            }
            else if (randTemp == 13 && m_NBROmbre > 0)
            {
                tempDeck[i] = Instantiate(m_CardPrefab, Vector3.zero, Quaternion.identity, m_PlayerDeck);
                tempDeck[i].GetComponent<Card>().m_CardName = "Ombre";
                tempDeck[i].name = "Ombre";
                if (!m_RandomDeck)
                {
                    m_NBROmbre--;
                }
            }
            else
            {
                i--;
            }
        }

        m_Deck = tempDeck;
    }
}
