using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalExitManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject portalPrefab;
    public GameObject spaceshipPrefab;

    [Header("Scene Settings")]
    public string nextSceneName = "GhostFight";

    [Header("Portal Timing")]
    public float portalDisappearTime = 3f;    // how long the portal drifts away
    public float timeBeforeNextPortal = 10f;  // wait before spawning new portal
    public float spaceshipFlyTime = 2f;       // time for ship to fly into portal

    private GameObject portalInstance;
    private GameObject spaceshipInstance;
    private bool newPortalSpawned = false;
    private float timer = 0f;

    // Optional singleton to prevent duplicates
    public static PortalExitManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SpawnPortalAndShip();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBeforeNextPortal && !newPortalSpawned)
        {
            SpawnNewPortal();
            newPortalSpawned = true;
        }
    }

    void SpawnPortalAndShip()
    {
        Vector3 exitPos = new Vector3(
            PlayerPrefs.GetFloat("PortalExitX", 0),
            PlayerPrefs.GetFloat("PortalExitY", 0),
            PlayerPrefs.GetFloat("PortalExitZ", 0)
        );

        // Spawn portal behind the spaceship
        Vector3 portalPos = exitPos - Camera.main.transform.forward * 2f + Vector3.up * 2f;
        Vector3 shipPos = exitPos + Camera.main.transform.forward * 2f + Vector3.up * 2f;

        portalInstance = Instantiate(portalPrefab, portalPos, Quaternion.identity);
        spaceshipInstance = Instantiate(spaceshipPrefab, shipPos, Quaternion.identity);

        // Move first portal away
        StartCoroutine(MovePortalAway(portalInstance));
    }

    System.Collections.IEnumerator MovePortalAway(GameObject portal)
    {
        if (portal == null) yield break;

        Vector3 startPos = portal.transform.position;
        Vector3 endPos = startPos - Camera.main.transform.right * 10f; // slide sideways off-screen

        float elapsed = 0f;

        while (elapsed < portalDisappearTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / portalDisappearTime;
            t = t * t * (3f - 2f * t); // smoothstep easing
            portal.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        if (portal != null)
            Destroy(portal);
    }

    // ----------------------------
    // SECOND PORTAL LOGIC (slides in from right)
    // ----------------------------
    void SpawnNewPortal()
    {
        if (spaceshipInstance == null) return;

        // Define where the portal should appear from (off-screen to the right)
        Vector3 rightOffset = Camera.main.transform.right * 10f;
        Vector3 finalPortalPos = spaceshipInstance.transform.position + Camera.main.transform.forward * 5f;
        Vector3 startPortalPos = finalPortalPos + rightOffset;

        GameObject newPortal = Instantiate(portalPrefab, startPortalPos, Quaternion.identity);

        // Animate the portal sliding in from the right
        StartCoroutine(SlidePortalIn(newPortal, startPortalPos, finalPortalPos));

        // Once portal finishes sliding in, move spaceship into it
        StartCoroutine(DelayedShipFly(newPortal, 1.5f)); // wait 1.5s before flying
    }

    System.Collections.IEnumerator SlidePortalIn(GameObject portal, Vector3 startPos, Vector3 endPos)
    {
        if (portal == null) yield break;

        float slideTime = 1.5f;
        float elapsed = 0f;

        while (elapsed < slideTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideTime;
            t = t * t * (3f - 2f * t); // smoothstep easing
            portal.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        portal.transform.position = endPos;
    }

    System.Collections.IEnumerator DelayedShipFly(GameObject portal, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TransportSpaceshipIntoPortal(portal));
    }

    System.Collections.IEnumerator TransportSpaceshipIntoPortal(GameObject portal)
    {
        if (spaceshipInstance == null || portal == null) yield break;

        Vector3 startPos = spaceshipInstance.transform.position;
        Vector3 endPos = portal.transform.position;

        // Rotate spaceship to face portal
        spaceshipInstance.transform.LookAt(endPos);

        float elapsed = 0f;

        // Move spaceship toward portal
        while (elapsed < spaceshipFlyTime)
        {
            if (spaceshipInstance == null || portal == null) yield break;

            elapsed += Time.deltaTime;
            float t = elapsed / spaceshipFlyTime;
            t = t * t * (3f - 2f * t); // smoothstep easing

            spaceshipInstance.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Shrink spaceship before disappearing
        float shrinkTime = 0.5f;
        elapsed = 0f;
        Vector3 originalScale = spaceshipInstance.transform.localScale;

        while (elapsed < shrinkTime)
        {
            if (spaceshipInstance == null) yield break;

            elapsed += Time.deltaTime;
            float t = elapsed / shrinkTime;

            spaceshipInstance.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Load next scene
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
}
