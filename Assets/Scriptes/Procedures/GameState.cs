using System;
using FishNet;
using UnityEngine;

public class GameState : State
{
    public override void Enter()
    {
        Debug.Log(" game State enter");
        SL.Get<CampService>().CurrentCamp.OnChange += OnCampChange;
        if (InstanceFinder.NetworkManager.IsServerStarted)
        {
             SL.Get<CampService>().InitializeTheCamp();
        }
       
    }

  

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        SL.Get<CampService>().CurrentCamp.OnChange -= OnCampChange;
    }

    private void OnCampChange(int prev, int next, bool asserver)
    {
        if (prev != next)
        {
            
        }
    }
}