/*
ViewManager
Used on:    GameObject
For:    Defines methods inherent to a View and shows the chosen View at the start of the game
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager _instance;

    [SerializeField] private View startingView; // The menu we begin on
    [SerializeField] private View[] views;      // List of all menus

    private View currentView;                   // Pretty self-explanatory

    private readonly Stack<View> history = new Stack<View>();   // Stack for history of views, so we can easily go back

    public static T GetView<T>() where T : View              // I'm not sure if some of these syntactical choices are necessary or preference
    {                                                       // of the tutorial maker, but the code is clear, so sure
        for (int i = 0; i < _instance.views.Length; i++)     // Method is used to retrieve a desired view
        {
            if (_instance.views[i] is T tView)
            {
                return tView;
            }
        }

        return null;
    }   // End of GetView

    public static void Show<T>(bool remember) where T : View // This was made as bool remember = true in the tutorial, so that half the time
    {                                                       // you don't need to input the paramenter, but I'm doing it differently for
        for (int i = 0; i < _instance.views.Length; i++)     // internal consistency
        {                                                   // Method is used to hide the current view and show a new one
            if (_instance.views[i] is T)
            {
                if (_instance.currentView != null)   // If the current view isn't null
                {
                    if (remember)   // If we marked that we want to remember the current view for later
                    {
                        _instance.history.Push(_instance.currentView);
                    }

                    _instance.currentView.Hide();   // Hide the current view if we need to
                }

                _instance.views[i].Show();          // Show the view we want shown, and make it the current view
                _instance.currentView = _instance.views[i];
            }
        }
    }   // End of Show<T>

    /*public static void ShowFade<T>(bool remember) where T : View // This was made as bool remember = true in the tutorial, so that half the time
    {                                                       // you don't need to input the paramenter, but I'm doing it differently for
        for (int i = 0; i < _instance.views.Length; i++)     // internal consistency
        {                                                   // Method is used to hide the current view and show a new one
            if (_instance.views[i] is T)
            {
                if (_instance.currentView != null)   // If the current view isn't null
                {
                    if (remember)   // If we marked that we want to remember the current view for later
                    {
                        _instance.history.Push(_instance.currentView);
                    }

                    _instance.currentView.Hide();   // Hide the current view if we need to
                }

                _instance.views[i].Show();          // Show the view we want shown, and make it the current view
                _instance.currentView = _instance.views[i];
                _instance.currentView.GetComponent<FadeUI>().UIFadeIn();
            }
        }
    }  */ // End of ShowFade<T>

    public static void Show(View view, bool remember)   // Same purpose as above, different implementation
    {
        if (_instance.currentView != null)   // If the current view isn't null
        {
            if (remember)   // If we marked that we want to remember the current view for later
            {
                _instance.history.Push(_instance.currentView);
            }

            _instance.currentView.Hide();   // Hide the current view if we need to
        }

        view.Show();          // Show the view we want shown, and make it the current view
        _instance.currentView = view;
    }   // End of Show

    public static void ShowLast()   // Used to go back in time through our history stack and show a previous View
    {
        if (_instance.history.Count != 0) // If it is possible to go back
        {
            Show(_instance.history.Pop(), false);
        }
    }

    /*public static void ShowLastFade()   // DO NOT USE THIS ANYWHERE BUT CROSSFADING INVENTORY AND BATTLE UI IN BATTLE
    {
        if (_instance.history.Count != 0) // If it is possible to go back
        {
            _instance.history.Pop();
            ShowFade<BattleUIView>(false);

        }
    }*/

    // ---------------------------
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].Initialize();
            views[i].Hide();
        }

        if (startingView != null)
        {
            Show(startingView, true);
        }
    }

}   // End of class

// https://www.youtube.com/watch?v=rdXC2om16lo Tutorial used
