using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    private TitleSceneState state;
    private static int levelSelected;

    public static bool hideSplashScreen;

    public static int LevelSelected
    {
        get
        {
            return levelSelected;
        }
    }

    private void Awake()
    {
        state = TitleSceneState.SPLASH_SCREEN;

        if(hideSplashScreen)
        {
            StartCoroutine(ShowTitleScreen(0));
        }
        else
        {
            StartCoroutine(ShowTitleScreen(3.5f));
        }
    }

    private void Update()
    {
        if (state == TitleSceneState.TITLE_SCREEN)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpacePressed();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(0);
            }
        }
        else if (state == TitleSceneState.CREDITS_SCREEN)
        {
            if (UIController.GetCreditsPosition() < 112)
            {
                UIController.MoveCredits(50);
            }
            if (Input.GetMouseButton(0))
            {
                state = TitleSceneState.SELECTOR_SCREEN;
                UIController.HideCredits();
            }
        }
    }

    private void SpacePressed()
    {
        state = TitleSceneState.SELECTOR_SCREEN;
        UIController.ShowSelectorScreen();
    }

    IEnumerator ShowTitleScreen(float t)
    {
        //Se esperan 3.5 segundos para que termine la animación de la Splash Screen.
        yield return new WaitForSeconds(t);
        UIController.ShowTitleScreen(hideSplashScreen);
        state = TitleSceneState.TITLE_SCREEN;
    }

    //Metodos públicos que se llaman desde UIButtons
    public void ButtonCredits()
    {
        state = TitleSceneState.CREDITS_SCREEN;
        UIController.ShowCredits();

    }
    public void ButtonLevel(int level)
    {
        levelSelected = level;

        SceneManager.LoadScene("GameScene");
    }
}

public enum TitleSceneState
{
    SPLASH_SCREEN,
    TITLE_SCREEN,
    SELECTOR_SCREEN,
    CREDITS_SCREEN
}
