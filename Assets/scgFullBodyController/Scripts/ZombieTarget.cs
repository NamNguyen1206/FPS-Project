using UnityEngine;

public class ZombieTarget : MonoBehaviour
{
    public float detectRadius = 18f;
    public string npcTag = "NPC";
    public string playerTag = "Player";

    [HideInInspector] public Transform currentTarget;

    private void Update()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        currentTarget = FindNearestTargetWithTag(npcTag);

        if (currentTarget == null)
        {
            currentTarget = FindNearestTargetWithTag(playerTag);
        }
    }

    private Transform FindNearestTargetWithTag(string targetTag)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);
        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Transform targetTransform = GetTaggedTransform(hit.transform, targetTag);

            if (targetTransform == null)
                continue;

            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = targetTransform;
            }
        }

        return nearestTarget;
    }

    private Transform GetTaggedTransform(Transform candidate, string targetTag)
    {
        if (candidate.CompareTag(targetTag))
            return candidate;

        if (candidate.root != candidate && candidate.root.CompareTag(targetTag))
            return candidate.root;

        return null;
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }
}
