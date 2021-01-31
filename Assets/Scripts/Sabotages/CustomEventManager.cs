﻿using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CustomEventManager : Singleton<CustomEventManager>
{
    public CooldownManager CooldownManager;
    private RaiseEventOptions _raiseEventOptions;
    private void Start()
    {
        _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    }

    // public void SabotageLights()
    // {
        // if(CanSabotage()){
        //     PhotonNetwork.RaiseEvent(EventsConstants.SABOTAGE_LIGHT, null, _raiseEventOptions, SendOptions.SendReliable);
        // }
    // }
    
    // public void FixLights()
    // {
    //     PhotonNetwork.RaiseEvent(EventsConstants.FIX_LIGHT, null, _raiseEventOptions, SendOptions.SendReliable);
    // }
}
