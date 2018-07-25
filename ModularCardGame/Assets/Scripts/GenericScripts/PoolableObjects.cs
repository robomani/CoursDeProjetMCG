using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjects : MonoBehaviour
{
    public EPoolType m_PoolType;
    public int m_Quantity;

    private void OnEnable()
    {
        if (PoolManager.Instance != null)
        {
            //PoolManager.Instance.m_OnChangeScene += RetrurnToPool;
        }

    }

    private void OnDisable()
    {
        if (PoolManager.Instance != null)
        {
            //LevelManager.Instance.
            //PoolManager.Instance.m_OnChangeScene -= RetrurnToPool;
        }

    }

    public void StartTimer(float a_Time)
    {
        StartCoroutine(WaitAndReturn(a_Time));
    }

    private IEnumerator WaitAndReturn(float a_Time)
    {
        yield return new WaitForSeconds(a_Time);
        RetrurnToPool();
    }

    public void RetrurnToPool()
    {
        StopAllCoroutines();
        PoolManager.Instance.ReturnToPool(m_PoolType, gameObject);
    }

    private void OnchangeScene()
    {

    }
}
