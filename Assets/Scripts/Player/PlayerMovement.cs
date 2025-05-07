using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private Vector3  _lastPosition;
    [SerializeField] private Transform diceThrowerPosition;

    [ContextMenu("Teleport to dice")]
    public void TeleportToDice()
    {
        _lastPosition = transform.position;
        transform.position = diceThrowerPosition.position;
        Debug.Log("Teleporting to Dice");
    }
    
    [ContextMenu("Teleport from dice")]
    public void TeleportFromDice()
    {
        transform.position = _lastPosition;
        Debug.Log("Teleporting from Dice");
    }
    
}
