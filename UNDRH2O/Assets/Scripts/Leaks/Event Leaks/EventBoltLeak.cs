using UnityEngine;
using System.Collections;

public class EventBoltLeak : BoltLeak {

    public int eventMessage;

    public GameObject prefabReplacement;

    public GameObject eventUI;

    public GameObject gripUI;
    bool hasShownAlready;

    LevelManager levelManager;

    public override void Start()
    {
        base.Start();

        levelManager = FindObjectOfType<LevelManager>();
    }

    public override void Update()
    {
        base.Update();

        if (gripUI && isInteracting && gripUI.activeSelf)
            gripUI.SetActive(false);
        else if ( gripUI && !isInteracting && hasShownAlready && !gripUI.activeSelf)
            gripUI.SetActive(true);

    }

    public override void Burst()
    {
        base.Burst();

        if (eventUI)
            eventUI.SetActive(true);
    }

    void OnTriggerEnter(Collider col)
    {
        Wrench wrench = col.GetComponent<Wrench>();
        if (wrench && gripUI)
        {
            gripUI.SetActive(true);
            hasShownAlready = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        Wrench wrench = col.GetComponent<Wrench>();
        if (wrench && gripUI)
            gripUI.SetActive(false);
    }

    protected override IEnumerator Dead()
    {
        gm.score += pointsForFixing;

        //Have UI and sound to indicate 
        GameObject pointsUI = (GameObject)Instantiate(pointsPrefab, transform.position + Vector3.up * 0.1f, transform.rotation, transform);
        pointsUI.GetComponentInChildren<TextMesh>().text = "+" + pointsForFixing;

        Destroy(gripUI);
        // AUDIO HERE : Leak Fixed
        source.Stop();
        loopSource.Stop();
        //Play dead clip

        gm.leaksFixed++;

        waitingForRegen = true;
        stats.dead = true;
        state = State.Kill;
        EnableProperParticleSystems();

        if (eventUI)
            eventUI.SetActive(false);

        levelManager.EventLeakFixed(eventMessage);

        yield return null;

    }

    public override void Replace()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        GameObject newLeak = (GameObject)Instantiate(prefabReplacement, transform.position, transform.rotation, gm.leaksParent);

        gm.NewLeak(newLeak.GetComponent<Leak>());

        Destroy(gameObject);
    }
}
