using UnityEngine;

public class MovementComponent
{
    private readonly Transform owner;
    private readonly Animator animator;
    public float MoveSpeed { get; private set; }

    public MovementComponent(Transform owner, Animator animator, float moveSpeed)
    {
        this.owner = owner;
        this.animator = animator;

        MoveSpeed = Mathf.Max(moveSpeed, 0);
    }
    public void SetMoveSpeed(float value)
    {
        MoveSpeed = Mathf.Max(value, 0);
    }

    public void FaceTowards(Vector3 worldPosition)
    {
        Vector3 dir = worldPosition - owner.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.01f) return;

        owner.rotation = Quaternion.LookRotation(dir);
    }

    public void StopWalking()
    {
        if (animator == null) return;

        animator.SetBool(AnimatorParams.IsWalking, false);
    }

    public void MoveTowards(Vector3 worldPosition)
    {
        Vector3 dir = worldPosition - owner.position;
        dir.y = 0f;         
        if (dir.sqrMagnitude < 0.01f)
        {
            StopWalking();
            return;
        }
        Vector3 dir1 = dir.normalized; //여기쪽 네이밍도 좀 고민이네

        owner.position += dir1 * MoveSpeed * Time.deltaTime;
        owner.rotation = Quaternion.LookRotation(dir);

        animator?.SetBool(AnimatorParams.IsWalking, true);

    }
}
