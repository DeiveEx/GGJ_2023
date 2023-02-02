using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Custom/New Ingredient")]
public class CraftIngredientSO : ScriptableObject
{
    [SerializeField] private CraftIngredient _ingredientInfo;

    public CraftIngredient IngredientInfo => _ingredientInfo;
}
