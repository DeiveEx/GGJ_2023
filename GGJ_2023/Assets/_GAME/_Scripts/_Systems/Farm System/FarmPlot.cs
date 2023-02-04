using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    private Plant _plantInfo;
    private int _daysPlanted;
    private bool _isWatered;

    public Plant PlantInfo => _plantInfo;
    public int DaysPlanted => _daysPlanted;
    public bool IsWatered => _isWatered;
    public PlantStage CurrentStage => _plantInfo.GetStageFromDays(_daysPlanted).stage;

    public void AssignPlant(Plant plant)
    {
        _plantInfo = plant;
    }

    public void WaterPlot()
    {
        _isWatered = true;
    }

    public CraftIngredient HarvestPlot()
    {
        switch (CurrentStage)
        {
            case PlantStage.ReadyToHarvest:
                var reward = _plantInfo.HarvestReward.IngredientInfo;
                _plantInfo = null;
                Debug.Log($"Harvested {reward.ItemName}");
                return reward;
            
            case PlantStage.Dead:
                Debug.Log($"Plant was dead, so no item was harvested");
                _plantInfo = null;
                return null;
            
            default:
                return null;
        }
    }
    
    public void SkipDay()
    {
        _isWatered = false;
        _daysPlanted += 1;
    }
}
