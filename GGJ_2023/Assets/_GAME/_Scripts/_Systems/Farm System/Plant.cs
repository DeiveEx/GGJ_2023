using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlantStage
{
    None = 0,
    Seed = 1,
    Sapling = 2,
    ReadyToHarvest = 3,
    Dead = 4,
}

[Serializable]
public class Plant : Item
{
    [Serializable]
    public class PlantStageInfo
    {
        public PlantStage stage;
        public int duration;
    }

    [SerializeField] private int _maxDaysWithoutWater;
    [SerializeField] private List<PlantStageInfo> _stages = new();
    [SerializeField] private CraftIngredientSO _harvestReward;

    public List<PlantStageInfo> Stages => _stages;
    public CraftIngredientSO HarvestReward => _harvestReward;
    public int MaxDaysWithoutWater => _maxDaysWithoutWater;

    public Plant(string plantName) : base(plantName) { }

    public PlantStageInfo GetStageFromDays(int days)
    {
        var orderedStages = _stages.OrderBy(x => (int) x.stage);

        int totalDays = 0;
        
        foreach (var stage in orderedStages)
        {
            totalDays += stage.duration;

            if (days <= totalDays)
                return stage;
        }

        return _stages.First(x => x.stage == PlantStage.Dead);
    }
}
