using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] public GameObject teleportDoorCloseEffect;

    public void PlayTeleportDoorCloseEffect()
    {
        SFXHandler.Instance.PlaySFX(SFXHandler.Instance.sfxSO.teleportDoorDestroyedSFX, Camera.main.transform.position);
        GameObject newObject = Instantiate(teleportDoorCloseEffect, transform.position, teleportDoorCloseEffect.transform.rotation);
    }
}
