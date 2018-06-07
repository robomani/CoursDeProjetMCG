using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    
    public GameObject m_TilePrefab;
    public GameObject m_AltarPrefab;
    public GameObject[] m_Tiles;
    public GameObject[] m_Altars;

    private int m_TilesBetweenAltars = 5;
    private int m_TilesAlongAltars = 3;
    private float m_DivideBetweenTiles = 0.5f;

    private void Start ()
    {
        if (m_TilePrefab && m_AltarPrefab)
        {
            
            GameObject[] tileTemp = new GameObject[m_TilesAlongAltars * m_TilesBetweenAltars];
            Vector3 nextPosition = transform.position;
            m_Altars[0] = Instantiate(m_AltarPrefab, nextPosition, transform.rotation, transform) as GameObject;
            m_Altars[0].transform.localScale = new Vector3((1.5f + m_DivideBetweenTiles )* (m_TilesAlongAltars + 2) - m_DivideBetweenTiles, 1f,2f);
            nextPosition.x = transform.position.x - ((m_TilesAlongAltars * 1.5f)/2 - m_DivideBetweenTiles/2);
            nextPosition.z = transform.position.z + 2f + m_DivideBetweenTiles;
            for (int i = 0; i < m_TilesAlongAltars; i++)
            {
                for (int j = 0; j < m_TilesBetweenAltars; j++)
                {
                    tileTemp[i * m_TilesBetweenAltars + j] = Instantiate(m_TilePrefab, nextPosition, transform.rotation, transform) as GameObject;
                    nextPosition.z += 2f + m_DivideBetweenTiles;
                }
                nextPosition.x += (1.5f + m_DivideBetweenTiles);
                nextPosition.z = transform.position.z + 2f + m_DivideBetweenTiles;
            }
            m_Tiles = tileTemp;
            nextPosition.z += (2f * (m_TilesBetweenAltars + 1)) + m_DivideBetweenTiles;
            nextPosition.x = transform.position.x;
            m_Altars[1] = Instantiate(m_AltarPrefab, nextPosition, transform.rotation, transform) as GameObject;
            m_Altars[1].transform.localScale = new Vector3((1.5f + m_DivideBetweenTiles) * (m_TilesAlongAltars + 2) - m_DivideBetweenTiles, 1f, -2f);
        }
	}
}
