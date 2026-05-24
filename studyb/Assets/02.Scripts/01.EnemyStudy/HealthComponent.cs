using System;
using UnityEngine;

public class HealthComponent
{
    private float maxHP;
    private float currentHP;
    private float defense;
    private bool isDead;
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public bool IsDead => isDead;

    //이벤트 선언
    public event Action<float> OnDamaged;
    public event Action OnDied;
    //상수 선언
    private const float minDamage = 1f;
    public HealthComponent(float hp, float def)
    {
        maxHP = Mathf.Max(hp, 1);
        currentHP = maxHP;
        defense = Mathf.Max(def, 0);

    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float actualDamage = Mathf.Max(damage - defense, minDamage);

        currentHP -= actualDamage;

        if (currentHP <= 0)
        {
            currentHP = 0f;
            isDead = true;
            OnDamaged?.Invoke(actualDamage);
            OnDied?.Invoke();

        }
        else
        {
            OnDamaged?.Invoke(actualDamage);
        }
    }

    public void SetDefense(float value)
    {
        defense = Mathf.Max(value, 0f);
    }
    public void SetMaxHP(float value)
    {
        maxHP = Mathf.Max(value, 1f);
        currentHP = Mathf.Min(currentHP, maxHP);
    }
    public void FullHeal()
    {
        currentHP = maxHP;
    }
}
