using UnityEngine;
using System.Collections;

public class HelpUIController : MonoBehaviour
{

    public GameObject HelpUI;

    SteamVR_TrackedController thisControllerInput;
    public SteamVR_TrackedController oppositeControllerInput;

    public SpriteRenderer menuSprite;
    public SpriteRenderer dPadSprite;
    public SpriteRenderer gripSprite;
    public SpriteRenderer triggerSprite;

    public Color highlightColor;

    //REMOVE THESE LINES FOR FUTHER LEVELS//
    GDEX2016LevelManager levelManager;
    bool begunTutorial = false;
    ///////////////////////////////////////
    
    void OnEnable()
    {
        if (name == "Controller (right) [Physical]" || name == "Controller (left) [Physical]")
        {
            HelpUI.SetActive(false);
            Destroy(this);
        }
        thisControllerInput.MenuButtonClicked += HighlightMenu;
        thisControllerInput.MenuButtonUnclicked += UnHighlightMenu;
        oppositeControllerInput.MenuButtonClicked += HighlightMenu;
        oppositeControllerInput.MenuButtonUnclicked += UnHighlightMenu;
        thisControllerInput.TriggerClicked += HighlightTrigger;
        thisControllerInput.TriggerUnclicked += UnHighlightTrigger;
        thisControllerInput.Gripped += HighlightGrip;
        thisControllerInput.Ungripped += UnHighlightGrip;
        thisControllerInput.PadClicked += HighlightDPad;
        thisControllerInput.PadUnclicked += UnHighlightDpad;
    }

    void OnDisable()
    {
        thisControllerInput.MenuButtonClicked -= HighlightMenu;
        thisControllerInput.MenuButtonUnclicked -= UnHighlightMenu;
        oppositeControllerInput.MenuButtonClicked -= HighlightMenu;
        oppositeControllerInput.MenuButtonUnclicked -= UnHighlightMenu;
        thisControllerInput.TriggerClicked -= HighlightTrigger;
        thisControllerInput.TriggerUnclicked -= UnHighlightTrigger;
        thisControllerInput.Gripped -= HighlightGrip;
        thisControllerInput.Ungripped -= UnHighlightGrip;
        thisControllerInput.PadClicked -= HighlightDPad;
        thisControllerInput.PadUnclicked -= UnHighlightDpad;
    }

    void Awake()
    {
        //REMOVE THIS AFTER GDEX//
        levelManager = FindObjectOfType<GDEX2016LevelManager>();
        //////////////////////////

        thisControllerInput = GetComponent<SteamVR_TrackedController>();

        if (name == "Controller (right) [Physical]" || name == "Controller (left) [Physical]")
        {
            HelpUI.SetActive(false);
            Destroy(this);
        }
    }

    void HighlightTrigger(object sender, ClickedEventArgs e)
    {
        triggerSprite.color = highlightColor;
    }
    void UnHighlightTrigger(object sender, ClickedEventArgs e)
    {
        triggerSprite.color = Color.white;
    }
    void HighlightGrip(object sender, ClickedEventArgs e)
    {
        gripSprite.color = highlightColor;
    }
    void UnHighlightGrip(object sender, ClickedEventArgs e)
    {
        gripSprite.color = Color.white;
    }
    void HighlightDPad(object sender, ClickedEventArgs e)
    {
        dPadSprite.color = highlightColor;
    }
    void UnHighlightDpad(object sender, ClickedEventArgs e)
    {
        dPadSprite.color = Color.white;
    }
    void HighlightMenu(object sender, ClickedEventArgs e)
    {
        menuSprite.color = highlightColor;
        HelpUI.SetActive(!HelpUI.activeSelf);
        if (begunTutorial == false)
        {
            Debug.Log("Closing Help");
            levelManager.Begin();
        }
    }
    void UnHighlightMenu(object sender, ClickedEventArgs e)
    {
        menuSprite.color = Color.white;
    }
}
