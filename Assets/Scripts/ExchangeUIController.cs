using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeUIController : MonoBehaviour
{
    private Dictionary<AnimalType, int> desiredAnimals = new();
    private Dictionary<AnimalType, int> offeredAnimals = new();

    private Player player;

    public TMP_Text valueDifferenceText;
    public TMP_Text exchangeStatusText;
    public TMP_Text errorMessageText;
    public TMP_InputField countInputField;
    public Button confirmExchangeButton;
    public GameObject exchangeUIController;
    public List<AnimalTextPair> playerAnimalTextsList = new();
    public List<AnimalTextPair> offerAnimalTextsList = new();

    private Dictionary<AnimalType, TMP_Text> playerAnimalTexts;
    private Dictionary<AnimalType, TMP_Text> offerAnimalTexts;

    private AnimalType currentSelectedAnimal = AnimalType.Rabbit;

    private int currentCount = 0;

    private int targetValue = 0;

    void Awake()
    {
        playerAnimalTexts = playerAnimalTextsList.ToDictionary(pair => pair.animalType, pair => pair.text);
        offerAnimalTexts = offerAnimalTextsList.ToDictionary(pair => pair.animalType, pair => pair.text);
        
        if (countInputField != null)
        {
            countInputField.text = "1";
            currentCount = 1;
            countInputField.onValueChanged.AddListener(OnCountChanged);
        }
    }

    public void SetExchangeTarget(AnimalType type, int count)
    {
        ClearErrorMessage();
        desiredAnimals.Clear();
        desiredAnimals[type] = count;
        targetValue = count * ExchangeRates.ToRabbitValue[type];
        UpdateSummary();
        UpdateAnimalTexts();
    }

    public void AddToOffer(AnimalType type)
    {
        ClearErrorMessage();

        int playerCount = player.GetAnimalCount(type);
        int currentlyOffered = GetOfferedAmount(type);

        if (type == AnimalType.Rabbit)
        {
            if (playerCount - currentlyOffered <= 1)
            {
                return;
            }
        }
        else
        {
            if (playerCount <= currentlyOffered)
            {
                return;
            }
        }

        offeredAnimals[type] = currentlyOffered + 1;
        UpdateSummary();
        UpdateAnimalTexts();
    }

    public void RemoveFromOffer(AnimalType type)
    {
        ClearErrorMessage();

        if (offeredAnimals.TryGetValue(type, out int count) && count > 0)
        {
            offeredAnimals[type] = count - 1;
            if (offeredAnimals[type] == 0)
                offeredAnimals.Remove(type);
            UpdateSummary();
            UpdateAnimalTexts();
        }
    }

    public void AddToOfferByName(string animalName)
    {
        if (System.Enum.TryParse<AnimalType>(animalName, out var animalType))
        {
            AddToOffer(animalType);
        }
        else
        {
            Debug.LogWarning($"Niepoprawna nazwa zwierzęcia: {animalName}");
        }
    }

    public void RemoveFromOfferByName(string animalName)
    {
        if (System.Enum.TryParse<AnimalType>(animalName, out var animalType))
        {
            RemoveFromOffer(animalType);
        }
        else
        {
            Debug.LogWarning($"Niepoprawna nazwa zwierzęcia: {animalName}");
        }
    }

    private int GetOfferedAmount(AnimalType type) =>
        offeredAnimals.TryGetValue(type, out var val) ? val : 0;

    private void UpdateSummary()
    {
        int offeredValue = offeredAnimals
            .Sum(pair => pair.Value * ExchangeRates.ToRabbitValue[pair.Key]);

        int diff = offeredValue - targetValue;

        valueDifferenceText.text = $"{diff}";

        if (diff == 0)
        {
            exchangeStatusText.text = "Perfect match!";
        }
        else if (diff > 0)
        {
            exchangeStatusText.text = "Too much (in rabbits)";
        }
        else
        {
            exchangeStatusText.text = "Too little (in rabbits)";
        }

        confirmExchangeButton.interactable = (diff == 0);
    }

    public void OnAnimalSelected(int index)
    {
        if (System.Enum.IsDefined(typeof(AnimalType), index))
        {
            currentSelectedAnimal = (AnimalType)index;
            UpdateTarget();
        }
        else
        {
            Debug.LogWarning("Niepoprawny index dla AnimalType");
        }
    }

    public void OnCountChanged(string countStr)
    {
        if (int.TryParse(countStr, out int count))
        {
            currentCount = count > 0 ? count : 1;  
        }
        else
        {
            currentCount = 1; 
        }

        if (player != null)
        {
            UpdateTarget();
        }

        if (countInputField != null && countInputField.text != currentCount.ToString())
        {
            countInputField.text = currentCount.ToString();
        }
    }

    private void ResetCountInput()
    {
        currentCount = 1;
        if (countInputField != null)
        {
            countInputField.text = "1";
        }
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        if (currentCount > 0)
        {
            SetExchangeTarget(currentSelectedAnimal, currentCount);
        }
        else
        {
            desiredAnimals.Clear();
            targetValue = 0;
            UpdateSummary();
            UpdateAnimalTexts();
        }
    }

    public void ConfirmExchange()
    {
        ClearErrorMessage();

        if (player == null)
        {
            ShowErrorMessage("Player is null, cannot confirm exchange.");
            return;
        }

        var mainStock = GameMaster.Instance.GetMainStock();

        foreach (var pair in desiredAnimals)
        {
            if (!mainStock.TryGetValue(pair.Key, out int availableCount) || availableCount < pair.Value)
            {
                ShowErrorMessage($"Not enough animals of type {pair.Key} in the main stock.");
                return;
            }
        }

        foreach (var pair in offeredAnimals)
        {
            player.RemoveAnimal(pair.Key, pair.Value);
            if (mainStock.ContainsKey(pair.Key))
                mainStock[pair.Key] += pair.Value;
            else
                mainStock[pair.Key] = pair.Value;
        }

        foreach (var pair in desiredAnimals)
        {
            mainStock[pair.Key] -= pair.Value;
            player.AddAnimal(pair.Key, pair.Value);
        }

        offeredAnimals.Clear();
        desiredAnimals.Clear();
        UpdateSummary();
        UpdateAnimalTexts();
        ClearErrorMessage();
        HideExchangeUI();
    }

    private void UpdateAnimalTexts()
    {
        RefreshAnimalTextDictionaries();

        foreach (var type in System.Enum.GetValues(typeof(AnimalType)).Cast<AnimalType>())
        {
            int playerCount = player.GetAnimalCount(type);
            int offerCount = offeredAnimals.TryGetValue(type, out var count) ? count : 0;

            if (playerAnimalTexts != null && playerAnimalTexts.TryGetValue(type, out var playerText))
                playerText.text = playerCount.ToString();

            if (offerAnimalTexts != null && offerAnimalTexts.TryGetValue(type, out var offerText))
                offerText.text = offerCount.ToString();
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
        UpdateAnimalTexts();
    }

    private void RefreshAnimalTextDictionaries()
    {
        playerAnimalTexts = playerAnimalTextsList.ToDictionary(pair => pair.animalType, pair => pair.text);
        offerAnimalTexts = offerAnimalTextsList.ToDictionary(pair => pair.animalType, pair => pair.text);
    }

    public void ShowExchangeUI(Player player)
    {
        if (exchangeUIController != null)
        {
            SetPlayer(player);
            ResetCountInput(); 
            ClearErrorMessage();
            UpdateSummary();
            UpdateAnimalTexts();
            exchangeUIController.gameObject.SetActive(true);
        }
    }


    public void HideExchangeUI()
    {
        if (exchangeUIController != null)
        {
            offeredAnimals.Clear();
            desiredAnimals.Clear();
            exchangeUIController.gameObject.SetActive(false);
            OnExchangeUIHidden?.Invoke();
        }
    }


    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
            errorMessageText.gameObject.SetActive(true);
        }
    }

    private void ClearErrorMessage()
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = "";
            errorMessageText.gameObject.SetActive(false);
        }
    }

    public void CancelExchange()
    {

        HideExchangeUI();
    }
    
    public delegate void ExchangeUIHiddenHandler();
    public event ExchangeUIHiddenHandler OnExchangeUIHidden;

}
