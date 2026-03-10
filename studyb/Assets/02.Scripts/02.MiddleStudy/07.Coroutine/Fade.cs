using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class Fade : MonoBehaviour
{
    public Image fadeImage;

    void Start()
    {
        //코루틴을 사용할때는 start에  StartCoroutine(함수 or string문장)을 넣는다  
        StartCoroutine(FadeIn());
        // StartCoroutine("FadeIn");  // 이렇게 스트링 문장으로 넣어서 코루틴을 사용하면 중간에 인위적으로 멈출 수 있다
        

    }

    //for문으로 그냥 fade in out 기능을 만들면 너무 빨리 동작해서 서서히 줄어들게 할 수 없기 떄문에 코루틴을 사용해서 대기시간을 추가
    IEnumerator FadeIn() 
    {
        Color startColor = fadeImage.color;

        for (int i = 0; i < 100; i++)
        {
            startColor.a = startColor.a - 0.01f;   //1.0은 투명도가 255인것 a는 알파 값 조절 
            fadeImage.color = startColor;
            yield return new WaitForSeconds(0.01f); // WaitForSeconds 코루틴에서 대기시간을 삽입하는 함수
            //대기시간 줄때는 yield return 다음에 대기시간에 대한 데이터 타입을 명시해주면 된다.
        }
    }
        


}
