using UnityEngine;
 
public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    private int minPLatformsNumber = 5;
    [SerializeField]
    private int maxPlatformsNUmber = 7;
    [SerializeField]
    private InstantiatePoolObjects[] powerUpPools;
    [SerializeField]
    private float powerUpOffset = 2f;
    private int platformNumber;
    private int platformCounter= 0;
    public void Awake()
    {
        SetPlatformsNumber();
    }
    private void SetPlatformsNumber()
    {
        platformNumber = Random.Range(minPLatformsNumber,maxPlatformsNUmber );
    }
    public void PlatformPassed(Platform platform)
    {
        platformCounter ++;
        if (platformCounter >= platformNumber)
        {
            SpawnPowerUp (platform);
            platformCounter = 0;
            SetPlatformsNumber();
        }
    }
    private void SpawnPowerUp(Platform platform)
    {
        InstantiatePoolObjects pool = powerUpPools[Random.Range(0, powerUpPools.Length)];
        pool.InstantiateObject(Vector3.zero);
        GameObject powerUp = pool.GetCurrentObject();
        platform.AddPowerUp(powerUp);
        powerUp.transform.localPosition += Vector3.up * powerUpOffset;
    }
}
 
 