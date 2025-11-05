using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalExitManager : MonoBehaviour
{
    [Header("Portal Prefabs")]
    public GameObject exitPortalPrefab;

    [Header("Scene Settings")]
    public string nextSceneName = "GhostFight";

    [Header("Portal Timing & Movement")]
    public float timeBeforeExitPortal = 10f; // seconds before portal appears
    public float portalMoveSpeed = 3f;       // speed portal moves toward ship
    public float shipFlySpeed = 5f;          // speed ship flies into portal
    public float shrinkTime = 0.5f;          // time for ship to shrink

    private GameObject currentExitPortal;
    private GameObject spaceshipTarget; // CACHE the spaceship
    private bool exitPortalSpawned = false;
    private float timer = 0f;

    public static PortalExitManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPortalTimer();
    }

    private void Update()
    {
        if (!exitPortalSpawned)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforeExitPortal)
            {
                SpawnExitPortal();
            }
        }

        if (exitPortalSpawned && currentExitPortal != null && spaceshipTarget != null)
        {
            MovePortalTowardSpaceship();
        }
    }

    private void SpawnExitPortal()
    {
        if (exitPortalSpawned || currentExitPortal != null) return;

        spaceshipTarget = FindAnyObjectByType<SpaceshipHealth>()?.gameObject;
        if (spaceshipTarget == null)
        {
            Debug.LogWarning("[PortalExitManager] No spaceship found!");
            return;
        }

        if (exitPortalPrefab == null)
        {
            Debug.LogError("[PortalExitManager] Exit portal prefab not assigned!");
            return;
        }

        Vector3 startPos = spaceshipTarget.transform.position + Vector3.right * 10f;
        currentExitPortal = Instantiate(exitPortalPrefab, startPos, Quaternion.identity);
        exitPortalSpawned = true;

        Debug.Log("Exit portal spawned!");
    }

    private void MovePortalTowardSpaceship()
    {
        if (currentExitPortal == null || spaceshipTarget == null) return;

        Vector3 shipPos = spaceshipTarget.transform.position;
        currentExitPortal.transform.position = Vector3.MoveTowards(
            currentExitPortal.transform.position,
            shipPos,
            portalMoveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(currentExitPortal.transform.position, shipPos) < 1f)
        {
            StartCoroutine(TransportSpaceshipToNextScene(spaceshipTarget));
        }
    }

    private IEnumerator TransportSpaceshipToNextScene(GameObject ship)
    {
        if (ship == null || currentExitPortal == null) yield break;

        exitPortalSpawned = false;

        Vector3 portalPos = currentExitPortal.transform.position;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * shipFlySpeed;
            ship.transform.position = Vector3.Lerp(ship.transform.position, portalPos, elapsed);
            yield return null;
        }

        elapsed = 0f;
        Vector3 originalScale = ship.transform.localScale;

        while (elapsed < shrinkTime)
        {
            elapsed += Time.deltaTime / shrinkTime;
            ship.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsed);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    public void ResetPortalTimer()
    {
        timer = 0f;
        exitPortalSpawned = false;

        if (currentExitPortal != null)
        {
            Destroy(currentExitPortal);
            currentExitPortal = null;
        }

        spaceshipTarget = null; // reset target
    }

    public void OnSpaceshipRespawn()
    {
        ResetPortalTimer();
    }
}



