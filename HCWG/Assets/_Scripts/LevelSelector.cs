using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class LevelSelector : MonoBehaviour
{
    public SceneHandler m_sceneHandler;
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public int numberOfLevels = 50;
    public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;
    [SerializeField] private Sprite completedLevelSprite;
    [SerializeField] private MMFeedbacks mMFeedbacks;

    // Start is called before the first frame update
    void Start()
    {
        LoadButtons();
        //panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        //iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        //int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
        //int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
        //amountPerPage = maxInARow * maxInACol;
        //int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        //LoadPanels(totalPages);
    }
    void LoadPanels(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfPanels;

        for (int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            //panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
            panel.GetComponent<RectTransform>().localPosition = new Vector2(0, -panelDimensions.height * (i - 1));
            SetUpGrid(panel);
            int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
            LoadIcons(numberOfIcons, panel);
        }
        Destroy(panelClone);
    }
    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.spacing = iconSpacing;
    }
    void LoadIcons(int numberOfIcons, GameObject parentObject)
    {
        for (int i = 1; i <= numberOfIcons; i++)
        {
            currentLevelCount++;
            GameObject icon = Instantiate(levelIcon) as GameObject;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            icon.name = "Level " + i;
            icon.GetComponentInChildren<TextMeshProUGUI>().SetText("Level " + currentLevelCount);
            Button button = icon.GetComponent<Button>();
            button.onClick.AddListener(() => m_sceneHandler.LoadLevel(icon.name));
        }
    }

    void LoadButtons()
    {
        int i = 1;
        var nbLevelCompleted = PlayerPrefs.GetInt("CompletedLevel");
        foreach (Transform child in levelHolder.transform)
        {
            child.name = "Level " + i;
            child.GetComponentInChildren<TextMeshProUGUI>().SetText("" + i);
            if (i <= nbLevelCompleted)
            {
                child.GetComponentInChildren<Image>().sprite = completedLevelSprite;
                if (i == nbLevelCompleted)
                    PlayCompletedLevelFeedback(child);
            }
            Button button = child.GetComponent<Button>();
            button.onClick.AddListener(() => m_sceneHandler.LoadLevel(child.name));
            ++i;
        }
    }

    private void PlayCompletedLevelFeedback(Transform target)
    {
        foreach (var feedback in mMFeedbacks.Feedbacks)
        {
            var fb = feedback as MMFeedbackRotation;
            if(fb != null)
            {
                fb.AnimateRotationTarget = target;
            }
            var fb1 = feedback as MMFeedbackScale;
            if(fb1 != null)
            {
                fb1.AnimateScaleTarget = target;
            }
        }

        mMFeedbacks.PlayFeedbacks();
    }
}
