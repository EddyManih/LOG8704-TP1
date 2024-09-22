using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DropdownManager : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown m_dropdown;
    public UnityEvent<string> m_languageChanged;

    public void SelectedLanguageChanged(int selectedIndex)
    {
        if (m_languageChanged == null)
        {
            m_languageChanged = new UnityEvent<string>();
        }

        if (m_dropdown.options[selectedIndex].text.Equals("English"))
        {
            m_languageChanged.Invoke("en");
        }
        else if (m_dropdown.options[selectedIndex].text.Equals("Français"))
        {
            m_languageChanged.Invoke("fr");
        }
    }
}
