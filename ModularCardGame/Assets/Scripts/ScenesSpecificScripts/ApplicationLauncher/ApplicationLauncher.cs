using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationLauncher : MonoBehaviour
{
    private void Start()
    {
        LevelManager.Instance.ChangeLevel("MainMenu", 0f);
    }
}
