using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace scgFullBodyController
{
    public class HealthController : MonoBehaviour
    {
        [Header("Basics")]
        public float health = 100f;

        private float maxHealth;

        public float armor;

        private float maxArmor = 100f;

        [Header("Death")]
        public GameObject ragdoll;
        public bool dontSpawnRagdoll;
        public float deadTime = 10f;

        [Header("Character Type")]
        public bool isAiOrDummy = false;

        private bool isDead = false;
        private bool meleeDeath = false;

        private GameObject tempdoll;

        [Header("Sound")]
        public bool playNoiseOnHurt;
        public float percentageToPlay;
        public AudioClip hurtNoise;

        [Header("Regen")]
        public bool regen;
        public float timeBeforeRegen;
        private float origTimeBeforeRegen;
        public float regenSpeed;
        private bool alreadyRegenning;

        // =========================
        // NPC
        // =========================

        private NPCFollow npcFollow;

        // =========================
        // UNITY
        // =========================

        private void Awake()
        {
            // Nếu HealthController nằm trên NPC
            npcFollow = GetComponent<NPCFollow>();
        }

        private void Start()
        {
            // Lưu HP ban đầu
            maxHealth = health;

            origTimeBeforeRegen = timeBeforeRegen;

            armor = Mathf.Clamp(armor, 0f, maxArmor);
        }

        private void Update()
        {
            if (isDead)
                return;

            // Nếu HP <= 0
            if (health <= 0)
            {
                if (!meleeDeath)
                {
                    Die();
                }

                return;
            }

            // =========================
            // PLAYER HUD
            // =========================

            if (!isAiOrDummy && npcFollow == null)
            {
                GameObject ui = GameObject.FindGameObjectWithTag("hud");

                hudController hud =
                    ui != null
                        ? ui.GetComponent<hudController>()
                        : null;

                if (hud != null)
                {
                    hud.SetHealth(health, maxHealth);
                    hud.SetArmor(armor);
                }
            }

            // =========================
            // STOP REGEN
            // =========================

            if (health >= maxHealth && regen && alreadyRegenning)
            {
                alreadyRegenning = false;

                StopCoroutine(nameof(regenHealth));
            }
        }

        // ============================================================
        // DAMAGE
        // ============================================================

        public void Damage(float damage)
        {
            if (isDead)
                return;

            if (damage <= 0)
                return;

            // ========================================================
            // NPC
            // ========================================================

            if (npcFollow != null)
            {
                health -= damage;

                Debug.Log(
                    $"NPC [{name}] took {damage} damage. " +
                    $"HP: {health}/{maxHealth}"
                );

                // NPC chết
                if (health <= 0)
                {
                    health = 0;

                    Die();
                }

                RestartRegen();

                return;
            }

            // ========================================================
            // PLAYER
            // ========================================================

            if (!isAiOrDummy)
            {
                if (armor > 0)
                {
                    armor = Mathf.Max(armor - 20f, 0f);

                    Debug.Log(
                        $"{name} armor: {armor}"
                    );
                }
                else
                {
                    health -= damage;
                }

                PlayHurtSound();

                RestartRegen();

                return;
            }

            // ========================================================
            // AI / DUMMY
            // ========================================================

            health -= damage;

            Animator aiAnimator = GetComponent<Animator>();

            if (aiAnimator != null)
            {
                aiAnimator.SetTrigger("hit");
            }

            AiController aiController =
                GetComponent<AiController>();

            if (aiController != null)
            {
                aiController.overrideAttack = true;
            }

            PlayHurtSound();

            RestartRegen();
        }

        // ============================================================
        // REGEN
        // ============================================================

        private void RestartRegen()
        {
            if (!regen)
                return;

            timeBeforeRegen = origTimeBeforeRegen;

            StopCoroutine(nameof(regenHealth));

            CancelInvoke();

            alreadyRegenning = true;

            Invoke(
                nameof(regenEnumeratorStart),
                timeBeforeRegen
            );
        }

        private void regenEnumeratorStart()
        {
            if (isDead)
                return;

            StartCoroutine(nameof(regenHealth));
        }

        private IEnumerator regenHealth()
        {
            while (!isDead && health < maxHealth)
            {
                health++;

                if (health > maxHealth)
                    health = maxHealth;

                yield return new WaitForSeconds(regenSpeed);
            }

            alreadyRegenning = false;
        }

        // ============================================================
        // ARMOR
        // ============================================================

        public void AddArmor(float amount)
        {
            armor = Mathf.Min(
                armor + amount,
                maxArmor
            );

            Debug.Log(
                $"{name} armor: {armor}"
            );
        }

        // ============================================================
        // HURT SOUND
        // ============================================================

        private void PlayHurtSound()
        {
            if (!playNoiseOnHurt)
                return;

            if (hurtNoise == null)
                return;

            if (Random.value >= percentageToPlay)
                return;

            AudioSource audioSource =
                GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.PlayOneShot(hurtNoise);
            }
        }

        // ============================================================
        // DIE
        // ============================================================

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;

            Debug.Log(
                $"{name} died."
            );

            // ========================================================
            // NPC
            // ========================================================

            if (npcFollow != null)
            {
                npcFollow.DieFromHealthController();

                return;
            }

            // ========================================================
            // AI / PLAYER
            // ========================================================

            if (!dontSpawnRagdoll)
            {
                if (ragdoll == null)
                {
                    Debug.LogWarning(
                        $"{name}: Ragdoll is not assigned!"
                    );

                    Destroy(gameObject, deadTime);

                    return;
                }

                tempdoll = Instantiate(
                    ragdoll,
                    transform.position,
                    transform.rotation
                );

                ragdollCamera ragdollCam =
                    tempdoll.GetComponent<ragdollCamera>();

                if (ragdollCam != null)
                {
                    ragdollCam.isAi = isAiOrDummy;
                }

                Destroy(gameObject);

                if (isAiOrDummy)
                {
                    Destroy(
                        tempdoll,
                        deadTime
                    );
                }
            }
            else if (isAiOrDummy)
            {
                DisableAI();

                Destroy(
                    gameObject,
                    deadTime
                );
            }
        }

        // ============================================================
        // DISABLE AI
        // ============================================================

        private void DisableAI()
        {
            Animator animator =
                GetComponent<Animator>();

            if (animator != null)
                animator.enabled = false;

            AiController aiController =
                GetComponent<AiController>();

            if (aiController != null)
                aiController.enabled = false;

            enabled = false;

            SimpleFootsteps footsteps =
                GetComponent<SimpleFootsteps>();

            if (footsteps != null)
                footsteps.enabled = false;

            NavMeshAgent agent =
                GetComponent<NavMeshAgent>();

            if (agent != null)
                agent.enabled = false;

            OffsetRotation offsetRotation =
                GetComponentInChildren<OffsetRotation>();

            if (offsetRotation != null)
                offsetRotation.enabled = false;

            AiGunController aiGun =
                GetComponentInChildren<AiGunController>();

            if (aiGun != null)
                aiGun.enabled = false;

            Adjuster adjuster =
                GetComponentInChildren<Adjuster>();

            if (adjuster != null)
                adjuster.enabled = false;
        }

        // ============================================================
        // KICK DEATH
        // ============================================================

        public void DamageByKick(
            Vector3 pos,
            float kickForce,
            int kickDamage)
        {
            if (isDead)
                return;

            health -= kickDamage;

            if (health <= 0)
            {
                health = 0;

                meleeDeath = true;

                if (ragdoll == null)
                {
                    Die();
                    return;
                }

                tempdoll = Instantiate(
                    ragdoll,
                    transform.position,
                    transform.rotation
                );

                ragdollCamera ragdollCam =
                    tempdoll.GetComponent<ragdollCamera>();

                if (ragdollCam != null)
                {
                    ragdollCam.isAi = isAiOrDummy;
                }

                Destroy(gameObject);

                foreach (
                    Rigidbody rb
                    in tempdoll.GetComponentsInChildren<Rigidbody>()
                )
                {
                    rb.AddForce(
                        pos * kickForce
                    );
                }
            }
            else
            {
                Animator animator =
                    GetComponent<Animator>();

                if (animator != null)
                {
                    animator.SetTrigger("hit");
                }
            }
        }

        // ============================================================
        // PUBLIC INFORMATION
        // ============================================================

        public bool IsDead()
        {
            return isDead;
        }

        public float GetHealth()
        {
            return health;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}