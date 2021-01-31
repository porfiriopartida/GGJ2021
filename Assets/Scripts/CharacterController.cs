using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using PUN;
using Scripts;
using UnityEngine;

// [RequireComponent(typeof(CharacterController))]
public class CharacterController : MonoBehaviour
{
    public float maxRadiansDelta = 1;
    public float maxMagnitudeDelta = 0f;
    //   private Rigidbody2D _rigidbody;
    public Float MovSpeed;
    private float RealSpeed;
    private float OriginalRealSpeed;
    private float SlowRealSpeed;
    private Vector3 _cacheDirection = Vector3.forward;
    private Vector3 lastDirection = Vector3.forward;
    private Rigidbody _rigidbody3D;
    public PhotonView photonView;
    private Animator _animator;
    public GameEvent SomeoneDiedEvent;
    public CooldownManager CooldownManager;
    // public int FlagsCount = 1;
    public GunController GunController;
    public int CurrentHP;
    public int MaxHp; //TODO: SO
    public HandComponent Hand;
    // public GameObject flagPrefab;
    private void Start()
    {
        _rigidbody3D = GetComponent<Rigidbody>();
        // photonView = PhotonView.Get(this);
        _animator = GetComponent<Animator>();
        LoadCustomizations();
    }
    private void LoadCustomizations()
    {
        //Load Customizations from Room Properties.
        float movSpeed = (float) PhotonNetwork.CurrentRoom.CustomProperties["MovSpeed"];
        OriginalRealSpeed = MovSpeed.Value * movSpeed;
        SlowRealSpeed = MovSpeed.Value * movSpeed * 0.75f;
        RealSpeed = MovSpeed.Value * movSpeed;
    }
    public void Stop()
    {
        Move(Vector3.zero);
    }
    #region PUNRPC
    public void Move(Vector3 direction)
    {
        if (_cacheDirection == direction)
        {
            return;
        }

        if (direction != Vector3.zero)
        {
            lastDirection = direction;
        }

        _cacheDirection = direction;

        // PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_Move", RpcTarget.AllViaServer, direction);
    }
    
    public void GetKilled()
    {
        DropFlag();
        photonView.RPC("RPC_Die", RpcTarget.AllViaServer);
    }
    
    [PunRPC]
    public void RPC_Die()
    {
        Die();
    }

    public void Die()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            GetComponent<PlayerSetup>().Reposition();
        }
    }

    public void DropFlag()
    {
        if (CooldownManager.GetTimer("FLAG") < 0)
        {
            CooldownManager.AddTimer("FLAG", .5f);
        }
        if (Hand.GetItem() != null)
        {
            photonView.RPC("RPC_DropFlag", RpcTarget.AllViaServer, Hand.GetItem().GetComponent<PhotonView>().ViewID, transform.position);
        }
        Flag = null;
    }


    [PunRPC]
    public void RPC_DropFlag(int viewId, Vector3 position)
    {
        PhotonView flagPhotonView = PhotonView.Find(viewId);
        Transform flagTransform = flagPhotonView.gameObject.transform;
        flagTransform.parent = null;
        RealSpeed = OriginalRealSpeed;
        // flagTransform.Find("Sight").GetComponent<ShowOnProximity>().SetIsRunning(false);
    }


    //
    // [PunRPC]
    // public void RPC_DropFlag(Player Owner)
    // {
    //     GameObject spawnedFlag = null;
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         spawnedFlag = PhotonNetwork.Instantiate(flagPrefab.name, transform.position, Quaternion.identity);
    //         spawnedFlag.name = "Flag";
    //         OwnedComponent fc = spawnedFlag.GetComponent<OwnedComponent>();
    //         fc.Owner = Owner;
    //         fc.UserId = Owner.UserId;
    //         
    //         GrabFlag(spawnedFlag.GetPhotonView().ViewID);
    //     }
    //
    //     if (spawnedFlag != null)
    //     {
    //         spawnedFlag.GetComponent<PhotonView>().TransferOwnership(Owner);
    //     }
    // }
    public void Fire()
    {
        Transform flag = Hand.GetItem(); 
        if (flag == null)
        {
            GunController.Fire(lastDirection);
        }
    }

    public void GrabFlag(int viewId)
    {
        // if (FlagsCount == 0)
        // {
        //     photonView.RPC("RPC_GrabFlag", RpcTarget.AllViaServer, viewId, photonView.ViewID);
        // }
        photonView.RPC("RPC_GrabFlag", RpcTarget.AllViaServer, viewId, photonView.ViewID);
    }
    [PunRPC]
    public void RPC_GrabFlag(int flagPhotonViewId, int playerPhotonViewId)
    {
        PhotonView flagPhotonView = PhotonView.Find(flagPhotonViewId);
        PhotonView playerPhotonView = PhotonView.Find(playerPhotonViewId);
        Flag = flagPhotonView.gameObject;
        Flag.name = "Flag";
        Transform flagTransform = Flag.transform;
        flagTransform.parent = playerPhotonView.GetComponent<CharacterController>().Hand.transform;
        flagTransform.localPosition = Vector3.zero;
        RealSpeed = SlowRealSpeed;
        // flagTransform.Find("Sight").GetComponent<ShowOnProximity>().SetIsRunning(false);
    }

    [PunRPC]
    public void RPC_Move(Vector3 direction)
    {
        if (_rigidbody3D)
        {
            _rigidbody3D.velocity = direction * RealSpeed;
            _rigidbody3D.transform.rotation.SetLookRotation(direction);
            if (direction != Vector3.zero)
            {
                LookAt(direction.normalized);
            }
        }
    }
    
    void LookAt(Vector3 targetPosition)
    {
        // targetPosition.y = 0;
        // Vector2 localTransform2 = new Vector2(transform.position.x, transform.position.z);
        // Vector2 targetPosition2 = new Vector2(targetPosition.x, targetPosition.z);
        // Vector2 diff = targetPosition2 - localTransform2;
        // float radians = (Mathf.Atan2(diff.y, diff.x));
        float radians = Mathf.Atan2(targetPosition.z, targetPosition.x);
        float degrees = radians * 180 / Mathf.PI;
        
        if (degrees > 90)
        {
            degrees = 450 - degrees;
        }
        else
        {
            degrees = 90 - degrees;
        }

        Quaternion rotation = Quaternion.AngleAxis(degrees, yv);
        transform.rotation = rotation;
    }
    public Vector3 yv = new Vector3(0, 1, 0);
    #endregion
    private void FixedUpdate()
    {
    }

    [PunRPC]
    public void RPC_LookAt(float angle)
    {
        // Debug.Log("Angle :" + angle);
        if (_rigidbody3D)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
    public void LookAt(float angle)
    {
        photonView.RPC("RPC_LookAt", RpcTarget.AllViaServer, angle);
    }

    public GameObject Flag;
    private void OnTriggerEnter(Collider other)
    {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == LayerMask.NameToLayer("bullet"))
        {
            OwnedComponent ownedComponent = other.GetComponent<OwnedComponent>();
            if (PhotonNetwork.IsMasterClient && !photonView.Owner.UserId.Equals(ownedComponent.UserId))
            {
                GetKilled();
            }
            
            Destroy(other);
        } else if (otherLayer == LayerMask.NameToLayer("flag"))
        {
            Flag = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == LayerMask.NameToLayer("flag"))
        {
            Flag = null;
        }
    }

    public void Interact()
    {
        if (Hand.GetItem() == null)
        {
            if(Flag != null){
                GrabFlag(Flag.GetPhotonView().ViewID);
            }
        }
        else
        {
            DropFlag();
        }
    }
}
