using UnityEngine;

public class Piercing : Item
{
    /// <summary> Maximum active time </summary>
    [SerializeField] private float maxActiveTime;

    private void Start()
    {
        // Activate when get hit
        itemName = "Piercing";
    }

    /// <summary> Make the ball piercing blocks until effect time runs out </summary>
    public override void Tick()
    {
        var ball = GameHandler.Instance.ball;

        // If effect time runs out
        if (ball.curPiercingActiveTime <= 0)
        {
            // Return ball to normal state
            DeactivePiercingEffect();

            // Remove this object
            ItemManager.Instance.DeactivateItem(this);
        }

        ball.curPiercingActiveTime -= Time.deltaTime;
    }

    public override void GetHit()
    {
        // Prevent hitting the same item twice
        // (Some item needs to be left on the field to activate it's effect but need to hide it's visual)
        if (isActivated) return;

        // If ball already has this effect
        var ball = GameHandler.Instance.ball;
        if (ball.hasPiercing)
        {
            // Reset active time
            ball.curPiercingActiveTime = maxActiveTime;

            // Set this item to deactivate so time doesn't decrease multiple times
            // and remove this object
            ItemManager.Instance.DeactivateItem(this);

            return;
        }

        // Raise activation flag
        base.GetHit();

        // Remove direction calculating in Ball class
        ActivePiercingEffect();
    }

    /// <summary> Activate Piercing effect </summary>
    private void ActivePiercingEffect() { GameHandler.Instance.ball.hasPiercing = true; }

    /// <summary> Deactivate Piercing effect </summary>
    private void DeactivePiercingEffect() { GameHandler.Instance.ball.hasPiercing = false; }
}
