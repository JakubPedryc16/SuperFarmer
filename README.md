# Superfarmer (Unity Game)

**Superfarmer** is a digital adaptation of the classic Polish board game, developed in Unity using C#. The objective is to collect at least one of each key animal: rabbit, sheep, pig, cow, and horse.

## 🎮 Gameplay

Players take turns in the following order:

1. **Trading phase** – trade animals with the central farm stock according to the trading table.
2. **Dice rolling** – roll two animal dice and interpret the outcome.
3. **Breeding phase** – animals reproduce based on dice and current livestock.
4. **Turn end** – control passes to the next player.

The first player to collect at least one rabbit, one sheep, one pig, one cow, and one horse wins the game.

## 🔄 Trading Table

Players may trade animals with the farm according to the following exchange rates:

| Give              | Receive         |
|-------------------|-----------------|
| 6 Rabbits         | 1 Sheep         |
| 2 Sheep           | 1 Pig           |
| 3 Pigs            | 1 Cow           |
| 2 Cows            | 1 Horse         |
| 1 Sheep           | 6 Rabbits       |
| 1 Pig             | 2 Sheep         |
| 1 Cow             | 3 Pigs          |
| 1 Horse           | 2 Cows          |

Dogs can also be bought:

- 1 Small Dog = 1 Sheep  
- 1 Big Dog = 1 Cow

Dogs cannot be traded back.

## 🎲 Dice & Animals

There are **two 12-sided dice** (Dice A and Dice B), each with a different set of animals and one predator:

### Dice A Faces:

- Rabbit (x6)
- Sheep (x1)
- Pig (x1)
- Cow (x1)
- Small Dog (x1)
- Fox (x1)
- Horse (x1)

### Dice B Faces:

- Rabbit (x6)
- Sheep (x2)
- Pig (x1)
- Cow (x1)
- Big Dog (x1)
- Wolf (x1)

**Predators:**
- **Fox** – eats all rabbits unless you have a Small Dog (which is then lost).
- **Wolf** – eats all animals except horses, unless you have a Big Dog (which is then lost).

## ✅ Features

- 2–4 player support
- Dynamic animal trading system
- Animal breeding and predator events
- Interactive UI with clear animal inventories
- Game end detection

## 🛠️ Technologies

- Unity 2024+
- C#

## 🧩 Key Classes

- `GameMaster` – manages turn order and game state
- `Player` – stores individual player data and inventory
- `AnimalBreeder` – handles animal reproduction logic
- `AnimalLossHandler` – manages predator effects
- `GameUIManager` – updates player and game visuals
- `PlayerManager` – enables player number and names configuration

## 🐾 Animal Types (`AnimalType` enum)

- `Rabbit`
- `Sheep`
- `Pig`
- `Cow`
- `Horse`
- `SmallDog`
- `BigDog`
- `Fox`
- `Wolf`

## 🚀 How to Run

1. Clone the repository.
2. Open the project in Unity.
3. Load the main game scene.
4. Enter player names and start playing!

## 📦 In-Game Screenshots

You can view your current animals, with the number of each displayed below their icons. There's also a button to roll the dice.  
![Animals Overview](https://github.com/user-attachments/assets/4fe8494c-638a-49d0-8793-33f94ae22948)

Here, the dice rolled two rabbits. The player receives `(12 + 2) / 2 = 7` new rabbits, bringing the total to 19.  
![Dice Roll - Rabbits](https://github.com/user-attachments/assets/cb805336-391e-45f6-a924-4749960bcd49)

At the start of each turn, you can choose to either **trade** or **skip**.  
![Trade or Skip](https://github.com/user-attachments/assets/4ca72bc2-6ff2-4e0f-9dee-01d4aef72046)

### 🛒 Trading Screen

- **Bottom Left:** Select the animal you want to receive and the desired quantity.
- **Bottom Center:** See how many more animals (in rabbit-equivalents) you need to offer to complete the trade.
- **Top:** Your herd is displayed.
- **Below Each Animal:** `Return` and `Give` buttons let you manage which animals are part of the trade.
- The requirement updates dynamically. Once it reaches **0**, you can submit the trade.

![Trading Interface 1](https://github.com/user-attachments/assets/7b38df6f-1d2a-4aa4-a7cb-b5ce4dc380a5)  
![Trading Interface 2](https://github.com/user-attachments/assets/56af403c-956f-44b9-aca0-65f4e3019788)  
![Trading Interface 3](https://github.com/user-attachments/assets/900da90a-164a-46a1-a83a-bcd03ab01631)

