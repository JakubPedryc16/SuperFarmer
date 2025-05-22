using System.Collections.Generic;

public static class ExchangeRates
{
    public static readonly Dictionary<AnimalType, int> ToRabbitValue = new()
    {
        { AnimalType.Rabbit, 1 },
        { AnimalType.Sheep, 6 },
        { AnimalType.Pig, 12 },
        { AnimalType.Cow, 36 },
        { AnimalType.Horse, 72 },
        { AnimalType.SmallDog, 6 },
        { AnimalType.BigDog, 36 }
    };
}