using UnityEngine;

public class DiceRoller
{
    private Dice dice1;
    private Dice dice2;

    public DiceRoller()
    {
        AnimalType[] dice1Faces = new AnimalType[] {
            AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit,
            AnimalType.Sheep, AnimalType.Sheep, AnimalType.Sheep,
            AnimalType.Pig,
            AnimalType.Cow,
            AnimalType.Wolf
        };

        AnimalType[] dice2Faces = new AnimalType[] {
            AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit, AnimalType.Rabbit,
            AnimalType.Sheep, AnimalType.Sheep,
            AnimalType.Pig, AnimalType.Pig,
            AnimalType.Horse,
            AnimalType.Fox
        };

        dice1 = new Dice(dice1Faces);
        dice2 = new Dice(dice2Faces);
    }

    public (AnimalType, AnimalType) Roll()
    {
        AnimalType result1 = dice1.Roll();
        AnimalType result2 = dice2.Roll();
        return (result1, result2);
    }
}