using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Light light;

    public int currentTorchLevel;

    private int maxTorch = 100;
    private int minTorch = 0;

    public int rateOfDecay;

    public bool torchOn = false;

    private IEnumerator torchDecay;

    public event Action<bool> DeclareTorchOnOffEvent;

    private void OnEnable()
    {
        torchDecay = TorchDecay();
        currentTorchLevel = maxTorch;

        SetTorchOnOff(true);
    }

    public void SetTorchOnOff(bool input)
    {   
        torchOn = input;

        if (input)
            StartCoroutine(TorchDecay());

        else
            StopAllCoroutines();

        DeclareTorchOnOffEvent?.Invoke(input);
    }

    private IEnumerator TorchDecay()
    {
        while (torchOn)
        {
            if (currentTorchLevel <= 0)
            {
                torchOn = false;
                yield return null;
                break;
            }

            ChangeTorchLevel(rateOfDecay);

            yield return new WaitForSeconds(1);
        }
    }

    public void ChangeTorchLevel(int amount)
    {
        int newCurrentTorchLevel = currentTorchLevel + amount;

        if (newCurrentTorchLevel >= maxTorch)
            newCurrentTorchLevel = maxTorch;

        else if (newCurrentTorchLevel <= minTorch)
            newCurrentTorchLevel = minTorch;

        currentTorchLevel = newCurrentTorchLevel;
        UpdateLightRange();
    }

    private void UpdateLightRange()
    {
        // Map currentTorchLevel to the desired lightRange range (0 to 50)
        // InverseLerp gets the current position of the number between 0 and 100
        // then Lerps to the equivelant (range is 0 to 50)

        float normalizedTorchLevel = Mathf.InverseLerp(0, 100, currentTorchLevel);
        float mappedLightRange = Mathf.Lerp(0, 50, normalizedTorchLevel);
        
        // Update the light's range
        light.range = mappedLightRange;
    }
}