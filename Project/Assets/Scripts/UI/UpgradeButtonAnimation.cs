using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtonAnimation : MonoBehaviour
{
    public int upgradeIndex = 0;
    private bool hovered = false;
    private float animRatio = 0;
    public float animDuration = 0.5f;
    private float angle = 0;

    public Vector3 maxOffset = Vector3.up * 20;
    public Vector3 startPosition;
    public RectTransform translationElement;
    public Transform rotationElement;
    public float rotationSpeed = 30;
    private float hoverEndRotation = 0;
    private float hoverEndHeight = 0;
    private float hoverEndTime = 0;
    public float oscillationSize = 20;
    public float oscillationMaxAngle = 30;
    public float oscillationFrequency = 0.5f;
    public float angleOscillationFrequency = 0.5f;
    public float angleOscillationOffset = 0;
    private float rotationDirection = 1;
    private UpgradeMenu upgradeMenu;

    void Start()
    {
        upgradeMenu = GetComponentInParent<UpgradeMenu>();
    }

    public void HoverStart()
    {
        hovered = true;
        angle = 0;
        animRatio = 0;
        rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        upgradeMenu.ShowUpgradeDescription(upgradeIndex);
    }
    public void Select()
    {
        upgradeMenu.SelectUpgrade(upgradeIndex);
    }

    public void HoverEnd()
    {
        hovered = false;
        hoverEndRotation = oscillationMaxAngle * Mathf.Sin(angle * angleOscillationFrequency * Mathf.PI) * rotationDirection;
        while(hoverEndRotation > 180)
            hoverEndRotation -= 360;
        while(hoverEndRotation < -180)
            hoverEndRotation -= 360;
        hoverEndHeight = animRatio + oscillationSize * Mathf.Sin(angle / oscillationFrequency * Mathf.PI);
        hoverEndTime = 0;
        upgradeMenu.HideUpgradeDescription();
    }

    void Update()
    {
        startPosition = new Vector2(0, translationElement.sizeDelta.y / 2);
        if(hovered)
        {
            animRatio += Time.unscaledDeltaTime * (1 - animRatio) / animDuration;
            if(animRatio > 1)
                animRatio = 1;
            angle += Time.unscaledDeltaTime * animRatio * rotationSpeed;
            translationElement.anchoredPosition = startPosition + maxOffset * (animRatio + oscillationSize * Mathf.Sin(angle * oscillationFrequency * Mathf.PI));
            rotationElement.localRotation = Quaternion.AngleAxis(oscillationMaxAngle * Mathf.Sin((angle * angleOscillationFrequency) * Mathf.PI) * rotationDirection, Vector3.forward);
        }
        else
        {
            animRatio -= Time.unscaledDeltaTime * animRatio / animDuration;
            if(animRatio < 0)
                animRatio = 0;
            hoverEndTime += Time.unscaledDeltaTime;
            if(hoverEndTime > animDuration)
                hoverEndTime = animDuration;
            float hoverEndAnimRatio = hoverEndTime / animDuration;
            translationElement.anchoredPosition = startPosition + maxOffset * hoverEndHeight * (1 - hoverEndAnimRatio) * (1 - hoverEndAnimRatio);
            rotationElement.localRotation = Quaternion.AngleAxis(Mathf.Lerp(hoverEndRotation, 0, 1 - (1 - hoverEndAnimRatio) * (1 - hoverEndAnimRatio)), Vector3.forward);
        }
    }
}
