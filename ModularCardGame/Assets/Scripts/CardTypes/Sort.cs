using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : Card
{
    

    public Sort(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
        
    }

    protected override void Awake()
    {
        base.Awake();
        m_Portee = 2;
    }
}
