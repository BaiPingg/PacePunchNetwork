using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public  class UIPanel : UIBehaviour
{
    public CanvasGroup canvasGroup;
    
    public virtual void OnOpen(UIService service)
    {
    }

    public virtual void OnClose()
    {
    }
    public virtual void OnCover()
    {
        canvasGroup.alpha = 0;
    }
    public virtual void OnPause()
    {
      
    }
    public virtual void OnResume()
    {  canvasGroup.alpha = 1;
    }
}