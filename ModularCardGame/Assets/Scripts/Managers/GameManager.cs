using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    public Action m_OnChangeScene;
    public GameObject m_OptionScreen;
    private bool m_AICheatMode;
    private bool m_RandomDeck;
    private int m_RandomDeckSize = 0;

    public void TriggerOptionScreen(bool? i_State = null)
    {
        if (i_State == null)
        {
            m_OptionScreen.SetActive(!m_OptionScreen.activeSelf);
        }
        else
        {
            m_OptionScreen.SetActive((bool)i_State);
        }
    }

    public void RestartGame()
    {
        LevelManager.Instance.ChangeLevel("MainMenu", 0f);
        LevelManager.Instance.ChangeLevel("Game", 0f);
        TriggerOptionScreen(false);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlayMenuSelectSound();
        LevelManager.Instance.ChangeLevel("MainMenu", 0f);
        TriggerOptionScreen(false);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayMenuSelectSound();
        Application.Quit();
    }

    public int m_AICreaturesInPlay { get; private set; }
    public void AddOrRemoveCreatureToAI(bool i_Add)
    {
        if (i_Add)
        {
            m_AICreaturesInPlay++;
        }
        else
        {
            m_AICreaturesInPlay--;
        }
    }

    public int m_AIBuildingsInPlay { get; private set; }
    public void AddOrRemoveBuildingToAI(bool i_Add)
    {
        if (i_Add)
        {
            m_AIBuildingsInPlay++;
        }
        else
        {
            m_AIBuildingsInPlay--;
        }
    }

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool ToggleCheatMode()
    {
        m_AICheatMode = !m_AICheatMode;
        return m_AICheatMode;
    }

    public bool ToggleRandomDeck()
    {
        m_RandomDeck = !m_RandomDeck;
        return m_RandomDeck;
    }

    public int RandomDeckSize(int i_Size = -1)
    {
        if (i_Size > 14)
        {
            m_RandomDeckSize = i_Size;
        }
        return m_RandomDeckSize;
    }
}
