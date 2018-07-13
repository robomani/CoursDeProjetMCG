using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Card
{
    public Wall(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    protected override void Awake()
    {
        base.Awake();
        m_CharacterPrefab = (GameObject)Resources.Load("CharWall", typeof(GameObject));
    }
}
