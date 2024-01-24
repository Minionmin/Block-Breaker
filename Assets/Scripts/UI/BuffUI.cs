using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public static BuffUI Instance { get; private set; }

    /// <summary> Preemptively prepared buff data </summary>
    [SerializeField] private SO_Buffs buffsSO;

    /// <summary> 3 buff buttons </summary>
    [SerializeField] private List<Button> buffButtons;

    /// <summary> Buff description </summary>
    [SerializeField] private List<TextMeshProUGUI> buffTextLabels;

    /// <summary> List of buffs take from prepared scriptable object </summary>
    List<Buff> _buffList = new List<Buff>();

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        // Get data from Scriptable Object
        foreach (Buff buff in buffsSO.buffs)
        {
            _buffList.Add(buff);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        RandomizeBuff();
        // Card animation
        StartCoroutine(ShowCard());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator ShowCard()
    {
        // Make them shrink first before popping out
        foreach(var card in buffButtons)
        {
            card.transform.localScale = Vector3.zero;
        }

        foreach (var card in buffButtons)
        {
            card.transform.DOScale(Vector3.one * 0.4f, 1.5f).SetEase(Ease.OutBack, 0.5f);

            yield return new WaitForSeconds(0.25f);
        }
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
            } 
            while (IgnoredIndex.Contains(randomIndex));

            // Completed randomizing
            IgnoredIndex.Add(randomIndex);

            // All of the buff variables are initialized in Awake() state so we have to create it first
            GameObject dummyBuffObject = Instantiate(_buffList[randomIndex]).gameObject;
            Buff dummyBuff = dummyBuffObject.GetComponent<Buff>();

            // Assigning buff information into the button
            // Purposely leave created buff objects as they are because they're going to get destroyed anyway
            // when the scene is changed
            buffTextLabels[i].text = dummyBuff.buffDesc;
            buffButtons[i].onClick.AddListener(() =>
            {
                // Apply the buff and remove that buff from the list
                dummyBuff.Apply();
                _buffList.Remove(_buffList[randomIndex]);

                GameHandler.Instance.IncrementBuffCount();

                // Normal Mode
                if(GameHandler.Instance.GetGamemode() == GameHandler.Gamemode.Normal)
                {
                    // Process the UI
                    ClearUI.Instance.Show();
                    NextMatchUI.Instance.Show();
                    Hide();
                }
                // Endless Mode
                else if (GameHandler.Instance.GetGamemode() == GameHandler.Gamemode.Endless)
                {
                    // BuffUI will be inactive before blocks finished generating
                    GameHandler.Instance.StartCoroutine(GameHandler.Instance.GenerateBlocks(GameHandler.Instance.buffCount * 5));
                    Hide();
                }

                // Prevent subscribed functions to stack up the buffs ON ALL BUTTONS
                foreach (var buffButton in buffButtons)
                {
                    buffButton.onClick.RemoveAllListeners();
                }
            });
        }
    }
}
