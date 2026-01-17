using UnityEngine;

public class CraneGrabber : MonoBehaviour
{
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabDistance = 1f;

    private BaseBlock heldBlock;


    // Update is called once per frame
    void Update()
    {
        if (heldBlock != null)
        {
            heldBlock.transform.position = grabPoint.position;
            heldBlock.OnCraneUpdate();
        }
    }

    public void PickUp()
    {
        if (heldBlock != null)
            return;
            
        if (Vector2.Distance(grabPoint.position, blockSpawner.transform.position) > grabDistance)
            return;

        heldBlock = blockSpawner.TakeBlock();
        if (heldBlock != null)
        {
            heldBlock.OnCranePickup();
        }
    }

    public void Drop()
    {
        if (heldBlock != null)
        {
            heldBlock.OnCraneDrop();
            heldBlock = null;
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (grabPoint == null || blockSpawner == null)
            return;

        if (Vector2.Distance(grabPoint.position, blockSpawner.transform.position) < grabDistance)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.yellow;

        Gizmos.DrawLine(grabPoint.position, blockSpawner.transform.position);
    }
}
