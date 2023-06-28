using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private ShootVisualizer directionVisualizer;
    [SerializeField] private GameObject cameraPoint;
    
    private bool _isShooting;
    private Vector3 _shootDirection;
    private float _shootPower;  
    private Rigidbody _ballRb;
    private Vector3 _initialMousePosition;
    private float _dragDistance;
    private Vector3 _cameraPos;
    [SerializeField] private float maxRotationDegrees = 30f;
    private float _currentRotation;
    private Camera _camera;
    private float idleTimer;
    [SerializeField] private float idleDuration = 3f;
    [SerializeField] private float slowdownFactor = 10f; 

    private void Start()
    {
        _camera = Camera.main;
        _ballRb = ball.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _cameraPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        cameraPoint.transform.position = _cameraPos;
        if (_ballRb.velocity.magnitude < 1f)
        {
            SlowDownBall();
        }
        else if (_ballRb.velocity.magnitude <= 0.1f)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                MakeBallStationary();
            }
        }
        else
        {
            idleTimer = 0f;
        }
        var ballIsStationary = _ballRb.velocity.magnitude <= 0.1f;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Ball") && ballIsStationary)
                {
                    _isShooting = true;
                    _initialMousePosition = Input.mousePosition;
                    directionVisualizer.ShowVisualizer();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_isShooting)
            {
                ShootBall();
                directionVisualizer.HideVisualizer();
            }

            _isShooting = false;    
        }

        if (_isShooting)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            CalculateShootMultiplier();
            float hitDistance;
            if (plane.Raycast(ray, out hitDistance))
            {
                _shootDirection = transform.position - ray.GetPoint(hitDistance);
                _shootDirection.Normalize();
                UpdateRotation();
            }
        }
    }

    private void CalculateShootMultiplier()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, transform.position);
        if (plane.Raycast(ray, out var hitDistance))
        {
            var currentMousePosition = ray.GetPoint(hitDistance);
            var dragVector = currentMousePosition - transform.position;
            dragVector.y = 0;
            _dragDistance = dragVector.magnitude * 10;
            _shootPower = Mathf.Clamp(_dragDistance, 0, 25);
        }
    }
    
    private void UpdateRotation()
    { 
        var rotationAmount = Mathf.Lerp(-maxRotationDegrees, maxRotationDegrees, _dragDistance / 10f);
        // seems buggy in low power?
        var angle = Mathf.Sin(Time.time * 2f) * rotationAmount;
        _currentRotation = angle;
        var rotation = Quaternion.Euler(0f, _currentRotation, 0f);
        _shootDirection = rotation * _shootDirection;
        directionVisualizer.SetShootData(_shootDirection, _shootPower);
    }
    
    private void SlowDownBall()
    {
        _ballRb.velocity *= (1f - Time.deltaTime * slowdownFactor);
    }
    
    private void MakeBallStationary()
    {
        _ballRb.velocity = Vector3.zero;
        _ballRb.angularVelocity = Vector3.zero;
    }

    private void ShootBall()
    {
        var shootForce = _shootPower;
        _shootDirection.y = 0;
        GameManager.StrokeCount++;
        _ballRb.AddForce(_shootDirection * shootForce, ForceMode.Impulse);
        Debug.Log(_shootDirection * _shootPower);
    }
}