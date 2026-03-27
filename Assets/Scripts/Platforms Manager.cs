using UnityEngine;
using UnityEngine.Events;
 
public class PlatformsManager : MonoBehaviour
{
   [SerializeField]
   private Transform platformsPivot;
   [SerializeField]
    private InstantiatePoolObjects[] platformPrefabs;
    [SerializeField]
    private InstantiatePoolObjects[] securePlatformsPrefabs;
    [SerializeField]
    private InstantiatePoolObjects[] flyingPlatformPrefabs;
    [SerializeField]
    private  float flyingPlatformHeight = 4.5f;
    [SerializeField]
    private int initialPlatforms = 5;
    [SerializeField]
    private float minSpeed = 5f;
     [SerializeField]
    private float maxSpeed = 12f;
     [SerializeField]
    private float acceleration= 0.1f;
    [SerializeField]
    private UnityEvent<Platform> onPlatformPassed;
    private bool isRunning = true;
    private GameObject lastPlatform;
    private int platformsInstantiated = 0;
    private float speed;
    public void StartGame()
    {
        speed=minSpeed;
        lastPlatform = null;
        platformsInstantiated = 0;
        initializePlatforms();
        InstantiatePlatform(initialPlatforms);
        transform.position = platformsPivot.position;
        isRunning = true;
    }
    public void initializePlatforms()
    {
        foreach (var platform in platformPrefabs)
        {
            platform.DeactivateAllObjects();
        }
        foreach (var securePlatform in securePlatformsPrefabs)
        {
            securePlatform.DeactivateAllObjects();
        }
    }
    public void InstantiateFlyingPlatform(Transform character)
    {
        InstantiatePoolObjects instantiatePool = flyingPlatformPrefabs[Random.Range(0,flyingPlatformPrefabs.Length)];
        Vector3 spawnPosition = character.position - transform.position+ Vector3.forward * 2f;
        instantiatePool.InstantiateObject(spawnPosition);
        GameObject createPlatform = instantiatePool.GetCurrentObject();
        Platform newPlatform = createPlatform.GetComponent<Platform>();
        newPlatform.transform.SetParent(transform);
        newPlatform.transform.localPosition=spawnPosition + newPlatform.ColliderSize * Vector3.forward + Vector3.up * flyingPlatformHeight;
    }
    public void InstantiatePlatform(int number)
    {
        for (int i=  0; i < number; i++)
        {
            InstantiatePoolObjects instantiatePool;
            if (platformsInstantiated<2)
            {
                instantiatePool = securePlatformsPrefabs[Random.Range(0, securePlatformsPrefabs.Length)];
            }else
            {
                instantiatePool = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            }
            platformsInstantiated++;
            Vector3 spawnPosition = Vector3.zero;
            if (lastPlatform !=null)
            {
                spawnPosition = lastPlatform.transform.localPosition + lastPlatform.GetComponent<Platform>().ColliderSize * Vector3.forward;
            }
            instantiatePool.InstantiateObject(spawnPosition);
            GameObject createdPlatform = instantiatePool.GetCurrentObject();
            Platform newPlatform = createdPlatform.GetComponent<Platform>();
            newPlatform.transform.SetParent(transform);
            newPlatform.transform.localPosition = spawnPosition + newPlatform.ColliderSize * Vector3.forward;
            lastPlatform = newPlatform.gameObject;
            onPlatformPassed?.Invoke(newPlatform);
        }
    }
    private void Update()
    {
    if (isRunning)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            speed = Mathf.Min(speed+acceleration * Time.deltaTime, maxSpeed);
        }    
    }
    public void StopPlatforms()
    {
        isRunning = false;
    }
}
 