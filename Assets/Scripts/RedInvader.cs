using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedInvader : Invader
{
    private void OnEnable()
    {
        hp = 3;
        score = 3;
    }
}
