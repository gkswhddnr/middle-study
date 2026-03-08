using UnityEngine;
using UnityEngine.Events;
//이벤트 시스템 이벤트는 이벤트가 발동되면 거기에 등록된 기능들이 연쇄적으로 발동이 되는데 중요한건 서로에게 관심이 없다
// 이벤트를 발동시키는쪽도 이벤트가 발동될때 같이 따라가서 발동이 되는쪽도 서로에게 관심이 없다 코드가 깔끔해짐

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent onPlayerDead;

    /*public UIManager uiManager;
    public AchivementSystem achivementSystem;
    public GameManager gameManager; */

    private void Dead()
    {
        /* uiManager.OnPlayerDead();
         gameManager.OnPlayerDead();
         achivementSystem.UnLockAchivement("레전드 도전과제");*/
        // 이벤트 기능을 사용하지 않으면 이렇게 위에처럼 변수로 다른스크립트를 다 가져온 뒤에 그 기능들을 하나하나 써야한다.
        // 이런식으로 하면 PlayerHealth 스크립트에 전혀 관련없는 애들이 막 엮여서 번잡해진다.

        onPlayerDead.Invoke(); //인보크로 발동시켜버림
        Debug.Log("죽었다");
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        Dead();
    }

}
