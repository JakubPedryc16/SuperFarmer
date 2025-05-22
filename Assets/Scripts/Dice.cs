using UnityEngine;

public class Dice
{
    private AnimalType[] faces;

    public Dice(AnimalType[] faces)
    {
        this.faces = faces;
    }

    public AnimalType Roll()
    {
        int index = Random.Range(0, faces.Length);
        return faces[index];
    }
}