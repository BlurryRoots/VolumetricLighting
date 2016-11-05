using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlurryRoots;

public class HighSignalAnalyzer : BlurryBehaviour, AudioCallbacks {

    public float threshold = 0.012f;
    public int spectrumIndex = 6;

    public event System.Action High;

    public void OnOnbeatDetected () {

    }

    public void OnSpectrum (float[] spectrum) {
        var v = spectrum[spectrumIndex];

        var isHigh = this.threshold < v;
        if (isHigh) {
            if (null != this.High) {
                this.High ();
            }
        }
    }

    protected override void OnStart () {
        var processor = FindObjectOfType<AudioProcessor> ();
        processor.addAudioCallback (this);
    }

}
