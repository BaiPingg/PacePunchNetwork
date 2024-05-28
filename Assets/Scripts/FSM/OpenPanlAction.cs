using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/OpenPanlAction")]
public class OpenPanlAction : Action
{
    public UIPanel target;

    public override void Act(StateController controller)
    {
        if (target == null)
        {
            return;
        }

        var panel = Instantiate(target.gameObject).GetComponent<UIPanel>();

        ServiceLocator.Current.Get<UIService>().OpenPanel(panel);
    }
}