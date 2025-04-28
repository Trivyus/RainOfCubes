using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Cube : SpawnableObject<Cube>
{
    [SerializeField] private int _minDelay = 2;
    [SerializeField] private int _maxDelay = 5;
    [SerializeField] private Color _initialColor = Color.white;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _releaseCoroutine;

    private bool _isPlatformTouched = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Init()
    {
        ResetState();
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
            _releaseCoroutine = StartCoroutine(ReleaseAfterDelay());
        }
    }

    private IEnumerator ReleaseAfterDelay()
    {
        GetRandomColor(_renderer);
        yield return new WaitForSeconds(GetRandomDelay(_minDelay, _maxDelay));
        NotifyTimerEnded();
    }

    private void ResetState()
    {
        if (_releaseCoroutine != null)
        {
            StopCoroutine(_releaseCoroutine);
            _releaseCoroutine = null;
        }

        _isPlatformTouched = false;
        _renderer.material.color = _initialColor;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.Sleep();

        transform.rotation = Quaternion.identity;
    }

    private void GetRandomColor(Renderer renderer) => 
        renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

    private int GetRandomDelay(int min, int max) => 
        UnityEngine.Random.Range(min, max + 1);
}
