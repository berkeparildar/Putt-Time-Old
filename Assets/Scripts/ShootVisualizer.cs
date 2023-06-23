using UnityEngine;

public class ShootVisualizer : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float visualizerMaxLength = 5f;
    [SerializeField] private float arrowSpacing;

    private Vector3 _shootDirection;
    private float _shootForce;
    private GameObject _visualizerObject;
    [SerializeField] private GameObject[] _arrowInstances;
    
    void Start()
    {
        _visualizerObject = new GameObject("ShootDirectionVisualizer");
    }

    void Update()
    {
        _visualizerObject.transform.position = ballTransform.position;
        float shootLength = _shootForce * visualizerMaxLength;
        int arrowCount = Mathf.CeilToInt(shootLength / arrowSpacing);
        Debug.Log(arrowCount);
        if (arrowCount != 0)
        {
            for (int i = 0; i < _arrowInstances.Length; i++)
            {
                if (i < arrowCount)
                {
                    if (_arrowInstances[i] == null)
                    {
                        _arrowInstances[i] = Instantiate(arrowPrefab, _visualizerObject.transform);
                    }
                    _arrowInstances[i].SetActive(true);
                    _arrowInstances[i].transform.position
                        = ballTransform.position + _shootDirection * ((i + 1) * arrowSpacing);
                    _arrowInstances[i].transform.rotation = Quaternion.LookRotation(-_shootDirection);
                }
                else
                {
                    if (_arrowInstances[i] != null)
                    {
                        _arrowInstances[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void SetShootData(Vector3 direction, float force)
    {
        _shootDirection = direction.normalized;
        _shootForce = force;
    }

    public void ShowVisualizer()
    {
        var maxArrowCount = Mathf.CeilToInt(visualizerMaxLength / arrowSpacing);
        _arrowInstances = new GameObject[maxArrowCount];
    }

    public void HideVisualizer()
    {
        // Destroy arrow instances
        if (_arrowInstances != null)
        {
            foreach (var arrow in _arrowInstances)
            {
                if (arrow != null)
                {
                    _shootForce = 0;
                    Debug.Log("should destroy?");
                    Destroy(arrow.gameObject);
                }
            }
        }
    }
}