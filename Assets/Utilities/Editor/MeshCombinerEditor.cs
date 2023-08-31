using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
[CustomEditor (typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    private string folderPath = "";

    private string lastDirectory;

    private UnityEngine.Object meshParent;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshCombiner mc = target as MeshCombiner;

        //GUILayout.Label(folderPath);

        meshParent = EditorGUILayout.ObjectField("Parent of sub-meshes:", meshParent, typeof(GameObject), true);

        //folderPath = EditorGUILayout.TextField("Mesh export folder:", folderPath);

        // if (GUILayout.Button("Browse mesh output folder"))
        // {
        //     folderPath = EditorUtility.OpenFolderPanel("Mesh output location", "", "");
        // }
        if (GUILayout.Button("Generate Meshes!"))//mc.transform.position + Vector3.up * 5, Quaternion.LookRotation(Vector3.up), 1, 1, Handles.CylinderCap))
        {
            if (meshParent != null)
            {
                // folderPath = EditorUtility.OpenFolderPanel("Mesh output location", "", "");
                folderPath = EditorUtility.SaveFilePanel("Save new mesh as...", "", "CombinedMesh", "obj");
                if (folderPath != null && Directory.Exists(Path.GetDirectoryName(folderPath)))
                {
                    mc.CombineMeshes((GameObject)meshParent, folderPath);
                }
            }
        }

        // if (GUILayout.Button("Delete Mesh"))
        // {
        //     mc.DeleteMesh();
        // }

        // if (GUILayout.Button("Delete Children"))
        // {
        //     mc.DeleteChildren();
        // }
    }

}
#endif