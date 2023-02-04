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
    [SerializeField] private List<IngredientHolder> _initialIngredients = new();

    private List<Button> _buttons = new();
    private CraftIngredient _selectedIngredient;

    private Inventory Inventory => GameManager.Instance.Inventory;

    public override void Init()
    {
        AddInitialIngredients();

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
    }

    protected override void OnShow()
    {
        UpdateUI();
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
        
        //Instantiate buttons
        foreach (var item in Inventory.CurrentItems)
        {
            var button = Instantiate(_buttonPrefab);
            string buttonTxt = $"{item.Key.IngredientName}";

            if (item.Key.ItemType == ItemType.Potion)
            {
                button.transform.SetParent(_potionParent, false);
                
                button.onClick.AddListener(() =>
                {
                    _itemText.text = item.Key.ToString();
                });
            }
            else
            {
                buttonTxt += $": {item.Value}";
                button.transform.SetParent(_ingredientParent, false);
                
                button.onClick.AddListener(() =>
                {
                    _itemText.text = item.Key.ToString();
                    _selectedIngredient = item.Key;
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
            sb.Append($"- {ingredient.IngredientName}\n");
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
