using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text currentPlayerText;
    
    public Button primaryButton;
    public Button secondaryButton;
    public TMP_Text primaryButtonText;
    public TMP_Text secondaryButtonText;

    public GameObject winMessagePanel;
    public TMP_Text winMessageText;
    
    [System.Serializable]
    public struct AnimalTextEntry
    {
        public AnimalType animalType;
        public TMP_Text countText;
    }

    public List<AnimalTextEntry> animalTexts = new List<AnimalTextEntry>();

    private Dictionary<AnimalType, TMP_Text> animalTextDict;
    
    public Image diceResultImage1;
    public Image diceResultImage2;
    public Image diceResultImage1Shadow;
    public Image diceResultImage2Shadow;
    
    [System.Serializable]
    public struct AnimalSpriteEntry
    {
        public AnimalType animalType;
        public Sprite sprite;
    }

    public List<AnimalSpriteEntry> animalSprites;

    private Dictionary<AnimalType, Sprite> animalSpriteDict;

    private void Awake()
    {
        animalTextDict = new Dictionary<AnimalType, TMP_Text>();
        foreach (var entry in animalTexts)
        {
            animalTextDict[entry.animalType] = entry.countText;
        }
        
        animalSpriteDict = new Dictionary<AnimalType, Sprite>();
        foreach (var entry in animalSprites)
        {
            animalSpriteDict[entry.animalType] = entry.sprite;
        }
    }

    public void UpdateCurrentPlayer(string playerName)
    {
        currentPlayerText.text = playerName;
    }

    public void UpdateAnimalCount(AnimalType type, int count)
    {
        if (animalTextDict.TryGetValue(type, out TMP_Text text))
        {
            text.text = count.ToString();
        }
    }

    public void UpdateAllAnimalCounts(Dictionary<AnimalType, int> animals)
    {
        foreach (var pair in animals)
        {
            UpdateAnimalCount(pair.Key, pair.Value);
        }
    }
    
    public void ShowTradeOptions()
    {
        primaryButton.gameObject.SetActive(true);
        secondaryButton.gameObject.SetActive(true);

        primaryButtonText.text = "Trade";
        secondaryButtonText.text = "Don't Trade";

        primaryButton.onClick.RemoveAllListeners();
        secondaryButton.onClick.RemoveAllListeners();

        primaryButton.onClick.AddListener(() => GameMaster.Instance.OnTradeChosen());
        secondaryButton.onClick.AddListener(() => GameMaster.Instance.OnDontTradeChosen());
    }

    public void ShowRollDiceOption()
    {
        primaryButton.gameObject.SetActive(true);
        secondaryButton.gameObject.SetActive(false);

        primaryButtonText.text = "Roll the Dice";

        primaryButton.onClick.RemoveAllListeners();
        primaryButton.onClick.AddListener(() => GameMaster.Instance.OnRollDiceChosen());
    }

    public void ShowEndTurnOption()
    {
        primaryButton.gameObject.SetActive(true);
        secondaryButton.gameObject.SetActive(false);

        primaryButtonText.text = "End Turn";

        primaryButton.onClick.RemoveAllListeners();
        primaryButton.onClick.AddListener(() => GameMaster.Instance.NextTurn());
    }
    
    public void ShowDiceResults(AnimalType type1, AnimalType type2)
    {
        if (animalSpriteDict.TryGetValue(type1, out var sprite1))
            diceResultImage1.sprite = sprite1;

        if (animalSpriteDict.TryGetValue(type2, out var sprite2))
            diceResultImage2.sprite = sprite2;
        
        diceResultImage1Shadow.gameObject.SetActive(true);
        diceResultImage2Shadow.gameObject.SetActive(true);
    }

    public void HideDiceResults()
    {
        diceResultImage1Shadow.gameObject.SetActive(false);
        diceResultImage2Shadow.gameObject.SetActive(false);
    }
    
    public void ShowWinMessage(string playerName)
    {
        if (winMessageText != null && winMessagePanel != null)
        {
            winMessageText.text = $"Player {playerName} won the game!";
            winMessagePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win message UI elements not set!");
        }
    }

    public void HideWinMessage()
    {
        if (winMessagePanel != null)
        {
            winMessagePanel.SetActive(false);
        }
    }

}