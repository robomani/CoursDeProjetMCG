using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFoward : MonoBehaviour
{
    public Transform m_Target;
    private Vector3 m_StartPos;

    private void Start()
    {
        if (GetComponentInParent<Card>().m_Owner == Card.Players.AI)
        {
            m_Target = GameObject.Find("Altar Player/Avatar").transform;
        }
        else
        {
            m_Target = GameObject.Find("Altar AI/Avatar").transform;
        }
        
        m_StartPos = transform.position;
    }

    private void Update()
    {
        LookAtTarget();
        transform.position = m_StartPos;
    }

    public void LookAtTarget()
    {
        transform.LookAt(m_Target);
    }

    public void UpdatePos(Vector3 i_Pos)
    {
        m_StartPos = i_Pos;
    }
}
