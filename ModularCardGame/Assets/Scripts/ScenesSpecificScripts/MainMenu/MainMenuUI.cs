using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Slider m_RandomDeckSize;
    public TextMeshProUGUI m_SizeText;

    private bool m_RandomDeck;

    private void Start()
    {
        m_RandomDeckSize.gameObject.SetActive(false);
        m_SizeText.enabled = false;
        AudioManager.Instance.MenuStart();
    }

    public void StartGame()
    {
        LevelManager.Instance.ChangeLevel("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeckCreation()
    {
        LevelManager.Instance.ChangeLevel("DeckCreation");
    }

    public void ToggleAICheatMode()
    {
        GameManager.Instance.ToggleCheatMode();
    }

    public void ToggleRandomDeck()
    {
        GameManager.Instance.ToggleRandomDeck();
        if (m_RandomDeckSize != null)
        {
            m_RandomDeck = !m_RandomDeck;
            if (m_RandomDeck)
            {
                m_RandomDeckSize.gameObject.SetActive(true);
                m_SizeText.enabled = true;
            }
            else
            {
                m_RandomDeckSize.gameObject.SetActive(false);
                m_SizeText.enabled = false;  
            }
        }
    }

    public void SetRandomDeckSize()
    {
        m_SizeText.text = m_RandomDeckSize.value.ToString();
        GameManager.Instance.RandomDeckSize((int)m_RandomDeckSize.value);
    }
}
