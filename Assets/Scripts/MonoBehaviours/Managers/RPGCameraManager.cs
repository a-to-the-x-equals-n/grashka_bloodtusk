using Cinemachine;
using UnityEngine;

public class RPGCameraManager : MonoBehaviour
{
    // A variable used to access the singleton object
    public static RPGCameraManager sharedInstance = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    // Ensure only a single isntance of the RPGCameraManager exists
    // It's possibel to get multiple instances if multiple copies of the RPGCameraManager exists in the Hierarchy
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

        // Find the virtual camera object in the current scene
        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");

        // Get a reference to the virtual camera component of the virtual camera
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
