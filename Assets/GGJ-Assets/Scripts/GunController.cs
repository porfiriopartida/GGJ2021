using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Scripts
{
    public class GunController : MonoBehaviour
    {
        public CooldownManager CooldownManager;
        public const string FIRE = "FIRE";
        public PhotonView photonView;
        public float FireRate = .03f;
        public GameObject bulletPrefab;
        public GameObject bulletSpawner;
        
        public void Fire(Vector3 direction)
        {
            if (CooldownManager.GetTimer(FIRE)<=0)
            {
                CooldownManager.AddTimer(FIRE, FireRate);
                photonView.RPC("RPC_Fire", RpcTarget.AllViaServer, photonView.Owner, direction);
            }
        }
        [PunRPC]
        public void RPC_Fire(Player Owner, Vector3 direction)
        {
            //GameObject spawnedFlag = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawner.transform.position, Quaternion.identity);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawner.transform.position, Quaternion.identity);
            OwnedComponent fc = bullet.GetComponent<OwnedComponent>();
            fc.Owner = Owner;
            fc.UserId = Owner.UserId;
            BulletComponent bulletComponent = bullet.GetComponent<BulletComponent>();
            bulletComponent.Player = Owner;
            bulletComponent.SetDirection(direction);
        }
    }
}