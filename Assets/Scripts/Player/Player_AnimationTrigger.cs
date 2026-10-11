using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_AnimationTrigger : Entity_AnimationTrigger
{
    private ProjectileShooter shooter;

    protected override void Awake()
    {
        base.Awake();

        shooter = GetComponentInParent<ProjectileShooter>();
    }

    private void ThrowProjectile()
    {
        shooter.ShootProjectile();
    }
}
