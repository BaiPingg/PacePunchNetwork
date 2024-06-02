using System;

public abstract class Timer
{
    public Action OnInit;
    public Action<float> OnUpdate;
    public Action OnComplete;
    protected bool init = false;

    public abstract bool Execute(float deltatime);

}