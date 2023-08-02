using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
#if (UNITY_EDITOR) 
    /*  combines all child meshes of an object into 1 mesh */
    public void CombineMeshes(GameObject meshParent, string outputPath)
    {

        // I'm really lazy and don't wanna check if the game object is a prefab or not
        try
        {
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        DeleteMesh();

        Vector3 oldPos = meshParent.transform.position;
        Quaternion oldRot = meshParent.transform.rotation;

        meshParent.transform.rotation = Quaternion.identity;
        meshParent.transform.position = Vector3.zero;

        MeshFilter[] filters = meshParent.transform.GetComponentsInChildren<MeshFilter>();
        Mesh finalMesh = new Mesh();
        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            if (filters[i].transform != transform)
            {
                combiners[i].subMeshIndex = 0;
                combiners[i].mesh = filters[i].sharedMesh;
                combiners[i].transform = filters[i].transform.localToWorldMatrix;
            }
        }

        finalMesh.CombineMeshes(combiners);

        string relativePath = FileUtil.GetProjectRelativePath(outputPath);

        // save to file
        SaveMesh(finalMesh, outputPath, true, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // load in mesh we just exported 
        Mesh newMesh = (Mesh)AssetDatabase.LoadAssetAtPath(relativePath, typeof(Mesh));

        if (meshParent.TryGetComponent<MeshFilter>(out MeshFilter filter))
        {
            filter.sharedMesh = newMesh;
        }
        else
        {
            MeshFilter newFilter = meshParent.AddComponent<MeshFilter>();
            newFilter.sharedMesh = newMesh;
        }

        meshParent.transform.rotation = oldRot;
        meshParent.transform.position = oldPos;
    }

    /*  deletes all child objects */
    public void DeleteChildren()
    {

        if (EditorUtility.DisplayDialog("Confirm", "Are you sure you want to delete " + transform.childCount + " objects?", "Delete", "Do Not Delete"))
        {
            for (int i = 0; i < transform.childCount + i; i++)
            {
                UnityEngine.Object.DestroyImmediate(transform.GetChild(0).gameObject);
                //transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void DeleteMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(mesh, true);
    }

    public static void SaveMesh (Mesh mesh, string filePath, bool makeNewInstance, bool optimizeMesh)
    {        
		string path = FileUtil.GetProjectRelativePath(filePath);

		Mesh meshToSave = (makeNewInstance) ? UnityEngine.Object.Instantiate(mesh) as Mesh : mesh;
		
		if (optimizeMesh)
        {
            MeshUtility.Optimize(meshToSave);
        }
        
        string ext = Path.GetExtension(filePath);
        if (ext == ".obj")
        {
            ExportToObj(meshToSave, filePath);
        }
        else if (ext == ".fbx")
        {

        }
        else
        {
            Debug.LogError(String.Format("Invalid mesh extension \"{0}\". Please save as .obj or .fbx", ext));
        }

		//AssetDatabase.CreateAsset(meshToSave, path);
		//AssetDatabase.SaveAssets();
	}

    static void ExportToObj(Mesh mesh, string exportPath)
    {
        string objName = Path.GetFileNameWithoutExtension(exportPath);

        StringBuilder sb = new StringBuilder();
 
        foreach(Vector3 v in mesh.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        foreach(Vector3 v in mesh.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        for (int material=0; material < mesh.subMeshCount; material++)
        {
            sb.Append(string.Format("\ng {0}\n", objName));
            int[] triangles = mesh.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n",
                triangles[i] + 1,
                triangles[i + 1] + 1,
                triangles[i + 2] + 1));
            }
        }
        StreamWriter writer = new StreamWriter(exportPath);
        writer.Write(sb.ToString());
        writer.Close();
    }

    #endif
}
