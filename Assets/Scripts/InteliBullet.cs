using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteliBullet : Bullet
{
    private Vector3 startPosition;
    private float posX, posY;

    private void Awake()
    {
        Shoot();
    }

    private void Start()
    {
        startPosition = transform.position;
        posY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        posY += 0.05f;
        transform.position = startPosition + new Vector3(Mathf.Sin(Time.time * 10f) * 2, posY, 0.0f);

        if(transform.position.y > GameController.Bounds.upBound)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Hit()
    {
        Destroy(this.gameObject);
    }
}
