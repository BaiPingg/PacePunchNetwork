using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class GameNetLogic : NetworkBehaviour
{
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        NetAddressable.InstantiatePrefab("Assets/Prefabs/PlayerArmature.prefab",LocalConnection);
     
    }
}
