using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RayInteraction : MonoBehaviour
{
    //레이캐스트를 통해  눈앞에 있는 사물을 감지하고 옮기는 코드

    public LayerMask whatIsTarget;

    public float distance = 100f;
    private Camera playerCam;

    private Transform moveTarget;
    private float targetDistance;

    void Start()
    {
        playerCam = Camera.main; //씬에서 Tag가 MainCamera인 카메라의 Camera 컴포넌트를 가져온다
    }
  /*  hit가 가져오는 정보
 ├ collider(맞은 콜라이더)
 ├ transform(맞은 오브젝트 transform)
 ├ point(맞은 위치)
 ├ normal(표면 방향)
 ├ distance(레이 시작점에서 충돌까지 거리)*/
    void Update()
    {
        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f));
        Vector3 rayDir = playerCam.transform.forward;

        Debug.DrawRay(rayOrigin, rayDir * 100f, Color.green); //실제 레이처리가 아니라 개발자가 확인할 수 있게 레이를 그려주는 역할만 함
        if (Input.GetMouseButtonDown(0))
        {
           
            RaycastHit hit; // 레이캐스트에 의한 정보를 담아다주는 단순 정보 컨테이너
            if (Physics.Raycast(rayOrigin, rayDir, out hit, distance, whatIsTarget)) //(광선출발위치,광선방향,out ,광선거리,광선레이어필터) out은 입력으로 들어간 값이 내부에서 어떤 값이 생겨서 빠져나가는 것
            {
                GameObject hitTarget = hit.collider.gameObject;

                hitTarget.GetComponent<Renderer>().material.color = Color.red;

                moveTarget = hitTarget.transform;
                targetDistance = hit.distance;
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (moveTarget != null)
            {
                moveTarget.GetComponent<Renderer>().material.color = Color.white;
            }
            moveTarget = null;
        }
        if (moveTarget != null)
        {
            moveTarget.position = rayOrigin + rayDir * targetDistance;
        }
    }
}
