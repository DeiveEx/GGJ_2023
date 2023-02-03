using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    [Serializable]
    public class IngredientHolder
    {
        public CraftIngredientSO ingredient;
        public int available;
    }
    
    [SerializeField] private CauldronCraftStation _cauldron;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Transform _ingredientParent;
    [SerializeField] private Transform _potionParent;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private List<IngredientHolder> _initialIngredients = new();

    private List<Button> _buttons = new();

    private Inventory Inventory => GameManager.Instance.Inventory;

    private void Start()
    {
        AddInitialIngredients();
        UpdateUI();

        Inventory.onItemAdded += (sender, args) => UpdateUI();
        Inventory.onItemRemoved += (sender, args) => UpdateUI();
        _cauldron.onCauldronUpdated += (sender, args) => UpdateUI();
    }
    
    private void AddInitialIngredients()
    {
        foreach (var initialIngredient in _initialIngredients)
        {
            for (int i = 0; i < initialIngredient.available; i++)
            {
                GameManager.Instance.Inventory.AddItem(initialIngredient.ingredient.IngredientInfo);
            }
        }
    }

    public void Cook()
    {
        var potion = _cauldron.EvaluateRecipe();

        if (potion == null)
        {
            Debug.LogError("Invalid potion");
            return;
        }
        
        Inventory.AddItem(potion);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        //Clear previous buttons
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        
        _buttons.Clear();
        
        //Instantiate new buttons
        foreach (var item in Inventory.CurrentItems)
        {
            var button = Instantiate(_buttonPrefab);
            string buttonTxt = $"{item.Key.IngredientName}";

            if (item.Key.IsPotion)
            {
                button.transform.SetParent(_potionParent);
                button.onClick.AddListener(() =>
                {
                    ShowPotionInfo(item.Key);
                });
            }
            else
            {
                buttonTxt += $": {item.Value}";
                button.transform.SetParent(_ingredientParent);
                button.onClick.AddListener(() =>
                {
                    _cauldron.AddIngredient(item.Key);
                    Inventory.RemoveItem(item.Key);
                });
            }

            button.GetComponentInChildren<TMP_Text>().text = buttonTxt;
            _buttons.Add(button);
        }
        
        ShowCauldronInfo();
    }

    private void ShowCauldronInfo()
    {
        //Cauldron Info
        StringBuilder sb = new();

        sb.Append("Current ingredients:\n");

        foreach (var ingredient in _cauldron.CurrentIngredients)
        {
            sb.Append($"- {ingredient.IngredientName}\n");
        }

        sb.Append("\n");
        sb.Append("Current properties:\n");

        foreach (var property in _cauldron.CurrentProperties)
        {
            sb.Append($"- {property.Value.property.PropertyName}: {property.Value.amount}\n");
        }
        
        _infoText.text = sb.ToString();
    }

    private void ShowPotionInfo(CraftIngredient potion)
    {
        StringBuilder sb = new();

        sb.Append("Potion properties:\n");

        foreach (var property in potion.Properties)
        {
            sb.Append($"- {property.property.PropertyName}: {property.amount}\n");
        }
        
        _infoText.text = sb.ToString();
    }
}
