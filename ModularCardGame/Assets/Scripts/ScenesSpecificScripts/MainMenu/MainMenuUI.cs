using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.ChangeLevel("Game");
    }
}
