using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    [SerializeField] private bool autoInitialize;
    [SerializeField] private List<View> views = new List<View>();
    [SerializeField] private View defaultView;

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
    /// Cache tous les panels h�ritant de View.
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

    /// <summary>
    /// Met � jour la liste des Views afin d'�viter que des Views qui n'existent plus soient pr�sents.
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
} 
