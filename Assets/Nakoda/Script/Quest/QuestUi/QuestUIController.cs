using TMPro;
using UnityEngine;
using DG.Tweening;

public class QuestUIController : MonoBehaviour
{
    public static QuestUIController Instance;

    [Header("UI Elements")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI objectiveProgressText;


    [Header("Animation")]
    public float transitionDuration = 0.5f;
    public Vector2 hiddenPosition = new Vector2(-600f, 0); // Slide from left
    public Vector2 visiblePosition = Vector2.zero;

    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = hiddenPosition;
    }

    public void ShowQuest(string questTitle, string questDescription)
    {
        titleText.text = questTitle;
        descriptionText.text = questDescription;

        rectTransform.DOAnchorPos(visiblePosition, transitionDuration).SetEase(Ease.OutBack);
    }

    public void HideQuest()
    {
        rectTransform.DOAnchorPos(hiddenPosition, transitionDuration).SetEase(Ease.InBack);
    }

public void UpdateQuest(string questTitle, string questDescription, string progressText)
{
    rectTransform.DOAnchorPos(hiddenPosition, transitionDuration).SetEase(Ease.InBack).OnComplete(() =>
    {
        ShowQuest(questTitle, questDescription);
        UpdateObjectiveProgress(progressText);
    });
}
    
public void UpdateObjectiveProgress(string progressText)
{
    objectiveProgressText.text = progressText;
}

}
