using System;
using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    [SerializeField] private PotionManager _potionManager;
    [SerializeField] private PatientManager _patientManager;
    [SerializeField] private FarmManager _farmManager;
    
    private Inventory _inventory = new();
    private ManagerBase _currentManager;
    private int _daysPlayed;

    public Inventory Inventory => _inventory;
    public PotionManager PotionManager => _potionManager;
    public PatientManager PatientManager => _patientManager;
    public FarmManager FarmManager => _farmManager;
    public int DaysPlayed => _daysPlayed;
    
    public event EventHandler onDaySkipped;

    private void Start()
    {
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

    private void HideCurrentManager()
    {
        if(_currentManager != null)
            _currentManager.Hide();
    }
}
