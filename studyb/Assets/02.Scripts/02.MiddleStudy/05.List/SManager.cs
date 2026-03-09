using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SManager : MonoBehaviour
{
    public List<int> scores = new List<int>();


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int randomNumber = Random.Range(0, 100);
            scores.Add(randomNumber);
        }
        if (Input.GetMouseButtonDown(1))
        {
            scores.RemoveAt(3);
            // RemoveAt();은 순번(인덱스)을 넣어주면 그 순번에 있는 요소가 빠진다.
            // 인덱스는 0부터 시작하므로 RemoveAt(3)은 4번째 요소가 삭제된다.
            // 4번째 요소가 빠지면 뒤에 있던 요소들이 한 칸씩 앞으로 당겨진다.
            // 즉 5번째 요소가 4번째 위치로 오게 된다.
            // 4번째 방에 있던 사람이 빠지고 5번째 사람이 옮겨오는 것이 아니라,
            // 4번째 방 자체가 사라져서 뒤에 있던 방들이 한 칸씩 앞으로 당겨진 것이다.

            // Remove();는 값을 넣어주면 리스트에서 그 값과 같은 요소를 찾아 삭제한다.
            // 같은 값이 여러 개 있어도 전부 삭제되는 것이 아니라
            // 가장 먼저 발견된 요소 하나만 삭제된다.
            // 예: [10, 20, 30, 20] 에서 Remove(20)을 하면
            // 첫 번째 20만 삭제되어 [10, 30, 20] 이 된다.

            // RemoveAt(index) : 인덱스(순번)로 삭제
            // Remove(value)   : 값으로 검색해서 삭제 (첫 번째로 발견된 값만 삭제)

            // Array(배열) → Length
            // List(리스트) → Count

        }
    }
}
