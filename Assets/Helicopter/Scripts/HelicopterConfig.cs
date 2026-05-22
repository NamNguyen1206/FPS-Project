using UnityEngine;

[CreateAssetMenu(fileName = "HelicopterConfig", menuName = "Helicopter/Helicopter Config")]
public class HelicopterConfig : ScriptableObject
{
    [Header("Flight")]
    public float TurnForce = 3f;
    public float ForwardForce = 10f;
    public float ForwardTiltForce = 20f;
    public float TurnTiltForce = 30f;
    public float EffectiveHeight = 100f;
    public float TurnTiltForcePercent = 1.5f;
    public float TurnForcePercent = 1.3f;

    [Header("Bullet")]
    public float BulletSpeed = 180f;
    public float BulletLifeTime = 3f;
    public float BulletFireCooldown = 0.06f;
    public float BulletSpawnForwardOffset = 2f;
    public Vector3 BulletRotationOffset = new Vector3(-90f, 0f, 0f);

    [Header("Rocket")]
    public float RocketSpeed = 120f;
    public float RocketLifeTime = 6f;
    public float RocketFireCooldown = 0.25f;
    public float RocketSpawnForwardOffset = 1.5f;
    public Vector3 RocketRotationOffset;

    [Header("Pooling")]
    public int BulletPoolPreload = 40;
    public int RocketPoolPreload = 8;
    public bool ExpandPools = true;
}
