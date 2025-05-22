using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0;

    private Dictionary<AnimalType, int> mainStock = new Dictionary<AnimalType, int>();
    private DiceRoller diceRoller;
    public GameUIManager gameUIManager;
    public ExchangeUIController exchangeUIController;
    
    private TurnPhase currentPhase;
    private AnimalBreeder animalBreeder;
    private AnimalLossHandler animalLossHandler;



    public enum TurnPhase
    {
        TradeDecision,
        AwaitingDiceRoll,
        AwaitingEndTurn
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        mainStock[AnimalType.Rabbit] = 60;
        mainStock[AnimalType.Sheep] = 24;
        mainStock[AnimalType.Pig] = 20;
        mainStock[AnimalType.Cow] = 12;
        mainStock[AnimalType.Horse] = 6;
        mainStock[AnimalType.SmallDog] = 4;
        mainStock[AnimalType.BigDog] = 2;

        diceRoller = new DiceRoller();
        animalBreeder = new AnimalBreeder(mainStock);
        animalLossHandler = new AnimalLossHandler(mainStock);
        
        foreach (string playerName in PlayerManager.Instance.playerNames)
        {
            Player player = new Player(playerName);
            player.AddAnimal(AnimalType.Rabbit, 1);
            players.Add(player);
        }

        StartPlayerTurn();
        
        gameUIManager.HideDiceResults();
        exchangeUIController.HideExchangeUI();
        exchangeUIController.OnExchangeUIHidden += OnExchangeUIHidden;
    }

    public void NextTurn()
    {
        Player currentPlayer = players[currentPlayerIndex];

        if (CheckWinCondition(currentPlayer))
        {
            Debug.Log($"Gracz {currentPlayer.Name} wygrał grę!");
            gameUIManager.ShowWinMessage(currentPlayer.Name); // Zakładam, że masz taką metodę w UI
            // Opcjonalnie: zablokuj dalszą rozgrywkę
            return;
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        StartPlayerTurn();
    }


    public void RollDice()
    {
        var (roll1, roll2) = diceRoller.Roll();
        Debug.Log($"Rzut kostkami: {roll1} i {roll2}");

        gameUIManager.ShowDiceResults(roll1, roll2);

        Player currentPlayer = players[currentPlayerIndex];
        animalLossHandler.HandleAnimalLoss(currentPlayer, roll1, roll2);

        animalBreeder.BreedAnimals(currentPlayer, roll1, roll2);

        UpdateUI();
    }

    private void UpdateUI()
    {
        Player currentPlayer = players[currentPlayerIndex];
        gameUIManager.UpdateCurrentPlayer(currentPlayer.Name);
        gameUIManager.UpdateAllAnimalCounts(currentPlayer.GetAllAnimals());
    }

    public void StartPlayerTurn()
    {
        UpdateUI();
        currentPhase = TurnPhase.TradeDecision;
        gameUIManager.ShowTradeOptions();
        gameUIManager.HideDiceResults();
    }

    public void OnTradeChosen()
    {
        exchangeUIController.ShowExchangeUI(GetCurrentPlayer());
        currentPhase = TurnPhase.AwaitingDiceRoll;
        gameUIManager.ShowRollDiceOption();
    }

    public void OnExchangeUIHidden()
    {
        UpdateUI();
    }


    public void OnDontTradeChosen()
    {
        currentPhase = TurnPhase.AwaitingDiceRoll;
        gameUIManager.ShowRollDiceOption();
    }

    public void OnRollDiceChosen()
    {
        currentPhase = TurnPhase.AwaitingEndTurn;
        RollDice();
        gameUIManager.ShowEndTurnOption();
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
    
    public Dictionary<AnimalType, int> GetMainStock()
    {
        return mainStock;
    }
    private bool CheckWinCondition(Player player)
    {

        foreach (AnimalType animal in new AnimalType[] { AnimalType.Rabbit, AnimalType.Sheep, AnimalType.Pig, AnimalType.Cow, AnimalType.Horse })
        {
            if (player.GetAnimalCount(animal) < 1)
                return false;
        }
        return true;
    }


}
