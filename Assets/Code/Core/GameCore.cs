using System;
using Code.Core;
using UnityEngine;

public enum GameState
{
    Menu,
    Normal,
    Inspecting,
    Frozen
    
}
public class GameCore : MonoBehaviour
{
    public static GameState State { get; set; }
    public static GameCore Instance { get; private set; }
    public static Inventory PlayerInventory { get; private set; }
    public static InteractionSystem InteractionSystem { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        
        PlayerInventory = FindObjectOfType<Inventory>();
        InteractionSystem = FindObjectOfType<InteractionSystem>();
    }

    private void Start()
    {
        State = GameState.Normal;
    }
}