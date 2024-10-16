
public interface IInteract
{
    public void Interact(PlayerInteract from);
}

public interface IInteractWithLight
{
    public void Interact(LightType lightType); 
    public void EndInteract(LightType lightType);
}