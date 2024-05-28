using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClosePanlAction")]
public class ClosePanlAction : Action
{
    public override void Act(StateController controller)
    {
        Debug.Log("close panel");
        ServiceLocator.Current.Get<UIService>().ClosePanel();
    }
}