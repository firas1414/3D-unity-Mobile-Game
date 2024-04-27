using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] Transform ScanerPivot; // THIS IS THE CENTER POINT OF THE SCAN, (WHICH IN OUR CASE IS THE PLAYER)

    public delegate void OnScanDetectionUpdated(GameObject newDetection);

    public event OnScanDetectionUpdated onScanDetectionUpdated;

    [SerializeField] float scanRange;
    [SerializeField] float scaneDuration;

    internal void SetScanRange(float scanRange)
    {
        this.scanRange = scanRange;
    }

    internal void SetScanDuration(float duration)
    {
        scaneDuration = duration;
    }

    internal void AddChildAttached(Transform newChild) // TAKES THE VFX AS A PARAMETER
    {
        newChild.parent = ScanerPivot;
        newChild.localPosition = Vector3.zero; // Ensure that the child's position is precisely at the parent's origin
    }

    internal void StartScan()
    {
        ScanerPivot.localScale = Vector3.zero; // Making the scanning area invisible or non-existent at the start of the scan
        StartCoroutine(StartScanCoroutine());
    }
    

    // GRADUALLY INCREASE THE SCALE OF A PIVOT OBJECT OVER A SPECIFIED DURATION, SIMULATING A SCANNING ANIMATION EFFECT
    IEnumerator StartScanCoroutine()
    {
        // scanRange represents the desired final size of the scan, and scaneDuration represents the time it takes to reach that size.
        // By dividing the scanRange by the scaneDuration, we get the rate at which the scan should grow per unit of time.
        float scanGrowthRate = scanRange / scaneDuration;
        float startTime = 0; // This variable keeps track of the elapsed time during the scan process
        while (startTime < scaneDuration)
        {
            startTime += Time.deltaTime;
            ScanerPivot.localScale += Vector3.one * scanGrowthRate * Time.deltaTime; // Increases the scale
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        onScanDetectionUpdated?.Invoke(other.gameObject);
    }
}
