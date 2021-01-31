using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FlagComponent : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            RepositionFlag();
        }
        
        int newColorIdx = SceneStateManager.Instance.GetColorIdx(photonView.Owner);
        Material newColorMaterial = SceneStateManager.Instance.GetMaterial(newColorIdx);
        MeshRenderer.material = newColorMaterial;
    }

    public void RepositionFlag()
    {
        // Vector3 newPosition = SceneStateManager.Instance.SpawnPoints[Random.Range(0, SceneStateManager.Instance.SpawnPoints.Length)]
        //     .transform.position;
        int idx = photonView.Owner.IsMasterClient ? 0 : 1;
        Vector3 newPosition = SceneStateManager.Instance.SpawnPoints[idx].transform.position;
        photonView.RPC("RPC_RepositionFlag", RpcTarget.AllViaServer, newPosition);
    }

    [PunRPC]
    public void RPC_RepositionFlag(Vector3 newPosition)
    {
        var o = this.gameObject;
        o.transform.parent = SceneStateManager.Instance.Spawn.transform;
        o.transform.position = newPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
