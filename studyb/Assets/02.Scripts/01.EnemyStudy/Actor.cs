using UnityEngine;
//[배경] Actor는 Monster와 (나중에) Player가 공유하는 부모 클래스다. 공용 부품(이동·체력)을 여기 모은다.
//Actor 자신은 MonoBehaviour — GameObject에 붙는 컴포넌트이고,
//그 안에 1~10단계에서 만든 순수 클래스 부품들을 품는다(합성). 이번 단계는 골격만, 생명주기는 12단계, 초기화는 13단계.
public abstract class Actor : MonoBehaviour, IDamageable
{
    protected MovementComponent movement;
    protected HealthComponent health;
    protected Animator actorAnimator;

    public MovementComponent Movement => movement;
    public HealthComponent Health => health;
    public Animator ActorAnimator => actorAnimator;

    public bool IsDead => health != null && health.IsDead;

    public float Defense { get; protected set; }

    private void Awake()
    {
        OnAwake();
    }
    private void Start()
    {
        OnStart();
    }
    private void Update()
    {
        OnTick();
    }

    protected virtual void OnAwake()
    {

    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnTick()
    {

    }

    protected void InitActor(float maxHP, float defense, float moveSpeed)
    {
        Defense = defense;
        actorAnimator = GetComponentInChildren<Animator>();   
        movement = new MovementComponent(this.transform, actorAnimator,moveSpeed); 
    }
    public void TakeDamage(float damage)
    {
        if (health == null) return;
        health.TakeDamage(damage);
    }
}


