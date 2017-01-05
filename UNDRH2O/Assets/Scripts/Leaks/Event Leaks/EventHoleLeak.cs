using UnityEngine;
using System.Collections;

public class EventHoleLeak : HoleLeak {

    public int eventMessage;
    
    public GameObject prefabReplacement;

    public GameObject eventUI;

    LevelManager levelManager;

    public override void Start()
    {
        base.Start();

        levelManager = FindObjectOfType<LevelManager>();
    }

    public override void Burst()
    {

        base.Burst();

        
        if (eventUI)
            eventUI.SetActive(true);
    }

    protected override IEnumerator Dead()
    {

        gm.score += pointsForFixing;

        //Have UI and sound to indicate 
        GameObject pointsUI = (GameObject)Instantiate(pointsPrefab, transform.position + Vector3.up * 0.1f, transform.rotation, transform);
        pointsUI.GetComponentInChildren<TextMesh>().text = "+" + pointsForFixing;

        //AUDIO HERE : Leak Fixed
        source.Stop();
        loopSource.Stop();
        //Play dead clip

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
