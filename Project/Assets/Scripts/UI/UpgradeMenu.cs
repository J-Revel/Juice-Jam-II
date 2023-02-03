using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Animator[] animators;
    public Image[] iconImages;
    public float startAnimInterval = 0.2f;
    public TMPro.TextMeshProUGUI titleText;
    public TMPro.TextMeshProUGUI descriptionText;
    public Upgrade[] availableUpgrades;
    public Upgrade[] displayedUpgrades;
    public Color textColor = Color.black;
    public Button confirmButton;
    public TMPro.TextMeshProUGUI confirmButtonText;
    private int selectedIndex = -1;
    private int displayedIndex = -1;

    public float descriptionChangeAnimDuration = 0.3f;
    private PauseEffect pauseEffect = new PauseEffect(0);
    private Coroutine playingCoroutine;

    IEnumerator Start()
    {
        TimeManager.AddPauseEffect(pauseEffect);
        titleText.color = new Color(1, 1, 1, 0);
        descriptionText.color = new Color(1, 1, 1, 0);
        displayedUpgrades = new Upgrade[animators.Length];
        List<Upgrade> unusedUpgrades = new List<Upgrade>();
        foreach(Upgrade upgrade in availableUpgrades)
            unusedUpgrades.Add(upgrade);
        for(int i=0; i<displayedUpgrades.Length; i++)
        {
            int index = Random.Range(0, unusedUpgrades.Count);

            displayedUpgrades[i] = unusedUpgrades[index];
            unusedUpgrades.RemoveAt(index);
            iconImages[i].sprite = displayedUpgrades[i].spriteIcon;
            iconImages[i].SetNativeSize();
        }
        confirmButton.onClick.AddListener(ValidateUpgrade);
        for(int i=0; i<animators.Length; i++)
        {
            animators[i].SetTrigger("PlayAppearAnim");
            yield return new WaitForSecondsRealtime(startAnimInterval);
        }
    }

    public void ShowUpgradeDescription(int index)
    {
        if(playingCoroutine != null)
        {
            StopAllCoroutines();
            Color color = new Color(textColor.r, textColor.g, textColor.b, 0);
            descriptionText.color = color;
            titleText.color = color;
            displayedIndex = -1;
        }
        playingCoroutine = StartCoroutine(ShowUpgradeDescriptionAnim(index));
    }

    public void HideUpgradeDescription()
    {
        if(selectedIndex >= 0)
            ShowUpgradeDescription(selectedIndex);
        else playingCoroutine = StartCoroutine(HideUpgradeDescriptionAnim());
    }

    public void SelectUpgrade(int index)
    {
        confirmButton.interactable = true;
        confirmButtonText.color = Color.white;
        selectedIndex = index;
        for(int i=0; i<animators.Length; i++)
        {
            animators[i].SetBool("Selected", i == index);
        }
    }

    public IEnumerator ShowUpgradeDescriptionAnim(int index)
    {
        if(index != displayedIndex)
        {
            Color color = new Color(textColor.r, textColor.g, textColor.b);
            if(displayedIndex >= 0)
            {
                for(float time=0; time < descriptionChangeAnimDuration; time += Time.unscaledDeltaTime)
                {
                    color.a = 1 - (time / descriptionChangeAnimDuration);
                    descriptionText.color = color;
                    titleText.color = color;
                    yield return null;
                }
            }
            titleText.text = displayedUpgrades[index].title;
            descriptionText.text = displayedUpgrades[index].description;
            displayedIndex = index;
            for(float time=0; time < descriptionChangeAnimDuration; time += Time.unscaledDeltaTime)
            {
                color.a = time / descriptionChangeAnimDuration;
                descriptionText.color = color;
                titleText.color = color;
                yield return null;
            }
        }
    }

    public IEnumerator HideUpgradeDescriptionAnim()
    {
        displayedIndex = -1;
        Color color = new Color(textColor.r, textColor.g, textColor.b);
        for(float time=0; time < descriptionChangeAnimDuration; time += Time.unscaledDeltaTime)
        {
            color.a = 1 - (time / descriptionChangeAnimDuration);
            descriptionText.color = color;
            titleText.color = color;
            yield return null;
        }
        color.a = 0;
        descriptionText.color = color;
        titleText.color = color;
    }

    private void OnDestroy()
    {
        pauseEffect.finished = true;
    }

    void Update()
    {
        
    }

    public void ValidateUpgrade()
    {
        PlayerStatsData.instance.ApplyUpgrade(displayedUpgrades[selectedIndex]);
        Destroy(gameObject);
    }
}
