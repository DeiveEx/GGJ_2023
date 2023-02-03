using System;
using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    [SerializeField] private PotionManager _potionManager;
    [SerializeField] private PatientManager _patientManager;
    [SerializeField] private FarmManager _farmManager;
    
    private Inventory _inventory = new();
    private ManagerBase _currentManager;

    public Inventory Inventory => _inventory;
    public PotionManager PotionManager => _potionManager;
    public PatientManager PatientManager => _patientManager;
    public FarmManager FarmManager => _farmManager;

    private void Start()
    {
        _potionManager.Init();
        _potionManager.Hide();
        
        _patientManager.Init();
        _patientManager.Hide();
        
        _farmManager.Init();
        _farmManager.Hide();
        
        ShowPatientScreen();
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
