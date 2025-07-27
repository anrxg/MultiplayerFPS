using UnityEngine;

public class SingleShotGun : Gun
{
    public override void Use()
    {
        Debug.Log("Using gun " + itemInfo.itemName);
    }
}
