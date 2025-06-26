using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeController : MonoBehaviour
{
    public Button speedButton;
    public float[] speedOptions = { 1f, 2f, 4f };
    private int currentSpeedIndex = 0;

    void Start()
    {
        UpdateTimeScale();
        speedButton.onClick.AddListener(ChangeSpeed);
        UpdateButtonText();
    }

    void ChangeSpeed()
    {
        currentSpeedIndex = (currentSpeedIndex + 1) % speedOptions.Length;
        UpdateTimeScale();
        UpdateButtonText();
    }

    void UpdateTimeScale()
    {
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Time.timeScale = speedOptions[currentSpeedIndex];
    }

    void UpdateButtonText()
    {
        speedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Speed: x" + speedOptions[currentSpeedIndex];
    }
}