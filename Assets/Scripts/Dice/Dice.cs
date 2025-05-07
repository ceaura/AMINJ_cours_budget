using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public Transform[] diceFaces;

    new public Rigidbody rb;
    
    private bool _hasStoppedRolling;
    private bool _delayFinished;

    public static UnityAction<int> OnDiceResult;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_delayFinished) return;

        if (!_hasStoppedRolling && rb.linearVelocity.sqrMagnitude == 0f)
        {
            _hasStoppedRolling = true;
            GetNumberOnTopFace();
        }
    }

    [ContextMenu("Get Top Face")]
    private int GetNumberOnTopFace()
    {
        if (diceFaces == null) return -1;
        
        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }
        
        Debug.Log($"Dice result { topFace + 1}");
        
        OnDiceResult?.Invoke(topFace + 1);
        
        return topFace + 1;
        
        
    }
    
    public void RollDice(float throwForce, float rollForce)
    {
        var randomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce * randomVariance), ForceMode.Impulse);
        
        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);
        
        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + randomVariance), ForceMode.Impulse);

        DelayResult();
    }

    private async void DelayResult()
    {
        await Task.Delay(1000);
        _delayFinished = true;
    }
}
