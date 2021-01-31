using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    public int idx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("flag"))
        {
            //OwnedComponent ownedComponent = other.gameObject.GetComponent<OwnedComponent>();
            PhotonView photonView =  other.gameObject.GetComponent<PhotonView>();

            if (photonView == null)
            {
                return;
            }

            Player Owner = photonView.Owner;

            Debug.Log(Owner.NickName + " FLAG is touching idx " + idx);
            if (idx == 0)
            {
                if (!Owner.UserId.Equals(PhotonNetwork.MasterClient.UserId))
                {
                    //Left base touched by BLUE's flag.
                    //Master Wins
                    SceneStateManager.Instance.GameOver(true);
                }
            } else if (idx == 1) {
                if (Owner.UserId.Equals(PhotonNetwork.MasterClient.UserId))
                {
                    //Right base touched by master's flag.
                    //BLUE Wins
                    SceneStateManager.Instance.GameOver(false);
                }
            }
        }
    }
}
