using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private float[] scalings;
    public void Initialize(Timing timing, Vector3 newMovementVector)
    {
        Initialize(newMovementVector);
        
        // For testing.
        switch (timing)
        {
            case Timing.Bad:
                Destroy(gameObject);
                break;
            case Timing.Good:
                transform.localScale *= scalings[0];
                break;
            case Timing.Amazing:
                transform.localScale *= scalings[1];
                break;
            case Timing.Perfect:
                transform.localScale *= scalings[2];
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
        
        base.OnTriggerEnter(other);
    }
}
