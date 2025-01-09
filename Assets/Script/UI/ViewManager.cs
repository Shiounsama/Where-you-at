using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    [SerializeField] private bool autoInitialize;
    [SerializeField] private View[] views;
    [SerializeField] private View defaultView;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
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
            defaultView.Show();
    }

    /// <summary>
    /// Affiche un panel TView h�ritant de View.
    /// </summary>
    /// <typeparam name="TView">Classe h�ritant de View.</typeparam>
    /// <param name="args"></param>
    public void Show<TView>(object args = null) where TView : View
    {
        foreach (var view in views)
        {
            if (view is TView)
            {
                view.Show();
            }
            else
            {
                view.Hide();
            }
        }
    }

    /// <summary>
    /// Renvoie un script TView h�ritant de View.
    /// </summary>
    /// <typeparam name="TView">Classe h�ritant de View.</typeparam>
    /// <param name="args"></param>
    /// <returns>Script T h�ritant de View.</returns>
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
} 
