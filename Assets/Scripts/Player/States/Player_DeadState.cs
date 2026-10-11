using UnityEngine;

public class Player_DeadState : Player_EntityState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
}
