using UnityEngine;

public static class AnimatorParams
{
    //StringToHash 사용하는이유: 보통 애니메이션을 선언해서 가져와 쓸때는 문자열을 숫자 ex) 8012323 뭐 이런식으로 컴퓨터가 변환해서 가져다가 쓰는데
    //이 과정을 미리하는것
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Death = Animator.StringToHash("Death");
}
