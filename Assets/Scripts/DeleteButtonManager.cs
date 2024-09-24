using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DeleteButtonManager : MonoBehaviour
{
    private Button buttonComponent;

    [SerializeField]
    [Tooltip("The object spawner component in charge of spawning new objects.")]
    ObjectSpawner m_ObjectSpawner;
    public ObjectSpawner objectSpawner
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    [SerializeField]
    XRInteractionGroup m_InteractionGroup;
    public XRInteractionGroup interactionGroup
    {
        get => m_InteractionGroup;
        set => m_InteractionGroup = value;
    }

    private void Awake()
    {
        buttonComponent = gameObject.GetComponent<Button>();
    }

    public void DeleteObject() {
        var currentFocusedObject = m_InteractionGroup.focusInteractable;
        if (currentFocusedObject != null)
        {
            Destroy(currentFocusedObject.transform.gameObject);
        }
        else ClearAllObjects();
    }
    public void ClearAllObjects()
    {
        foreach (Transform child in m_ObjectSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
