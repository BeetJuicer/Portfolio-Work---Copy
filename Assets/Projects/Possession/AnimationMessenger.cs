namespace StateMachineCore
{
using UnityEngine;

//This class is used to send animEvents upwards to the main IAttacker implementation in the case that the visuals are a child of the parent statemachine object.
public class AnimationMessenger : MonoBehaviour
{
    private IAttacker attacker;

    private void Awake()
    {
        attacker = GetComponentInParent<IAttacker>();
    }

    // Called by animation events
    public void InflictDamage() => attacker.InflictDamage();
    public void StartAttack() => attacker.StartAttack();
    public void StopAttack() => attacker.StopAttack();
}
}