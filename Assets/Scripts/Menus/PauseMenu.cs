using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;
    [SerializeField] private AudioMixerSnapshot unpausedSnapshot;

    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onResume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        pauseAction.action.Enable();
    }
    void OnDisable()
    {
        pauseAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                PauseGame();
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {   
        pausedSnapshot.TransitionTo(0f);

        GameManager.Instance.PauseGame();
        onPause.Invoke();
    }

    private void ResumeGame()
    {        
        unpausedSnapshot.TransitionTo(0f);

        GameManager.Instance.ResumeGame();
        onResume.Invoke();
    }
}