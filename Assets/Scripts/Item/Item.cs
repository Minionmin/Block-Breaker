using UnityEngine;

public class Item : MonoBehaviour, IHitInterface
{
    /// <summary> This item name </summary>
    protected string itemName;

    /// <summary> Has this item been hit or not </summary>
    public bool isActivated = false;

    /// <summary> Item visual </summary>
    [SerializeField] protected SpriteRenderer spriteRenderer;

    /// <summary> Some item might need to run every frame to active the effect </summary>
    public virtual void Tick(){}

    /// <summary> Effect to activate after getting hit by the ball </summary>
    public virtual void GetHit()
    {
        // Prevent hitting the same item twice
        // (Some item needs to be left on the field to activate it's effect but need to hide it's visual)
        if (isActivated) return;

        // Set activation flag to true
        isActivated = true;

        // Then hide item visual
        spriteRenderer.enabled = false;
    }
}
