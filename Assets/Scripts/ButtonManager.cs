using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the delete button so we can disable it.")]
    GameObject m_DeleteButton;
    public ObjectSpawner DeleteButton
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    [SerializeField]
    [Tooltip("The object spawner component in charge of spawning new objects.")]
    ObjectSpawner m_ObjectSpawner;
    public ObjectSpawner objectSpawner
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    private void Update()
    {
        if (m_ObjectSpawner.transform.childCount > 0)
        {
            if (!m_DeleteButton.activeSelf)
            {
                m_DeleteButton.SetActive(true);
            }
        }
        else
        {
            if (m_DeleteButton.activeSelf)
            {
                m_DeleteButton.SetActive(false);
            }
        }
    }
}
