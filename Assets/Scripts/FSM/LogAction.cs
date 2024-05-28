
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableSM/Actions/LogAction")]
public class LogAction : Action
{
    [SerializeField] private string _message;
    public override void Act(StateController controller)
    {
        Debug.Log(_message);
    }
}
