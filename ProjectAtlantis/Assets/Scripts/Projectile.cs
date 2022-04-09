using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Vector3 movementVector;

    private void FixedUpdate()
    {
        transform.position += movementVector;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector3 newMovementVector)
    {
        movementVector = newMovementVector * projectileSpeed;
    }
}
