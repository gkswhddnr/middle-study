using UnityEngine;
//CreateAssetMenu가 없는건 이건 SO의 뼈대고 이걸 받는애들을 생성하는거여서 맞지? 그래서 추상클래스로 선언한거고
public abstract class MonsterAction : ScriptableObject
{
    [SerializeField] private float attackSpeed = 1f;

    public float AttackSpeed => attackSpeed;

    public abstract void Act(Monster monster);
}
