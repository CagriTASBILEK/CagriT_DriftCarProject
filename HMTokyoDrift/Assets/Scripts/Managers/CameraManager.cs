using System.Collections;
using Cinemachine;
using Core.Events;
using Core.Singleton;
using UnityEngine;
using Vehicles;

namespace Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float waitTime = 0.1f; 

        private void Start()
        {
            if (virtualCamera == null)
            {
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                if (virtualCamera == null)
                {
                    Debug.LogError("Virtual Camera is empty");
                }
            }
        }

        private void OnEnable()
        {
            GameEvents.OnGameStart += HandleGameStart;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= HandleGameStart;
        }

        private void HandleGameStart()
        {
            if (virtualCamera == null) return;
            StartCoroutine(WaitForPlayerSpawn());
        }

        private IEnumerator WaitForPlayerSpawn()
        {
            PlayerVehicle player = null;
            float elapsedTime = 0f;
        
            while (player == null && elapsedTime < 2f) 
            {
                player = PlayerManager.Instance?.CurrentPlayer;
                if (player != null)
                {
                    virtualCamera.Follow = player.transform;
                    virtualCamera.LookAt = player.transform;
                    break;
                }
                elapsedTime += waitTime;
                yield return new WaitForSeconds(waitTime);
            }
        
        }
    }
}