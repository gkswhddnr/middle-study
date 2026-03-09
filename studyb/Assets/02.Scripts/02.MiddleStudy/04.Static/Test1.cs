using UnityEngine;

public class Test1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Dog의 총 갯수:" + Puppy.count);
        //static으로 count를 해놨기 떄문에 단 하나에 count를 그냥 바로 가져와서 쓸 수 있다.
        Puppy.ShowAnimalType(); // 이 함수도 마찬가지 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
