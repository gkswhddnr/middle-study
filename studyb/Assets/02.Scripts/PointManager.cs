using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    //프로퍼티 변수처럼 사용하지만 내부에는 어떤 처리가 들어갈 수 있다.
    // int a = point; GET
    // point = 100; SET  이퀄을 통해 받아드리는 값을 value라는 키워드를 통해 가져온다
    
    public int point
    {
        get
        {
            Debug.Log(m_point);
            return m_point;
        }

        set
        {
            if(value < 0)
            {
                m_point = 0;
            }
            else
            {
                m_point = value;
            }
        }
    }
   private int m_point = 10;


}
