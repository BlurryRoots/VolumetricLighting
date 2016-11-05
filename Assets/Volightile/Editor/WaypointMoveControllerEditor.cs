using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (WaypointMoveController))]
public class WaypointMoveControllerEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector ();

        var controller = (WaypointMoveController)this.target;
        var length = null != controller.waypoints
            ? controller.waypoints.Count
            : 0
            ;

        controller.startIndex = EditorGUILayout.IntField ("Start at: ", controller.startIndex);
        if (-1 > controller.startIndex) {
            controller.startIndex = -1;
        }
        if (controller.waypoints.Count < controller.startIndex - 1) {
            controller.startIndex = controller.waypoints.Count - 1;
        }

        if (GUILayout.Button ("Snap to start")) {
            if (null != controller.start) {
                this.SnapTo (controller.start);
            }
        }

        this.selectedWaypoint = EditorGUILayout.IntField ("Snap to waypoint: ", this.selectedWaypoint);
        this.selectedWaypoint = 0 < this.selectedWaypoint
            ? (length > this.selectedWaypoint ? this.selectedWaypoint : length - 1)
            : 0
            ;

        if (GUILayout.Button ("Snap to " + this.selectedWaypoint) && 0 < controller.waypoints.Count) {
            this.SnapTo (controller.waypoints[this.selectedWaypoint]);
        }

        if (GUILayout.Button ("Update waypoint " + this.selectedWaypoint) && 0 < controller.waypoints.Count) {
            controller.waypoints[this.selectedWaypoint].transform.position = controller.gameObject.transform.position;
            controller.waypoints[this.selectedWaypoint].transform.rotation = controller.gameObject.transform.rotation;
        }

        var createWaypoint = GUILayout.Button ("Create new waypoint");
        var createStart = GUILayout.Button ("Create start");
        if (createWaypoint || createStart) {
            var lengthName = createStart ? "start" : (createWaypoint ? "waypoint" : "null");
            var snapshotName = string.Format (
                "{0}-{1}-{2}", controller.gameObject.name, lengthName, length
            );

            var go = new GameObject (snapshotName);
            go.transform.position = controller.transform.position;
            go.transform.rotation = controller.transform.rotation;
            if (createWaypoint) {
                controller.waypoints.Add (go.transform);
            }
            if (createStart) {
                controller.start = go.transform;
            }
        }
    }

    private void SnapTo (Transform t) {
        var controller = (WaypointMoveController)this.target;

        controller.gameObject.transform.position = t.position;
        controller.gameObject.transform.rotation = t.rotation;
    }

    private int selectedWaypoint;

}
