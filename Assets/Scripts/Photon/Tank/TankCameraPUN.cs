using UnityEngine;

public class TankCameraPUN : MonoBehaviour {

    public float m_cameraDistance = 16f;
    public float m_cameraHeight = 16f;

    private Transform m_mainCameraTransform;
    private Vector3 m_cameraOffSet;
    
    void Start () {
        m_mainCameraTransform = Camera.main.transform;
        m_cameraOffSet = new Vector3(0f, m_cameraHeight, -m_cameraDistance);
    }
	
	void FixedUpdate () {
        MoveCamera();
	}

    void MoveCamera()
    {
        m_mainCameraTransform.position = transform.position;
        m_mainCameraTransform.rotation = transform.rotation;
        m_mainCameraTransform.Translate(m_cameraOffSet);
        m_mainCameraTransform.LookAt(transform);
    }
}
