using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueInvader : Invader
{
    private void OnEnable()
    {
        hp = 2;
        score = 2;
    }
}
