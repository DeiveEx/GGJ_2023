using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    [SerializeField] private PotionManager _potionManager;
    
    private Inventory _inventory = new();

    public Inventory Inventory => _inventory;
    public PotionManager PotionManager => _potionManager;
}
