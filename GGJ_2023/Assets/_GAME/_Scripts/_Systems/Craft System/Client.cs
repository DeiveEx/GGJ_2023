using System;
using UnityEngine;

[Serializable]
public class Client
{
    [SerializeField] private string _clientName;

    private Sickness _currentSickness;
    private int _daysSick;
    private int _clinicReturnCount;

    public Client() { }
    
    public Client(string clientName, Sickness currentSickness, int daysSick, int clinicReturnCount)
    {
        _clientName = clientName;
        _currentSickness = currentSickness;
        _daysSick = daysSick;
        _clinicReturnCount = clinicReturnCount;
    }
}
