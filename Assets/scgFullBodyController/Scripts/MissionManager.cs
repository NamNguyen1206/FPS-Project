using System.Collections;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Mission UI")]
    [SerializeField] private GameObject missionPanel;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private float displayTime = 3f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        missionPanel.SetActive(false);
        //ShowMission("MISSION UPDATED", "Find the Exit");
    }

    public void ShowMission(string title, string description)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMissionRoutine(title, description));
    }

    private IEnumerator ShowMissionRoutine(string title, string description)
    {
        missionPanel.SetActive(true);

        titleText.text = title;
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTime);

        missionPanel.SetActive(false);

        currentRoutine = null;
    }
}