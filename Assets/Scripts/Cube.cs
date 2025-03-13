using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _minDelay = 2;
    [SerializeField] private int _maxDelay = 5;

    private bool _isPlatformTouched = false;

    private UserUtilites _userUtilities = new();
    private Renderer _renderer;
    private Rigidbody _rigidbody;

    public event Action<Cube> TimerEnded;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _isPlatformTouched = false;
        _renderer.material.color = Color.white;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isPlatformTouched == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isPlatformTouched = true;
            StartCoroutine(ReleaseAfterDelay());
        }
    }

    private IEnumerator ReleaseAfterDelay()
    {
        _userUtilities.Color(_renderer);
        yield return new WaitForSeconds(_userUtilities.Range(_minDelay, _maxDelay));
        TimerEnded?.Invoke(this);
    }
}
