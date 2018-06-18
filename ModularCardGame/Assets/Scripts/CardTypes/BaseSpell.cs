using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : Card
{
    

    public BaseSpell(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
        
    }

    protected override void Awake()
    {
        base.Awake();
        m_CastRange = 2;
    }
}
