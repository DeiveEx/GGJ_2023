using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    private Plant _plantInfo;
    private int _daysPlanted;
    private bool _isWatered;
    private int _daysWithoutWater;

    public Plant PlantInfo => _plantInfo;
    public int DaysPlanted => _daysPlanted;
    public bool IsWatered => _isWatered;
    public int DaysWithoutWater => _daysWithoutWater;
    public PlantStage CurrentStage
    {
        get
        {
            //If we haven't got any water, the plant is dead
            if(_daysWithoutWater >= _plantInfo.MaxDaysWithoutWater)
                return _plantInfo.GetStageFromDays(int.MaxValue).stage;
            
            return _plantInfo.GetStageFromDays(_daysPlanted).stage;
        }
    }

    public void AssignPlant(Plant plant)
    {
        _plantInfo = plant;
        _daysPlanted = 0;
        _daysWithoutWater = 0;
        _isWatered = false;
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
        if(_plantInfo == null)
            return;
        
        if (_isWatered)
            _daysWithoutWater = 0;
        else
            _daysWithoutWater += 1;
        
        _isWatered = false;
        _daysPlanted += 1;
    }
}
