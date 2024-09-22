using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager m_TrackedImageManager;
    [SerializeField] GameObject m_countryNamePrefab;

    readonly Dictionary<TrackableId, GameObject> m_trackedImages = new();

    void OnEnable() => m_TrackedImageManager.trackablesChanged.AddListener(OnChanged);

    void OnDisable() => m_TrackedImageManager.trackablesChanged.RemoveListener(OnChanged);

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            if (newImage.referenceImage.name == "EtatsUnis")
            {
                InstantiateCountryName(newImage, "États-Unis");
            }
            else if (newImage.referenceImage.name == "Mexique")
            {
                InstantiateCountryName(newImage, "Mexique");
            }
        }

        /*
        foreach (var updatedImage in eventArgs.updated)
        {
            GameObject tracked_country_name = m_trackedImages[updatedImage.trackableId];

            if (tracked_country_name.activeInHierarchy && updatedImage.trackingState.Equals(TrackingState.Limited))
            {
                tracked_country_name.SetActive(false);
            }
            else if (!tracked_country_name.activeInHierarchy && updatedImage.trackingState.Equals(TrackingState.Tracking))
            {
                tracked_country_name.SetActive(true);
            }
        }
        */
    }

    private void InstantiateCountryName(ARTrackedImage newImage, string countryName)
    {
        GameObject countryNameObj = Instantiate(m_countryNamePrefab, newImage.transform);
        TextMeshPro countryNameText = countryNameObj.transform.GetComponent<TextMeshPro>();
        countryNameText.text = countryName;
        m_trackedImages.Add(newImage.trackableId, countryNameObj);
    }
}
