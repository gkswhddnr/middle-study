using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void Boost(Character target);

    public  Boost playerBoost;
    // public event Boost playerBoost; event를 해주면 델리게이트의 기능이 이벤트가 아닌 방향으로
    // 작성되는 것을 막아준다 + - 는 되지만 =로 덮어씌우는 건 안됨 event가 없어도 delegate로만 할 수 있다 실수를 안 한다면

    public string playerName = "Jemin";

    public float hp = 100;

    public float defense = 50;

    public float damage = 30;

    private void Start()
    {
        playerBoost(this); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerBoost(this);
        }
    }
}
