using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goblin : BaseMonster
{
    public override void Attack()  //추상클래스에서 어택을 abstract 껍데기로만 만들었기 떄문에 BaseMonster를 상속받고 그 안에 껍데기들을 채워줘야한다.
    {
        Debug.Log("한 캐릭터를 공격했다. 공격력:" + damage);
       //  throw new System.NotImplementedException();
    }
}
