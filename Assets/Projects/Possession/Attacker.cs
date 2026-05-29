namespace StateMachineCore
{
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Attacker : MonoBehaviour, IAttacker
{
    [SerializeField] private Collider2D hitBox;
    private TopDownCharacterStateMachine stateMachine;

    private AttackData currentAttackData;
    AttackData IAttacker.attackData => currentAttackData;


    private List<Collider2D> collidersInTrigger = new List<Collider2D>();

    public event Action OnAttackComplete;

    private void OnTriggerEnter2D(Collider2D collision) => collidersInTrigger.Add(collision);
    private void OnTriggerExit2D(Collider2D collision) => collidersInTrigger.Remove(collision);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine = GetComponent<TopDownCharacterStateMachine>();    
    }

    public void StartAttack()
    {
        stateMachine.Animator.SetBool(currentAttackData.animBool, true);
    }

    public void StopAttack()
    {
        stateMachine.Animator.SetBool(currentAttackData.animBool, false);
        OnAttackComplete?.Invoke();
    }

    public void InflictDamage()
    {
        collidersInTrigger.RemoveAll(c => c == null); //check for dead ones
        foreach (Collider2D col in collidersInTrigger)
        {
            if (col.TryGetComponent(out IDamageable target))
                target.TakeDamage(currentAttackData.damage);
        }
    }

    public void SetAttackData(AttackData attackData)
    {
        this.currentAttackData = attackData;
    }
}

[System.Serializable]
public struct AttackData
{
    public string animBool;
    public float damage;
}
}