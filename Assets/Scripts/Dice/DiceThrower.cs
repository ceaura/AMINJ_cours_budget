using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DiceThrower : MonoBehaviour
{
    private static DiceThrower _instance;

    public static DiceThrower Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("UI manager is null");
            }
            return _instance;
        }
    }
    
    [SerializeField] private Dice diceToThrow;
    [SerializeField] private float throwForce;
    [SerializeField] private float rollForce;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public async void RollDice()
    {
        if (!diceToThrow) return;
        Dice dice = Instantiate(diceToThrow, transform.position, transform.rotation);
        dice.RollDice(throwForce, rollForce);
        await Task.Yield();
    }

    
}
