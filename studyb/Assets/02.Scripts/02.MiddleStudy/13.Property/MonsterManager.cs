using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterManager : MonoBehaviour
{
    public int count
    {
        get
        {
           Monster[] monsters =  FindObjectsOfType<Monster>();
            return monsters.Length;
        }

    }
    //set을 안 만들었기 떄문에 바깥에서 =을 통해서 값을 집어 넣을 수가 없다.
    // 바깥에서 몬스터매니저의 카운트를 임이로 덮어쓰기가 불가능하기 떄문에 예외사항과 버그가 방지가된다.

}
