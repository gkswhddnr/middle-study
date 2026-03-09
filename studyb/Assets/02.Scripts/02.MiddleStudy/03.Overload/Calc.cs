using UnityEngine;

public class Calc : MonoBehaviour
{
   void Start()
    {
        Debug.Log(Sum(1,1));
        Debug.Log(Sum(3, 3, 3));
        Debug.Log(Sum(3.2f, 3.44f));
        Debug.Log(Sum(3.14f, 3.14f, 3.14f));

    }

    public int Sum(int a,int b)
    {
        return a + b;
    }
    public int Sum(int a,int b,int c)
    {
        return a + b + c;

    }

    public float Sum(float a, float b , float c)
    {
        return a + b + c;

    }
    public float Sum(float a, float b)
    {
        return a + b;
    }
}
