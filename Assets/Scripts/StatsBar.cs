using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class StatsBar : MonoBehaviour
{

    private Image statsBar;

    [SerializeField] private Gradient colors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsBar = GetComponent<Image>();
        UpdateBar(1);
        
    }

    // Update is called once per frame
    public void UpdateBar (float width)
    {
        // make sure that the image' width is changed to match the incoming variable
        statsBar.fillAmount = width;
        statsBar.material.color = colors.Evaluate(width);
    }
}
