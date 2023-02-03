using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron_Tester : MonoBehaviour
{
    [Serializable]
    public class IngredientHolder
    {
        public CraftIngredientSO ingredient;
        public int available;
    }
    
    [SerializeField] private List<IngredientHolder> _availableIngredients = new();
    [SerializeField] private List<SicknessSO> _sicknesses = new();
    [SerializeField] private CauldronCraftStation _cauldron;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private TMP_Text _ingredientText;
    [SerializeField] private TMP_Text _propertyText;
    [SerializeField] private TMP_Text _sicknessText;

    private void Start()
    {
        foreach (var availableIngredient in _availableIngredients)
        {
            var button = Instantiate(_buttonPrefab, _buttonParent);
            button.gameObject.SetActive(true);
            
            button.onClick.AddListener(() =>
            {
                AddIngredient(availableIngredient);
                UpdateButton(button, availableIngredient);
                UpdateInfo();
            });
            
            UpdateButton(button, availableIngredient);
            UpdateInfo();
        }
    }

    public void AddIngredient(IngredientHolder selectedIngredient)
    {   
        if(selectedIngredient.available <= 0)
            return;
        
        _cauldron.AddIngredient(selectedIngredient.ingredient.IngredientInfo);
        selectedIngredient.available--;
    }

    public void Cook()
    {
        StringBuilder sb = new StringBuilder();

        var result = _cauldron.EvaluateRecipe();
        UpdateInfo();

        if (result == null)
        {
            Debug.Log("Invalid recipe");
            return;
        }

        sb.Append("Result:\n");
        sb.AppendLine(result.ToString());

        string resultText = sb.ToString();
        sb.Clear();
        
        foreach (var sickness in _sicknesses)
        {
            sb.Append($"SicknessName: {sickness.SicknessInfo.SicknessName}\n");
            
            sb.Append($"- Cure requirements:\n");

            foreach (var cureRequirement in sickness.SicknessInfo.CureRequirements)
            {
                sb.Append($"-- {cureRequirement.property.PropertyName}: {cureRequirement.amount}\n");
            }
            
            bool canCure = DoesPotionCureSickness(result, sickness.SicknessInfo, out var leftovers);
            sb.Append($"- Does potion cure sickness: {canCure}\n");

            if (canCure && leftovers != null)
            {
                sb.Append($"- Leftover Effects:\n");
                foreach (var leftoverEffect in leftovers)
                {
                    sb.Append($"-- {leftoverEffect.property.PropertyName}: {leftoverEffect.amount}\n");
                }
            }
            
            sb.Append("\n");
        }

        string sicknessText = sb.ToString();
        sb.Clear();
        
        UpdateInfo(resultText, "", sicknessText);
    }

    private void UpdateButton(Button button, IngredientHolder selectedIngredient)
    {
        button.interactable = selectedIngredient.available > 0;

        var buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = $"{selectedIngredient.ingredient.IngredientInfo.IngredientName}: {selectedIngredient.available}";
    }

    private void UpdateInfo(string ingredientsMessage = null, string propertiesMessage = null, string sicknessMessage = null)
    {
        _ingredientText.text = "";
        _propertyText.text = "";
        _sicknessText.text = "";

        StringBuilder sb = new StringBuilder();

        //Ingredients
        if (ingredientsMessage != null)
        {
            sb.Append(ingredientsMessage);
        }
        else
        {
            var ingredients = _cauldron.CurrentIngredients.ToList();

            if (!ingredients.Any())
            {
                _ingredientText.text = "No ingredients";
                return;
            }

            sb.AppendLine("Ingredients:");

            foreach (var ingredient in ingredients)
            {
                sb.AppendLine($"- {ingredient.IngredientName}");
            }
        }

        _ingredientText.text = sb.ToString();
        sb.Clear();

        //Properties
        if (propertiesMessage != null)
        {
            sb.Append(propertiesMessage);
        }
        else
        {
            var properties = _cauldron.CurrentProperties;

            sb.AppendLine("Recipe values:");

            foreach (var property in properties)
            {
                sb.AppendLine($"- {property.Key.PropertyName}: {property.Value.amount}");
            }
        }

        _propertyText.text = sb.ToString();
        sb.Clear();
        
        //Sicknesses
        if (sicknessMessage != null)
        {
            sb.Append(sicknessMessage);
        }
        else
        {
            foreach (var sickness in _sicknesses)
            {
                sb.Append($"SicknessName: {sickness.SicknessInfo.SicknessName}\n");
                sb.Append($"- Days to kill: {sickness.SicknessInfo.DaysToKill}\n");
                sb.Append($"- Cure requirements:\n");

                foreach (var cureRequirement in sickness.SicknessInfo.CureRequirements)
                {
                    sb.Append($"-- {cureRequirement.property.PropertyName}: {cureRequirement.amount}\n");
                }
            
                sb.Append("\n");
            }
        }

        _sicknessText.text = sb.ToString();
    }

    private bool DoesPotionCureSickness(CraftIngredient potion, Sickness sickness, out IEnumerable<PropertySpec> leftoverEffects)
    {
        leftoverEffects = null;
        var leftovers = new List<PropertySpec>();

        foreach (var potionProperty in potion.Properties)
        {
            //Check if this sickness needs this property
            var cureRequirement = sickness.CureRequirements.FirstOrDefault(x => x.property == potionProperty.property);

            if (cureRequirement != null)
            {
                //If the amount is equal, the potion is still valid but has no leftovers, so we just continue
                if (potionProperty.amount == cureRequirement.amount)
                    continue;
                
                //if the amount is lower, the potion is invalid
                if (potionProperty.amount < cureRequirement.amount)
                    return false;
                
                //if the amount is greater, the potion is still valid and we have some leftovers
                if (potionProperty.amount > cureRequirement.amount)
                {
                    leftovers.Add(new PropertySpec()
                    {
                        property = potionProperty.property,
                        amount = potionProperty.amount - cureRequirement.amount
                    });
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

        leftoverEffects = leftovers;
        return true;
    }
}
