using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public Gradient lightColor; // Color gradient over the day
    public AnimationCurve lightIntensity; // Intensity curve
    public RotateOverTime clockRef; // Reference to clock script
    public float fullCycleDuration = 300f; // seconds

    void Update()
    {
        if (clockRef == null || directionalLight == null) return;

        float t = Mathf.Clamp01(clockRef.elapsedTime / fullCycleDuration);

        directionalLight.color = lightColor.Evaluate(t);
        directionalLight.intensity = lightIntensity.Evaluate(t);

        //also rotate the sun
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(t * 360f - 90f, 170f, 0));
    }
}