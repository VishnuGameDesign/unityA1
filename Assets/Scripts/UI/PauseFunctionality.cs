using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseFunctionality : MonoBehaviour
{
    [SerializeField] private string _gameplayActionMapName = "Player";
    [SerializeField] private string _pauseActionMapName = "UI";

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        SwitchActionMap(_pauseActionMapName);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        SwitchActionMap(_gameplayActionMapName);
    }

    public void SwitchActionMap(string mapName)
    {
        // assume singleplayer game, find PlayerInput and change map
        PlayerInput playerInput = FindFirstObjectByType<PlayerInput>();
        playerInput?.SwitchCurrentActionMap(mapName);
    }
}