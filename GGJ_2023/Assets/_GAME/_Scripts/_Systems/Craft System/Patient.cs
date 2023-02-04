using System;
using UnityEngine;

[Serializable]
public class Patient
{
    [SerializeField] private string _patientName;

    public string PatientName => _patientName;
}
