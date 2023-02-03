using System;
using UnityEngine;

[Serializable]
public class Patient
{
    [SerializeField] private string _patientName;

    private Sickness _currentSickness;
    private int _daysSick;
    private int _clinicReturnCount;

    public string PatientName => _patientName;
    public Sickness CurrentSickness => _currentSickness;
    public int DaysSick => _daysSick;
    public int ClinicReturnCount => _clinicReturnCount;

    public Patient() { }
    
    public Patient(string patientName, Sickness currentSickness, int daysSick, int clinicReturnCount)
    {
        _patientName = patientName;
        _currentSickness = currentSickness;
        _daysSick = daysSick;
        _clinicReturnCount = clinicReturnCount;
    }
}
