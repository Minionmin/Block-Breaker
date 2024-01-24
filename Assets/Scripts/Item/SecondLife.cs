using UnityEngine;

public class SecondLife : Item
{
    private void Start()
    {
        // Activate when get hit
        itemName = "SecondLife";
    }

    public override void GetHit()
    {
        // Raise activation flag
        base.GetHit();

        // Raise ball flag
        ActiveSecondLifeEffect();
    }

    /// <summary> Activate Second Life effect </summary>
    private void ActiveSecondLifeEffect() { GameHandler.Instance.ball.GainSecondLife(); }
}
