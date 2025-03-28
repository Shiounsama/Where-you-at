using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button returnButton;

    public bool IsInitialized { get; private set; }

    public virtual void Initialize()
    {
        if (returnButton)
        {
            returnButton.onClick.AddListener(OnClick_Return);
        }

        if (IsInitialized)
            return;

        IsInitialized = true;
    }

    #region Button Events
    public virtual void OnClick_Return() { }
    #endregion

    public virtual void Show(object args = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
