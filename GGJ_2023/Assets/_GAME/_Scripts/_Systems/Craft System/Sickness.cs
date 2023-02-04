using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"Sickness Name:\n{_sicknessName}\n");
        sb.Append($"Days to kill: {_daysToKill}\n");
        sb.Append($"Cure Requirements:\n");

        foreach (var cureRequirement in _cureRequirements)
        {
            sb.Append($"- {cureRequirement.property.PropertyName}: {cureRequirement.amount}\n");
        }

        return sb.ToString();
    }
}
