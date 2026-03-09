using UnityEngine;

//Instantiate: 원본을 집어 넣어주면 그 원본을 게임 도중에 실시간으로 찍어내는 역할을 한다.
public class Spawner : MonoBehaviour
{
    public Transform spawnPosition;

    public GameObject target;
    // GameObject가 아니라 Rigidbody로 찍어내면 instace.GetComponent<Rigidbody>();를 하지않고 바로 rigidbody를 사용할 수 있다
    void Start()
    {
       GameObject instace =  Instantiate(target, spawnPosition.transform.position, spawnPosition.transform.rotation);
        Debug.Log(instace.name);

        instace.GetComponent<Rigidbody>().AddForce(0, 1000, 0);
    }

 
}
/* Instantiate 자주 쓰는 상황 
 * 1.그냥 복사  Instantiate(prefab); 
 * 예: Instantiate(bulletPrefab); 이러면 bulletPrefab을 복제해서 만든다.
 * 2.위치만 정해서 생성 Instantiate(prefab, position, rotation);
 * 예: Instantiate(bulletPrefab, transform.position, transform.rotation);
 * 3. 부모 밑에 생성 Instantiate(prefab, parent);
 * 예:Instantiate(buttonPrefab, canvasTransform); -> 버튼 프리팹을 만들어서 캔버스에 자식으로 넣어라
 * 4. 위치/회전/부모 다 정하기 Instantiate(prefab, position, rotation, parent);
 * 예: Instantiate(effectPrefab, transform.position, Quaternion.identity, transform); */