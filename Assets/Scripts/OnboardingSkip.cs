using UnityEngine;

public class OnboardingSkip : MonoBehaviour
{
    [SerializeField] GameObject m_currentStep;
    [SerializeField] GameObject m_mainUI;

    public void SkipOnboarding()
    {
        if (m_currentStep != null)
        {
            m_currentStep.SetActive(false);
        }

        m_mainUI.SetActive(true);
    }
}
