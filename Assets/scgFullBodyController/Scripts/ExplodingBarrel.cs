using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private GameObject _flameEffect;
    private MeshRenderer _meshRenderer;
    private Hitpoint _hitPoint;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        hitPoint = GetComponent<Hitpoint>();
    }
    public void BarrelLit()
    {
        Debug.Log("Barrel Lit");
        _flameEffect.SetActive(true);
    }
    public void Explode()
    {
        Debug.Log("Barrel Exploded");
        _explosionEffect.Play();
        _meshRenderer.enabled = false;
        _flameEffect.SetActive(false);
        PlayerManager.Instance.CameraShake(PlayerManager.ShakeStrength.Normal);
        Invoke("DestroyBarrel", 7f);
        
    }
    private void DestroyBarrel()
    {
        Destroy(this.gameObject);
    }
    IEnumerator DecreaseHealth()
    {
        while (true)
        {
            _hitPoint.TakeDamage(4);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
