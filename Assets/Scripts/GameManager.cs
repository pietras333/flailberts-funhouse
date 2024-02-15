using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    [Space]
    [Header("References")]
    [SerializeField] CameraLock cameraLock;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        SpawnPlayers();
        InitializeComponents();
    }

    void InitializeComponents()
    {
        if (!cameraLock || players.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogError("One or more references in the GameManager not found!", gameObject);
        }
        if (players.Count != spawnPoints.Count)
        {
            Debug.LogError("Number of players and spawn points in the GameManager don't match!", gameObject);
            return;
        }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject player = Instantiate(players[i]);
            player.transform.position = spawnPoints[i].transform.position;
        }
    }

    public void ResetPlayersPosition()
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject player = players[i];
            player.transform.position = spawnPoints[i].position;
        }
    }
}
