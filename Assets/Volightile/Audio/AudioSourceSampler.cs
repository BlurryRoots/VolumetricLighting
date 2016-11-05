using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlurryRoots;

public class AudioSourceSampler : BlurryBehaviour {

    public int channel = 1;
    public event System.Action<float, int> Sampled;

    protected override void OnStart () {
        this.source = this.GetComponent<AudioSource> ();

        var clip = this.source.clip;
        Debug.Log (string.Format ("{0} @ {1}", clip.samples, clip.channels));
        clip.LoadAudioData ();
        this.sampleData = new float[clip.samples * clip.channels];
        clip.GetData (this.sampleData, 0);
    }

    protected override void OnUpdate () {
        var pos = this.source.timeSamples;
        var value = this.sampleData[pos * this.channel];
        if (null != this.Sampled) {
            this.Sampled (value, pos);
        }
    }

    private AudioSource source;
    private float[] sampleData;

}
