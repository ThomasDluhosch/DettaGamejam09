using UnityEngine;

/// <summary>
/// Singleton class for MonoBehaviour objects.
/// </summary>
/// <typeparam name="T">The type of the MonoBehaviour object.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// The instance of the MonoBehaviour object.
    /// </summary>
    private static T _instance;

    /// <summary>
    /// Lock object
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// Flag to check if the application is quitting.
    /// </summary>
    private static bool _applicationIsQuitting = false;

    /// <summary>
    /// The instance of the MonoBehaviour object.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                // If the instance is null, try to find an object of type T
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance != null)
                    {
                        // If the object is found, set it as the instance
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    else
                    {
                        // If the object is not found, create a new object

                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[Singleton] Another instance of " + typeof(T) + " detected! Destroying this one.");
                Destroy(gameObject); // Destroy the new instance if another exists
            }
        }
        Awake_();
    }

    protected virtual void Awake_()
    {
        
    }

    protected virtual void OnApplicationQuit()
    {
        lock (_lock)
        {
            if (_instance == this)
            {
                _applicationIsQuitting = true;
            }
        }
    }
}
