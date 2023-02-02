using UnityEngine;

[CreateAssetMenu(fileName = "Sickness", menuName = "Custom/New Sickness")]
public class SicknessSO : ScriptableObject
{
    [SerializeField] private Sickness _sicknessInfo;

    public Sickness SicknessInfo => _sicknessInfo;
}
