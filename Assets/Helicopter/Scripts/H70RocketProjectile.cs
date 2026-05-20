using System;
using UnityEngine;

public class H70RocketProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float destroyTime;
    private Transform ignoredRoot;
    private Action<H70RocketProjectile> finished;

    public void Init(Vector3 moveDirection, float moveSpeed, float lifeTime, Transform rootToIgnore, Action<H70RocketProjectile> onFinished = null)
    {
        direction = moveDirection.normalized;
        speed = moveSpeed;
        ignoredRoot = rootToIgnore;
        destroyTime = Time.time + lifeTime;
        finished = onFinished;

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>(true))
        {
            particle.Clear(true);
            particle.Play(true);
        }
    }

    private void Awake()
    {
        if (direction == Vector3.zero)
            direction = transform.forward;
    }

    private void Update()
    {
        if (Time.time >= destroyTime)
        {
            Finish();
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = currentPosition + direction * speed * Time.deltaTime;
        Vector3 step = nextPosition - currentPosition;

        RaycastHit hit;
        if (Physics.Raycast(currentPosition, direction, out hit, step.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (ignoredRoot == null || !hit.transform.IsChildOf(ignoredRoot))
            {
                transform.position = hit.point;
                Finish();
                return;
            }
        }

        transform.position = nextPosition;
    }

    private void Finish()
    {
        if (finished != null)
        {
            finished(this);
            return;
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        finished = null;
        speed = 0f;

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>(true))
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
