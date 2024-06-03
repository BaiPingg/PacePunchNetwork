using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class NetProcedureService : NetworkBehaviour, IService
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        SL.Register<NetProcedureService>(this);
    }

    [ServerRpc]
    public void ChangeToLoading()
    {
        var loading = new LoadingGameState();
        SL.Get<ProcedureService>().SwitchState(loading);
    }
}