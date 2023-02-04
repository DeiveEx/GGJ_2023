using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Custom/New Plant")]
public class PlantSO : ScriptableObject
{
    [SerializeField] private Plant _plantInfo;

    public Plant PlantInfo => _plantInfo;
}
