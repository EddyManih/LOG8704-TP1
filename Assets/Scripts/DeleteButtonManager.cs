using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class DeleteButtonManager : MonoBehaviour
{


    [SerializeField]
    [Tooltip("The object spawner component in charge of spawning new objects.")]
    ObjectSpawner m_ObjectSpawner;
    public ObjectSpawner objectSpawner
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    public void ClearAllObjects()
    {
        foreach (Transform child in m_ObjectSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
