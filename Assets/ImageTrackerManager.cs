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
        foreach (var newImage in eventArgs.added)
        {
            if (newImage.name == "Armoirie  - Republique Tcheque")
            {
                Instantiate(m_debugCube, newImage.transform);
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            if (removedImage.Value.name == "Armoirie  - Republique Tcheque")
            {
                Destroy(m_debugCube);
            }
        }
    }
}
