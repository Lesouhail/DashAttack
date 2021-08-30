using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Editor
{

    using DashAttack.Level;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WayPointSpawner))]
    public class WayPointSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var spawner = (WayPointSpawner)target;
            if (GUILayout.Button("Add Waypoint"))
            {
                spawner.AddWayPoint();
            }
        }
    }

}
