using UnityEngine;

public class Booster : MonoBehaviour
{
    public void HealthBoost(Character target)
    {
        Debug.Log(target.playerName + "의 체력을 강화했다!");
        target.hp += 10;

    }
    public void ShieldBoost(Character target)
    {
        Debug.Log(target.playerName + "의 방어력을 강화했다!");
        target.defense += 10;
    }
    public void DamageBoost(Character target)
    {
        Debug.Log(target.playerName + "의 공격력을 강화했다!");
        target.damage += 10;
    }

    private void Awake()
    {
        Character player = FindObjectOfType<Character>();

        player.playerBoost += HealthBoost;
        player.playerBoost += ShieldBoost;
        player.playerBoost += DamageBoost;
        // player.playerBoost = DamageBoost; event키워드를 쓰면 이게 에러가 뜬다.


    }

}
