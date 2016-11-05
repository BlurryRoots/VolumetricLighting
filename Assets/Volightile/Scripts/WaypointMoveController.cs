using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlurryRoots;

public class WaypointMoveController : BlurryBehaviour {

    [Range (0.1f, float.MaxValue)]
    public float transitionTime = 1f;
    [HideInInspector]    
    public int startIndex = -1;
    public List<Transform> waypoints = new List<Transform> ();
    public bool skipStartOnLoop;
    public bool takeGameObjectPositionAsStart;
    public Transform start;

    protected override void OnStart () {
        if (this.takeGameObjectPositionAsStart || null == this.start) {
            this.start = this.gameObject.transform;
        }

        this.StartCoroutine (this.Move ());
    }

    protected override void OnUpdate () {
        //
    }

    private IEnumerator Move () {
        var lightIndex = 0;
        var lastWaypoint = this.start;

        if (-1 < this.startIndex) {
            lightIndex = this.startIndex + 1;
            lastWaypoint = this.waypoints[this.startIndex];
        }

        while (null != this.waypoints && lightIndex < this.waypoints.Count + 1) {
            var nextWaypoint = lightIndex == this.waypoints.Count
                ? (this.hasLoopedAlready && this.skipStartOnLoop ? this.waypoints[0] : this.start)
                : this.waypoints[lightIndex]
                ;
            
            var runningTime = 0f;
            while (runningTime < this.transitionTime) {
                var alpha = runningTime / this.transitionTime;
                var lerpPosition = Vector3.Lerp (
                    lastWaypoint.position, nextWaypoint.position,
                    alpha
                );
                var lerpRotation = Quaternion.LerpUnclamped (
                    lastWaypoint.rotation, nextWaypoint.rotation,
                    alpha
                );

                this.gameObject.transform.position = lerpPosition;
                this.gameObject.transform.rotation = lerpRotation;

                runningTime += Time.deltaTime;
                yield return null;
            }

            ++lightIndex;
            lastWaypoint = nextWaypoint;

            yield return null;

            if (lightIndex == this.waypoints.Count) {
                this.hasLoopedAlready = true;
            }
        }

        this.StartCoroutine (this.Move ());
    }

    private bool hasLoopedAlready;

}
