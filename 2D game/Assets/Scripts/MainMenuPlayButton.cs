using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuPlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnEnter();

    public event OnEnter onEnter;

    public delegate void OnExit();

    public event OnExit onExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit();
    }

    public void onButtonPress()
    {
        Application.LoadLevel("Main game");

    }
}