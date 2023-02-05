using UnityEngine;

public abstract class GameplayManagerBase : MonoBehaviour
{
    [SerializeField] protected GameObject _panel;

    public virtual void Init() { }
    
    public void Show()
    {
        _panel.SetActive(true);
        OnShow();
    }

    public void Hide()
    {
        OnHide();
        _panel.SetActive(false);
    }
    
    protected virtual void OnShow() { }
    protected virtual void OnHide() { }
}
