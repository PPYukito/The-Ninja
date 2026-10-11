using UnityEngine;

public abstract class EntityState
{
    protected CharacterEntity character;
    protected StateMachine stateMachine;
    protected string stateName;
    protected Animator anim;

    protected bool isTriggerCalled;

    public EntityState(CharacterEntity character, StateMachine stateMachine, string stateName)
    {
        this.character = character;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
        anim = character.anim;
    }

    public virtual void Enter()
    {
        //Debug.LogWarning(character.gameObject.name + " has entered " + this + " state");
        isTriggerCalled = false;
        anim.SetBool(stateName, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        anim.SetBool(stateName, false);
    }

    public void CallStateTrigger() => isTriggerCalled = true;
}
