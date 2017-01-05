using UnityEngine;
using System.Collections;

public class GDEX2016LevelManager : LevelManager
{

    public EventBoltLeak wrenchTutorialLeak;
    public EventHoleLeak hammerTutorialLeak;
    public EventHoleLeak tapeTutorialLeak;

    public GameObject closeHelpToStart;

    public GameObject countDownObject;
    TextMesh countDowntext;
    public int countdownLength;

    protected override void Awake()
    {
        base.Awake();
        openingLeaksFixed = 0;
        countDowntext = countDownObject.GetComponentInChildren<TextMesh>();
    }

    public void Begin()
    {
        StartCoroutine(WaitForNextBolt(0));
    }

    IEnumerator WaitForNextBolt(int message)
    {
        if (closeHelpToStart)
            Destroy(closeHelpToStart);
        yield return new WaitForSeconds(3.0f);

        switch (message)
        {
            case 0:
                wrenchTutorialLeak.Burst();
                break;
            case 1:
                hammerTutorialLeak.Burst();
                break;
            case 2:
                tapeTutorialLeak.Burst();
                break;
            default:
                break;
        }
    }

    public override void EventLeakFixed(int message)
    {
        openingLeaksFixed++;
        Debug.Log("Leaks Fixed: " + openingLeaksFixed);
        if (openingLeaksFixed == 3)
        {
            StartCoroutine(CountDown());
            return;
        }
        StartCoroutine(WaitForNextBolt(message));
    }

    IEnumerator CountDown()
    {
        countDowntext.text = "";
        countDownObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        for (int i = countdownLength; i > 0; i--)
        {
            countDowntext.text = "Tutorial Complete\n Starting in: " + i;
            yield return new WaitForSeconds(1.0f);
        }
        countDowntext.text = "Don't Drown";
        gameManager.source.Play();
        yield return new WaitForSeconds(2.0f);
        
        countDownObject.SetActive(false);
        gameManager.openingFinished = true;
        wrenchTutorialLeak.Replace();
        hammerTutorialLeak.Replace();
        tapeTutorialLeak.Replace();

        //Play Audio
    }
}
