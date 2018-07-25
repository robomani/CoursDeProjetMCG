using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatars : MonoBehaviour
{
    private Transform m_Target;
    private Vector3 m_StartPos;

    private void Start ()
    {
        m_StartPos = gameObject.transform.position;
        if (gameObject.tag == "AvatarAI")
        {
            m_Target = GameObject.Find("Altar Player/Avatar").transform;
        }
        else
        {
            m_Target = GameObject.Find("Altar AI/Avatar").transform;
        }
	}
	
	private void Update ()
    {
        transform.LookAt(m_Target);
        transform.position = m_StartPos;
    }
}
