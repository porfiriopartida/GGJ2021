using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PUN
{
    public class PlayerSetup : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        Text playerNameText;

        public GameObject PlayerNamePanel;

        public InputController InputController;

        // public Canvas Hud;

        public MeshRenderer MeshRenderer;
        private void Start()
        {
            Camera mainCamera = Camera.main;

            CharacterController characterController = transform.GetComponent<CharacterController>();
            if (photonView.IsMine)
            {
                SceneStateManager.Instance.CharacterController = characterController;
                InputController.enabled = true;
                transform.GetComponent<InputController>().enabled = true;
                if (mainCamera != null)
                {
                    mainCamera.gameObject.GetComponent<CameraFollow>().SetTarget(transform);
                }
                
                // Hud.gameObject.SetActive(true);
                // Hud.worldCamera = mainCamera;
                // Hud.planeDistance = 50;
            }
            else
            {
                InputController.enabled = false;
                // Hud.gameObject.SetActive(false);
                transform.GetComponent<InputController>().enabled = false;
            }

            this.gameObject.name = photonView.Owner.NickName;
                
            SetPlayerUI();

            //Inject color
            if (photonView.Owner != null)
            {
                int newColorIdx = SceneStateManager.Instance.GetColorIdx(photonView.Owner);
                Material newColorMaterial = SceneStateManager.Instance.GetMaterial(newColorIdx);
                MeshRenderer.material = newColorMaterial;
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Reposition();
                }
            }
        }


        public void SetPlayerUI()
        {
            if (playerNameText.text != null && photonView.Owner != null)
            {
                playerNameText.text = photonView.Owner.NickName;
            }

            if (!photonView.IsMine)
            {
                //TODO: If we want to make this configurable, add isMine check so impostor doesn't know.
                playerNameText.color = Color.red;
            }
        }
        
        public void Reposition()
        {
            // Vector3 newPosition = SceneStateManager.Instance.SpawnPoints[Random.Range(0, SceneStateManager.Instance.SpawnPoints.Length)]
            //     .transform.position;
            int idx = photonView.Owner.IsMasterClient ? 0 : 1;
            Vector3 newPosition = SceneStateManager.Instance.SpawnPoints[idx].transform.position;
            photonView.RPC("RPC_Reposition", RpcTarget.AllViaServer, newPosition);
        }

        [PunRPC]
        public void RPC_Reposition(Vector3 newPosition)
        {
            var o = this.gameObject;
            o.transform.parent = SceneStateManager.Instance.Spawn.transform;
            o.transform.position = newPosition;
        }
    }
}
