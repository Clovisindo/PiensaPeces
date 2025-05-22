using UnityEngine;

public class SpawnFoodButton : MonoBehaviour
{
    [SerializeField] private FoodSpawnerController spawner;

    public void OnClick()
    {
        spawner.SpawnFood();
    }
}
