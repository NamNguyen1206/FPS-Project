using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CapsuleColor : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;

    private void Awake()
    {
        ApplyColor();
    }

    private void OnValidate()
    {
        ApplyColor();
    }

    private void ApplyColor()
    {
        Renderer capsuleRenderer = GetComponent<Renderer>();
        if (capsuleRenderer == null)
        {
            return;
        }

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        capsuleRenderer.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        block.SetColor("_BaseColor", color);
        capsuleRenderer.SetPropertyBlock(block);
    }
}
