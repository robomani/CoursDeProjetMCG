using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<EPoolType, List<GameObject>> m_Pool = new Dictionary<EPoolType, List<GameObject>>();
    private Vector3 m_PoolPos = new Vector3(-100f, -100f, -100f);

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        CreatePool();
    }

    public GameObject GetFromPool(EPoolType a_Type, Vector3 a_Pos)
    {
        if (m_Pool.ContainsKey(a_Type))
        {
            if (m_Pool[a_Type].Count > 0)
            {
                GameObject go = m_Pool[a_Type][0];
                m_Pool[a_Type].Remove(go);
                go.transform.position = a_Pos;
                go.SetActive(true);
                return go;
            }
            Debug.LogError("No More: " + a_Type + " in pool. PLEASE ADD MORE");
        }    
        return null;
    }

    public void ReturnToPool(EPoolType a_Type, GameObject a_Object)
    {
        if (m_Pool.ContainsKey(a_Type))
        {
            m_Pool[a_Type].Add(a_Object);

        }
        else
        {
            m_Pool.Add(a_Type, new List<GameObject>() { a_Object });
        }
        a_Object.SetActive(false);
        a_Object.transform.position = m_PoolPos;
        a_Object.transform.SetParent(transform);
    }

    public void CreatePool()
    {
        List<PoolableObjects> poolableObjects = new List<PoolableObjects>(GetComponentsInChildren<PoolableObjects>());
        PoolableObjects po;
        GameObject go;

        for (int i = 0; i < poolableObjects.Count; i++)
        {
            po = poolableObjects[i];
            for (int x = 0; x < po.m_Quantity; x++)
            {
                go = Instantiate(po.gameObject);
                ReturnToPool(po.m_PoolType, go);
            }
            ReturnToPool(po.m_PoolType, po.gameObject);
        }
    }
}
