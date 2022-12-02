using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuHelpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //setup an event for when the mouse hovers over the button
    public delegate void OnEnter();
    public event OnEnter onEnter;

    //setup an event for when the mouse stops hovering over the button
    public delegate void OnExit();
    public event OnExit onExit;

    //when the mouse hovers over the button call the onEnter event
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter();
    }

    //when the mouse stops hovering over the button call the onExit event
    public void OnPointerExit(PointerEventData eventData)
    {
        onExit();
    }
}
