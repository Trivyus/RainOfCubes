using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _minDelay = 2;
    [SerializeField] private int _maxDelay = 5;

    private bool TouchedPlatform = false;

    private GetRandom _getRandom = new();
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
        TouchedPlatform = false;
        _renderer.material.color = Color.white;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform) && TouchedPlatform == false)
        {
            TouchedPlatform = true;
            StartCoroutine(ReleaseAfterDelay());
        }
    }

    private IEnumerator ReleaseAfterDelay()
    {
        _getRandom.Color(gameObject.GetComponent<Renderer>());
        yield return new WaitForSeconds(_getRandom.Range(_minDelay, _maxDelay));
        TimerEnded?.Invoke(this);
    }
}
