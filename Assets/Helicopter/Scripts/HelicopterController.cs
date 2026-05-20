using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour
{
    // Các reference được gán từ scene: âm thanh, input, Rigidbody, rotor và prefab vũ khí.
    public AudioSource HelicopterSound;
    public ControlPanel ControlPanel;
    public Rigidbody HelicopterModel;
    public HeliRotorController MainRotorController;
    public HeliRotorController SubRotorController;
    public GameObject BulletPrefab;
    public GameObject RocketPrefab;
    public Transform[] RocketSpawnPoints;

    // Thông số vật lý bay: lực rẽ, lực tiến, độ nghiêng và độ cao hiệu quả của lực nâng.
    public float TurnForce = 3f;
    public float ForwardForce = 10f;
    public float ForwardTiltForce = 20f;
    public float TurnTiltForce = 30f;
    public float EffectiveHeight = 100f;

    // Thông số đạn súng máy: tốc độ, thời gian tồn tại, tốc độ bắn và offset xoay mesh.
    public float BulletSpeed = 180f;
    public float BulletLifeTime = 3f;
    public float BulletFireCooldown = 0.06f;
    public float BulletSpawnForwardOffset = 2f;
    public Vector3 BulletRotationOffset = new Vector3(-90f, 0f, 0f);

    // Thông số tên lửa H70: tốc độ, thời gian tồn tại, cooldown và offset spawn.
    public float RocketSpeed = 120f;
    public float RocketLifeTime = 6f;
    public float RocketFireCooldown = 0.25f;
    public float RocketSpawnForwardOffset = 1.5f;
    public Vector3 RocketRotationOffset;

    // Object pool giúp tái sử dụng đạn/tên lửa thay vì Instantiate/Destroy liên tục.
    public int BulletPoolPreload = 40;
    public int RocketPoolPreload = 8;
    public bool ExpandPools = true;

    // Hệ số tính lực xoay và nghiêng khi máy bay đang tiến/lùi.
    public float turnTiltForcePercent = 1.5f;
    public float turnForcePercent = 1.3f;

    private float _engineForce;
    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            // EngineForce điều khiển tốc độ rotor, pitch âm thanh và text UI hiển thị lực động cơ.
            if (MainRotorController != null)
                MainRotorController.RotarSpeed = value * 80;
            if (SubRotorController != null)
                SubRotorController.RotarSpeed = value * 40;
            if (HelicopterSound != null)
                HelicopterSound.pitch = Mathf.Clamp(value / 40, 0, 1.2f);
            if (UIGameController.runtime != null && UIGameController.runtime.EngineForceView != null)
                UIGameController.runtime.EngineForceView.text = string.Format("Engine value [ {0} ] ", (int)value);

            _engineForce = value;
        }
    }

    private Vector2 hMove = Vector2.zero;
    private Vector2 hTilt = Vector2.zero;
    private float hTurn = 0f;
    private float nextBulletFireTime;
    private float nextRocketFireTime;
    private int rocketSpawnIndex;
    private Renderer[] rocketSpawnRenderers;
    private readonly Queue<GameObject> bulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> rocketPool = new Queue<GameObject>();
    private Transform poolRoot;
    public bool IsOnGround = true;

    // Đăng ký input từ ControlPanel và chuẩn bị cache/pool khi scene bắt đầu.
    void Start()
    {
        ControlPanel.KeyPressed += OnKeyPressed;
        ControlPanel.BulletFireHeld += FireBullet;
        ControlPanel.FirePressed += FireRocket;
        rocketSpawnRenderers = FindRocketRenderers();
        InitializeProjectilePools();
    }

    // Gỡ đăng ký event và xóa root pool khi object bị hủy để tránh reference cũ.
    private void OnDestroy()
    {
        if (ControlPanel == null) return;

        ControlPanel.KeyPressed -= OnKeyPressed;
        ControlPanel.BulletFireHeld -= FireBullet;
        ControlPanel.FirePressed -= FireRocket;

        if (poolRoot != null)
            Destroy(poolRoot.gameObject);
    }

    void Update()
    {
    }

    // FixedUpdate dùng cho Rigidbody: nâng máy bay, di chuyển và nghiêng model.
    void FixedUpdate()
    {
        LiftProcess();
        MoveProcess();
        TiltProcess();
    }

    // Xử lý lực tiến/lùi và torque xoay quanh trục Y dựa trên input đã lưu trong hMove.
    private void MoveProcess()
    {
        var turn = TurnForce * Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y));
        hTurn = Mathf.Lerp(hTurn, turn, Time.fixedDeltaTime * TurnForce);
        HelicopterModel.AddRelativeTorque(0f, hTurn * HelicopterModel.mass, 0f);
        HelicopterModel.AddRelativeForce(Vector3.forward * Mathf.Max(0f, hMove.y * ForwardForce * HelicopterModel.mass));
    }

    // Tạo lực nâng. Càng lên gần EffectiveHeight thì lực nâng càng giảm.
    private void LiftProcess()
    {
        var upForce = 1 - Mathf.Clamp(HelicopterModel.transform.position.y / EffectiveHeight, 0, 1);
        upForce = Mathf.Lerp(0f, EngineForce, upForce) * HelicopterModel.mass;
        HelicopterModel.AddRelativeForce(Vector3.up * upForce);
    }

    // Nghiêng thân máy bay theo hướng di chuyển để tạo cảm giác bay tự nhiên hơn.
    private void TiltProcess()
    {
        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);
        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);
        HelicopterModel.transform.localRotation = Quaternion.Euler(hTilt.y, HelicopterModel.transform.localEulerAngles.y, -hTilt.x);
    }

    // Bắn một tên lửa H70 khi chuột phải được nhấn. Lấy object từ rocket pool.
    private void FireRocket()
    {
        if (RocketPrefab == null || Time.time < nextRocketFireTime)
            return;

        nextRocketFireTime = Time.time + RocketFireCooldown;

        Vector3 direction = HelicopterModel.transform.forward;
        Vector3 spawnPosition = GetRocketSpawnPosition(direction);
        Quaternion spawnRotation = Quaternion.LookRotation(direction, HelicopterModel.transform.up) * Quaternion.Euler(RocketRotationOffset);
        GameObject rocket = GetProjectileFromPool(rocketPool, RocketPrefab, "Rocket");
        if (rocket == null)
            return;

        rocket.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        rocket.SetActive(true);

        H70RocketProjectile projectile = rocket.GetComponent<H70RocketProjectile>();
        if (projectile == null)
            projectile = rocket.AddComponent<H70RocketProjectile>();

        projectile.Init(direction, RocketSpeed, RocketLifeTime, HelicopterModel.transform, ReturnRocketToPool);
        IgnoreRocketCollisionWithHelicopter(rocket);
    }

    // Bắn đạn liên tục khi giữ chuột trái. Lấy object từ bullet pool và cho bay thẳng.
    private void FireBullet()
    {
        if (BulletPrefab == null || Time.time < nextBulletFireTime)
            return;

        nextBulletFireTime = Time.time + BulletFireCooldown;

        Vector3 direction = HelicopterModel.transform.forward;
        Vector3 spawnPosition = GetRocketSpawnPosition(direction) + direction * BulletSpawnForwardOffset;
        Quaternion spawnRotation = Quaternion.LookRotation(direction, HelicopterModel.transform.up) * Quaternion.Euler(BulletRotationOffset);
        GameObject bullet = GetProjectileFromPool(bulletPool, BulletPrefab, "Bullet");
        if (bullet == null)
            return;

        bullet.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        bullet.SetActive(true);

        H70RocketProjectile projectile = bullet.GetComponent<H70RocketProjectile>();
        if (projectile == null)
            projectile = bullet.AddComponent<H70RocketProjectile>();

        projectile.Init(direction, BulletSpeed, BulletLifeTime, HelicopterModel.transform, ReturnBulletToPool);
        IgnoreRocketCollisionWithHelicopter(bullet);
    }

    // Tạo object cha cho pool và preload sẵn một số đạn/tên lửa.
    private void InitializeProjectilePools()
    {
        poolRoot = new GameObject("HelicopterProjectilePool").transform;

        PreloadPool(bulletPool, BulletPrefab, BulletPoolPreload, "Bullet");
        PreloadPool(rocketPool, RocketPrefab, RocketPoolPreload, "Rocket");
    }

    // Tạo trước nhiều projectile inactive để khi bắn không phải Instantiate liên tục.
    private void PreloadPool(Queue<GameObject> pool, GameObject prefab, int count, string objectName)
    {
        if (prefab == null)
            return;

        for (int i = 0; i < count; i++)
            pool.Enqueue(CreatePooledProjectile(prefab, objectName));
    }

    // Lấy projectile inactive từ pool. Nếu pool rỗng và ExpandPools bật thì tạo thêm.
    private GameObject GetProjectileFromPool(Queue<GameObject> pool, GameObject prefab, string objectName)
    {
        while (pool.Count > 0)
        {
            GameObject projectile = pool.Dequeue();
            if (projectile != null)
                return projectile;
        }

        return ExpandPools && prefab != null ? CreatePooledProjectile(prefab, objectName) : null;
    }

    // Tạo một projectile mới cho pool và đảm bảo có H70RocketProjectile để điều khiển bay/trả pool.
    private GameObject CreatePooledProjectile(GameObject prefab, string objectName)
    {
        GameObject projectile = Instantiate(prefab, poolRoot);
        projectile.name = objectName;

        if (projectile.GetComponent<H70RocketProjectile>() == null)
            projectile.AddComponent<H70RocketProjectile>();

        projectile.SetActive(false);
        return projectile;
    }

    // Callback khi đạn hết lifetime/va chạm: trả đạn về bullet pool.
    private void ReturnBulletToPool(H70RocketProjectile projectile)
    {
        ReturnProjectileToPool(projectile, bulletPool);
    }

    // Callback khi tên lửa hết lifetime/va chạm: trả tên lửa về rocket pool.
    private void ReturnRocketToPool(H70RocketProjectile projectile)
    {
        ReturnProjectileToPool(projectile, rocketPool);
    }

    // Tắt projectile, đưa về pool root và enqueue để lần sau tái sử dụng.
    private void ReturnProjectileToPool(H70RocketProjectile projectile, Queue<GameObject> pool)
    {
        if (projectile == null)
            return;

        GameObject projectileObject = projectile.gameObject;
        projectileObject.SetActive(false);
        projectileObject.transform.SetParent(poolRoot, false);
        pool.Enqueue(projectileObject);
    }

    // Tìm vị trí bắn. Ưu tiên spawn point gắn tay, sau đó renderer Hydra trên model, cuối cùng là thân máy bay.
    private Vector3 GetRocketSpawnPosition(Vector3 direction)
    {
        Transform spawnPoint = GetNextManualSpawnPoint();
        if (spawnPoint != null)
            return spawnPoint.position + direction * RocketSpawnForwardOffset;

        Renderer rocketRenderer = GetNextRocketRenderer();
        if (rocketRenderer != null)
            return rocketRenderer.bounds.center + direction * RocketSpawnForwardOffset;

        return HelicopterModel.transform.position + direction * RocketSpawnForwardOffset;
    }

    // Lấy spawn point được gán thủ công trong Inspector, xoay vòng nếu có nhiều điểm bắn.
    private Transform GetNextManualSpawnPoint()
    {
        if (RocketSpawnPoints == null || RocketSpawnPoints.Length == 0)
            return null;

        for (int i = 0; i < RocketSpawnPoints.Length; i++)
        {
            Transform spawnPoint = RocketSpawnPoints[rocketSpawnIndex % RocketSpawnPoints.Length];
            rocketSpawnIndex++;
            if (spawnPoint != null)
                return spawnPoint;
        }

        return null;
    }

    // Lấy renderer của rocket/Hydra trên model để dùng làm vị trí bắn từ hình tên lửa.
    private Renderer GetNextRocketRenderer()
    {
        if (rocketSpawnRenderers == null || rocketSpawnRenderers.Length == 0)
            return null;

        Renderer renderer = rocketSpawnRenderers[rocketSpawnIndex % rocketSpawnRenderers.Length];
        rocketSpawnIndex++;
        return renderer;
    }

    // Cache các renderer có material/name giống rocket để không phải tìm mỗi lần bắn.
    private Renderer[] FindRocketRenderers()
    {
        if (HelicopterModel == null)
            return new Renderer[0];

        var renderers = new List<Renderer>();
        foreach (Renderer renderer in HelicopterModel.GetComponentsInChildren<Renderer>(true))
        {
            if (IsRocketRenderer(renderer))
                renderers.Add(renderer);
        }

        return renderers.ToArray();
    }

    // Kiểm tra renderer có phải cụm tên lửa trên model dựa vào tên material/object.
    private bool IsRocketRenderer(Renderer renderer)
    {
        foreach (Material material in renderer.sharedMaterials)
        {
            if (material == null)
                continue;

            string materialName = material.name.ToLowerInvariant();
            if (materialName.Contains("hydra") || materialName.Contains("h70") || materialName.Contains("rocket"))
                return true;
        }

        string objectName = renderer.name.ToLowerInvariant();
        return objectName.Contains("hydra") || objectName.Contains("h70") || objectName.Contains("rocket");
    }

    // Bỏ qua va chạm giữa projectile mới bắn ra và collider của máy bay.
    private void IgnoreRocketCollisionWithHelicopter(GameObject rocket)
    {
        Collider[] rocketColliders = rocket.GetComponentsInChildren<Collider>();
        Collider[] helicopterColliders = HelicopterModel.GetComponentsInChildren<Collider>();

        foreach (Collider rocketCollider in rocketColliders)
        {
            foreach (Collider helicopterCollider in helicopterColliders)
                Physics.IgnoreCollision(rocketCollider, helicopterCollider);
        }
    }

    // Nhận input từ ControlPanel và cập nhật hMove/EngineForce/torque theo phím đang bấm.
    private void OnKeyPressed(PressedKeyCode[] obj)
    {
        float tempY = 0;
        float tempX = 0;

        // Tự ổn định trục tiến/lùi khi không giữ phím.
        if (hMove.y > 0)
            tempY = -Time.fixedDeltaTime;
        else if (hMove.y < 0)
            tempY = Time.fixedDeltaTime;

        // Tự ổn định trục trái/phải khi không giữ phím.
        if (hMove.x > 0)
            tempX = -Time.fixedDeltaTime;
        else if (hMove.x < 0)
            tempX = Time.fixedDeltaTime;


        foreach (var pressedKeyCode in obj)
        {
            switch (pressedKeyCode)
            {
                case PressedKeyCode.SpeedUpPressed:

                    EngineForce += 0.1f;
                    break;
                case PressedKeyCode.SpeedDownPressed:

                    EngineForce -= 0.12f;
                    if (EngineForce < 0) EngineForce = 0;
                    break;

                case PressedKeyCode.ForwardPressed:

                    if (IsOnGround) break;
                    tempY = Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.BackPressed:

                    if (IsOnGround) break;
                    tempY = -Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.LeftPressed:

                    if (IsOnGround) break;
                    tempX = -Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.RightPressed:

                    if (IsOnGround) break;
                    tempX = Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.TurnRightPressed:
                    {
                        if (IsOnGround) break;
                        var force = (turnForcePercent - Mathf.Abs(hMove.y)) * HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;
                case PressedKeyCode.TurnLeftPressed:
                    {
                        if (IsOnGround) break;

                        var force = -(turnForcePercent - Mathf.Abs(hMove.y)) * HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;

            }
        }

        hMove.x += tempX;
        hMove.x = Mathf.Clamp(hMove.x, -1, 1);

        hMove.y += tempY;
        hMove.y = Mathf.Clamp(hMove.y, -1, 1);

    }

    // Chạm đất thì khóa điều khiển tiến/lùi/trái/phải cho đến khi cất cánh.
    private void OnCollisionEnter()
    {
        IsOnGround = true;
    }

    // Rời khỏi mặt đất thì cho phép điều khiển đầy đủ.
    private void OnCollisionExit()
    {
        IsOnGround = false;
    }
}
