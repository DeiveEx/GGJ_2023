using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PatientManager : ManagerBase
{
    [Serializable]
    public class PatientSpec
    {
        public Patient patientInfo;
        public Sickness currentSickness;
        public int daysSick;
        public int clinicReturnCount;

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"Patient name:\n{patientInfo.PatientName}\n");
            sb.Append($"Days sick: {daysSick}\n");
            sb.Append($"Return count: {clinicReturnCount}\n");
            sb.Append($"{currentSickness}\n");
            
            return sb.ToString();
        }
    }
    
    [SerializeField] private List<PatientSO> _availablePatients = new();
    [SerializeField] private List<SicknessSO> _availableSickness = new();
    [SerializeField] private int _maxPatientAmount = 3;
    [SerializeField] private int _maxReturnsBeforeDeath = 3;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Transform _patientParent;
    [SerializeField] private Transform _potionParent;
    [SerializeField] private TMP_Text _patientInfo;
    [SerializeField] private TMP_Text _potionInfo;
    
    private List<PatientSpec> _currentPatients = new();
    private Queue<PatientSpec> _patientsWithSideEffects = new();
    private PatientSpec _selectedPatient;
    private CraftIngredient _selectedPotion;
    private List<Button> _buttons = new();
    
    private Inventory Inventory => GameManager.Instance.Inventory;

    public override void Init()
    {
        UpdateCurrentPatientList();
        UpdateUI();
        
        Inventory.onItemAdded += (sender, args) => UpdateUI();
        Inventory.onItemRemoved += (sender, args) => UpdateUI();
        GameManager.Instance.onDaySkipped += (sender, args) => OnDaySkipped();
    }

    protected override void OnShow()
    {
        UpdateUI();
    }

    public void GivePotionToPatient()
    {
        if(_selectedPotion == null ||
           _selectedPatient == null)
            return;
        
        //Check if potion can cure sickness
        bool canCure = DoesPotionCureSickness(_selectedPotion, _selectedPatient.currentSickness, out var leftoverEffects);

        if (!canCure)
        {
            Debug.LogError($"This potion cannot cure this sickness");
            return;
        }

        Inventory.RemoveItem(_selectedPotion);
        _currentPatients.Remove(_selectedPatient);

        StringBuilder sb = new("Patient Cured!\n");

        //If we have leftovers effects, find out which sickness he's gonna have and add this patient to the sideEffectList
        if (leftoverEffects != null)
        {
            var sickness = GetSicknessFromProperties(leftoverEffects);
            var patient = CreateNewPatient(_selectedPatient.patientInfo, sickness);

            patient.clinicReturnCount += 1;
        
            _patientsWithSideEffects.Enqueue(patient);
            
            sb.Append($"Leftover effects:\n");
            
            foreach (var leftoverEffect in leftoverEffects)
            {
                sb.Append($"- {leftoverEffect.property}: {leftoverEffect.amount}\n");
            }
        }
        
        UpdateUI();
        Debug.Log(sb.ToString());
    }

    private void AddPatientWithSideEffect(PatientSpec patient)
    {
        _patientsWithSideEffects.Enqueue(patient);
    }

    private PatientSpec CreateNewPatient(Patient patientInfo, Sickness sickness)
    {
        var patient = new PatientSpec()
        {
            patientInfo = patientInfo,
            currentSickness = sickness
        };
        
        return patient;
    }

    private void UpdateCurrentPatientList()
    {
        //If we can't receive any more patients, return
        if(_currentPatients.Count >= _maxPatientAmount)
            return;

        //If we have any patients that were not fully recovered, they take priority
        while (_patientsWithSideEffects.Count > 0 &&
               _currentPatients.Count < _maxPatientAmount)
        {
            var previousPatient = _patientsWithSideEffects.Dequeue();
            _currentPatients.Add(previousPatient);
        }
        
        //If we still have any space left, choose a random number of patients to add
        if (_currentPatients.Count < _maxPatientAmount)
        {
            int patientsToAdd = Random.Range(1, _maxPatientAmount - _currentPatients.Count);

            for (int i = 0; i < patientsToAdd; i++)
            {
                //Get only patients that are NOT currently in-game
                var validPatients = _availablePatients
                    .Where(template => _currentPatients.All(inGamePatient => template.PatientInfo != inGamePatient.patientInfo))
                    .ToList();

                if (validPatients.Count == 0)
                {
                    Debug.LogError($"There's not enough patients to choose from! Create more patients!");
                    return;
                }
                
                var newPatient = CreateNewPatient(
                    validPatients[Random.Range(0, validPatients.Count)].PatientInfo,
                    _availableSickness[Random.Range(0, _availableSickness.Count)].SicknessInfo);
                _currentPatients.Add(newPatient);
            }
        }
    }

    private IEnumerable<PatientSpec> CheckCurrentPatientsDeaths()
    {
        var deadPatients = _currentPatients
            .Where(x => x.daysSick >= x.currentSickness.DaysToKill || x.clinicReturnCount > _maxReturnsBeforeDeath)
            .ToList();

        foreach (var deadPatient in deadPatients)
        {
            _currentPatients.Remove(deadPatient);
        }
        
        return deadPatients;
    }
    
    private bool DoesPotionCureSickness(CraftIngredient potion, Sickness sickness, out List<PropertySpec> leftoverEffects)
    {
        leftoverEffects = null;
        var leftovers = new List<PropertySpec>();

        int requirementCount = sickness.CureRequirements.Count();
        
        foreach (var potionProperty in potion.Properties)
        {
            //Check if this sickness needs this property
            var cureRequirement = sickness.CureRequirements.FirstOrDefault(x => x.property == potionProperty.property);

            if (cureRequirement != null)
            {
                //If the amount is lower, the potion is invalid
                if (potionProperty.amount < cureRequirement.amount)
                    return false;
                
                //If the amount is greater or equal, the potion is still valid
                if (potionProperty.amount >= cureRequirement.amount)
                {
                    requirementCount--;
                    
                    //if the amount is greater, we have some leftovers
                    if (potionProperty.amount > cureRequirement.amount)
                    {
                        leftovers.Add(new PropertySpec()
                        {
                            property = potionProperty.property,
                            amount = potionProperty.amount - cureRequirement.amount
                        });
                    }
                }
            }
            else
            {
                //If this property does nothing to the sickness, it's a leftover
                leftovers.Add(new PropertySpec()
                {
                    property = potionProperty.property,
                    amount = potionProperty.amount
                });
            }
        }

        //Check if we fulfilled all requirements for the sickness
        if (requirementCount != 0)
            return false;

        if(leftovers.Count > 0)
            leftoverEffects = leftovers;
        
        return true;
    }

    private void OnDaySkipped()
    {
        foreach (var patient in _currentPatients)
        {
            patient.daysSick += 1;
        }
        
        var deadPatients = CheckCurrentPatientsDeaths();

        foreach (var deadPatient in deadPatients)
        {
            string causeOfDeath = null;

            if (deadPatient.daysSick >= deadPatient.currentSickness.DaysToKill)
                causeOfDeath = deadPatient.currentSickness.SicknessName;
            else if (deadPatient.clinicReturnCount >= _maxReturnsBeforeDeath)
                causeOfDeath = "medical failure (too many returns)";
            
            Debug.Log($"Patient [{deadPatient.patientInfo.PatientName}] died because of [{causeOfDeath}]");
        }
        
        UpdateCurrentPatientList();

        UpdateUI();
    }

    private Sickness GetSicknessFromProperties(IEnumerable<PropertySpec> properties)
    {
        foreach (var sicknessSo in _availableSickness)
        {
            
        }
        
        Debug.LogError($"No sickness found with the given properties!");
        return null;
    }

    private void UpdateUI()
    {
        //Clear previous buttons
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        
        _buttons.Clear();
        
        //Patients
        foreach (var patientSpec in _currentPatients)
        {
            var button = Instantiate(_buttonPrefab, _patientParent, false);
            button.GetComponentInChildren<TMP_Text>().text = patientSpec.patientInfo.PatientName;
            
            button.onClick.AddListener(() =>
            {
                _selectedPatient = patientSpec;
                UpdateUI();
            });
            
            _buttons.Add(button);
        }
        
        //Potions
        foreach (var item in Inventory.CurrentItems.Keys)
        {
            if(item.ItemType != ItemType.Potion)
                continue;
            
            var button = Instantiate(_buttonPrefab, _potionParent, false);
            button.GetComponentInChildren<TMP_Text>().text = item.IngredientName;
            
            button.onClick.AddListener(() =>
            {
                _selectedPotion = item;
                UpdateUI();
            });
            
            _buttons.Add(button);
        }
        
        _potionInfo.text = _selectedPotion == null ? "" :  _selectedPotion.ToString();
        _patientInfo.text = _selectedPatient == null ? "" : _selectedPatient.ToString();
    }
}