using UnityEngine;

public class Archer : Card
{
    public Archer(string i_Name, int i_Position)
    {
        m_CardName = i_Name;
        m_Position = i_Position;
    }

    protected override void Awake()
    {
        base.Awake();
        m_CharacterPrefab = (GameObject)Resources.Load("CharArcher", typeof(GameObject));
    }
}
