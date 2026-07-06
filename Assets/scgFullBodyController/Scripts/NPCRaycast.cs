using UnityEngine;

public class NPCRaycast : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(
                new Vector3(0.5f,0.5f));

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                NPCDialogue npc =
                    hit.collider.GetComponentInParent<NPCDialogue>();

                if (npc != null)
                {
                    npc.NPCInteract();
                }
            }
        }
    }
}
