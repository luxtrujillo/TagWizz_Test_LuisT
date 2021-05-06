using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : Bullet
{
    private float velocity;

    private bool canShootIt;
    private void Awake()
    {
        canShootIt = false;
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        canShootIt = true;
        Shoot();
    }
    private void OnDisable()
    {
        canShootIt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canShootIt)
        {
            transform.Translate(0, velocity * Time.deltaTime, 0);
            if (transform.position.y > GameController.Bounds.upBound)
            {
                Destroy();
            }
        }
    }

    public void SetBullet(float v)
    {
        velocity = v;
    }
}
