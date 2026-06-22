using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;

    private bool isOpen = false;

  private void Awake()
{
    anim = GetComponent<Animator>();

    Debug.Log("Animator Object = " + anim.gameObject.name);
}

    public void Interact()
    {
        Debug.Log("Door Interact");
        if (!isOpen)
        {
            Debug.Log("Open Trigger");
            anim.SetTrigger("DoorOpen");
            isOpen = true;
        }
        else
        {
            Debug.Log("Close Trigger");
            anim.SetTrigger("DoorClose");
            isOpen = false;
        }
    }
}
