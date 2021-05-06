using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IShootable
{
    PlayerShipController playerShip;


    public void Shoot()
    {
        playerShip = GameObject.FindObjectOfType(typeof(PlayerShipController)) as PlayerShipController;
        transform.position = playerShip.transform.Find("ShootPoint").position;
    }

    public virtual void Hit()
    {
        Destroy();
    }

    public void Destroy()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.SetActive(false);
        GameController.bulletList.Add(this);
    }
}
