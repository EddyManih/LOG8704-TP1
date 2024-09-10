using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    GameObject m_debugCube;

    void OnEnable() => m_TrackedImageManager.trackablesChanged.AddListener(OnChanged);

    void OnDisable() => m_TrackedImageManager.trackablesChanged.RemoveListener(OnChanged);

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        Debug.Log(
            $"There are {m_TrackedImageManager.trackables.count} images being tracked."
        );

        foreach (var trackedImage in m_TrackedImageManager.trackables)
        {
            Debug.Log(
                $"Image: {trackedImage.referenceImage.name} is at " + $"{trackedImage.transform.position}"
            );
        }

        foreach (var newImage in eventArgs.added)
        {
            if (newImage.referenceImage.name == "ID1")
            {
                Debug.Log("Detected image");
                Instantiate(m_debugCube, newImage.transform);
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            if (removedImage.Value.referenceImage.name == "ID1")
            {
                Debug.Log("image out of FOV");
                Destroy(m_debugCube);
            }
        }
    }
}
