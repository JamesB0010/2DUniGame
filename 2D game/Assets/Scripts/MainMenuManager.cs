using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject mainImage;
    private int leftBound = 415;
    private int rightBound = 659;
    private Camera mainCamera;

    [SerializeField]
    private GameObject button1, button2, button3;

    [SerializeField]
    private Texture image1, image2, image3, image4;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();

        button1.GetComponent<MainMenuPlayButton>().onEnter += button1Handler;
        button2.GetComponent<MainMenuHelpButton>().onEnter += button2Handler;
        button3.GetComponent<MainMenuQuitButton>().onEnter += button3Handler;
        button1.GetComponent<MainMenuPlayButton>().onExit += buttonExitHandler;
        button2.GetComponent<MainMenuHelpButton>().onExit += buttonExitHandler;
        button3.GetComponent<MainMenuQuitButton>().onExit += buttonExitHandler;

    }
    public void button1Handler()
    {
        mainImage.GetComponent<RawImage>().texture = image2;
        StopCoroutine("waitBeforeHandleExit");
    }

    public void button2Handler()
    {
        mainImage.GetComponent<RawImage>().texture = image3;
        StopCoroutine("waitBeforeHandleExit");
    }

    public void button3Handler()
    {
        mainImage.GetComponent<RawImage>().texture = image4;
        StopCoroutine("waitBeforeHandleExit");
    }

    public void buttonExit()
    {
        mainImage.GetComponent<RawImage>().texture = image1;
    }

    public void buttonExitHandler()
    {
        StartCoroutine("waitBeforeHandleExit");
    }

    public IEnumerator waitBeforeHandleExit()
    {
        yield return new WaitForSeconds(1);
        buttonExit();
    }

    void Update()
    {
        updateMainImagePos();
    }

    void updateMainImagePos()
    {
        float posX;
        float posY;

        posX = ExtraFunctions.Map(Input.mousePosition.x, 0, mainCamera.pixelWidth, 1344, 1450);

        posY = ExtraFunctions.Map(Input.mousePosition.y, 0, mainCamera.pixelHeight, 504, 586);

        mainImage.transform.position = new Vector3(posX, posY, 0);
    }
}
