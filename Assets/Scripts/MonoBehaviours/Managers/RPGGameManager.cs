using UnityEngine;
using UnityEngine.SceneManagement; // Include for scene management

public class RPGGameManager : MonoBehaviour
{
    // Reference to the camera manager class
    public RPGCameraManager cameraManager;

    // Reference to the spawn point designed for the player
    // Needed so the player can be re-spawned when they die
    public SpawnPoint playerSpawnPoint;

    // A variable used to access the singleton object
    public static RPGGameManager sharedInstance = null;

    // Ensure only a single instance of the RPGGameManager exists
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
        // Exit the game when Escape key is pressed
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Restart the game and load "Hell_1" when the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return)) // Use KeyCode.Return for the Enter key
        {
            SceneManager.LoadScene("Hell_1");
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
