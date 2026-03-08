using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오버라이드 부모 클래스의 virtual 함수를 자식 클래스에서 새 동작으로 덮어쓰는 것
// virtual 이 함수는 기본 동작을 가지고 있지만, 자식 클래스가 원하면 바꿔서 쓸 수 있다
public class BaseRotaotr : MonoBehaviour
{
    public float speed = 60f;


    private void Update()
    {
        Rotate();
    }
    protected virtual void Rotate()  //자식들이 커스텀 해야하는 동작 방식만 virtual로 덮어씌운다.
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0); // 이 친구는 지워도 아무런 상관이 없다 결국 Rotate라는 껍데기만 있고 그 안에 것은 자식들이 커스텀해서 사용하기 때문
    }

}
