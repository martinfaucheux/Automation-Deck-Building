public class ResourceConsumer : ResourceHolder
{
    public override bool IsAllowedToGive() => false;

    public override bool IsAllowedToReceive() => true;

    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder) => true;

    public override void OnTick()
    {
        if (heldResource != null)
        {
            Destroy(heldResource.gameObject);
            heldResource = null;
        }
    }
}
