using UnityEngine;

//델리게이트(delegate)
public class Calculator : MonoBehaviour
{

    delegate float Calculate(float a, float b); //이건 그냥 타입

    Calculate onCalculate;

    private void Start()
    {
        onCalculate = Sum; //Sum();를 안 하는 이유는 Sum이라는 함수를 명부에 등록시킬 뿐이지 함수를 발동 시키는 것이 아니기 떄문
        onCalculate += Subtract;
        onCalculate += Multiply;
    }

    public float Sum(float a, float b)
    {
        Debug.Log(a + b);
        return a + b;
    }

    public float Subtract(float a, float b)
    {
        Debug.Log(a - b);
        return a - b;
    }

    public float Multiply(float a, float b)
    {
        Debug.Log(a * b);
        return a * b;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Sum(1,10)
           Debug.Log("결과값" + onCalculate(1, 10)); //리턴값은 마지막으로 실행된 친구의 리턴값만 가져온다 그래서 Multiply값이 디버그 로그로 나옴
        }
    }
}

