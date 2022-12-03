using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class UpgradeSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject popUpWindow;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ive been clicked");
        popUpWindow.GetComponent<Image>().enabled = true;
        floodHideShow(popUpWindow, true);
    }

    public void floodHideShow(GameObject root, bool show)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).GetComponent<Image>() != null)
            {
                root.transform.GetChild(i).GetComponent<Image>().enabled = show;

            }
            if (root.transform.GetChild(i).GetComponent<TextMeshProUGUI>() != null)
            {
                root.transform.GetChild(i).GetComponent<TextMeshProUGUI>().enabled = show;
            }
            if (root.transform.GetChild(i).transform.childCount > 0)
            {
                floodHideShow(root.transform.GetChild(i).gameObject, show);
            }
        }
    }
}
