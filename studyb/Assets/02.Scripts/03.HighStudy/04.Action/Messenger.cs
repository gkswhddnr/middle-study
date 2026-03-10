using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 람다함수의 정의
//람다식은 클릭 코드를 구현하기 위해 다음과 같은 상황에서 적극 활용한다
// 1. 한 두줄로 이루어진 매서드 또는 프로퍼티 
// 2. 특히 한줄로 이루어진 get 프로퍼티가 존재하는 경우
public class Messenger : MonoBehaviour
{
    public delegate void Send(string reciever);

    Send onSend;

    private void Start()
    {
        onSend += SendMail;
        onSend += SendMoney;
        onSend += man => Debug.Log("Assainate" + man);  //람다함수는 이름이 없는 함수 

       // onSend += (string man) =>  {Debug.Log("Assainate" + man); Debug.Log("Hide Body"); };
        //두줄씩 쓰면 중괄호를 사용해야됨 위에는 (string man) 이렇게 안 한 이유는 c#컴파일러가 똑똑해서 알아서 string처리를 했기때문
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))

        {
            onSend("JongWook");
        }
    }

    void SendMail(string reciever)
    {
        Debug.Log("Mail sent to:" + reciever);
    }

    void SendMoney(string reciever)
    {
        Debug.Log("Money sent to:" + reciever);
    }

}
