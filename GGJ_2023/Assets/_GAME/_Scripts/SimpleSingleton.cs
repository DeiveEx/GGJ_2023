using UnityEngine;
using Object = UnityEngine.Object;

public abstract class SimpleSingleton<T> : MonoBehaviour where T : Object
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();

            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null)
            Destroy(this.gameObject);
    }
}
