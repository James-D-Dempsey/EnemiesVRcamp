using UnityEngine;
using UnityEngine.UI;

public class WristHealthRing : MonoBehaviour
{
    public HealthData healthData;
    public Image ringImage;

    void Update()
    {
        if (healthData != null && ringImage != null)
        {
            float fill = healthData.currentHealth / healthData.maxHealth;
            ringImage.fillAmount = fill;
        }
    }
}
