using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] public TMP_Text m_language;
    public UnityEvent<string> m_languageChanged;

    public void LanguageChanged()
    {
        if (m_languageChanged == null)
        {
            m_languageChanged = new UnityEvent<string>();
        }

        if (m_language.text == "English")
        {
            m_languageChanged.Invoke("fr");
            m_language.text = "Français";
        }

        else if (m_language.text == "Français")
        {
            m_languageChanged.Invoke("en");
            m_language.text = "English";
        }
    }
}
