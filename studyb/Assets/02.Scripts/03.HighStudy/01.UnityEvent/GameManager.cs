using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public void OnPlayerDead()
    {
        Invoke("Restart",5f);  // Invoke(함수이름, 지연시간) 이러면 지연시간이 지나고 저 함수가 발동된다.
    }
    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
