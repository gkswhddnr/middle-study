using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalTest : MonoBehaviour
{
    void Start()
    {
        Cat nate = new Cat(); // new라는 키워드는 뒤에 클래스가 들어가면 메모리 상에 그 클래스를 찍어내는 것
        nate.name = "Nate";
        nate.weight = 1.5f;
        nate.year = 3;

        Dog jack = new Dog();
        jack.name = "Jack";
        jack.weight = 5f;
        jack.year = 2;

        // Animal someAnimal = jack; 
        /*여기서는 jack을 Animal로서 다루고 있기 떄문에 Animal의 기능인 Print는 사용 가능하지만 Dog의 기능인 Hunt는 사용이 불가능하다 
          물론 사용이 불가능한거지 메모리상에서 Dog의 기능이 사라지는것은 아니다.*/


        Animal[] animals = new Animal[2];

        animals[0] = nate;
        animals[1] = jack;

        for (int i = 0; i < animals.Length; i++)
        {
            animals[i].Print(); 
            // 이렇게 상속의 다형성이라는 특성을 사용해서 공통된 Base에서 여러가지로 파생된 아이들을 한번에 관리할 수도 있다.
            // 당연하겠지만 여기서는 Cat의 스텔스기능과 Dog의 헌트기능은 사용이 불가능하다 Animal 클래스의 기능만 사용 가능
        }


    }

}
