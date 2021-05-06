using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    private float velocity;
    private float movDelay;
    //Time in seconds between every shot.
    private float shootRate;

    private static bool canShoot;

    private BulletType bulletType;

    public bool CanShoot
    {
        get
        {
            return canShoot;
        }

        set
        {
            canShoot = value;
        }
    }

    public PlayerShipController(float v, float s)
    {
        velocity = v;
        shootRate = s;
    }

    // Start is called before the first frame update
    void Start()
    {

        CanShoot = true;

        bulletType = BulletType.SINGLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameController.State)
        {
            case GameState.IN_GAME:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (canShoot)
                    {
                        PlayerShot(bulletType);
                        StartCoroutine(ShootDelay());
                    }
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    if (transform.position.x > GameController.Bounds.leftBound)
                    {
                        transform.Translate(-velocity * Time.deltaTime, 0, 0);
                    }
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    if (transform.position.x < GameController.Bounds.rightBound)
                    {
                        transform.Translate(velocity * Time.deltaTime, 0, 0);
                    }
                }
            break;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Powerup_Triple(Clone)"))
        {
            Destroy(other.gameObject);
            bulletType = BulletType.TRIPLE;
            StartCoroutine(SetSingleBulletType(10));
        }
        else if (other.name.Equals("Powerup_Inteli(Clone)"))
        {
            Destroy(other.gameObject);
            bulletType = BulletType.INTELI;
        }
    }

    private void PlayerShot(BulletType t)
    {
        switch(t)
        {
            case BulletType.SINGLE:
                try
                {
                    var bullet = GameController.bulletList[0];
                    bullet.gameObject.SetActive(true);
                    GameController.bulletList.Remove(bullet);
                }
                catch
                {
                    Debug.Log("ArgumentOutOfRangeException: Without Bullets In List");
                }
                break;

            case BulletType.TRIPLE:
                try
                {
                    var bullet = GameController.bulletList[0];
                    bullet.gameObject.SetActive(true);
                    bullet.transform.rotation = Quaternion.Euler(0, 0, -30);
                    GameController.bulletList.Remove(bullet);

                    bullet = GameController.bulletList[1];
                    bullet.gameObject.SetActive(true);
                    GameController.bulletList.Remove(bullet);

                    bullet = GameController.bulletList[2];
                    bullet.gameObject.SetActive(true);
                    bullet.transform.rotation = Quaternion.Euler(0, 0, 30);
                    GameController.bulletList.Remove(bullet);
                }
                catch
                {
                    Debug.Log("ArgumentOutOfRangeException: Without Bullets In List");
                }
                break;

            case BulletType.INTELI:
                Instantiate(GameController.GetPowerUpInteli(), transform.position, Quaternion.identity);
                StartCoroutine(SetSingleBulletType(10));
                break;

            default:
                break;
        }
        
    }

    IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }

    IEnumerator SetSingleBulletType(float t)
    {
        yield return new WaitForSeconds(t);
        bulletType = BulletType.SINGLE;
    }

    #region PUBLIC_FUNCTIONS
    public void SetPlayerShip(float v, float s)
    {
        velocity = v;
        shootRate = s;
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
    }
    #endregion
}

enum BulletType
{
    SINGLE,
    TRIPLE,
    INTELI
}
