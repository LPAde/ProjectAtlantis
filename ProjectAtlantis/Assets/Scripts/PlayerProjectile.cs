using UnityEngine;

public class PlayerProjectile : Projectile
{
    public void Initialize(Timing timing, Vector3 newMovementVector)
    {
        Initialize(newMovementVector);
        
        // For testing.
        switch (timing)
        {
            case Timing.Bad:
                transform.localScale *= 0.5f;
                break;
            case Timing.Good:
                break;
            case Timing.Amazing:
                transform.localScale *= 1.5f;
                break;
            case Timing.Perfect:
                transform.localScale *= 2f;
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
