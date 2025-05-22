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
        
        Dictionary<AnimalType, int> totalAnimals = new Dictionary<AnimalType, int>(player.GetAllAnimals());
  
        if (!totalAnimals.ContainsKey(roll1)) totalAnimals[roll1] = 0;
        if (!totalAnimals.ContainsKey(roll2)) totalAnimals[roll2] = 0;

        totalAnimals[roll1]++;
        totalAnimals[roll2]++;

        foreach (var kvp in totalAnimals)
        {
            int pairs = kvp.Value / 2;
            if (pairs > 0 && mainStock.ContainsKey(kvp.Key))
            {
                int available = mainStock[kvp.Key];
                int toAdd = Mathf.Min(pairs, available);
                if (toAdd > 0)
                {
                    if (!additions.ContainsKey(kvp.Key)) additions[kvp.Key] = 0;
                    additions[kvp.Key] += toAdd;
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