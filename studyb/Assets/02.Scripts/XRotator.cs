using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRotator : BaseRotaotr
{
    protected override void Rotate()
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0);
    }

}
