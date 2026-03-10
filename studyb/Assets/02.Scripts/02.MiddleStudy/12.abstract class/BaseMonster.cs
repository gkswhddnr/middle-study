using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    public float damage = 100f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    public abstract void Attack();


}
// 추상 클래스 (abstract class)
// 완전히 구현되지 않은 "기본 설계 클래스"
// 직접 객체를 만들 수 없고 반드시 상속해서 사용해야 한다.

// 특징
// 1. new로 직접 생성할 수 없다.
// 2. 자식 클래스가 상속해서 사용한다.
// 3. abstract로 선언된 함수는 자식 클래스가 반드시 구현해야 한다.
// 4. 일반 변수와 일반 함수도 같이 가질 수 있다.

// 인터페이스와 차이
// interface : 기능의 "형태(함수 선언)"만 정의
// abstract class : 기본 기능 + 강제 구현 함수 둘 다 가능
// 즉, 인터페이스와 일반 클래스의 중간 개념이라고 볼 수 있다.