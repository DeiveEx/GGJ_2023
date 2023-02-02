using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Sickness
{
    [SerializeField] private string _sicknessName;
    [SerializeField] private int _daysToKill = 1;
    [SerializeField] private List<PropertySpec> _cureRequirements = new();

    public string SicknessName => _sicknessName;
    public int DaysToKill => _daysToKill;
    public IEnumerable<PropertySpec> CureRequirements => _cureRequirements;

    public Sickness() { }

    public Sickness(string sicknessName, int daysToKill, IEnumerable<PropertySpec> cureRequirements)
    {
        _sicknessName = sicknessName;
        _daysToKill = daysToKill;
        _cureRequirements = cureRequirements.ToList();
    }
}
