using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    [SerializeField] private PotionManager _potionManager;
    [SerializeField] private PatientManager _patientManager;
    [SerializeField] private FarmManager _farmManager;
    [SerializeField] private TMP_Text _mistakesText;
    [SerializeField] private int _maxMistakes;
    [SerializeField] private List<PotionManager.IngredientHolder> _initialIngredients = new();
    [SerializeField] private List<PlantSO> _initialSeeds = new();

    
    private Inventory _inventory = new();
    private ManagerBase _currentManager;
    private int _daysPlayed;
    private int _mistakes;

    public Inventory Inventory => _inventory;
    public PotionManager PotionManager => _potionManager;
    public PatientManager PatientManager => _patientManager;
    public FarmManager FarmManager => _farmManager;
    public int DaysPlayed => _daysPlayed;
    
    public event EventHandler onDaySkipped;

    private void Start()
    {
        AddInitialItems();
        
        _potionManager.Init();
        _potionManager.Hide();
        
        _patientManager.Init();
        _patientManager.Hide();
        
        _farmManager.Init();
        _farmManager.Hide();
        
        ShowPotionScreen();
    }

    public void SkipDay()
    {
        _daysPlayed++;
        Debug.Log($"Starting Day {_daysPlayed}");
        onDaySkipped?.Invoke(this, EventArgs.Empty);
    }

    public void ShowPotionScreen()
    {
        HideCurrentManager();
        _potionManager.Show();
        _currentManager = _potionManager;
    }

    public void ShowPatientScreen()
    {
        HideCurrentManager();
        _patientManager.Show();
        _currentManager = _patientManager;
    }

    public void ShowFarmScreen()
    {
        HideCurrentManager();
        _farmManager.Show();
        _currentManager = _farmManager;
    }

    public void AddMistake()
    {
        _mistakes += 1;
        _mistakesText.text = $"Mistakes: {_mistakes}";

        if (_mistakes >= _maxMistakes)
        {
            Debug.Log("Game Over");
        }
    }

    private void HideCurrentManager()
    {
        if(_currentManager != null)
            _currentManager.Hide();
    }

    private void AddInitialItems()
    {
        //Ingredients
        foreach (var initialIngredient in _initialIngredients)
        {
            Inventory.AddItem(initialIngredient.ingredient.IngredientInfo);
        }
        
        //Seeds
        foreach (var initialSeed in _initialSeeds)
        {
            Inventory.AddItem(initialSeed.PlantInfo);
        }
    }
}
