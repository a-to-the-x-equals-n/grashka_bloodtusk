using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    // Reference to the camera manager class
    public RPGCameraManager cameraManager;

    // Reference to the spawn point designed for the player
    // Needed so the player can be re-spawn when they die
    public SpawnPoint playerSpawnPoint;

    // A variable used to access the singleton object
    public static RPGGameManager sharedInstance = null;

    // Ensure only a single isntance of the RPGGameManager exists
    // It's possibel to get multiple instances if multiple copies of the RPGGameManager exists in the Hierarchy
    // or if multiple copies are programmatically instantiated
    public void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    // Update is called once per frame
    void Update()
    {
        // to leave game outisde unity
        if (Input.GetKey("Escape"))
        {
            Application.Quit();
        }

        
    }

    public void SetupScene()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();

            // Set the virtual camera to follow the player that was just spawned
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
