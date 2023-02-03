using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Property", menuName = "Custom/New Property")]
public class IngredientPropertySO : ScriptableObject
{
    [SerializeField] private string _propertyName;
    [SerializeField] private List<IngredientPropertySO> _cancelList = new();

    public string PropertyName => _propertyName;
    public IEnumerable<IngredientPropertySO> CancelList => _cancelList;
}
