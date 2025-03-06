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
    public int MaxPillos;

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

    void Update()
    {

    }

    private void OnEnable()
    {
        Pillo.onPilloConnected += AddPlayer;
        Pillo.onPilloDisconnected += RemovePlayer;

        SplatterSpawner.OnSplatterSpawned += AddSplatterToPillo;
    }

    private void OnDisable()
    {
        Pillo.onPilloConnected -= AddPlayer;
        Pillo.onPilloDisconnected -= RemovePlayer;

        SplatterSpawner.OnSplatterSpawned -= AddSplatterToPillo;
    }

    private void AddPlayer(Pillo p)
    {
        if (playerObjs.Count < MaxPillos) {
            playerObjs.Add(new PlayerObject(p));
        }
    }

    private void RemovePlayer(Pillo p)
    {
        PlayerObject toBeRemoved = null;

        foreach (PlayerObject player in playerObjs)
        {
            if (player.Pillo == p)
            {
                toBeRemoved = player;
                break;
            }
        }
        
        playerObjs.Remove(toBeRemoved);
    }

    private void AddSplatterToPillo(Pillo p, GameObject g) 
    {
        foreach (PlayerObject player in playerObjs)
        {
            if (player.Pillo == p)
            {
                g.GetComponent<SplatterTransformer>().Setup(p);
            }
        }
    }
}

[System.Serializable]
public class PlayerObject
{
    public Pillo Pillo;

    public PlayerObject(Pillo p)
    {
        Pillo = p;
    }
}
