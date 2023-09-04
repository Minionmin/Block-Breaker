using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public static BuffUI Instance { get; private set; }

    [SerializeField] private SO_Buffs buffsSO;
    [SerializeField] private List<Button> buffButtons;
    [SerializeField] private List<TextMeshProUGUI> buffTextLabels;

    List<string> _buffList = new List<string>();

    private void Awake()
    {
        Instance = this;
        Initialize();

        // write this later
        foreach(Button button in buffButtons)
        {
            button.onClick.AddListener(() => { ClearUI.Instance.Show(); NextMatchUI.Instance.Show(); Hide(); });
        }
    }

    private void Initialize()
    {
        foreach (string buffText in buffsSO.buffTexts)
        {
            _buffList.Add(buffText);
        }
    }

    public void Show()
    {
        RandomizeBuff();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RandomizeBuff()
    {
        List<int> IgnoredIndex = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            // Repeat randomization until get no repeated index
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, _buffList.Count);
            } while (IgnoredIndex.Contains(randomIndex));

            // Completed randomizing
            IgnoredIndex.Add(randomIndex);

            buffTextLabels[i].text = _buffList[randomIndex];
        }
    }
}
