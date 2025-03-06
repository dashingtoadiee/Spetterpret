using System;
using PilloPlay.Core;
using UnityEngine;

public class SplatterSpawner : MonoBehaviour
{
    public static event Action<Pillo, GameObject> OnSplatterSpawned;

    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private bool[] justSpawned;

    void Start()
    {
        TargetHandler th = GetComponent<TargetHandler>();
        justSpawned = new bool[th.MaxPillos];   
    }

    void Update()
    {
        for (int i = 0; i < Pillo.pillos.Count; i++)
        {
            if (Pillo.pillos[i].pressure > 0f && !justSpawned[i])
            {
                justSpawned[i] = true;
                SpawnSplatter(i);
            }
            
            if (Pillo.pillos[i].pressure == 0f)
            {
                justSpawned[i] = false;
            }
        }
    }

    private void SpawnSplatter(int i)
    {
        float xPos = UnityEngine.Random.Range(-8, 8);
        float yPos = UnityEngine.Random.Range(-4, 4);
        Vector3 spawnPosition = new Vector3(xPos, yPos, 0); //TO DO - determine Z position based on most recent splatter's Z pos
        GameObject g = Instantiate(splatterPrefab, transform);
        g.transform.position = spawnPosition;
        OnSplatterSpawned?.Invoke(Pillo.pillos[i], g);
    }
}
