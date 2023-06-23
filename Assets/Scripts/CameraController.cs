using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 3f; // Speed of camera rotation

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineOrbitalTransposer orbitalTransposer;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            orbitalTransposer.m_Heading.m_Bias += rotationX;
        }
    }
}