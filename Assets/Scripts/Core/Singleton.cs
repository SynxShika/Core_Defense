using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static bool _isQuitting;

    public static bool HasInstance => _instance != null;

    public static T Instance
    {
        get
        {
            if (_isQuitting) return _instance;

            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null) _instance = this as T;

        if (_instance == this)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}
