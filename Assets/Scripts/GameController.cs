using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    #region LEVEL_SETTINGS
    [Space(10)]
    [Header("Level Settings:")]
    [SerializeField] private int maxColumns;
    [SerializeField] private GameObject powerUpTriplePrefab;
    [SerializeField] private GameObject powerUpInteliPrefab;
    #endregion

    #region Player_Settings
    [Space(10)]
    [Header("PlayerShip Settings:")]
    [SerializeField] private GameObject playerShipPrefab;
    [Header("PlayerShip Side Velocity")]
    [SerializeField] private float playerVelocity;
    [Header("Time between shots in seconds")]
    [SerializeField] private float playerShootRate;
    #endregion

    #region Bullet_Settings
    [Space(10)]
    [Header("Bullet Settings:")]
    [Space(10)]
    [SerializeField] private GameObject singleBulletPrefab;
    [SerializeField] private float bulletVelocity;
    [Space(10)]
    [SerializeField] private GameObject inteliBulletPrefab;
    [SerializeField] private float inteliBulletVelocity;
    #endregion

    #region Invader_Settings
    [Space(10)]
    [Header("Invaders:")]
    [SerializeField] private GameObject[] arrayInvadersPrefabs;
    [Header("Invaders velocity (1 position each 'movementRate' seconds):")]
    [SerializeField] private float movementRate;
    #endregion

    #region UI_SCREENS
    [Space(10)]
    [Header("UI:")]
    [SerializeField] private GameObject _uiEndScreen;
    #endregion

    private static GameState state;
    private static int invaderCounter;
    private static int score;
    private PlayerShipController player;

    #region PUBLIC_FIELDS
    public static List<Bullet> bulletList = new List<Bullet>();
    public static GameState State
    {
        get
        {
            return state;
        }
    }
    public struct Bounds
    {
        public static float leftBound;
        public static float rightBound;
        public static float upBound;
    }
    #endregion

    private void Awake()
    {
        _uiEndScreen.SetActive(false);
        LoadLevel();
        state = GameState.IN_GAME;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Scene...");
        Debug.Log("LEVEL -> " + TitleSceneController.LevelSelected);
        

    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case GameState.LOADING:
                break;

            case GameState.IN_GAME:
                break;
        }
    }

    private void OnDisable()
    {
        bulletList.Clear();
    }

    private void LoadLevel()
    {
        state = GameState.LOADING;

        //Create Player
        player = Instantiate(playerShipPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerShipController>();
        player.SetPlayerShip(playerVelocity, playerShootRate);

        //Create bullets for the pool.
        for (int i = 0; i < 10; i++)
        {
            SingleBullet sb = Instantiate(singleBulletPrefab, Vector3.zero, Quaternion.identity).GetComponent<SingleBullet>();
            sb.SetBullet(bulletVelocity);
            bulletList.Add(sb);
        }

        //The limits of the game are defined.
        Bounds.leftBound = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        Bounds.rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        Bounds.upBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;

        //Create Level
        TextAsset text;
        string t;
        try
        {
            switch (TitleSceneController.LevelSelected)
            {
                case 1:
                    text = Resources.Load("level1") as TextAsset;
                    break;

                case 2:
                    text = Resources.Load("level2") as TextAsset;
                    break;

                default:
                    text = Resources.Load("level1") as TextAsset;
                    break;
            }
            t = text.text.Replace("\r\n", "");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            t = "3333333222222211111111111111";
        }
        
        

        Invader invader;
        float offsetX = 2;
        float offsetY = 1;
        Vector2 invaderPosition = new Vector2(Bounds.leftBound, Bounds.upBound + (Mathf.Round(t.Length/maxColumns) * offsetY) - 0.5f);
        invaderCounter = 0;
        int charCounter = 0;
        for (int i = 0; i < t.Length; i++)
        {
            //Debug.Log("Char " + i + ": " + t[i]);
            switch (t[i])
            {

                case '1':
                    invader = Instantiate(arrayInvadersPrefabs[0], invaderPosition, Quaternion.identity).GetComponent<Invader>();
                    invader.SetInvader(movementRate);
                    invader.Init();
                    invaderCounter++;
                    break;

                case '2':
                    invader = Instantiate(arrayInvadersPrefabs[1], invaderPosition, Quaternion.identity).GetComponent<Invader>();
                    invader.SetInvader(movementRate);
                    invader.Init();
                    invaderCounter++;
                    break;

                case '3':
                    invader = Instantiate(arrayInvadersPrefabs[2], invaderPosition, Quaternion.identity).GetComponent<Invader>();
                    invader.SetInvader(movementRate);
                    invader.Init();
                    invaderCounter++;
                    break;

                default:
                    break;
            }

            invaderPosition.x += offsetX;

            charCounter++;
            if (charCounter >= maxColumns)
            {
                charCounter = 0;
                invaderPosition.y -= offsetY;
                invaderPosition.x = Bounds.leftBound;
            }
        }
        
        score = 0;
    }

    public static void GameOver(bool win, int s)
    {
        state = GameState.GAME_OVER;
        GameController g = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
        g._uiEndScreen.transform.Find("TextScoreValue").GetComponent<Text>().text = score.ToString();
        g._uiEndScreen.SetActive(true);
        
        if (win)
        {
            g._uiEndScreen.transform.Find("TextEnd").GetComponent<Text>().text = "Player Wins";
        }
        else
        {
            Debug.Log("GAME_OVER");
            g.player.GameOver();
            Invader[] invadersArray;
            invadersArray = GameObject.FindObjectsOfType(typeof(Invader)) as Invader[];
            foreach(Invader i in invadersArray)
            {
                i.Stop();
            }
        }
    }

    public static void SetScore(int s)
    {
        score += s;
        invaderCounter -= 1;
        if (invaderCounter <= 0)
        {
            GameOver(true, score);
        }
    }

    public static void CreatePowerUp(string s, Vector3 position)
    {
        GameController g = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
        if (s.Equals("triple"))
        {
            Instantiate(g.powerUpTriplePrefab, position, Quaternion.identity);
        }
        else if (s.Equals("inteli"))
        {
            Instantiate(g.powerUpInteliPrefab, position, Quaternion.identity);
        }
    }

    public static GameObject GetPowerUpInteli()
    {
        GameController g = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
        return g.inteliBulletPrefab;
    }

    #region UI_BUTTONS
    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Title()
    {
        TitleSceneController.hideSplashScreen = true;
        SceneManager.LoadScene("TitleScene");
    }
    #endregion
}

public enum GameState
{
    LOADING,
    IN_GAME,
    PAUSED_GAME,
    GAME_OVER
}
