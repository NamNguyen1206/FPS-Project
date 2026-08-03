using System.Collections;
using UnityEngine;

public class MystoryBox : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private Animator keyAnimator;

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";

    [Header("Timing")]
    [SerializeField] private float openDelay = 0.8f;
    [SerializeField] private float keyOutDelay = 0.5f;
    [SerializeField] private float closeDelay = 0.8f;

    //private bool playerInRange;
    private bool isBusy;
    private bool isOpened;

    private void Reset()
    {
        chestAnimator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        if (chestAnimator == null)
        {
            chestAnimator = GetComponentInChildren<Animator>();
        }
    }

    private Coroutine sequenceRoutine;

    public void Interact()
    {
        Debug.Log("MystoryBox Interact");

        if (isBusy || isOpened)
        {
            return;
        }

        if (sequenceRoutine != null)
        {
            StopCoroutine(sequenceRoutine);
        }

        sequenceRoutine = StartCoroutine(OpenKeyCloseSequence());
    }

    private IEnumerator OpenKeyCloseSequence()
    {
        isBusy = true;

        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("OpenChest");
        }

        yield return new WaitForSeconds(openDelay);

        if (keyAnimator != null)
        {
            keyAnimator.SetTrigger("KeyOut");
        }

        yield return new WaitForSeconds(keyOutDelay);

        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("CloseChest");
        }

        yield return new WaitForSeconds(closeDelay);

        isOpened = true;
        isBusy = false;
        sequenceRoutine = null;
    }

    private void OnDisable()
    {
        if (sequenceRoutine != null)
        {
            StopCoroutine(sequenceRoutine);
            sequenceRoutine = null;
        }

        isBusy = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //playerInRange = true;
            Debug.Log("Player entered box trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //playerInRange = false;
            Debug.Log("Player exited box trigger");
        }
    }
}
