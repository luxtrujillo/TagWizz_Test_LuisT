using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static GameObject _uiSplashScreen;
    private static GameObject _uiTitleScreen;
    private static GameObject _uiSelectorScreen;
    private static GameObject _uiCreditsScreen;
    private static GameObject _textCredits;
    private static GameObject _logoSpaceInvaders;
    private static GameObject _uiButtonLevel1;
    private static GameObject _uiButtonLevel2;
    private static GameObject _uiButtonCredits;

    private static List<GameObject> objectsToHide = new List<GameObject>();

    private void Awake()
    {
        _uiSplashScreen = transform.Find("UISplashScreen").gameObject;
        _uiTitleScreen = transform.Find("UITitleScreen").gameObject;
        _uiSelectorScreen = transform.Find("UISelectorScreen").gameObject;
        _uiCreditsScreen = transform.Find("UICreditsScreen").gameObject;
        _textCredits = _uiCreditsScreen.transform.Find("Text").gameObject;
        _logoSpaceInvaders = transform.Find("UITitleScreen").Find("Logo_SpaceInvaders").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        _uiSplashScreen.SetActive(true);
        _uiTitleScreen.SetActive(false);
        _uiSelectorScreen.SetActive(false);
        _uiCreditsScreen.SetActive(false);

        //Se almacenan en una lista todos los objetos que están dentro de UISelectorScreen.
        for (int i = 0; i < _uiSelectorScreen.transform.childCount; i++)
        {
            objectsToHide.Add(_uiSelectorScreen.transform.GetChild(i).gameObject);
        }
        objectsToHide.Add(_logoSpaceInvaders);
    }

    private void OnDisable()
    {
        objectsToHide.Clear();
    }

    public static void ShowTitleScreen(bool hide)
    {
        _uiTitleScreen.SetActive(true);
        if(hide)
            _uiSplashScreen.SetActive(false);
    }
    

    public static void ShowSelectorScreen()
    {
        //MOVE SpaceInvaders Logo
        _uiTitleScreen.transform.Find("Logo_SpaceInvaders").GetComponent<Animator>().SetBool("animate_position", true);
        _uiSplashScreen.gameObject.SetActive(false);
        _uiTitleScreen.transform.Find("Text_PressSpace").gameObject.SetActive(false);

        _uiSelectorScreen.SetActive(true);

        _uiSelectorScreen.transform.Find("ButtonLevel1").GetComponent<Animator>().SetBool("init_button_level", true);
        _uiSelectorScreen.transform.Find("ButtonLevel2").GetComponent<Animator>().SetBool("init_button_level", true);
    }


    public static void ShowCredits()
    {
        foreach (GameObject g in objectsToHide)
        {
            g.SetActive(false);
        }
        _uiCreditsScreen.SetActive(true);
    }

    public static void HideCredits()
    {
        foreach (GameObject g in objectsToHide)
        {
            g.SetActive(true);
        }
        _uiCreditsScreen.SetActive(false);
        _textCredits.transform.localPosition = new Vector3(0, -620, 0);
    }

    public static void MoveCredits(float f)
    {
        _textCredits.transform.Translate(0, f * Time.deltaTime, 0);
    }
    public static float GetCreditsPosition()
    {
        return _textCredits.transform.localPosition.y;
    }
}
