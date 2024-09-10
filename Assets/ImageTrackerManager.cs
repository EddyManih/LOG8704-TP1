using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    GameObject m_debugCube;

    GameObject m_debugCubeInstance;

    void OnEnable() => m_TrackedImageManager.trackablesChanged.AddListener(OnChanged);

    void OnDisable() => m_TrackedImageManager.trackablesChanged.RemoveListener(OnChanged);

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            if (newImage.referenceImage.name == "ID1")
            {
                m_debugCubeInstance = Instantiate(m_debugCube, newImage.transform);
                Debug.Log(newImage.trackingState);
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {

            if (updatedImage.referenceImage.name == "ID1")
            {
                Debug.Log(updatedImage.trackingState);
                if (m_debugCubeInstance.activeInHierarchy && updatedImage.trackingState.Equals(TrackingState.Limited))
                {
                    m_debugCubeInstance.SetActive(false);
                }
                else if (!m_debugCubeInstance.activeInHierarchy && updatedImage.trackingState.Equals(TrackingState.Tracking))
                {
                    m_debugCubeInstance.SetActive(true);
                }
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {

        }
    }
}
