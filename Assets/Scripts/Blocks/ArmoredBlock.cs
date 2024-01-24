using UnityEngine;

public class ArmoredBlock : Block
{
    /// <summary> Hit left to destroy this block </summary>
    [SerializeField] protected int hitToDestroy;

    /// <summary> UI for the remaining hit </summary>
    [SerializeField] private HitUI hitUI;

    /// <summary> Check for ball's direction </summary>
    // To prevent getting hit multiple times in a row (by piercing effect)
    private Vector3 ballPreviousDir = Vector3.zero;

    /// <summary> Initializing UI and SFX </summary>
    protected override void Start()
    {
        hitUI.SetHitText(hitToDestroy);
        base.Start();
    }

    /// <summary> Destroy if hitToDestroy becomes 0, otherwise minus it by 1 </summary>
    public override void GetHit()
    {
        var ball = GameHandler.Instance.ball;

        // Prevent destroy armored block immediately due to multiple collision when ball has piercing effect
        if (ball.hasPiercing)
        {
            var ballCurDir = ball.moveDir;

            // Ball has hit this object but hasn't passed it yet so we do nothing
            if (ballPreviousDir == ballCurDir) return;

            // Remember ball direction after getting hit
            ballPreviousDir = ballCurDir;
        }

        if (hitToDestroy > 1)
        {
            hitToDestroy -= 1;
            SetHit(hitToDestroy);
        }
        else
        {
            base.GetHit();
        }
    }

    /// <summary> Update hitToDestroy value and it's UI in GameHandler.cs </summary>
    /// <param name="val"> hit left to destroy the block </param>
    public void SetHit(int val)
    {
        hitToDestroy = val;
        hitUI.SetHitText(hitToDestroy);
    }
}
