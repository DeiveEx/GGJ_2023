using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientTable : MonoBehaviour
{
    [SerializeField] private IngredientObject _ingredientPrefab;
    [SerializeField] private Transform _ingredientsParent;
    [SerializeField] private Transform _spawnA;
    [SerializeField] private Transform _spawnB;
    [SerializeField] private LayerMask _ingredientMask;

    private Dictionary<CraftIngredient, IngredientObject> _ingredientObjects = new();
    private IngredientObject _heldObject;

    private Inventory Inventory => GameManager.Instance.Inventory;

    private void Awake()
    {
        Inventory.onItemAdded += OnItemAdded;
        Inventory.onItemRemoved += OnItemRemoved;
    }

    private void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Clikc");
            if (_heldObject != null)
            {
                DropIngredient(_heldObject);
            }
            else
            {
                var rayPos = Camera.main.ScreenToWorldPoint(mousePos);
                var hit = Physics2D.Raycast(rayPos, Vector2.zero, .1f, _ingredientMask);
                
                if (hit && hit.collider.TryGetComponent<IngredientObject>(out var obj))
                {
                    Debug.Log(obj.Ingredient.ItemName);
                    GrabIngredient(obj);
                }
            }
        }

        if (_heldObject != null)
        {
            var pos = Camera.main.ScreenToWorldPoint(mousePos);
            pos.z = 0;
            _heldObject.transform.position = pos;
        }
    }

    public void GrabIngredient(IngredientObject ingredientObj)
    {
        var inventoryItem = Inventory.CurrentItems.First(x => x.Item == ingredientObj.Ingredient);
        _heldObject = Instantiate(ingredientObj);
        _heldObject.SetIngredient(ingredientObj.Ingredient);
        _heldObject.SetText($"x1");


        if (inventoryItem.Count == 1)
        {
            _ingredientObjects.Remove(ingredientObj.Ingredient);
            Destroy(ingredientObj.gameObject);
        }
        
        _heldObject.TogglePhysics(false);
        GameManager.Instance.PotionManager.SetSelectedIngredient(_heldObject.Ingredient);
    }

    public void DropIngredient(IngredientObject ingredientObj)
    {
        ingredientObj.TogglePhysics(true);
        _heldObject = null;
    }

    public void ReturnObjectToTable(IngredientObject ingredientObj)
    {
        if(_ingredientObjects.ContainsKey(ingredientObj.Ingredient))
            return;
        
        var newObj = Instantiate(_ingredientPrefab, _ingredientsParent, false);
        newObj.SetIngredient(ingredientObj.Ingredient);
            
        _ingredientObjects.Add(ingredientObj.Ingredient, newObj);
        UpdateObjects();
    }

    public void UpdateObjects()
    {
        int count = 0;
        foreach (var ingredientObject in _ingredientObjects.Values)
        {
            ingredientObject.transform.position = Vector3.Lerp(_spawnA.position, _spawnB.position, count / (float) _ingredientObjects.Count);
            
            var inventoryItem = Inventory.CurrentItems.First(x => x.Item == ingredientObject.Ingredient);
            ingredientObject.SetText($"x{inventoryItem.Count}");
            
            count++;
        }
    }
    
    private void OnItemAdded(object sender, InventoryArgs e)
    {
        if(e.itemEntry.Item is not CraftIngredient ingredient)
            return;
        
        if(ingredient.IngredientType == IngredientType.Potion)
            return;

        if (!_ingredientObjects.ContainsKey(ingredient))
        {
            var newObj = Instantiate(_ingredientPrefab, _ingredientsParent, false);
            newObj.SetIngredient(ingredient);
            
            _ingredientObjects.Add(ingredient, newObj);
        }
    }
    
    private void OnItemRemoved(object sender, InventoryArgs e)
    {
        if(e.itemEntry.Item is not CraftIngredient ingredient)
            return;
        
        if(ingredient.IngredientType == IngredientType.Potion)
            return;
        
        if (_ingredientObjects.TryGetValue(ingredient, out var obj) && e.itemEntry.Count == 0)
        {
            _ingredientObjects.Remove(ingredient);
            Destroy(obj.gameObject);
        }
    }
}
