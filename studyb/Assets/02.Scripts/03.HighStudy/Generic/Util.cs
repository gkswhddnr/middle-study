using UnityEngine;

public class Util : MonoBehaviour
{
    private void Start()
    {
        Container<string> container = new Container<string>(); //제너릭을 사용했기 때문에 <>로 타입을 명시
        container.messages = new string[3];

        container.messages[0] = "Hello";
        container.messages[1] = "Hell";
        container.messages[2] = "Hel";

        for (int i = 0; i < container.messages.Length; i++)
        {
            Debug.Log(container.messages[i]);
        }

        Container<int> container2 = new Container<int>(); 
        container2.messages = new int[3];

        container2.messages[0] = 1;
        container2.messages[1] = 2;
        container2.messages[2] = 3;

        for (int i = 0; i < container2.messages.Length; i++)
        {
            Debug.Log(container2.messages[i]);
        }
        //  Print<int>(30);
        //  Print<string>("Hello World");  //
        // 이렇게 <>안에 타입만 명시해주면 같은함수로 다양하게 사용가능함 오버라이드나 함수이름을 다 다르게해서 하나하나 만들 필요가없음
    }
   /* public void Print<T>(T inputMessage) //제네릭문법에서 <>안의 이름은 상관이 없음
    {
        Debug.Log(inputMessage);
    }*/
}

public class Container<T>
{
    public T[] messages; //String대신 제너릭을 사용한다. //제너릭은 제한도 걸 수도 있긴하다.
}
