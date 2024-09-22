using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


    /*
        Cette classe est la classe ARTemplateMenuManager sans les elements que nous n'utilisons pas
     
     */
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Button that opens the create menu.")]
    Button m_CreateButton;

    /// <summary>
    /// Button that opens the create menu.
    /// </summary>
    public Button createButton
    {
        get => m_CreateButton;
        set => m_CreateButton = value;
    }

    [SerializeField]
    [Tooltip("Button that deletes a selected object.")]
    Button m_DeleteButton;

    /// <summary>
    /// Button that deletes a selected object.
    /// </summary>
    public Button deleteButton
    {
        get => m_DeleteButton;
        set => m_DeleteButton = value;
    }

    [SerializeField]
    [Tooltip("The menu with all the creatable objects.")]
    GameObject m_ObjectMenu;

    /// <summary>
    /// The menu with all the creatable objects.
    /// </summary>
    public GameObject objectMenu
    {
        get => m_ObjectMenu;
        set => m_ObjectMenu = value;
    }

    [SerializeField]
    [Tooltip("The modal with debug options.")]
    GameObject m_ModalMenu;

    /// <summary>
    /// The modal with debug options.
    /// </summary>
    public GameObject modalMenu
    {
        get => m_ModalMenu;
        set => m_ModalMenu = value;
    }

    [SerializeField]
    [Tooltip("The animator for the object creation menu.")]
    Animator m_ObjectMenuAnimator;

    /// <summary>
    /// The animator for the object creation menu.
    /// </summary>
    public Animator objectMenuAnimator
    {
        get => m_ObjectMenuAnimator;
        set => m_ObjectMenuAnimator = value;
    }

    [SerializeField]
    [Tooltip("The object spawner component in charge of spawning new objects.")]
    ObjectSpawner m_ObjectSpawner;

    /// <summary>
    /// The object spawner component in charge of spawning new objects.
    /// </summary>
    public ObjectSpawner objectSpawner
    {
        get => m_ObjectSpawner;
        set => m_ObjectSpawner = value;
    }

    [SerializeField]
    [Tooltip("Button that closes the object creation menu.")]
    Button m_CancelButton;

    /// <summary>
    /// Button that closes the object creation menu.
    /// </summary>
    public Button cancelButton
    {
        get => m_CancelButton;
        set => m_CancelButton = value;
    }

    [SerializeField]
    [Tooltip("The interaction group for the AR demo scene.")]
    XRInteractionGroup m_InteractionGroup;

    /// <summary>
    /// The interaction group for the AR demo scene.
    /// </summary>
    public XRInteractionGroup interactionGroup
    {
        get => m_InteractionGroup;
        set => m_InteractionGroup = value;
    }

    [SerializeField]
    [Tooltip("The slider for activating plane debug visuals.")]
    DebugSlider m_DebugPlaneSlider;

    /// <summary>
    /// The slider for activating plane debug visuals.
    /// </summary>
    public DebugSlider debugPlaneSlider
    {
        get => m_DebugPlaneSlider;
        set => m_DebugPlaneSlider = value;
    }

    [SerializeField]
    [Tooltip("The plane prefab with shadows and debug visuals.")]
    GameObject m_DebugPlane;

    /// <summary>
    /// The plane prefab with shadows and debug visuals.
    /// </summary>
    public GameObject debugPlane
    {
        get => m_DebugPlane;
        set => m_DebugPlane = value;
    }

    [SerializeField]
    [Tooltip("The plane manager in the AR demo scene.")]
    ARPlaneManager m_PlaneManager;

    /// <summary>
    /// The plane manager in the AR demo scene.
    /// </summary>
    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    [SerializeField]
    [Tooltip("The slider for activating the debug menu.")]
    DebugSlider m_DebugMenuSlider;

    /// <summary>
    /// The slider for activating the debug menu.
    /// </summary>
    public DebugSlider debugMenuSlider
    {
        get => m_DebugMenuSlider;
        set => m_DebugMenuSlider = value;
    }

    [SerializeField]
    XRInputValueReader<Vector2> m_TapStartPositionInput = new XRInputValueReader<Vector2>("Tap Start Position");

    /// <summary>
    /// Input to use for the screen tap start position.
    /// </summary>
    /// <seealso cref="TouchscreenGestureInputController.tapStartPosition"/>
    public XRInputValueReader<Vector2> tapStartPositionInput
    {
        get => m_TapStartPositionInput;
        set => XRInputReaderUtility.SetInputProperty(ref m_TapStartPositionInput, value, this);
    }

    [SerializeField]
    XRInputValueReader<Vector2> m_DragCurrentPositionInput = new XRInputValueReader<Vector2>("Drag Current Position");

    /// <summary>
    /// Input to use for the screen tap start position.
    /// </summary>
    /// <seealso cref="TouchscreenGestureInputController.dragCurrentPosition"/>
    public XRInputValueReader<Vector2> dragCurrentPositionInput
    {
        get => m_DragCurrentPositionInput;
        set => XRInputReaderUtility.SetInputProperty(ref m_DragCurrentPositionInput, value, this);
    }

    bool m_IsPointerOverUI;
    bool m_ShowObjectMenu;
    bool m_ShowOptionsModal;
    bool m_InitializingDebugMenu;
    Vector2 m_ObjectButtonOffset = Vector2.zero;
    Vector2 m_ObjectMenuOffset = Vector2.zero;
    readonly List<ARFeatheredPlaneMeshVisualizerCompanion> featheredPlaneMeshVisualizerCompanions = new List<ARFeatheredPlaneMeshVisualizerCompanion>();

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void OnEnable()
    {
        m_CreateButton.onClick.AddListener(ShowMenu);
        m_CancelButton.onClick.AddListener(HideMenu);
        m_DeleteButton.onClick.AddListener(DeleteFocusedObject);
        m_PlaneManager.trackablesChanged.AddListener(OnPlaneChanged);
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void OnDisable()
    {
        m_ShowObjectMenu = false;
        m_CreateButton.onClick.RemoveListener(ShowMenu);
        m_CancelButton.onClick.RemoveListener(HideMenu);
        m_DeleteButton.onClick.RemoveListener(DeleteFocusedObject);
        m_PlaneManager.trackablesChanged.RemoveListener(OnPlaneChanged);
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void Start()
    {
        // Auto turn on/off debug menu. We want it initially active so it calls into 'Start', which will
        // allow us to move the menu properties later if the debug menu is turned on.
        m_InitializingDebugMenu = true;

        InitializeDebugMenuOffsets();
        HideMenu();
        m_PlaneManager.planePrefab = m_DebugPlane;
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    void Update()
    {
        if (m_InitializingDebugMenu)
        {
            m_InitializingDebugMenu = false;
        }

        if (m_ShowObjectMenu || m_ShowOptionsModal)
        {
            if (!m_IsPointerOverUI && (m_TapStartPositionInput.TryReadValue(out _) || m_DragCurrentPositionInput.TryReadValue(out _)))
            {
                if (m_ShowObjectMenu)
                    HideMenu();

                if (m_ShowOptionsModal)
                    m_ModalMenu.SetActive(false);
            }

            if (m_ShowObjectMenu)
            {
                m_DeleteButton.gameObject.SetActive(false);
            }
            else
            {
                m_DeleteButton.gameObject.SetActive(m_InteractionGroup?.focusInteractable != null);
            }

            m_IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
        }
        else
        {
            m_IsPointerOverUI = false;
            m_CreateButton.gameObject.SetActive(true);
            m_DeleteButton.gameObject.SetActive(m_InteractionGroup?.focusInteractable != null);
        }

        if (!m_IsPointerOverUI && m_ShowOptionsModal)
        {
            m_IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
        }
    }

    /// <summary>
    /// Set the index of the object in the list on the ObjectSpawner to a specific value.
    /// This is effectively an override of the default behavior or randomly spawning an object.
    /// </summary>
    /// <param name="objectIndex">The index in the array of the object to spawn with the ObjectSpawner</param>
    public void SetObjectToSpawn(int objectIndex)
    {
        if (m_ObjectSpawner == null)
        {
            Debug.LogWarning("Object Spawner not configured correctly: no ObjectSpawner set.");
        }
        else
        {
            if (m_ObjectSpawner.objectPrefabs.Count > objectIndex)
            {
                m_ObjectSpawner.spawnOptionIndex = objectIndex;
            }
            else
            {
                Debug.LogWarning("Object Spawner not configured correctly: object index larger than number of Object Prefabs.");
            }
        }

        HideMenu();
    }

    void ShowMenu()
    {
        m_ShowObjectMenu = true;
        m_ObjectMenu.SetActive(true);
        if (!m_ObjectMenuAnimator.GetBool("Show"))
        {
            m_ObjectMenuAnimator.SetBool("Show", true);
        }
    }

    /// <summary>
    /// Shows or hides the menu modal when the options button is clicked.
    /// </summary>
    public void ShowHideModal()
    {
        if (m_ModalMenu.activeSelf)
        {
            m_ShowOptionsModal = false;
            m_ModalMenu.SetActive(false);
        }
        else
        {
            m_ShowOptionsModal = true;
            m_ModalMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Shows or hides the plane debug visuals.
    /// </summary>
    public void ShowHideDebugPlane()
    {
        if (m_DebugPlaneSlider.value == 1)
        {
            m_DebugPlaneSlider.value = 0;
            ChangePlaneVisibility(false);
        }
        else
        {
            m_DebugPlaneSlider.value = 1;
            ChangePlaneVisibility(true);
        }
    }

    /// <summary>
    /// Clear all created objects in the scene.
    /// </summary>
    public void ClearAllObjects()
    {
        foreach (Transform child in m_ObjectSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Triggers hide animation for menu.
    /// </summary>
    public void HideMenu()
    {
        m_ObjectMenuAnimator.SetBool("Show", false);
        m_ShowObjectMenu = false;
    }

    void ChangePlaneVisibility(bool setVisible)
    {
        var count = featheredPlaneMeshVisualizerCompanions.Count;
        for (int i = 0; i < count; ++i)
        {
            featheredPlaneMeshVisualizerCompanions[i].visualizeSurfaces = setVisible;
        }
    }

    void DeleteFocusedObject()
    {
        var currentFocusedObject = m_InteractionGroup.focusInteractable;
        if (currentFocusedObject != null)
        {
            Destroy(currentFocusedObject.transform.gameObject);
        }
    }

    void InitializeDebugMenuOffsets()
    {
        if (m_CreateButton.TryGetComponent<RectTransform>(out var buttonRect))
            m_ObjectButtonOffset = new Vector2(0f, buttonRect.anchoredPosition.y + buttonRect.rect.height + 10f);
        else
            m_ObjectButtonOffset = new Vector2(0f, 200f);

        if (m_ObjectMenu.TryGetComponent<RectTransform>(out var menuRect))
            m_ObjectMenuOffset = new Vector2(0f, menuRect.anchoredPosition.y + menuRect.rect.height + 10f);
        else
            m_ObjectMenuOffset = new Vector2(0f, 345f);
    }

    void OnPlaneChanged(ARTrackablesChangedEventArgs<ARPlane> eventArgs)
    {
        if (eventArgs.added.Count > 0)
        {
            foreach (var plane in eventArgs.added)
            {
                if (plane.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = (m_DebugPlaneSlider.value != 0);
                }
            }
        }

        if (eventArgs.removed.Count > 0)
        {
            foreach (var plane in eventArgs.removed)
            {
                if (plane.Value != null && plane.Value.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                    featheredPlaneMeshVisualizerCompanions.Remove(visualizer);
            }
        }

        // Fallback if the counts do not match after an update
        if (m_PlaneManager.trackables.count != featheredPlaneMeshVisualizerCompanions.Count)
        {
            featheredPlaneMeshVisualizerCompanions.Clear();
            foreach (var trackable in m_PlaneManager.trackables)
            {
                if (trackable.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = (m_DebugPlaneSlider.value != 0);
                }
            }
        }
    }
}
