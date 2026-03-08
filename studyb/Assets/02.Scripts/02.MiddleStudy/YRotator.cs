using UnityEngine;

public class YRotator : BaseRotaotr
{
    protected override void Rotate()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
