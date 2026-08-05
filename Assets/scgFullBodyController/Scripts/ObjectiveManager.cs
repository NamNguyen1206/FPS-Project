using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    public GameObject npcMarker;

    private void Awake()
    {
        Instance = this;

        npcMarker.SetActive(false);
    }
}
