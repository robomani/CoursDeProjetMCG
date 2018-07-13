using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Card
{
    public Tower(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    protected override void Awake()
    {
        base.Awake();
        m_CharacterPrefab = (GameObject)Resources.Load("CharTower", typeof(GameObject));
        
    }
}
