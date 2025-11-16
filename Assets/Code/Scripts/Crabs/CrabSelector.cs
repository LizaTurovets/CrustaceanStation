using UnityEngine;

public class CrabSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Sprite[] sprites;
    public GameObject ChooseCrab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Length)];
    }
}
