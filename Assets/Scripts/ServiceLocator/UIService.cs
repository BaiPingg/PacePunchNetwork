using System.Collections;
using System.Collections.Generic;

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;


public class UIService : MonoBehaviour, IService
{
    public Canvas rootCanvas;
    public Stack<UIPanel> openedPanels = new Stack<UIPanel>();

    public UIPanel Current
    {
        get => openedPanels.Peek();
    }

    public async Task<UIPanel> OpenPanel(string path)
    {
        var res = ServiceLocator.Current.Get<ResourcesSerive>();
        GameObject target = await res.LoadAssetAsync<GameObject>(path).Task;
        var panel = Instantiate(target).GetComponent<UIPanel>();
        return OpenPanel(panel);
    }

    public UIPanel OpenPanel(UIPanel panel)
    {
        UIPanel prePanel;
        openedPanels.TryPeek(out prePanel);
        if (prePanel)
        {
            prePanel.OnCover();
        }

        openedPanels.Push(panel);
        panel.transform.SetParent(rootCanvas.transform);
        panel.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        panel.transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        panel.transform.localScale = Vector3.one;

        panel.OnOpen(this);
        return panel;
    }

    public void ClosePanel()
    {
        var panel = openedPanels.Pop();
        panel.OnClose();
        Destroy(panel.gameObject);
        UIPanel prePanel;
        openedPanels.TryPeek(out prePanel);
        if (prePanel)
        {
            prePanel.OnResume();
        }
    }


  
}