using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject m_LoadingScreen;

    private static LevelManager m_Instance;
    public static LevelManager Instance
    {
        get { return m_Instance; }
    }

    public Action m_OnChangeScene;

    private bool m_WaitTimeReady;
    private bool m_LoadingReady;

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

    private void OnLoadingDone(Scene i_Scene, LoadSceneMode i_Mode)
    {
        //we remove the function to the action/event list
        SceneManager.sceneLoaded -= OnLoadingDone;

        if (m_WaitTimeReady)
        {
        }
        else
        {
            m_LoadingReady = true;
        }
    }

    public void ChangeLevel(string i_Scene, float m_Time = 0f)
    {
        m_WaitTimeReady = false;
        m_LoadingReady = false;
        if (m_OnChangeScene != null)
        {
            m_OnChangeScene();
        }
        SceneManager.LoadScene(i_Scene);
        StartCoroutine(WaitLoading(m_Time));
        SceneManager.sceneLoaded += OnLoadingDone;
    }

    private IEnumerator WaitLoading(float i_Time)
    {
        if (i_Time <= 0)
        {
            i_Time = 0.1f;
        }
        yield return new WaitForSeconds(i_Time);
        m_WaitTimeReady = true;
    }
}
