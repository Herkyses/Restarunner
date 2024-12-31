

using UnityEngine;

public interface IMovable
{
    public void Movement();
    public void PlacedObject();
    public Transform GetMoveableObjectTransform();
    public Renderer[] GetRenderers();
}
