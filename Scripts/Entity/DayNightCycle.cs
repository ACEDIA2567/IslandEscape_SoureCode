using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; // Vector2 90 0 0
    private int endingDay = -1;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Awake()
    {
        GameManager.Instance.dayNightCycle = this;
    }

    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    public void EndingStart()
    {
        endingDay = 0;
    }

    public void ChangeMorning()
    {
        time = 0.2f;
        if (time < 0.2f || time > 0.8f)
        {

        }
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if(lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
            if (lightSource == sun)
            {
                // ��ħ�� �ǹǷ� ���� �����Ѵ�.
                GameManager.Instance.spawnManger.SpawnBear();

                if (endingDay != -1)
                {
                    endingDay++;
                }
                if (endingDay >= 2)
                {
                    // ���� üũ �� 2���� ������ �ʿ� �︮���� ����
                    GameManager.Instance.spawnManger.SpawnHelicopter();
                }
            }
        }
    }
}
