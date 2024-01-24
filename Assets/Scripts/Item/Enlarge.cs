using DG.Tweening;
using UnityEngine;

public class Enlarge : Item
{
    /// <summary> Target size </summary>
    [SerializeField] private float targetSize;

    /// <summary> Maximum active time </summary>
    [SerializeField] private float maxActiveTime;

    /// <summary> Ball original raycast length to return after effect worn out </summary>
    private float originalRayCastLength;
    /// <summary> Ball original speed to return after effect worn out </summary>
    private float originalSpeed;

    private void Start()
    {
        // Activate when get hit
        itemName = "Enlarge";
    }

    /// <summary> Enlarge the ball until effect time runs out </summary>
    public override void Tick()
    {
        var ball = GameHandler.Instance.ball;

        // If effect time runs out
        if (ball.curEnlargedActiveTime <= 0)
        {
            // Return ball to normal state
            DeactiveEnlargeEffect();

            // Remove this object
            ItemManager.Instance.DeactivateItem(this);
        }

        ball.curEnlargedActiveTime -= Time.deltaTime;
    }

    public override void GetHit()
    {
        // Prevent hitting the same item twice
        // (Some item needs to be left on the field to activate it's effect but need to hide it's visual)
        if (isActivated) return;

        // If ball already has this effect
        var ball = GameHandler.Instance.ball;
        if (ball.hasEnlarged)
        {
            // Reset active time
            ball.curEnlargedActiveTime = maxActiveTime;

            // Set this item to deactivate so time doesn't decrease multiple times
            // and remove this object
            ItemManager.Instance.DeactivateItem(this);

            return;
        }

        // Raise activation flag
        base.GetHit();

        // Raise ball size
        ActiveEnlargeEffect();
    }

    /// <summary> Activate Enlarge effect </summary>
    private void ActiveEnlargeEffect() 
    {
        var ball = GameHandler.Instance.ball;

        // Change raycast length to match ball size
        originalRayCastLength = ball.GetRaycastLength();
        ball.SetRaycastLength(originalRayCastLength * targetSize * 0.5f); // Divide by 2-ish because we scale each side targetsize/2

        // Slow down the ball due to bigger hitbox
        originalSpeed = ball.GetBallSpeed();
        ball.SetBallSpeed(originalSpeed * 0.5f);

        // To prevent outline going wrong when scale is changed
        ball.DeactivateOutline();

        // Initialize active time
        ball.curEnlargedActiveTime = maxActiveTime;
        ball.hasEnlarged = true;
        ball.transform.DOScale(targetSize, 0.5f);
    }

    /// <summary> Deactivate Enlarge effect </summary>
    private void DeactiveEnlargeEffect() 
    {
        var ball = GameHandler.Instance.ball;

        // Return everything back to normal
        ball.SetRaycastLength(originalRayCastLength);
        ball.SetBallSpeed(originalSpeed);

        // Return outline to the ball when scale is back to normal
        ball.ActivateOutline();

        ball.hasEnlarged = false;
        ball.transform.DOScale(Vector3.one, 0.5f);
        // Reset active time
        ball.curEnlargedActiveTime = maxActiveTime;
    }
}
