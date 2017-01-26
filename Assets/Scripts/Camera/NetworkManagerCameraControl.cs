using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Camera
{
    class NetworkManagerCameraControl : NetworkBehaviour
    {
        public Transform sceneCamera;
        public float cameraRotationRadius = 16f;
        public float cameraRotationSpeed = 10f;

        private bool canRotate = true;
        private float rotation = 0f;

        public override void OnStartClient()
        {
            canRotate = false;
        }

        public override void OnStartServer()
        {         
            canRotate = false;
        }

        public void Update()
        {
            if (!canRotate) return;

            rotation += cameraRotationSpeed * Time.deltaTime;
            if (rotation >= 360f) rotation -= 360f;

            sceneCamera.position = Vector3.zero;
            sceneCamera.rotation = Quaternion.Euler(0f, rotation, 0f);
            sceneCamera.Translate(0f, cameraRotationRadius, -cameraRotationRadius);
            sceneCamera.LookAt(Vector3.zero);
        }

    }
}
