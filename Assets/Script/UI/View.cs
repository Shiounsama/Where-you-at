using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool IsInitialized { get; private set; }

    public virtual void Initialize()
    {
        if (IsInitialized)
            return;

        IsInitialized = true;
    }

    public virtual void Show(object args = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
