using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Cube : SpawnableObject<Cube>
{
    [SerializeField] private int _minDelay = 2;
    [SerializeField] private int _maxDelay = 5;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _releaseCoroutine;

    private bool _isPlatformTouched = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ResetCubeState();
    }

    private void OnDisable()
    {
        if (_releaseCoroutine != null)
        {
            StopCoroutine(_releaseCoroutine);
            _releaseCoroutine = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isPlatformTouched == false && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _isPlatformTouched = true;
            StartCoroutine(ReleaseAfterDelay());
        }
    }

    private IEnumerator ReleaseAfterDelay()
    {
        GetRandomColor(_renderer);
        yield return new WaitForSeconds(GetRandomDelay(_minDelay, _maxDelay));
        NotifyTimerEnded();
    }

    private void ResetCubeState()
    {
        _isPlatformTouched = false;
        _renderer.material.color = Color.white;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.identity;
    }

    private void GetRandomColor(Renderer renderer) => 
        renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

    private int GetRandomDelay(int min, int max) => 
        UnityEngine.Random.Range(min, max + 1);
}
