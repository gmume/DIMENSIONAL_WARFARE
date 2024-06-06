using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Color firstColor, lastColor;
    public float timeEffect;
    public bool firstToLast, finished = false;
    private float speed, currentValue;
    private Image blackImage;
    private bool performEffect = false;

    void Start()
    {
        speed = 1 / timeEffect;
        blackImage = GetComponent<Image>();

        currentValue = firstToLast ? 0 : 1;
        blackImage.color = firstToLast ? firstColor : lastColor;
    }

    void FixedUpdate()
    {
        if (!performEffect) return;

        if (firstToLast)
        {
            if (PerformFadeIn()) finished = true;
        }
        else
        {
            if (PerformFadeOut()) finished = true;
        }

        blackImage.color = Color.Lerp(firstColor, lastColor, currentValue);

        if (!finished) return;

        performEffect = false;
        firstToLast = !firstToLast;
    }

    // Last to first color
    private bool PerformFadeIn()
    {
        if (currentValue != 1f)
        {
            currentValue += speed * Time.deltaTime;

            if (currentValue > 1f)
            {
                currentValue = 1f;
                return true;
            }
        }

        return false;
    }

    // First to last color
    private bool PerformFadeOut()
    {
        if (currentValue != 0f)
        {
            currentValue -= speed * Time.deltaTime;

            if (currentValue < 0f)
            {
                currentValue = 0f;
                return true;
            }
        }

        return false;
    }

    public void StartEffect()
    {
        performEffect = true;
        finished = false;
    }
}
