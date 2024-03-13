using Cinemachine;
using UnityEngine;

namespace Game
{
    public enum CameraMode
    {
        ThridPerson,
        FirstPerson
    }
    public class CameraController : MonoBehaviour
    {
        public CameraMode cameraMode;
        private CinemachineFreeLook _freeLookCamera;
    }
}