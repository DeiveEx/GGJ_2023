using TMPro;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private TMP_Text _countText;
    
    private CraftIngredient _ingredient;

    public CraftIngredient Ingredient => _ingredient;

    public void SetIngredient(CraftIngredient ingredient)
    {
        _ingredient = ingredient;
        _renderer.sprite = ingredient.Sprite;
    }

    public void TogglePhysics(bool enable)
    {
        _rigidbody2D.isKinematic = !enable;
        
        if(!enable)
            _rigidbody2D.velocity = Vector2.zero;
    }

    public void SetText(string text)
    {
        _countText.text = text;
    }
}
