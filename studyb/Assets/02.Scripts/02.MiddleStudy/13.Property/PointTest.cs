using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTest : MonoBehaviour
{
    public PointManager pointManager;
    public MonsterManager monsterManager;

    private void Start()
    {
        /*  pointManager.point = -100;
          Debug.Log("현재 점수:" + pointManager.point);

          pointManager.point = 100;
          Debug.Log("현재 점수:" + pointManager.point); */

        Debug.Log(monsterManager.count);
       // monsterManager.count = 0; count에 set을 설정 안 했기 떄문에 수정할 수 없음 

    }
}
