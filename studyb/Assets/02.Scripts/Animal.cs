using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상속 
public class Animal
{

    public string name;
    public float weight;
    public int year;

    public void Print()
    {
        Debug.Log(name +"| 몸무게:" + weight +"| 나이:" + year);
    }

    protected float GetSpeed() //protected는 바깥에서는 안 보이지만 상속을 받은 애들은 보인다. 자식에겐 public 외부에겐 private
    {
        return CalcSpeed();
    }

    private float CalcSpeed()
    {
          return 100f / (weight * year);
        

    }

}
//샌드박스 패턴: 미리 필요한 기능들을 부모 클래스에 몰아넣고 자식클래스에서 부모 클래스에 기능을 마구잡이로 조합해서 자기만의 기능을 만드는 것 
public class Dog:Animal //이 dog는 animal 클래스를 상속한다  
{
    public void Hunt() //부모 클래스 도구들로 Hunt 함수 만들기
    {
        //부모 클래스에서 private로 선언한것은 자식클래스에게 보이지 않는다.

        float speed = GetSpeed();
        Debug.Log(speed + "의 속도로 달려가서 사냥했다");

        weight += 10f;
    }
}

public class Cat : Animal
{
    public void Stealth()
    {
        Debug.Log("숨었다");
    }
}
