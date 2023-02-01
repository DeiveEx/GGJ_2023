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
    [SerializeField] private CauldronCraftStation _cauldron;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private TMP_Text _ingredientText;
    [SerializeField] private TMP_Text _propertyText;

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
        var result = _cauldron.EvaluateRecipe();
        _cauldron.ClearIngredients();
        UpdateInfo();

        if (result == null)
        {
            Debug.Log("Invalid recipe");
            return;
        }

        StringBuilder sb = new StringBuilder("Result:\n");
        
        foreach (var ingredient in result)
        {
            sb.AppendLine($"- {ingredient.ingredientName}");
        }

        UpdateInfo(sb.ToString());
    }

    private void UpdateButton(Button button, IngredientHolder selectedIngredient)
    {
        button.interactable = selectedIngredient.available > 0;

        var buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = $"{selectedIngredient.ingredient.IngredientInfo.ingredientName}: {selectedIngredient.available}";
    }

    private void UpdateInfo(string forceMessage = null)
    {
        _ingredientText.text = "";
        _propertyText.text = "";
        
        if (forceMessage != null)
        {
            _ingredientText.text = forceMessage;
            return;
        }

        var ingredients = _cauldron.CurrentIngredients.ToList();

        if (!ingredients.Any())
        {
            _ingredientText.text = "No ingredients";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Ingredients:");

        foreach (var ingredient in ingredients)
        {
            sb.AppendLine($"- {ingredient.ingredientName}");
        }

        _ingredientText.text = sb.ToString();

        var values = Helper.GetPropertiesFromIngredients(ingredients);

        sb.Clear();
        sb.AppendLine("Recipe values:");

        foreach (var value in values)
        {
            sb.AppendLine($"- {value.Key}: {value.Value}");
        }

        _propertyText.text = sb.ToString();
    }
}
