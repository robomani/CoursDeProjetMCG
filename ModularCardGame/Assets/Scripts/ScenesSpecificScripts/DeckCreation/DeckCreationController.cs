using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DeckCreationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_BaseCards;
    [SerializeField]
    private List<GameObject> m_CurrentDeck;
    [SerializeField]
    private GameObject m_BaseCardPos;
    [SerializeField]
    private GameObject m_CurrentCardPos;
    private GameObject m_CurrentCard;
    private GameObject m_BaseCard;
    private int m_BaseCardIndex = 0;


    #region Buttons
    [Tooltip("Le bouton qui permet d'ajouter la composante de base a la carte en cours de construction")]
    [SerializeField]
    private GameObject m_BtnAdd;

    [Tooltip("Le bouton qui permet de soustraire la composante de base a la carte en cours de construction")]
    [SerializeField]
    private GameObject m_BtnSubstract;

    [Tooltip("Le bouton qui permet de changer le status de la carte en cours de construction pour 'Dans la main'")]
    [SerializeField]
    private GameObject m_BtnInHand;

    [Tooltip("Le bouton qui permet de changer le status de la carte en cours de construction pour 'En jeu'")]
    [SerializeField]
    private GameObject m_BtnInPlay;

    [Tooltip("Le bouton qui permet de detruire la carte en cours de construction")]
    [SerializeField]
    private GameObject m_BtnDestroy;

    [Tooltip("Le bouton qui permet d'ajouter la carte en cours de construction au deck")]
    [SerializeField]
    private GameObject m_BtnAddToDeck;

    [Tooltip("Le bouton qui permet d'enlever la carte selectionner du deck")]
    [SerializeField]
    private GameObject m_BtnRemoveFromDeck;

    [Tooltip("Le bouton qui permet de sauvegarder le deck")]
    [SerializeField]
    private GameObject m_BtnSaveDeck;
    #endregion

    [Tooltip("Le transform de la position des cartes de base non selectionner")]
    [SerializeField]
    private Transform m_PoolCardPosition;

    [Tooltip("Le transform de la position de la carte de base")]
    [SerializeField]
    private Transform m_BaseCardPosition;

    [Tooltip("Le transform de la position dde la carte en construction")]
    [SerializeField]
    private Transform m_CurrentCardPosition;

    [Tooltip("Le parent qui contien et montre les cartes dans le deck")]
    [SerializeField]
    private GameObject m_DeckView;

    [SerializeField]
    private GameObject m_CardListRow;

    [Tooltip("TextField qui contien et montre le nom de la carte en cours de construction")]
    [SerializeField]
    private TMP_InputField m_TxtCardName;

    [SerializeField]
    private CardsData m_CardData;

    [Tooltip("Le préfab des cartes à instancier")]
    [SerializeField]
    private GameObject m_CardPrefab;

    [Tooltip("Liste des types de cartes à placer dans les decks")]
    [SerializeField]
    private List<CardClass> m_CardTypes = new List<CardClass>();

    public void ReturnToMenu()
    {
        LevelManager.Instance.ChangeLevel("MainMenu");
    }

    public void NextCard()
    {
        if (++m_BaseCardIndex >= m_BaseCards.Count)
        {
            m_BaseCardIndex = 0;
        }

        m_BaseCard.transform.position = m_PoolCardPosition.position;
        m_BaseCard = m_BaseCards[m_BaseCardIndex];
        m_BaseCard.transform.position = m_BaseCardPos.transform.position;
        if (m_BaseCard.GetComponent<Card>().CardType == CardType.Component)
        {
            m_BtnAdd.SetActive(true);
            m_BtnSubstract.SetActive(true);
        }
        else
        {
            m_BtnAdd.SetActive(false);
            m_BtnSubstract.SetActive(false);
        }
    }

    public void PreviousCard()
    {
        if (--m_BaseCardIndex < 0)
        {
            m_BaseCardIndex = m_BaseCards.Count - 1;
        }
        m_BaseCard.transform.position = m_PoolCardPosition.position;
        m_BaseCard = m_BaseCards[m_BaseCardIndex];
        m_BaseCard.transform.position = m_BaseCardPos.transform.position;
        if (m_BaseCard.GetComponent<Card>().CardType == CardType.Component)
        {
            m_BtnAdd.SetActive(true);
            m_BtnSubstract.SetActive(true);
        }
        else
        {
            m_BtnAdd.SetActive(false);
            m_BtnSubstract.SetActive(false);
        }
    }

    public void Replace()
    {
        if (m_CurrentCard)
        {
            DestroyCard();
        }
        m_CurrentCard = Instantiate<GameObject>(m_BaseCard,m_CurrentCardPos.transform.position,m_PoolCardPosition.transform.rotation,m_PoolCardPosition);
        m_CurrentCard.GetComponent<Card>().CopyCard(m_BaseCard.GetComponent<Card>());
    }

    public void AddToCard()
    {
        if (m_CurrentCard)
        {
            m_CurrentCard.GetComponent<Card>().AddComponentCard(m_BaseCard.GetComponent<Card>());
        }
    }

    public void SubstractToCard()
    {
        if (m_CurrentCard)
        {
            m_CurrentCard.GetComponent<Card>().SubstractComponentCard(m_BaseCard.GetComponent<Card>());
        }
    }

    public void InHand()
    {
        if (m_CurrentCard)
        {
            m_CurrentCard.GetComponent<Card>().ChangeState(Card.States.InHand);
        }   
    }

    public void InPlay()
    {
        if (m_CurrentCard)
        {
            m_CurrentCard.GetComponent<Card>().ChangeState(Card.States.InPlay);
        }
    }

    public void DestroyCard()
    {
        if (m_CurrentCard)
        {
            Destroy(m_CurrentCard);
        }
    }

    public void AddToDeck()
    {
        if (m_CurrentCard)
        {
            if (m_TxtCardName.text != "")
            {
                m_CurrentCard.name = m_TxtCardName.text;
            }
            m_CurrentDeck.Add(m_CurrentCard);
            GameObject m_temp = Instantiate<GameObject>(m_CardListRow, m_DeckView.transform);
            m_temp.GetComponent<CardRowScript>().AddCard(m_CurrentCard.GetComponent<Card>());
        }

    }
    
    private void AddCardToBases(int i_Counter)
    {
        GameObject deckTemp = null;
        deckTemp = Instantiate(m_CardPrefab, m_PoolCardPosition.position, m_PoolCardPosition.rotation, m_PoolCardPosition);

        deckTemp.AddComponent(Type.GetType(m_CardTypes[i_Counter].m_ScriptToAttach));
        deckTemp.GetComponent<Card>().m_CardName = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().ChangeState(Card.States.InHand);
        deckTemp.name = m_CardTypes[i_Counter].m_CardTypeName;
        deckTemp.GetComponent<Card>().InitCard(m_CardData);
        deckTemp.GetComponent<Renderer>().material = m_CardTypes[i_Counter].m_CardTypeSkin;
        m_BaseCards.Add(deckTemp);       
    }

    private void Start()
    {
        for (int i = 0; i < m_CardTypes.Count; i++)
        {
            AddCardToBases(i);
        }
        m_BaseCard = m_BaseCards[0];
        m_BaseCard.transform.position = m_BaseCardPos.transform.position;
        
        m_BtnAdd.SetActive(false);
        m_BtnSubstract.SetActive(false);
    }
}
