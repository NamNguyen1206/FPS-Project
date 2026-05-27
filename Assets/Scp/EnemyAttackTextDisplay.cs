using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackTextDisplay : MonoBehaviour
{
    [SerializeField] private Text uiText;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private float showDuration = 0.75f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (uiText == null)
        {
            uiText = GetComponentInChildren<Text>(true);
        }

        if (tmpText == null)
        {
            tmpText = GetComponentInChildren<TMP_Text>(true);
        }

        SetVisible(false);
    }

    public void ShowAttackText(string message)
    {
        SetText(message);
        SetVisible(true);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        SetVisible(false);
        hideRoutine = null;
    }

    private void SetText(string message)
    {
        if (uiText != null)
        {
            uiText.text = message;
        }

        if (tmpText != null)
        {
            tmpText.text = message;
        }
    }

    private void SetVisible(bool visible)
    {
        if (uiText != null)
        {
            uiText.enabled = visible;
        }

        if (tmpText != null)
        {
            tmpText.enabled = visible;
        }
    }
}
