using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerManager : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager m_TrackedImageManager;
    [SerializeField] GameObject m_countryNamePrefab;
    [SerializeField] DropdownManager m_languageDropdownManager;
    private string m_selectedLanguage;

    Dictionary<string, Dictionary<string, string>> m_countryNames = new Dictionary<string, Dictionary<string, string>>()
    {
        { "US", new Dictionary<string, string>() { {"fr", "États-Unis"}, {"en", "United States"} }},
        { "MX", new Dictionary<string, string>() { {"fr", "Mexique"}, {"en", "Mexico"} }},
    };

    readonly Dictionary<TrackableId, GameObject> m_trackedImages = new();

    void OnEnable()
    {
        m_TrackedImageManager.trackablesChanged.AddListener(OnChanged);
        m_languageDropdownManager.m_languageChanged.AddListener(OnLanguageChanged);
        m_selectedLanguage = "en";
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackablesChanged.RemoveListener(OnChanged);
        m_languageDropdownManager.m_languageChanged.RemoveListener(OnLanguageChanged);
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            InstantiateCountryName(newImage, m_countryNames[newImage.referenceImage.name][m_selectedLanguage]);
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

    private void OnLanguageChanged(string newLanguage)
    {
        m_selectedLanguage = newLanguage;
        foreach (var trackedImage in m_TrackedImageManager.trackables)
        {
            if (m_trackedImages.ContainsKey(trackedImage.trackableId))
            {
                TextMeshPro countryNameText = m_trackedImages[trackedImage.trackableId].transform.GetComponent<TextMeshPro>();
                countryNameText.text = m_countryNames[trackedImage.referenceImage.name][m_selectedLanguage];
            }
        }
    }
}
