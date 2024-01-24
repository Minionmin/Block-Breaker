using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBlockVisual : MonoBehaviour
{
    private List<GameObject> blockList = new List<GameObject>();
    [SerializeField] private List<GameObject> blockTypes;

    [SerializeField] private Transform blockParentTransform;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private float animSpeed;
    [SerializeField] private float interval;

    private void Start()
    {
        StartCoroutine(GenerateBlocks());
    }

    public IEnumerator GenerateBlocks()
    {
        float blockWidth, blockHeight;
        blockWidth = 6f;
        blockHeight = 2f;

        int blockNumber = width * height;
        while (true)
        {
            // Random a position to generate a block
            int randomPosition = Random.Range(0, blockNumber);

            // Calculate from 1d array position to 2d array position (Block position is visualized in 2d array)
            int row = randomPosition / width;
            int col = randomPosition % width;

            int randomBlockType;
            randomBlockType = Random.Range(0, blockTypes.Count);
            // Generate the block
            GameObject generatedBlock = Instantiate(blockTypes[randomBlockType]);

            // Put generated block as a child in designed object so the hierachy is more beautiful
            generatedBlock.transform.SetParent(blockParentTransform, false);

            // Move block to their corresponding position
            // Block x range is from -15 to 15 (column)
            // Block z range is from 10 to 15 (row)
            generatedBlock.transform.position = new Vector3((col * blockWidth) - 15, 0, 10 - (row * blockHeight));
            blockList.Add(generatedBlock);

            // Generating animation
            generatedBlock.transform.localScale = Vector3.zero;
            generatedBlock.transform.DOScale(new Vector3(5, 1, 1), animSpeed).SetEase(Ease.OutBack)
                .onComplete = () =>
                {
                    generatedBlock.transform.DOScale(Vector3.zero, animSpeed).SetEase(Ease.OutBack);
                    blockList.Remove(generatedBlock);
                    Destroy(generatedBlock);
                };

            float animationWaitTime = interval;
            yield return new WaitForSeconds(animationWaitTime);
        }
    }
}
