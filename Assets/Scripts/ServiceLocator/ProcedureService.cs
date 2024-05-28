using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureService : StateController,IService
{

    public void OnGameStart()
    {
        TransitionToState(startState);
    }
}
