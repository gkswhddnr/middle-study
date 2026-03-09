using UnityEngine;

public class SetRotation : MonoBehaviour
{
    //쿼터니언은 (x,y,z,w)를 쓴다 쿼터니언을 이해할 필요는 없음 그냥 활용법만 익히기

    //Quaternion.Euler → Euler각(Vector3) → Quaternion으로 변환
    //eulerAngles → Quaternion → Euler각(Vector3)으로 변환
    // 언제나 z방향이 물체의 앞쪽이다.
    void Start()
    {
        Quaternion targetRotaiton = Quaternion.Euler(new Vector3(45, 0, 0));
        //  Vector3 dir = targetTransform.position - transform.position; 
        // 이걸 룩 로테이션 0,1,0대신 dir를 넣어주면 타겟 방향으로 바라본다.

        // Quaternion aRotation = Quaternion.Euler(new Vector3(30, 0, 0));
        //Quaternion bRotation = Quaternion.Euler(new Vector3(60, 0, 0));

        //Quaternion targetRotaiton = Quaternion.Lerp(aRotation, bRotation, 0.3f); //0.3의 결과는 약 39도
        // Lerp는 중간값을 구해준다.
        // a     |       b
        // 0    0.5      1 
        // 0.5f에 0을 넣으면 aRotation값이(30도) 가 나오고 1로 하면 60도가 나온다 
        // Quaternion newRotation = Quaternion.Euler(new Vector3(30, 30, 30));

        // Quaternion targetRotation = Quaternion.LookRotation(new Vector3(0, 1, 0)); 
        //방향을 넣어주면 해당방향으로 z축이 바라보게된다.

        // transform.rotation = Quaternion.Euler(new Vector3(30, 30, 30)); 
        // 이러면 벡터를 쿼터니언이라는 데이터 타입으로 바꿔버린다.


        transform.rotation = targetRotaiton;

        transform.Rotate(new Vector3(30, 0, 0));
        // transform.rotation : 회전을 직접 지정 transform.Rotate: 현재 회전에 회전을 추가

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
