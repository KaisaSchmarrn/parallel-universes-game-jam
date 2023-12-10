using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UniverseType_SO naturalUniverse;

    public event Action<GameResetEventArgs> OnGameReset;
    public event Action<RuneCollectedEventArgs> OnRuneCollected;
    public event Action<UniverseSwitchedEventArgs> OnUniverseSwitched;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        OnGameReset?.Invoke(new GameResetEventArgs());
        HandleUniverseSwitched(null, naturalUniverse);
    }

    public void HandleRuneCollected(UniverseType_SO universeType)
    {
        OnRuneCollected?.Invoke(new RuneCollectedEventArgs(universeType));
    }

    public void HandleUniverseSwitched(UniverseType_SO previousUniverseType, UniverseType_SO nextUniverseType)
    {
        OnUniverseSwitched?.Invoke(new UniverseSwitchedEventArgs(previousUniverseType, nextUniverseType));
    }
}

public struct GameResetEventArgs
{

}

public struct RuneCollectedEventArgs
{
    public UniverseType_SO universeType;

    public RuneCollectedEventArgs(UniverseType_SO universeType)
    {
        this.universeType = universeType;
    }
}

public struct UniverseSwitchedEventArgs
{
    public UniverseType_SO previousUniverseType;
    public UniverseType_SO nextUniverseType;

    public UniverseSwitchedEventArgs(UniverseType_SO previousUniverseType, UniverseType_SO nextUniverseType)
    {
        this.previousUniverseType = previousUniverseType;
        this.nextUniverseType = nextUniverseType;
    }
}
