/*
View
Used on:    ---
For:    Establishes the basic methods inherent to a "View" - one GameObject holding all the components that
        compose one distinct menu
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract void Initialize();

    public virtual void Hide() => gameObject.SetActive(false);

    public virtual void Show() => gameObject.SetActive(true);

    // For future reference: abstract means that it needs to be defined by any children,
    // virtual means there is an implementation present but the option exists for any children to override it
}

// https://www.youtube.com/watch?v=rdXC2om16lo Tutorial used
