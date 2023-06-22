using UnityEngine;

public class ShootVisualizer : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float visualizerMaxLength = 1f;
    [SerializeField] private float visualizerWidth = 0.1f;
    [SerializeField] private Material visualizerMaterial;

    private Vector3 _shootDirection;
    private float _shootForce;
    private GameObject _visualizerObject;
    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _visualizerObject = new GameObject("ShootDirectionVisualier");
        _lineRenderer = _visualizerObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = visualizerWidth;
        _lineRenderer.endWidth = visualizerWidth;
        _lineRenderer.material = visualizerMaterial;
        _lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _visualizerObject.transform.position = ballTransform.position;
        _lineRenderer.SetPosition(0, ballTransform.position);
        _lineRenderer.SetPosition(1, ballTransform.position + _shootDirection * _shootForce);
    }

    public void SetShootData(Vector3 direction, float force)
    {
        _shootDirection = direction.normalized;
        _shootForce = force;
        _lineRenderer.enabled = _shootForce > 0;
    }

    public void ShowVisualizer()
    {
        _lineRenderer.enabled = true;
    }

    public void HideVisualizer()
    {
        _lineRenderer.enabled = false;   
    }
}
