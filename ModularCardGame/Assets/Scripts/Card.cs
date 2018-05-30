﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : MonoBehaviour
{
    public string m_CardName;
    public int m_Position;

    public Card(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    public void Illuminate()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.green);
    }

    public void UnIlluminate()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.white);
    }
}
