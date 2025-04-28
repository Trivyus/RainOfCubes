using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Bomb : SpawnableObject<Bomb>
{
    [SerializeField] private int _minDelay = 2;
    [SerializeField] private int _maxDelay = 5;
    [SerializeField] private Material _material;
    [SerializeField] private Explosion _explosion;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private Rigidbody _rigidbody;

    private float _initialAlpha;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _initialAlpha = 1f;
    }

    private void OnEnable()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        ResetMaterialAlpha();

        StartCoroutine(FadeSmoothly());
    }

    private IEnumerator FadeSmoothly()
    {
        int fadeDuration = GetRandomDelay(_minDelay, _maxDelay);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(_initialAlpha, 0.5f, elapsedTime / fadeDuration);
            SetMaterialAlpha(newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetMaterialAlpha(0f);
        _explosion.Explode();
        NotifyTimerEnded();
    }

    private void SetMaterialAlpha(float alpha)
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", new Color(_material.color.r, _material.color.g, _material.color.b, alpha));
        _renderer.SetPropertyBlock(_propBlock);
    }

    private void ResetMaterialAlpha()
    {
        SetMaterialAlpha(_initialAlpha);
    }

    private int GetRandomDelay(int min, int max) =>
        UnityEngine.Random.Range(min, max + 1);
}
