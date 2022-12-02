using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseupgradeUi : MonoBehaviour
{

    [SerializeField]
    private GameObject backgroundPanel;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().OnRaceOver += onRaceOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onRaceOver()
    {
        StartCoroutine("helloGUI");
    }

    public IEnumerator helloGUI()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void continueButtonPressed()
    {
        FindObjectOfType<GameManager>().NewRace();
    }
}
