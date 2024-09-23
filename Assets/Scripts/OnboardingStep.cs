using UnityEngine;

public class OnboardingStep : MonoBehaviour
{
    [SerializeField] GameObject m_NextStep;

    // Update is called once per frame

    public void NextStep()
    {
        if (m_NextStep != null)
        {
            m_NextStep.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
