using UnityEngine;

public class RegularBlock : BaseBlock
{


    public override void OnCraneDrop()
    {
        base.OnCraneDrop();
    }

    public override void OnCranePickup()
    {
        print("RegularBlock picked up.");
    }

    public override void OnCraneUpdate()
    {
        
    }

}
