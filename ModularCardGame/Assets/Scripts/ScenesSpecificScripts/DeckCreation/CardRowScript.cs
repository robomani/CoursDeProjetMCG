using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardRowScript : MonoBehaviour
{
    public int m_NumberOfCardCopy = 1;
    public Card m_Card;

    [SerializeField]
    private TextMeshProUGUI m_LblCardName;
    [SerializeField]
    private TextMeshProUGUI m_LblNbrOfCard;
    [SerializeField]
    private Toggle m_Toggle;


    public void AddOneCard()
    {
        m_NumberOfCardCopy++;
        m_LblNbrOfCard.text = m_NumberOfCardCopy.ToString();
    }

    public void RemoveOneCard()
    {
        m_NumberOfCardCopy--;
        if (m_NumberOfCardCopy < 0)
        {
            m_NumberOfCardCopy = 0;
        }
        m_LblNbrOfCard.text = m_NumberOfCardCopy.ToString();
    }

    public void AddCard(Card i_card)
    {
        m_LblCardName.text = i_card.name;
        m_Card = i_card;
    }

    public bool IsSelected()
    {
        return m_Toggle.isOn;
    }


}
