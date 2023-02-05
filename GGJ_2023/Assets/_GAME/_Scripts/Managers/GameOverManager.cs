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
        _statisticsText.text += "";

        List<string> _statistics = new();
        
        _statistics.Add($"Days played: {GameData.daysPlayed}");
        _statistics.Add($"Patients cured: {GameData.patientsCured}");

        foreach (var statistic in _statistics)
        {
            yield return delay;
            _statisticsText.text += $"{statistic}\n";
        }
    }
}
