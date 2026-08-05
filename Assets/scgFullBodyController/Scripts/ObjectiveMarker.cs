using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public static ObjectiveMarker Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false); // Ban đầu ẩn marker
    }

    public void ShowMarker()
    {
        gameObject.SetActive(true);
    }

    public void HideMarker()
    {
        gameObject.SetActive(false);
    }
}
