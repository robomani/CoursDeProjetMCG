using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct DescStruct
{
    public string m_Name;
    public string m_Desc;
}

public class SpecialPowersExplication : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro m_TextZone;

    [SerializeField]
    private List<DescStruct> m_Desc = new List<DescStruct>();

    public void Selecting(int i_Select)
    {
        Debug.Log(i_Select);
        m_TextZone.text = m_Desc[i_Select].m_Desc;
        
    }
}
