using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    public Animator m_EnemyAvatar;
    public Animator m_PlayerAvatar;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayerHurt()
    {
        m_PlayerAvatar.SetTrigger("Hurt");
    }

    public void PlayerCast()
    {
        m_PlayerAvatar.SetTrigger("Cast");
    }

    public void EnemyHurt()
    {
        m_EnemyAvatar.SetTrigger("Hurt");
    }

    public void EnemyCast()
    {
        m_EnemyAvatar.SetTrigger("Cast");
    }
}
