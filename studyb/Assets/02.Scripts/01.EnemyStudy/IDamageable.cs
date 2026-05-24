public interface IDamageable
{
    //인터페이스는 애초에 멤버들이 강제적으로 public이기떄문에 아무것도 앞에 안 적어도 public으로 선언된다.
    float Defense { get; }
    void TakeDamage(float damage);

}
