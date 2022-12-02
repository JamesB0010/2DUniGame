using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    #region fields 

    //a reference to the main image at the right of the main menu
    [SerializeField]
    private GameObject mainImage;

    //the maximum the main image can move to the left
    private int leftBound = 415;

    //the maximum the main image can move to the right
    private int rightBound = 659;

    private Camera mainCamera;

    //References to the start, help and quit buttons
    [SerializeField]
    private GameObject button1, button2, button3;

    //references to different image textures, when the cursor hovers over a button the mainImage will swap its texture to be the texture associated with the button
    [SerializeField]
    private Texture image1, image2, image3, image4;

    #endregion

    #region setup

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();

        //subscibe to the different events broadcasted by the different buttons
        button1.GetComponent<MainMenuPlayButton>().onEnter += button1Handler;
        button2.GetComponent<MainMenuHelpButton>().onEnter += button2Handler;
        button3.GetComponent<MainMenuQuitButton>().onEnter += button3Handler;
        button1.GetComponent<MainMenuPlayButton>().onExit += buttonExitHandler;
        button2.GetComponent<MainMenuHelpButton>().onExit += buttonExitHandler;
        button3.GetComponent<MainMenuQuitButton>().onExit += buttonExitHandler;

    }

    //the button handlers swap the texture of the main image to be some different texture and stop the coroutine waitBeforeHandleEvent from running
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


    //button exit swaps the main image texture to be its default texture
    public void buttonExit()
    {
        mainImage.GetComponent<RawImage>().texture = image1;
    }


    //the button exit handler is called when the mouse stops hovering over a main menu button, it calls the waitBeforeHandleExit coroutine
    public void buttonExitHandler()
    {
        StartCoroutine("waitBeforeHandleExit");
    }

    //the waitBeforeHandleExit coroutine waits for a second then calls the buttonExit function which resets the main image texture
    public IEnumerator waitBeforeHandleExit()
    {
        yield return new WaitForSeconds(1);
        buttonExit();
    }
    #endregion

    #region private methods

    void Update()
    {
        updateMainImagePos();
    }

    void updateMainImagePos()
    {
        float posX;
        float posY;

        //map the mouse input from a range of 0 - width of screen to be in the range of 1344 - 1450, this number will be able to move the image in the X axis as the mouse moves
        posX = ExtraFunctions.Map(Input.mousePosition.x, 0, mainCamera.pixelWidth, 1344, 1450);

        //this is the same as posX apart from it is for the Y axis
        posY = ExtraFunctions.Map(Input.mousePosition.y, 0, mainCamera.pixelHeight, 504, 586);

        //finally update the position of the main image
        mainImage.transform.position = new Vector3(posX, posY, 0);
    }

    #endregion
}
