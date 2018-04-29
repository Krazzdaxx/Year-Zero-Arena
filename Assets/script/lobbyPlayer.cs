using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LostPolygon.AndroidBluetoothMultiplayer.Examples.UNet;
/// <summary>
/// player script to use in charselect scene
/// </summary>
public class lobbyPlayer : NetworkBehaviour
{
    public int pid;
    public override void OnStartLocalPlayer()
    {
        FindObjectOfType<charSelect>().SetLocalPlayerRef(this);
        pid = FindObjectOfType<BluetoothMultiplayerDemoNetworkManager>().LocalPconnection.connectionId;
    }
    /// <summary>
    /// called from ui button
    /// </summary>
    /// <param name="choice"></param>
    public void sendChoiceToServer(int choice)
    {
        CmdOnServerSetChoice(pid, choice);
    }
    /// <summary>
    /// called from Local player to server
    /// </summary>
    /// <param name="p"></param>
    /// <param name="c"></param>
    [Command]
    public void CmdOnServerSetChoice(int p, int c)
    {
        if (isServer)
        {
            //save choice on server
            FindObjectOfType<BluetoothMultiplayerDemoNetworkManager>().setPlayerCharChoice(p, c);
            RpcSetpic(p, c);
        }
    }
    /// <summary>
    /// changes the player portrait called on server run on clients
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="choice"></param>
    [ClientRpc]
    public void RpcSetpic(int pid, int choice)
    {
        FindObjectOfType<charSelect>().setPics(pid, choice);
    }
    [Command]
    public void CmdCallServerChangeScene(string level)
    {
        GameObject.FindObjectOfType<BluetoothMultiplayerDemoNetworkManager>().onServerChangeScene(level);
    }
}