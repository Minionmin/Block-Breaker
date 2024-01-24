using UnityEngine;

public class Block : MonoBehaviour, IHitInterface
{
    /// <summary> Get hit VFX </summary>
    [SerializeField] protected GameObject crashEffectObject;

    /// <summary> Item drop rate in percent </summary>
    [SerializeField] protected int itemDropRate;

    /// <summary> All SFXs </summary>
    protected SO_SFXs sfxs;

    protected virtual void Start()
    {
        sfxs = SFXHandler.Instance.sfxSO;
    }

    /// <summary> Play VFX when the block is destroyed </summary>
    /// <param name="playPosition"> Position to play the VFX</param>
    /// <param name="sfxClip"> SFX </param>
    protected void PlayCrashEffect(Vector3 playPosition, AudioClip sfxClip)
    {
        // SFX
        SFXHandler.Instance.PlaySFX(sfxClip, Camera.main.transform.position);
        // VFX
        GameObject newObject = Instantiate(crashEffectObject, playPosition, crashEffectObject.transform.rotation);
    }

    public virtual void GetHit()
    {
        DropItem();
        PlayCrashEffect(transform.position, sfxs.blockDestroyedSFX);
        GameHandler.Instance.DestroyBlock(this);
    }

    /// <summary> Drop item upon being destroy with certain rate </summary>
    protected void DropItem()
    {
        // Item drop rate
        var probability = Random.Range(0, 100);

        // There is a chance that power-up item will drop upon this block being destroyed
        if( probability <= itemDropRate)
        {
            // Get random item type as int
            var randomItemType = Random.Range(0, ItemManager.Instance.GetItemTypeCount());
            // Then cast it to target type
            var itemType = (ItemManager.ItemTypes)randomItemType;

            // Drop the result item type
            ItemManager.Instance.DropItem(itemType, transform);
        }
    }
}
