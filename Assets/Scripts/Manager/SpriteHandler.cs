using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField] private Image spriteImageContainer;
    [SerializeField] private List<Sprite> spriteList;
    [SerializeField] float secondBeforeNextSprite;

    public IEnumerator RunSprite()
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            yield return new WaitForSeconds(secondBeforeNextSprite);
            spriteImageContainer.sprite = spriteList[i];

            // i will be incremented after the loop end so -1
            if (i == spriteList.Count - 1) i = -1;
        }
    }
}
