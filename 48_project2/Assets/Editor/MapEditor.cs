using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateLevel))]
public class MapEditor : Editor {
    public int depth;
    public Room curRoom;
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        GenerateLevel map = target as GenerateLevel;
        map.generate(depth, curRoom);

    }
}
