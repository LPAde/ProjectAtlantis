using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        offset = transform.position - GameManager.Instance.Player.PlayerController.transform.position;
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
