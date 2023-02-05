using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _statisticsText;
    [SerializeField] private float _statisticsDelayPerLine;

    private GameData GameData => GlobalManager.Instance.GameData;

    private void Start()
    {
        StartCoroutine(ShowStatistics());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void ReturnToTitleScreen()
    {
        GlobalManager.Instance.ShowTitleScreen();
    }

    private IEnumerator ShowStatistics()
    {
        var delay = new WaitForSeconds(_statisticsDelayPerLine);
        _statisticsText.text = "";

        List<string> _statistics = new()
        {
            $"Days played: {GameData.daysPlayed}",
            $"Patients cured: {GameData.patientsCured}",
            $"Patients fully cured: {GameData.patientsFullyCured}",
            $"Patients returns: {GameData.patientReturns}",
            $"Patients dead: {GameData.patientsDead}",
            $"Patients dead from Sickness: {GameData.patientsDeadForSickness}",
            $"Patients dead from Medical Failure: {GameData.patientsDeadForMedicalFailure}",
            $"Potions created: {GameData.potionsCreated}",
            $"Ingredients used: {GameData.ingredientsUsed}",
            $"Seeds planted: {GameData.seedsPlanted}",
            $"Plants harvested: {GameData.plantsHarvested}",
            $"Plants dead: {GameData.plantsDead}",
        };
        
        foreach (var statistic in _statistics)
        {
            yield return delay;
            _statisticsText.text += $"{statistic}\n";
        }
    }
}
