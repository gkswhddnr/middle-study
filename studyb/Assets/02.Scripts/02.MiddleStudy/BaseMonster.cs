using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//추상 클래스
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
