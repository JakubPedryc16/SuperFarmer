using System.Collections.Generic;

public class AnimalLossHandler
{
    private Dictionary<AnimalType, int> mainStock;

    public AnimalLossHandler(Dictionary<AnimalType, int> mainStock)
    {
        this.mainStock = mainStock;
    }

    public void HandleAnimalLoss(Player player, AnimalType roll1, AnimalType roll2)
    {
        var rolledAnimals = new HashSet<AnimalType> { roll1, roll2 };
        
        if (rolledAnimals.Contains(AnimalType.Fox))
        {
            if (player.HasAnimal(AnimalType.SmallDog))
            {
                player.RemoveAnimal(AnimalType.SmallDog, 1);
                mainStock[AnimalType.SmallDog]++;
            }
            else
            {
                int rabbits = player.GetAnimalCount(AnimalType.Rabbit);
                if (rabbits > 1)
                {
                    int toRemove = rabbits - 1;
                    player.RemoveAnimal(AnimalType.Rabbit, toRemove);
                    mainStock[AnimalType.Rabbit] += toRemove;
                }
            }
        }
        
        if (rolledAnimals.Contains(AnimalType.Wolf))
        {
            if (player.HasAnimal(AnimalType.BigDog))
            {
                player.RemoveAnimal(AnimalType.BigDog, 1);
                mainStock[AnimalType.BigDog]++;
            }
            else
            {
                foreach (AnimalType type in new[] { AnimalType.Sheep, AnimalType.Pig, AnimalType.Cow })
                {
                    int count = player.GetAnimalCount(type);
                    if (count > 0)
                    {
                        player.RemoveAnimal(type, count);
                        mainStock[type] += count;
                    }
                }
            }
        }
    }
}