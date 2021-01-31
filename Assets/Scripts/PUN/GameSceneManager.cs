using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PUN
{
    public class GameSceneManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static GameSceneManager Instance;
        public GameObject playerPrefab;
        public GameObject flagPrefab;
        public GameObject Authoritative;
        public GameObject _networkLaunchManager;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                Authoritative.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    SpawnCharacters();
                }
            }
            #if UNITY_EDITOR
            else if(_networkLaunchManager)
            {
                _networkLaunchManager.SetActive(true);
            }
            #endif
        }

        private void SpawnCharacters()
        {           
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.GAME_STARTED, null, raiseEventOptions, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventsConstants.GAME_STARTED)
            {
                SpawnMe();
            }
        }

        public void SpawnMe()
        {
            GameObject spawnedObject = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            Vector3 newPos = spawnedObject.transform.position;
            newPos.y = 1;
            GameObject spawnedFlag = PhotonNetwork.Instantiate(flagPrefab.name, newPos, Quaternion.identity);
            spawnedFlag.name = "Flag";
            
            // OwnedComponent fc = spawnedFlag.GetComponent<OwnedComponent>();
            // fc.Owner = spawnedObject.GetPhotonView().Owner;
            // fc.UserId = fc.Owner.UserId;
            //
            //
            // OwnedComponent fc2 = spawnedFlag.GetComponent<OwnedComponent>();
            // fc2.Owner = spawnedObject.GetPhotonView().Owner;
            // fc2.UserId = fc.Owner.UserId;
            //spawnedObject.GetComponent<CharacterController>().GrabFlag(spawnedFlag.GetPhotonView().ViewID);
            
            if (PhotonNetwork.IsMasterClient)
            {
                Authoritative.SetActive(true);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            // SceneStateManager.Instance.RemovePlayer(otherPlayer);
        }

        #region PUN
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnLeftRoom()
        {
            Leave();
        }

        private void Leave()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Scenes/Login");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Leave();
        }
        #endregion
    }
}
