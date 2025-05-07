using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public PlayerMovement movement { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
}
