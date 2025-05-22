using System.Collections.Generic;

public class Player
{
    public string Name { get; private set; }
    private Dictionary<AnimalType, int> Animals { get; set; }

    public Player(string name)
    {
        Name = name;
        Animals = new Dictionary<AnimalType, int>();

        foreach (AnimalType animal in System.Enum.GetValues(typeof(AnimalType)))
        {
            Animals[animal] = 0;
        }
    }

    public void AddAnimal(AnimalType type, int amount = 1)
    {
        Animals[type] += amount;
    }

    public void RemoveAnimal(AnimalType type, int amount = 1)
    {
        Animals[type] = System.Math.Max(0, Animals[type] - amount);
    }

    public int GetAnimalCount(AnimalType type)
    {
        return Animals[type];
    }
    
    public Dictionary<AnimalType, int> GetAllAnimals()
    {
        return new Dictionary<AnimalType, int>(Animals);
    }
    
    public bool HasAnimal(AnimalType type, int minimumAmount = 1)
    {
        return Animals[type] >= minimumAmount;
    }


}