using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoidsUIElement : MonoBehaviour
{
    public enum BoidsManagerElement
    {
        Timescale,
        BoidCount,
        MaxSpeed,
        MaxSteering,
        CohesionRadius,
        CohesionWeight,
        AlignmentRadius,
        AlignmentWeight,
        SeparationRadius,
        SeparationWeight,
        AreaWeight
    }

    // Inspector
    [SerializeField] private BoidsManagerElement element;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Vector2 range;
    
    // References
    private Slider slider;
    private TMP_InputField inputField;
    
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    private void Start()
    {
        if (slider)
        {
            slider.minValue = range.x;
            slider.maxValue = range.y;
            slider.onValueChanged.AddListener(UpdateValue);
        }
        inputField?.onValueChanged.AddListener(UpdateValue);
        label?.SetText(element.ToString() + ":");
        
        slider?.SetValueWithoutNotify(GetValue());
        inputField?.SetTextWithoutNotify(GetValue().ToString());
    }

    private float GetValue()
    {
        // Return the relevant value from BoidsManager based on the enum element
        switch (element)
        {
            case BoidsManagerElement.Timescale:
                return BoidsManager.instance.simulationTimeScale;
            case BoidsManagerElement.BoidCount:
                return BoidsManager.instance.boidsCount;
            case BoidsManagerElement.MaxSpeed:
                return BoidsManager.instance.boidsMaxSpeed;
            case BoidsManagerElement.MaxSteering:
                return BoidsManager.instance.boidsMaxSteering;
            case BoidsManagerElement.CohesionRadius:
                return BoidsManager.instance.cohesionRadius;
            case BoidsManagerElement.CohesionWeight:
                return BoidsManager.instance.cohesionWeight;
            case BoidsManagerElement.AlignmentRadius:
                return BoidsManager.instance.alignmentRadius;
            case BoidsManagerElement.AlignmentWeight:
                return BoidsManager.instance.alignmentWeight;
            case BoidsManagerElement.SeparationRadius:
                return BoidsManager.instance.separationRadius;
            case BoidsManagerElement.SeparationWeight:
                return BoidsManager.instance.separationWeight;
            case BoidsManagerElement.AreaWeight:
                return BoidsManager.instance.simulationAreaWeight;
        }

        return 0;
    }

    private void UpdateValue(float value)
    {
        value = Mathf.Clamp(value, range.x, range.y);
        
        switch (element)
        {
            case BoidsManagerElement.Timescale:
                BoidsManager.instance.simulationTimeScale = value;
                break;
            case BoidsManagerElement.BoidCount:
                BoidsManager.instance.boidsCount = (int) value;
                break;
            case BoidsManagerElement.MaxSpeed:
                BoidsManager.instance.boidsMaxSpeed = value;
                break;
            case BoidsManagerElement.MaxSteering:
                BoidsManager.instance.boidsMaxSteering = value;
                break;
            case BoidsManagerElement.CohesionRadius:
                BoidsManager.instance.cohesionRadius = value;
                break;
            case BoidsManagerElement.CohesionWeight:
                BoidsManager.instance.cohesionWeight = value;
                break;
            case BoidsManagerElement.AlignmentRadius:
                BoidsManager.instance.alignmentRadius = value;
                break;
            case BoidsManagerElement.AlignmentWeight:
                BoidsManager.instance.alignmentWeight = value;
                break;
            case BoidsManagerElement.SeparationRadius:
                BoidsManager.instance.separationRadius = value;
                break;
            case BoidsManagerElement.SeparationWeight:
                BoidsManager.instance.separationWeight = value;
                break;
            case BoidsManagerElement.AreaWeight:
                BoidsManager.instance.simulationAreaWeight = value;
                break;
        }
        
        slider?.SetValueWithoutNotify(GetValue());
        inputField?.SetTextWithoutNotify(GetValue().ToString());
    }

    private void UpdateValue(string value)
    {
        UpdateValue(float.Parse(value));
    }
}
