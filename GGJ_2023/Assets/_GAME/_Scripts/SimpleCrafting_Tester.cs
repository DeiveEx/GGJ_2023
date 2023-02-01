using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ignix.CraftSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimpleCrafting_Tester : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform _ingredientParent;
    [SerializeField] private TMP_Text _ingredientPrefab;
    [SerializeField] private Transform _resultParent;
    [SerializeField] private TMP_Text _resultPrefab;
    [SerializeField] private Transform _currentIngredientsParent;
    [SerializeField] private TMP_Text _currentIngredientsPrefab;
    [SerializeField] private Transform _ingredientButtonParent;
    [SerializeField] private Button _ingredientButtonPrefab;
    [SerializeField] private Button _craftButton;
    [SerializeField] private Button _clearButton;
    
    private int _selectedRecipeID;
    private SimpleCraftRecipe _selectedRecipe;
    private SimpleCraftingStation _station;
    private List<TMP_Text> _texts = new();
    private List<Button> _buttons = new();

    //Items
    private CraftItem _wood = new CraftItem("wood");
    private CraftItem _stone = new CraftItem("stone");
    private CraftItem _axe = new CraftItem("axe");
    private CraftItem _hammer = new CraftItem("hammer");
    private CraftItem _rope = new CraftItem("rope");
    private CraftItem _spear = new CraftItem("spear");
    private CraftItem _plank = new CraftItem("plank");
    private CraftItem _woodTable = new CraftItem("wood table");

    #endregion

    #region Unity Events

    private void Start()
    {
        SetupStation();
        GoToNextRecipe(0);
        
        UpdateUI();
    }

    private void Update()
    {
        int previousID = _selectedRecipeID;
        
        if (Keyboard.current[Key.A].wasPressedThisFrame)
        {
            _selectedRecipeID--;
        }
        
        if (Keyboard.current[Key.D].wasPressedThisFrame)
        {
            _selectedRecipeID++;
        }

        if (previousID != _selectedRecipeID)
        {
            int recipeCount = _station.Recipes.Count();
            
            if (_selectedRecipeID < 0)
                _selectedRecipeID = recipeCount - 1;

            if (_selectedRecipeID >= recipeCount)
                _selectedRecipeID = 0;
            
            _selectedRecipe = GetRecipe(_station, _selectedRecipeID);
            UpdateUI();
        }
    }

    #endregion

    #region Public Methods

    public void CraftCurrentRecipe()
    {
        var result = _station.GetCraftResult(_selectedRecipe);

        StringBuilder sb = new StringBuilder("Craft result:\n");
        
        foreach (SimpleCraftItem craftMaterial in result)
        {
            sb.AppendLine($"- {craftMaterial.Item.Name}: {craftMaterial.Amount}");
        }
        
        Debug.Log(sb);
        
        UpdateUI();
    }

    public void ClearCurrentIngredients()
    {
        while (_station.Ingredients.Any())
        {
            _station.RemoveIngredient(_station.Ingredients.First());
        }
        
        UpdateUI();
    }

    public void GoToNextRecipe(int dir)
    {
        _selectedRecipeID += dir;
        
        int recipeCount = _station.Recipes.Count();
            
        if (_selectedRecipeID < 0)
            _selectedRecipeID = recipeCount - 1;

        if (_selectedRecipeID >= recipeCount)
            _selectedRecipeID = 0;
            
        _selectedRecipe = GetRecipe(_station, _selectedRecipeID);
        UpdateUI();
    }

    #endregion

    #region Private Methods

    private void SetupIngredientButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        
        _buttons.Clear();
        
        var ingredientsFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var field in ingredientsFields)
        {
            if (field.FieldType == typeof(CraftItem))
            {
                var value = field.GetValue(this) as CraftItem;

                var button = Instantiate(_ingredientButtonPrefab, _ingredientButtonParent);
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TMP_Text>().text = value.Name;
                
                button.onClick.AddListener(() =>
                {
                    _station.AddIngredient(new SimpleCraftItem(value, 1));
                    UpdateUI();
                });
                
                _buttons.Add(button);
            }
        }
    }
    
    private void SetupStation()
    {
        var simpleStation = new SimpleCraftingStation();

        simpleStation.AddRecipe(GetAxeRecipe());
        simpleStation.AddRecipe(GetHammerRecipe());
        simpleStation.AddRecipe(GetSpearRecipe());
        simpleStation.AddRecipe(GetPlankRecipe());
        simpleStation.AddRecipe(GetWoodTableRecipe());
        
        _station = simpleStation;
    }

    private void UpdateUI()
    {
        foreach (var text in _texts)
        {
            Destroy(text.gameObject);
        }
        
        _texts.Clear();
        
        foreach (SimpleCraftItem ingredient in _selectedRecipe.Ingredients)
        {
            var t = Instantiate(_ingredientPrefab, _ingredientParent);
            t.gameObject.SetActive(true);
            t.text = $"{ingredient.Item.Name}: {ingredient.Amount}";
            _texts.Add(t);
        }
        
        foreach (SimpleCraftItem result in _selectedRecipe.Result)
        {
            var t = Instantiate(_resultPrefab, _resultParent);
            t.gameObject.SetActive(true);
            t.text = $"{result.Item.Name}: {result.Amount}";
            _texts.Add(t);
        }
        
        foreach (SimpleCraftItem result in _station.Ingredients)
        {
            var t = Instantiate(_currentIngredientsPrefab, _currentIngredientsParent);
            t.gameObject.SetActive(true);
            t.text = $"{result.Item.Name}: {result.Amount}";
            _texts.Add(t);
        }
        
        SetupIngredientButtons();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        _craftButton.interactable = _station.CanCraftRecipe(_selectedRecipe);
        _clearButton.interactable = _station.Ingredients.Any();
    }

    private SimpleCraftRecipe GetRecipe(ICraftingStation station, int id)
    {
        int count = 0;
        
        foreach (var craftRecipe in station.Recipes)
        {
            if (count == id)
                return craftRecipe as SimpleCraftRecipe;
            
            count++;
        }

        return null;
    }
    
    private SimpleCraftRecipe GetAxeRecipe()
    {
        var axeRecipe = new SimpleCraftRecipe();

        axeRecipe.AddIngredient(new SimpleCraftItem(_wood, 2));
        axeRecipe.AddIngredient(new SimpleCraftItem(_stone, 1));
        
        axeRecipe.AddResult(new SimpleCraftItem(_axe, 1));
        
        return axeRecipe;
    }
    
    private SimpleCraftRecipe GetHammerRecipe()
    {
        var axeRecipe = new SimpleCraftRecipe();

        axeRecipe.AddIngredient(new SimpleCraftItem(_wood, 1));
        axeRecipe.AddIngredient(new SimpleCraftItem(_stone, 2));
        
        axeRecipe.AddResult(new SimpleCraftItem(_hammer, 1));
        
        return axeRecipe;
    }
    
    private SimpleCraftRecipe GetSpearRecipe()
    {
        var axeRecipe = new SimpleCraftRecipe();

        axeRecipe.AddIngredient(new SimpleCraftItem(_wood, 1));
        axeRecipe.AddIngredient(new SimpleCraftItem(_stone, 1));
        axeRecipe.AddIngredient(new SimpleCraftItem(_rope, 1));
        
        axeRecipe.AddResult(new SimpleCraftItem(_spear, 1));
        
        return axeRecipe;
    }
    
    private SimpleCraftRecipe GetPlankRecipe()
    {
        var axeRecipe = new SimpleCraftRecipe();

        axeRecipe.AddIngredient(new SimpleCraftItem(_wood, 4));
        
        axeRecipe.AddResult(new SimpleCraftItem(_plank, 1));
        
        return axeRecipe;
    }
    
    private SimpleCraftRecipe GetWoodTableRecipe()
    {
        var axeRecipe = new SimpleCraftRecipe();

        axeRecipe.AddIngredient(new SimpleCraftItem(_plank, 4));
        axeRecipe.AddIngredient(new SimpleCraftItem(_rope, 1));
        
        axeRecipe.AddResult(new SimpleCraftItem(_woodTable, 1));
        
        return axeRecipe;
    }

    #endregion
}
