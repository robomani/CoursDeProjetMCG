using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : DontDestroyOnLoad
{
    //Pour le singleton satic veut dire que l'on peut y acceder de partout
    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    [SerializeField]
    private bool m_Menu = true;

    [SerializeField]
    private AudioSource m_AudioSourceMusic;

    [SerializeField]
    private AudioClip[] m_MenuMusicList;

    [SerializeField]
    private AudioClip[] m_GameMusicList;

    private int m_GameSong = 1;
    private int m_MenuSong = 0;
    private float m_TimeToSwich = 0;


    protected override void Awake()
    {
        //On s'assure qu'il n'y en ait qu'un seul
        if (m_Instance != null)
        {
            //on detruit le deuxième
            Destroy(gameObject);
        }
        else
        {
            //on déclare le premier à notre static
            m_Instance = this;
        }
        base.Awake();

    }

    private void Update()
    {
        
        m_TimeToSwich -= Time.deltaTime;
        if (m_TimeToSwich <= 0f && m_AudioSourceMusic.clip != null)
        {
            SwichGameMusic();
        }
        
    }

    private IEnumerator SwichMusicRoutine(float i_Duration, AudioClip i_NextClip)
    {
        if (i_Duration <= 0)
        {
            Debug.LogError("Duration is <= 0f, so you should Never use this fonction, tell Alexandre ML if this happens");
            yield break;
        }
        while (m_AudioSourceMusic.volume > 0f)
        {
            m_AudioSourceMusic.volume -= Time.deltaTime / i_Duration;
            yield return null;
        }
        m_TimeToSwich = i_NextClip.length;
        m_AudioSourceMusic.clip = i_NextClip;
        m_AudioSourceMusic.Play();
        

        while(m_AudioSourceMusic.volume < 0.5f)
        {
            m_AudioSourceMusic.volume += Time.deltaTime / i_Duration;
            yield return null;
        }
    }

    public void GameStart()
    {
        m_Menu = false;
        m_TimeToSwich = m_GameMusicList[m_GameSong].length;
        m_AudioSourceMusic.clip = m_GameMusicList[m_GameSong];
        m_AudioSourceMusic.Play();
    }

    public void MenuStart()
    {
        m_Menu = true;
        m_TimeToSwich = m_MenuMusicList[m_MenuSong].length;
        m_AudioSourceMusic.clip = m_MenuMusicList[m_MenuSong];
        m_AudioSourceMusic.Play();
    }

    public void SwichGameMusic(float i_Duration = 1f)
    {
        AudioClip nextClip;
        if (m_Menu)
        {
            m_MenuSong++;
            if (m_MenuSong >= m_MenuMusicList.Length)
            {
                m_MenuSong = 0;
            }
            nextClip = m_MenuMusicList[m_MenuSong];
        }
        else
        {
            m_GameSong++;
            if (m_GameSong >= m_GameMusicList.Length)
            {
                m_GameSong = 0;
            }
            nextClip = m_GameMusicList[m_GameSong];
        }
        StartCoroutine(SwichMusicRoutine(i_Duration, nextClip));
    }

    public void PlayPingSound()
    {

    }
    /*
    public void PlaySFX(AudioClip i_Clip, Vector3 i_Position)
    {
        SFXAudio audio = Instantiate(m_SFXAudioPrefab, i_Position, Quaternion.identity);
        audio.Setup(i_Clip);
        audio.Play();
    }
    */
}


