using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] GameObject m_OptionsModal;

    public void ToggleModal()
    {
        m_OptionsModal.SetActive(!m_OptionsModal.activeSelf);
    }
}
