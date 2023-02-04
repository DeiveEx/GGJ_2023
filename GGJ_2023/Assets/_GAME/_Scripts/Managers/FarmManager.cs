using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : ManagerBase
{   
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Transform _seedParent;
    [SerializeField] private FarmPlot _farmPlotPrefab;
    [SerializeField] private Transform _farmPlotParent;
    [SerializeField] private int _numberOfPlots;

    private List<Button> _buttons = new();
    private List<FarmPlot> _farmPlots = new();
    private Plant _selectedSeed;

    private Inventory Inventory => GameManager.Instance.Inventory;

    public override void Init()
    {
        GameManager.Instance.onDaySkipped += (sender, args) => OnDaySkipped();
        
        for (int i = 0; i < _numberOfPlots; i++)
        {
            var plot = Instantiate(_farmPlotPrefab, _farmPlotParent);
            
            plot.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                OnPlotClicked(plot);
                UpdateUI();
            });
            
            _farmPlots.Add(plot);
        }
    }

    protected override void OnShow()
    {
        UpdateUI();
    }

    private void OnPlotClicked(FarmPlot plot)
    {
        if (plot.PlantInfo == null)
        {
            if(PlantSeed(_selectedSeed, plot))
                _selectedSeed = null;
            
            return;
        }

        if (plot.CurrentStage == PlantStage.ReadyToHarvest ||
            plot.CurrentStage == PlantStage.Dead)
        {
            var reward = plot.HarvestPlot();
            
            if(reward != null)
                Inventory.AddItem(reward);
            
            return;
        }
        
        plot.WaterPlot();
    }

    private bool PlantSeed(Plant seed, FarmPlot plot)
    {
        if(_selectedSeed == null)
            return false;

        if (plot.PlantInfo != null)
        {
            Debug.LogError("You can't plant of an occupied plot!");
            return false;
        }
        
        plot.AssignPlant(seed);
        Inventory.RemoveItem(seed);
        return true;
    }

    private void OnDaySkipped()
    {
        foreach (var plot in _farmPlots)
        {
            if(plot.IsWatered)
                plot.SkipDay();
        }
        
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
        
        //Seed list
        foreach (var item in Inventory.CurrentItems)
        {
            if (item.Item is not Plant plantSeed)
                continue;

            var button = Instantiate(_buttonPrefab, _seedParent);
            button.GetComponentInChildren<TMP_Text>().text = $"{plantSeed.ItemName} seed";
            
            button.onClick.AddListener(() =>
            {
                _selectedSeed = plantSeed;
            });
            
            _buttons.Add(button);
        }
        
        //Farm Plots
        StringBuilder sb = new();
        
        foreach (var plot in _farmPlots)
        {
            if (plot.PlantInfo == null)
            {
                sb.Append("No plant");
            }
            else
            {
                sb.Append($"{plot.PlantInfo.ItemName}\n");
                sb.Append($"Days planted: {plot.DaysPlanted}\n");
                sb.Append($"Is Watered: {plot.IsWatered}\n");
                sb.Append($"Stage: {plot.PlantInfo.GetStageFromDays(plot.DaysPlanted).stage}\n");
            }
            
            plot.GetComponentInChildren<TMP_Text>().text = sb.ToString();
            sb.Clear();
        }
    }
}
