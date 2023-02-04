public class ResourceConsumer : ResourceHolder
{
    public override bool IsAllowedToGive() => false;

    public override void OnTick()
    {
        if (heldResource != null)
        {
            Destroy(heldResource.gameObject);
            SetHeldResource(null);
        }
    }
}
