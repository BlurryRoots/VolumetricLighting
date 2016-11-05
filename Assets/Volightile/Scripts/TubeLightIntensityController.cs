using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlurryRoots;

[RequireComponent (typeof (HighSignalAnalyzer))]
public class TubeLightIntensityController : BlurryBehaviour {

    public float intensityGravity = 3f;
    [Range (1f, 4f)]
    public float highBoost = 2f;

    protected override void OnStart () {
        this.rawTube = this.GetComponent<TubeLight> ();

        this.entry = new TubeLightEntry () {
            tube = rawTube,
            lowIntensity = rawTube.m_Intensity,
            highIntensity = this.highBoost * rawTube.m_Intensity
        };

        var analyzer = this.GetComponent<HighSignalAnalyzer> ();
        analyzer.High += this.OnHighSignal;
    }

    protected override void OnLateUpdate () {
        this.entry.alpha = Mathf.Clamp01 (
            this.entry.alpha - Time.deltaTime * this.intensityGravity
        );
        this.rawTube.m_Intensity = Mathf.Lerp (
            this.entry.lowIntensity, this.entry.highIntensity,
            this.entry.alpha
        );
    }

    private void OnHighSignal () {
        this.entry.alpha = 1f;
    }

    private TubeLight rawTube;
    private TubeLightEntry entry;

}
