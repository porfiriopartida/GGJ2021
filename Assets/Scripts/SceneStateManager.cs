using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using PUN;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneStateManager : Singleton<SceneStateManager>
{
    public CooldownManager CooldownManager;
    public GameConfiguration gameConfiguration;
    public InputController InputController;
    public GameObject[] SpawnPoints;
    public GameObject Spawn;
    public SceneState SceneState;
    public static string TAG_PLAYER = "Player";

    private CharacterController _characterController;

    private GameObject[] players;
    public CharacterController CharacterController
    {
        get => _characterController;
        set
        {
            _characterController = value;
            InputController = _characterController.GetComponent<InputController>();
        }
    }

    public bool IsGameRunning { get; set; }

    private void Start()
    {
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 1200000; // in milliseconds. any high value for debug
    }

    public void DisableRegularInput()
    {
        InputController.enabled = false;
    }

    public void EnableRegularInput()
    {
        InputController.enabled = true;
        CharacterController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void ResetPlayers()
    {
        SceneState.ResetPlayers();
    }

    public Hashtable ParseProperties()
    {
        return gameConfiguration.toHashtable();
    }

    public Material GetMaterial(int playerIdx)
    {
        return SceneState.GetMaterial(playerIdx);
    }
    public Color GetColor(Player player)
    {
        return SceneState.GetColor(player);
    }
    public int GetColorIdx(Player player)
    {
        return SceneState.GetColorIdx(player);
    }
    public List<int> GetTakenColors()
    {
        return SceneState.GetTakenColors();
    }

    public void SetColor(Player player, int idx)
    {
        Debug.Log("Assigning color to " + player.NickName + ":" + idx);
        SceneState.SetColor(player, idx);
    }

    public void SetIsAlive(Player player, bool b)
    {
        SceneState.SetIsAlive(player, b);
    }

    public bool IsAlive()
    {
        return SceneState.IsAlive(PhotonNetwork.LocalPlayer);
    }
    public bool IsAlive(Player player)
    {
        return SceneState.IsAlive(player);
    }

    public Player FindPlayer(string uuid)
    {
        return SceneState.FindPlayer(uuid);
    }

    public void RemoveAllUsers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
    }

    public void GameOver(bool isRedSideWin)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isRedSideWin)
            {
                StartCoroutine(GameScene("Scenes/RedWin"));
            }
            else
            {
                StartCoroutine(GameScene("Scenes/BlueWin"));
            }
        }
    }

    public IEnumerator GameScene(string scene)
    {
#if UNITY_EDITOR
        yield return new WaitForSeconds(gameConfiguration.ChangeSceneWaitingDebug);
#else
        yield return new WaitForSeconds(gameConfiguration.ChangeSceneWaiting);
#endif
            
        if (PhotonNetwork.IsMasterClient)
        {
            //Clean all objects because we are moving to a different scene.
            PhotonNetwork.DestroyAll();
        }
            
        SceneManager.LoadScene(scene);
    }
}
