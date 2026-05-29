using StateMachineCore;
using UnityEngine;

public class SS_PlayerController : ISideScrollerController
{
    public bool JumpInput => InputHandler.Instance.JumpInput.InputActive;

    public bool CrouchInput => false;

    public Vector2 MoveInput =>InputHandler.Instance.ActiveMoveInput;

    public void Update()
    {
    }
}
