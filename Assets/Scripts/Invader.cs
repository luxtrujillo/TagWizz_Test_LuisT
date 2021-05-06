using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour , IDestructible
{
    protected int hp;
    protected int score;

    private float velocity;
    private float horizontalPosition;
    private float verticalPosition;
    private bool canMove;
    private bool moveDown;
    private bool moveRight;
    private bool gameOver;

    private int numMov;

    private void Awake()
    {
        canMove = false;
        moveDown = true;
        moveRight = true;
        gameOver = false;
        horizontalPosition = transform.position.x;
        verticalPosition = transform.position.y;

        numMov = 5;
    }

    private void Update()
    {
        if(canMove && !gameOver)
        {
            canMove = false;
            if (moveDown)
            {
                verticalPosition -= 1f;
                moveDown = !moveDown;
                if (transform.position.y <= 1)
                {
                    Win();
                }
            }
            else
            {
                if (moveRight)
                {
                    horizontalPosition += 1;
                    numMov--;
                }
                else
                {
                    horizontalPosition += -1;
                    numMov--;
                }
                
            }
            transform.position = new Vector3(horizontalPosition, verticalPosition, 0);
            StartCoroutine(MovementDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var impact = other.GetComponent<IShootable>();
        if (impact == null) return;
        impact.Hit();
        Damage();
    }

    private void Win()
    {
        GameController.GameOver(false, 0);
        StopAllCoroutines();
    }

    public void SetInvader(float v)
    {
        velocity = v;
    }

    public void Init()
    {
        StartCoroutine(MovementDelay());
    }

    public void Damage()
    {
        StartCoroutine(DamageAnim());
        hp--;
        if (hp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        if (Random.Range(0, 20) == 1)
        {
            GameController.CreatePowerUp("triple", transform.position);
        }
        else if (Random.Range(0, 20) == 2)
        {
            GameController.CreatePowerUp("inteli", transform.position);
        }

        GameController.SetScore(score);
        StartCoroutine(DeathAnim());
    }

    public void Stop()
    {
        canMove = false;
        gameOver = true;
    }

    private IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(velocity);
        canMove = true;
        
        if(numMov <= 0)
        {
            moveDown = !moveDown;
            moveRight = !moveRight;
            numMov = 4;
        }
            
    }

    private IEnumerator DamageAnim()
    {
        Color c = transform.GetChild(0).GetComponent<Renderer>().material.color;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.color = c;
        }
    }
    private IEnumerator DeathAnim()
    {
        transform.GetComponent<Animator>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
            transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Random.Range(-4, 5), Random.Range(-4, 5), 0, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
