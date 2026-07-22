using System.Collections;
using UnityEngine;

public class MissionCompleteTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject missionCompletePanel;

    [Header("Settings")]
    public float displayTime = 3f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        activated = true;

        StartCoroutine(ShowMissionComplete());
    }

    IEnumerator ShowMissionComplete()
    {
        missionCompletePanel.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        missionCompletePanel.SetActive(false);

        // Nếu chỉ muốn kích hoạt một lần
        Destroy(gameObject);
    }
}