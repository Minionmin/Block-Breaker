using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Boss : MonoBehaviour, IHitInterface, ITeleportInterface
{
    //public event Action OnGettingHit;

    private const string SUMMON_TAG = "Summon";

    [SerializeField] private int hitToDestroy;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private Transform[] teleportDestinations;

    private int maxHitToDestroy;
    public Transform destinationTransform { get; set; }
    private Collision hitCollision;
    private HitUI hitUI;
    private HPBarUI hpBarUI;
    private GameObject summonObj;

    private void Awake()
    {
        hitUI = GetComponentInChildren<HitUI>();
        hpBarUI = GetComponentInChildren<HPBarUI>();
    }

    private void Start()
    {
        Initialize();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        hitCollision = collision;
    }

    public int GetHitToDestroy()
    {
        return hitToDestroy;
    }

    public void GetHit()
    {
        if (hitToDestroy > 0)
        {
            hitToDestroy -= 1;
        }

        if(GetHitToDestroy() >= 0 && GetHitToDestroy() <= 10)
        {
            summonObj.SetActive(true);
        }

        if (GetHitToDestroy() <= 0)
        {
            /*
            base.PlayCrashEffect(hitCollision.GetContact(0).point, sfxs.blockDestroyedSFX);
            hitCollision = null;
            base.DestroyBlock();
            */

            hitCollision = null;
            DestroyBoss();
        }
        else
        {
            // UI visual doesn't need to know about logic
            hitUI.SetHitText(hitToDestroy);
            hpBarUI.UpdateHPSliderUI(hitToDestroy, maxHitToDestroy);
        }
    }

    private void DestroyBoss()
    {
        GameHandler.Instance.RemoveObject(gameObject);
        if (GameHandler.Instance.IsNoBlockLeft())
        {
            GameHandler.Instance.HasWon();
        }
    }

    public void TeleportObjectToAnother(Transform obj)
    {
        Transform randomDestinationResult = teleportDestinations[UnityEngine.Random.Range(0, teleportDestinations.Length)];

        obj.position = new Vector3(randomDestinationResult.position.x, obj.position.y, randomDestinationResult.position.z);

        GameObject newObject = Instantiate(teleportEffect, randomDestinationResult.position, teleportEffect.transform.rotation);
        SFXHandler.Instance.PlaySFX(SFXHandler.Instance.sfxSO.teleportSFX, Camera.main.transform.position);
    }

    private void Initialize()
    {
        // Summon
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        GameObject[] objects = new GameObject[transforms.Length];

        for(int i = 0; i < transforms.Length; i++)
        {
            objects[i] = transforms[i].gameObject;
        }

        foreach (GameObject obj in objects)
        {
            if (obj.gameObject.tag == SUMMON_TAG)
            {
                summonObj = obj;
                summonObj.SetActive(false);
            }
        }

        // Hp
        maxHitToDestroy = hitToDestroy;

        // UI
        hitUI.SetHitText(hitToDestroy);
        hpBarUI.UpdateHPSliderUI(hitToDestroy, maxHitToDestroy);
    }
}
