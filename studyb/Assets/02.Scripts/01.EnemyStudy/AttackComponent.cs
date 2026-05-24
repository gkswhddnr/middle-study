using System.Collections.Generic;
using UnityEngine;
//[목표] 액션별 공격 쿨다운을 관리하는 AttackComponent를 만든다. Component 폴더의 마지막 부품.
public class AttackComponent
{
    private readonly Dictionary<MonsterAction, float> lastAttackTimes = new Dictionary<MonsterAction, float>();

    public bool CanAttack(MonsterAction action) //이 액션이 지금 공격 가능한지
    {
        float interval = 1 / Mathf.Max(action.AttackSpeed , 0.01f);

        if(lastAttackTimes.ContainsKey(action))
        {
            //아 모르겠다 힌트 Dictionary 자체를 내가 처음써봐서 너무 어색하네
        }

    }

    public void RegisterAttack(MonsterAction action) //공격 실행을 기록
    {
        lastAttackTimes[action] = Time.time;    
    }

}
