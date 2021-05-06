using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    void Update()
    {
        transform.Translate(0, -2 * Time.deltaTime, 0, Space.World);
        transform.Rotate(0, 0, 150 * Time.deltaTime);
    }
}
