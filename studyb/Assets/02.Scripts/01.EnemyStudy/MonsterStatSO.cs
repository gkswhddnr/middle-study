using UnityEngine;
[CreateAssetMenu(fileName = "MonsterStatSO", menuName = "Enemy/Core/MonsterStatSO")]
public class MonsterStatSO : ScriptableObject
{
    [Header("식별정보")]
    public string monsterID;
    public string monsterName;

    [Header("기본스탯")]
    public float maxHP = 100f;
    public float moveSpeed = 5f;
    public float attack = 10f;
    public float defense = 1f;

    [Header("전투스탯")]
    public float attackRange = 2f;

    [Header("피격스탯")]
    public float hitStun = 2f;
}
