using PilloPlay.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    private List<PlayerObject> playerObjs;
    public int TotalPlayerObjects => playerObjs.Count;
    public List<PilloTarget> Targets;
    public List<GameObject> GameObjs;
    [SerializeField] private int maxPlayers;

    public static event Action OnPlayerCountChange;

    public static TargetHandler Instance;

    private void Awake()
    {
        playerObjs = new List<PlayerObject>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else 
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Pillo.onPilloConnected += AddPlayer;
        Pillo.onPilloDisconnected += RemovePlayer;
    }

    private void OnDisable()
    {
        Pillo.onPilloConnected -= AddPlayer;
        Pillo.onPilloDisconnected -= RemovePlayer;
    }

    private void AddPlayer(Pillo p) 
    {
        OnPlayerCountChange?.Invoke();
    }

    private void RemovePlayer(Pillo p) 
    {
        OnPlayerCountChange?.Invoke();
    }
}

[System.Serializable]
public class PlayerObject
{
    public Pillo Pillo;
    public GameObject Paddle;

    public PlayerObject(Pillo p, GameObject g)
    {
        Pillo = p;
        Paddle = g;
    }
}
