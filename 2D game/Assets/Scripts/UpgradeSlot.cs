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
        floodHide(popUpWindow);
    }

    private void floodHide(GameObject root)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            if (root.transform.GetChild(i).GetComponent<Image>() != null)
            {
                root.transform.GetChild(i).GetComponent<Image>().enabled = true;

            }
            if (root.transform.GetChild(i).GetComponent<TextMeshProUGUI>() != null)
            {
                root.transform.GetChild(i).GetComponent<TextMeshProUGUI>().enabled = true;
            }
            if (root.transform.GetChild(i).transform.childCount > 0)
            {
                floodHide(root.transform.GetChild(i).gameObject);
            }
        }
    }
}
