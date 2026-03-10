using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Action의 정의
public class Worker : MonoBehaviour
{
    // delegate void Work(); // void를 return하고 () 입력이 없는 함수는 굉장히 많기 때문에 using System;을 넣어서 Action을 쓰는거다

    // Action work;
    // → delegate void Work();
    //    Work work;

    // Action<int> work;
    // → delegate void Work(int a);
    //    Work work;

    // Action<int, int> work;
    // → delegate void Work(int a, int b);
    //    Work work;
    Action work;
   

    void MoveBricks()
    {
        Debug.Log("벽돌을 옮겼다");
    }
    void DigIn()
    {
        Debug.Log("땅을 팠다");
    }
    private void Start()
    {
        work += MoveBricks;
        work += DigIn;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            work();
        }
    }
}
