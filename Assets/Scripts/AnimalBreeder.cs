using System.Collections.Generic;
using UnityEngine;

public class AnimalBreeder
{
    private Dictionary<AnimalType, int> mainStock;

    public AnimalBreeder(Dictionary<AnimalType, int> mainStock)
    {
        this.mainStock = mainStock;
    }

    public void BreedAnimals(Player player, AnimalType roll1, AnimalType roll2)
    {
        Dictionary<AnimalType, int> additions = new Dictionary<AnimalType, int>();
        
        Dictionary<AnimalType, int> rolledTypes = new Dictionary<AnimalType, int>();
        rolledTypes[roll1] = rolledTypes.ContainsKey(roll1) ? rolledTypes[roll1] + 1 : 1;
        rolledTypes[roll2] = rolledTypes.ContainsKey(roll2) ? rolledTypes[roll2] + 1 : 1;

        foreach (var kvp in rolledTypes)
        {
            AnimalType type = kvp.Key;
            int rolledAmount = kvp.Value;
            int total = player.GetAnimalCount(type) + rolledAmount;

            int pairs = total / 2;

            if (pairs > 0 && mainStock.TryGetValue(type, out int available))
            {
                int toAdd = Mathf.Min(pairs, available);
                if (toAdd > 0)
                {
                    additions[type] = toAdd;
                }
            }
        }

        foreach (var kvp in additions)
        {
            player.AddAnimal(kvp.Key, kvp.Value);
            mainStock[kvp.Key] -= kvp.Value;
        }
    }


}