using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
