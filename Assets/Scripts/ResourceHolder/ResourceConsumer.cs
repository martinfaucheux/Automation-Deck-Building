public class ResourceConsumer : ResourceHolder
{
    public override bool IsAllowedToGive() => false;

    public override bool IsAllowedResource(ResourceObject resourceObject) => true;

    public override void OnTick()
    {
        if (heldResource != null)
        {
            Destroy(heldResource.gameObject);
            heldResource = null;
        }
    }
}
