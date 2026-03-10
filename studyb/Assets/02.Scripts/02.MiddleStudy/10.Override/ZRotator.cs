using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZRotator : BaseRotaotr
{

    protected override void Rotate() //오버라이드를 할때는 함수의 형식이 같아야한다 protected면 protected로 이름이 Rotate면 Rotate로
    {
        // base.Rotate();
        // base를 활용해서 아예 새로 쓰는것이 아닌 부모에서 쓰던 거에 덭붙여서 사용할 수도 있다.
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }


}



