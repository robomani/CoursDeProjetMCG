using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    protected virtual void Awake()
    {
        //Notre objet ne se detruira pas grace à cette fonction
        DontDestroyOnLoad(gameObject);
    }
}
