using System.Text;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    private Plant _plantInfo;
    private int _daysPlanted;
    private bool _isWatered;
    private int _daysWithoutWater;
    private PlantStage _currentStage;

    public Plant PlantInfo => _plantInfo;
    public int DaysPlanted => _daysPlanted;
    public bool IsWatered => _isWatered;
    public int DaysWithoutWater => _daysWithoutWater;
    public PlantStage CurrentStage => _currentStage;

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
        
        //If we haven't got any water, the plant is dead
        if (_daysWithoutWater > _plantInfo.MaxDaysWithoutWater)
            _currentStage = PlantStage.Dead;
        else
            _currentStage = _plantInfo.GetStageFromDays(_daysPlanted).stage;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        if (PlantInfo == null)
        {
            sb.Append("No plant");
        }
        else
        {
            sb.Append($"{PlantInfo.ItemName}\n");
            sb.Append($"Days planted: {DaysPlanted + 1}\n");
            sb.Append($"Is Watered: {IsWatered}\n");
            sb.Append($"Days w/out water: {DaysWithoutWater}\n");
            sb.Append($"Max days w/out water: {PlantInfo.MaxDaysWithoutWater}\n");
            sb.Append($"Stage: {CurrentStage}\n");
        }

        return sb.ToString();
    }
}
