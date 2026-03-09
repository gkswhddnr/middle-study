using UnityEngine;

public class SetPosition : MonoBehaviour
{
   
    void Start()
    {
        transform.position = new Vector3(0, 0, 0); // 이건 글로벌 기준 0,0,0으로 보내는거다.
        transform.localPosition = new Vector3(0, 0, 0);  // 로컬 기준으로 0,0,0으로 보내는거

         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
