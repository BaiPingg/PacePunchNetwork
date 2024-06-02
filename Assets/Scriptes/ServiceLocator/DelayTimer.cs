public class DelayTimer : Timer
{
    private float _delay;
    private float timer;

    public DelayTimer(float delay)
    {
        _delay = delay;
        timer = 0f;
    }

    public override bool Execute(float deltatime)
    {
        if (init == false)
        {
            OnInit?.Invoke();
        }

        timer += deltatime;
        OnUpdate?.Invoke(timer);
        if (timer >= _delay)
        {
            OnComplete?.Invoke();
            return true;
        }

        return false;
    }
}