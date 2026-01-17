using UnityEngine;
using UnityEngine.Events;

public class ClimberHeight : MonoBehaviour
{
    [SerializeField] private float winHeight = 4f;

    [SerializeField] private UnityEvent onWin;

    private bool hasWon = false;
    // Update is called once per frame
    void Update()
    {
        if (hasWon) return;

        if (transform.position.y >= winHeight)
        {
            hasWon = true;
            onWin.Invoke();
            Debug.Log("Climber has reached the win height!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-5f, winHeight, 0f), new Vector3(5f, winHeight, 0f));
    }
}
