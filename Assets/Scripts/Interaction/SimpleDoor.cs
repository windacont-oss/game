using UnityEngine;

public class SimpleDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 0f, 2f);

    private Vector3 closedPosition;
    private bool isOpen;

    private void Awake()
    {
        if (door == null)
        {
            door = transform;
        }

        closedPosition = door.position;
    }

    public void Interact()
    {
        isOpen = !isOpen;
        door.position = isOpen ? closedPosition + openOffset : closedPosition;
    }
}
