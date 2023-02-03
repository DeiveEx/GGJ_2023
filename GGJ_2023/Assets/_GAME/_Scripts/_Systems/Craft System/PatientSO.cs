using UnityEngine;

[CreateAssetMenu(fileName = "Patient", menuName = "Custom/New Patient")]
public class PatientSO : ScriptableObject
{
    [SerializeField] private Patient _patientInfo;

    public Patient PatientInfo => _patientInfo;
}
