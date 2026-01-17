using UnityEngine;

public class ExpandingBlock : BaseBlock
{
    [SerializeField] private float expandRate = 2f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private float maxGravityScale = 3f;
    [SerializeField] private float mass = 2f;
    private float timeElapsed = 0f;

    public override void OnCraneUpdate()
    {
        Expand();
    }

    private void Expand()
    {
        timeElapsed += Time.deltaTime * expandRate;

        float t = (Mathf.Sin(timeElapsed) + 1f) / 2f;
        
        float scaleValue = Mathf.Lerp(1f, maxScale, t);
        
        transform.localScale = Vector3.one * scaleValue;

        Rigidbody.gravityScale = Mathf.Lerp(1f, maxGravityScale, t);

        Rigidbody.mass = 4 * Mathf.Pow(scaleValue, mass);
    }
}
