using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionManager : ManagerBase
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
    [SerializeField] private TMP_Text _itemText;
    [SerializeField] private TMP_Text _cauldronText;

    private List<Button> _buttons = new();
    private CraftIngredient _selectedIngredient;

    private Inventory Inventory => GameManager.Instance.Inventory;

    public override void Init()
    {
        Inventory.onItemAdded += (sender, args) => UpdateUI();
        Inventory.onItemRemoved += (sender, args) => UpdateUI();
        _cauldron.onCauldronUpdated += (sender, args) => UpdateUI();
    }

    public void AddSelectedIngredient()
    {
        if(_selectedIngredient == null)
            return;
        
        _cauldron.AddIngredient(_selectedIngredient);
        Inventory.RemoveItem(_selectedIngredient);

        _selectedIngredient = null;
    }

    protected override void OnShow()
    {
        UpdateUI();
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
        
        //Instantiate buttons
        foreach (var inventoryItem in Inventory.CurrentItems)
        {
            if(inventoryItem.Item is not CraftIngredient ingredient)
                continue;
            
            var button = Instantiate(_buttonPrefab);
            string buttonTxt = $"{ingredient.ItemName}";

            if (ingredient.IngredientType == IngredientType.Potion)
            {
                button.transform.SetParent(_potionParent, false);
                
                button.onClick.AddListener(() =>
                {
                    _itemText.text = ingredient.ToString();
                });
            }
            else
            {
                buttonTxt += $": {inventoryItem.Count}";
                button.transform.SetParent(_ingredientParent, false);
                
                button.onClick.AddListener(() =>
                {
                    _itemText.text = ingredient.ToString();
                    _selectedIngredient = ingredient;
                });
            }

            button.GetComponentInChildren<TMP_Text>().text = buttonTxt;
            _buttons.Add(button);
        }
        
        //Cauldron Info
        StringBuilder sb = new();

        sb.Append("Current ingredients:\n");

        foreach (var ingredient in _cauldron.CurrentIngredients)
        {
            sb.Append($"- {ingredient.ItemName}\n");
        }

        sb.Append("\n");
        sb.Append("Current properties:\n");

        foreach (var property in _cauldron.CurrentProperties)
        {
            sb.Append($"- {property.Value.property.PropertyName}: {property.Value.amount}\n");
        }
        
        _cauldronText.text = sb.ToString();
    }
}
