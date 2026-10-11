using UnityEngine;

public class Player_EntityState : EntityState
{
    protected Player player;
    protected NinjaActionSet input;

    public Player_EntityState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;

        anim = player.anim;
        input = player.input;
    }
}
