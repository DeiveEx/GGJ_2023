using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<IngredientObject>(out var obj))
            return;
        
        Debug.Log($"Dropped {obj.Ingredient.ItemName}");
        GameManager.Instance.PotionManager.IngredientTable.ReturnObjectToTable(obj);
        Destroy(obj.gameObject);
    }
}
