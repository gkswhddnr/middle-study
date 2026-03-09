using UnityEngine;
//정적함수 + 정적 변수
// static : 클래스에 하나만 존재하며 모든 인스턴스가 공유한다
// 인스턴스 생성 시 같이 생성되지 않는다
// static : 클래스에 하나만 존재하며 모든 오브젝트가 같은 값을 공유한다
public class Puppy : MonoBehaviour
{
    public string nickName;
    public float weight;
    public static int count = 0; //static 안 하면 1이 3개 찍혀나온다 

    private void Awake()
    {
        count += 1;
    }
    private void Start()
    {
        Bark();
    }
    public void Bark()
    {
        Debug.Log("모든 개들의 수:" + count);
        Debug.Log(nickName + ":Bark");
    }
  
    public static void ShowAnimalType()
    {
        Debug.Log("이것은 개입니다.");
    }
}
