using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        _ballRb = ball.GetComponent<Rigidbody>();
        _shootPower = 4;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPoint.transform.position = transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Ball"))
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            CalculateShootMultiplier();
            float hitDistance;
            if (plane.Raycast(ray, out hitDistance))
            {
                _shootDirection = transform.position - ray.GetPoint(hitDistance);
                _shootDirection.Normalize();
                directionVisualizer.SetShootData(_shootDirection, _shootPower);
            }
        }
    }

    private void CalculateShootMultiplier()
    {
        float dragDistance = Vector3.Distance(_initialMousePosition, Input.mousePosition);
        _shootPower = Mathf.Clamp(dragDistance / 25 , 0, 10);
    }

    private void ShootBall()
    {
        float shootForce = _shootPower;
        _ballRb.AddForce(_shootDirection * shootForce, ForceMode.Impulse);
    }
}