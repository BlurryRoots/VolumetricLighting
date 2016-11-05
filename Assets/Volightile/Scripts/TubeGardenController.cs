using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlurryRoots;

public class TubeGardenController : BlurryBehaviour {

    public float intensityGravity = 1f;

    protected override void OnAwake () {
        this.tubes = new List<TubeLightEntry> ();
    }

    protected override void OnStart () {
        var analyzer = this.GetComponent<HighSignalAnalyzer> ();
        analyzer.High += this.OnHighSignal;

        var rawTubes = new List<TubeLight> ();
        rawTubes.AddRange (this.GetComponentsInChildren<TubeLight> ());

        foreach (var rawTube in rawTubes) {
            this.tubes.Add (new TubeLightEntry () {
                tube = rawTube,
                lowIntensity = rawTube.m_Intensity,
                highIntensity = 2f * rawTube.m_Intensity
            });
        }
    }

    protected override void OnLateUpdate () {
        foreach (var t in this.tubes) {
            t.alpha = Mathf.Clamp01 (t.alpha - Time.deltaTime * this.intensityGravity);
            t.tube.m_Intensity = Mathf.Lerp (t.lowIntensity, t.highIntensity, t.alpha);
        }
    }

    private void OnHighSignal () {
        foreach (var t in this.tubes) {
            t.alpha = 1f;
        }
    }

    private List<TubeLightEntry> tubes;

}

[System.Serializable]
public class TubeLightEntry {

    public TubeLight tube;
    public float lowIntensity;
    public float highIntensity;
    public float alpha;

}
