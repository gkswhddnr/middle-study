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

        nate.Stealth();
        nate.Print();

        jack.Hunt();
        jack.Print();

    }

}
