using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Range(2, 4)]
    public int numberOfPlayers = 2;

    public List<string> playerNames = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerNames(List<string> names)
    {
        playerNames.Clear();
        for (int i = 0; i < numberOfPlayers && i < names.Count; i++)
        {
            playerNames.Add(names[i]);
        }
    }

    public string GetPlayerName(int index)
    {
        if (index >= 0 && index < playerNames.Count)
            return playerNames[index];
        return "Unknown";
    }
}