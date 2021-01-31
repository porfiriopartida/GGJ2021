using System;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Scripts
{
    public class BulletComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public float Speed = 10f;
        public CooldownManager CooldownManager;
        public float Life = 1.5f;
        public Player Player;
        public Vector3 Direction;
        public void SetDirection(Vector3 direction)
        {
            Direction = direction;
            GetComponent<Rigidbody>().velocity = direction.normalized * Speed;
        }

        private void Start()
        {
            CooldownManager.AddTimer("LIFE", Life);
        }

        private void OnCollisionEnter(Collision other)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Player == null)
            {
                Destroy(gameObject);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("player"))
            {
                if (!Player.UserId.Equals(other.gameObject.GetComponent<PhotonView>().Owner.UserId))
                {
                    Destroy(gameObject);
                }
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("map"))
            {
                Destroy(gameObject);
            }
        }

        public void Update()
        {
            if (CooldownManager.GetTimer("LIFE") < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}