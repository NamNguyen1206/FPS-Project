using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class registerHit : MonoBehaviour
    {
        //IMPORTANT, this script must be on root of bullet object

        public GameObject impactParticle;
        public GameObject impactBloodParticle;
        public float impactDespawnTime;
        [HideInInspector] public int damage;

        void OnCollisionEnter(Collision col)
        {
            //Debug.Log("Hit Object = " + col.transform.name);
            //Debug.Log("Hit Tag = " + col.transform.tag);
            ExplosiveBarrel explosiveBarrel = col.transform.GetComponentInParent<ExplosiveBarrel>();

            if (explosiveBarrel != null)
            {
                explosiveBarrel.TakeHit();
                Destroy(gameObject);
                return;
            }

            //If we (the bullet) hit the col object check for Player tag
            if (col.transform.tag == "Enemy" || col.transform.tag == "Player")
            {
                //If the root object we hit has a healthcontroller then apply damage
                if (col.transform.root.gameObject.GetComponent<HealthController>())
                {
                    col.transform.root.gameObject.GetComponent<HealthController>().Damage(damage);
                }

                //Spawn blood on player
                GameObject tempImpact;
                tempImpact = Instantiate(impactBloodParticle, this.transform.position, this.transform.rotation) as GameObject;
                tempImpact.transform.Rotate(Vector3.left * 90);
                Destroy(tempImpact, impactDespawnTime);
            }
            else if (col.transform.CompareTag("Zombie"))
            {
                //ZombieController zombie = col.transform.root.GetComponent<ZombieController>();
                //Debug.Log("Zombie Controller = " + zombie);
                //Debug.Log("Hit Object = " + col.transform.name);

            ZombieController zombie = col.transform.GetComponent<ZombieController>();
            //Debug.Log("GetComponent = " + zombie);
            ZombieController zombieParent = col.transform.GetComponentInParent<ZombieController>();
            //Debug.Log("GetComponentInParent = " + zombieParent);
            ZombieController zombieRoot = col.transform.root.GetComponent<ZombieController>();
            //Debug.Log("Root = " + zombieRoot);

            if (zombie != null)
            {
                zombie.TakeDamage(damage);
                }

                GameObject tempImpact =
                Instantiate(impactBloodParticle,
                transform.position,
                transform.rotation);

                tempImpact.transform.Rotate(Vector3.left * 90);
                Destroy(tempImpact, impactDespawnTime);
            }
            else
            {
                //We hit something else just spawn basic impact prefab
                GameObject tempImpact;
                tempImpact = Instantiate(impactParticle, this.transform.position, this.transform.rotation) as GameObject;
                tempImpact.transform.Rotate(Vector3.left * 90);
                Destroy(tempImpact, impactDespawnTime);
            }

            //Finally, destroy us (the bullet)
            Destroy(gameObject);
        }
    }
}
