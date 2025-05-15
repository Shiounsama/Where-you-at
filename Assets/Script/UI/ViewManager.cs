using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    [SerializeField] private bool autoInitialize;
    [SerializeField] private List<View> views = new List<View>();
    public View defaultView;

    private View _currentView;

    [Header("Transition fade")]
    [SerializeField] private CanvasGroup fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    public static bool IsFading = false;

    [Header("Faded background")]
    [SerializeField] private CanvasGroup fadedBackgroundImage;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (autoInitialize)
            Initialize();
    }

    /// <summary>
    /// Initialise chaque classe h�ritant de View.
    /// </summary>
    public void Initialize()
    {
        foreach (var view in views)
        {
            view.Initialize();
            view.Hide();
        }

        if (defaultView != null)
        {
            _currentView = defaultView;
            defaultView.Show();
        }
    }

    /// <summary>
    /// Affiche un panel TView héritant de View.
    /// </summary>
    /// <typeparam name="TView">Classe héritant de View.</typeparam>
    /// <param name="args"></param>
    public void Show<TView>(object args = null) where TView : View
    {
        // Debug.Log("ShowView");

        if (args != null)
        {
            HideAll();
            
            View v = args as View;
            _currentView = v;
            v.Show();

            return;
        }

        foreach (var view in views)
        {
            if (view is TView)
            {
                _currentView = view;

                view.Show();
            }
            else
            {
                view.Hide();
            }
        }
    }

    /// <summary>
    /// Cache tous les panels héritant de View.
    /// </summary>
    /// <param name="args"></param>
    public void HideAll(object args = null)
    {
        foreach (var view in views)
        {
            view.Hide();
        }
    }

    /// <summary>
    /// Renvoie un script TView héritant de View.
    /// </summary>
    /// <typeparam name="TView">Classe héritant de View.</typeparam>
    /// <param name="args"></param>
    /// <returns>Script T héritant de View.</returns>
    public TView GetView<TView>(object args = null) where TView: View
    {
        foreach (var view in views)
        {
            if (view is TView)
            {
                return (TView)view;
            }
        }

        return null;
    }

    public bool IsCurrentView<TView>(object args = null) where TView : View
    {
        return _currentView is TView;
    }

    /// <summary>
    /// Met à jour la liste des Views afin d'éviter que des Views qui n'existent plus soient présents.
    /// </summary>
    public void UpdateViewsList()
    {
        views.Clear();

        views = FindObjectsByType<View>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void AddView(View view)
    {
        views.Add(view);
    }

    public void RemoveView(View view)
    {
        views.Remove(view);

        Debug.Log("Removed a view");
    }

    public void Return()
    {
        _currentView.OnClick_Return();
    }

    #region Fade transition
    public void StartFadeIn()
    {
        IsFading = true;
        fadeImage.DOFade(1, fadeDuration).OnComplete(() => IsFading = false);
    }

    public void StartFadeOut()
    {
        IsFading = true;
        fadeImage.DOFade(0, fadeDuration).OnComplete(() => IsFading = false);
    }
    #endregion

    #region Faded background
    public void ShowFadedBackground(bool enable)
    {
        fadedBackgroundImage.alpha = enable ? 1 : 0;
    }
    #endregion
}
