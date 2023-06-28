using UnityEngine;
using UnityEngine.Serialization;

public class ShootVisualizer : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpacing;

    private Vector3 _shootDirection;
    private Color _powerColor;
    private float _shootForce;
    private readonly int _maxArrowCount = 5;
    private GameObject _visualizerObject; 
    [SerializeField] private GameObject[] arrowInstances;
    
    void Start()
    {
        _visualizerObject = new GameObject("ShootDirectionVisualizer");
        _powerColor = new Color(0, 1, 0);
    }

    void Update()
    {
        _visualizerObject.transform.position = ballTransform.position;
        float shootLength = _shootForce;
        int arrowCount = Mathf.CeilToInt(shootLength / _maxArrowCount);
        if (arrowCount != 0)
        {
            for (int i = 0; i < arrowInstances.Length; i++)
            {
                if (i < arrowCount)
                {
                    if (arrowInstances[i] == null)
                    {
                        arrowInstances[i] = Instantiate(arrowPrefab, _visualizerObject.transform);
                    }
                    arrowInstances[i].SetActive(true);
                    arrowInstances[i].GetComponent<MeshRenderer>().material.color
                        = new Color(0 + (i * 0.2f), 1 - (i * 0.2f), 0);
                    arrowInstances[i].transform.position = ballTransform.position + _shootDirection * ((i + 1) * arrowSpacing);
                    arrowInstances[i].transform.rotation = Quaternion.LookRotation(-_shootDirection);
                }
                else
                {
                    if (arrowInstances[i] != null)
                    {
                        arrowInstances[i].SetActive(false);
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
        arrowInstances = new GameObject[_maxArrowCount];
    }

    public void HideVisualizer()
    {
        if (arrowInstances != null)
        {
            foreach (var arrow in arrowInstances)
            {
                if (arrow != null)
                {
                    _shootForce = 0;
                    Destroy(arrow.gameObject);
                }
            }
        }
    }
}