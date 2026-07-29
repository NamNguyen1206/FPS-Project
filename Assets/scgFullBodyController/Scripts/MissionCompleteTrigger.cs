using UnityEngine;

public class MissionCompleteTrigger : MonoBehaviour
{
    [SerializeField] private string missionTitle = "MISSION COMPLETE";
    [SerializeField] private string missionDescription = "Escape Successful";

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        activated = true;

        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ShowMission(
                missionTitle,
                missionDescription
            );
        }

        Destroy(gameObject);
    }
}