using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class View : NetworkBehaviour
{
    private protected GameObject _defaultSelectedGameObject;

    [Header("Buttons")]
    [SerializeField] private Button returnButton;
    public Button submitButton;

    public bool IsInitialized { get; private set; }

    public GameObject TchatPanel;

    public virtual void Awake()
    {
        if (returnButton)
        {
            returnButton.onClick.AddListener(OnClick_Return);
        }

        TchatPanel = GameObject.FindGameObjectWithTag("TchatHistory");
    }

    public virtual void Initialize()
    {
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

        EventSystem.current.SetSelectedGameObject(_defaultSelectedGameObject);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void SubmitInput()
    {
        if (submitButton.interactable)
        {
            submitButton.onClick.Invoke();
        }
    }
}
