using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelloCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("HelloUnity");//9번쨰줄이 시작하자마자 바로 10번쨰로 넘어감 
        StartCoroutine(HiCSharp());
        Debug.Log("End");

        //직관적으로 봤을떈 순서대로 찍혀야하지만 코루틴은 비동기방식 코루틴은 여러작업을 동시에 할 수 있음

        //동기방식 A 작업 끝 → B 작업 시작 -> B 작업 끝 → C 작업 시작
        //비동기방식: 끝나는 시점을 안 알려줌 이걸 이용해서 멀티스레드 방식을 구현할 수 있음(작업은 순서대로 시작하겠지만 작업이 겹치는 시간이 생기기 떄문)
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine("HelloUnity"); //코루틴을 함수가아닌 스트링으로 지정하면 이렇게 코루틴을 중간에 멈출 수도 있다. 
        }
    }

    IEnumerator HelloUnity()
    {
        while (true) //원래 이렇게하면 무한으로 빠르게 돌아가서 터지지만 중간에 3초대기시간으로 인해 터지지않음
        {
            yield return new WaitForSeconds(3f); //yield return null; 쉬는시간을 지정하지 않으면 대충 한프레임(1/60)을 쉬게된다 
            Debug.Log("Hello Unity");
        }
   
    }

    IEnumerator HiCSharp()
    {
        Debug.Log("Hi");
        yield return new WaitForSeconds(5f);
        Debug.Log("CSharp");


    }

}
