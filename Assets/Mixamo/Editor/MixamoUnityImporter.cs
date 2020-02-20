using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Usage:
/// - Create a model using the Adobe Fuse application
/// - Export textures for Unity 5 into a new folder with the same name as the character
/// - From Fuse select File --> Animate with Mixamo
/// - Download the rigged character as a Unity FBX file
/// - Save the FBX beside the exported textures folder
/// - In Unity, click the menu Mixamo --> Import Character
/// - Select the FBX file
/// </summary>
public class MixamoUnityImporter
{
    private enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }

    private class BodyPart
    {
        public string Name;
        public BlendMode BlendMode = BlendMode.Opaque;
        public float BumpScale = 1.0f;
        public float GlossScale = 1.0f;
        public float CutoffAlpha = 0.5f;
    }

    // Configuration for each body part material.
    private static BodyPart[] _parts = {
      new BodyPart { Name="Body", GlossScale=0.7f },
      new BodyPart { Name="Eyes", GlossScale=0.75f },
      new BodyPart { Name="Eyelashes", BlendMode=BlendMode.Fade },
      new BodyPart { Name="Top" },
      new BodyPart { Name="Bottom" },
      new BodyPart { Name="Hair", BlendMode=BlendMode.Fade, GlossScale=0.95f },
      new BodyPart { Name="Moustache", BlendMode=BlendMode.Fade, GlossScale=0.6f },
      new BodyPart { Name="Beard", BlendMode=BlendMode.Fade, GlossScale=0.6f },
      new BodyPart { Name="Eyewear", BlendMode=BlendMode.Fade },
      new BodyPart { Name="Glove" },
      new BodyPart { Name="Shoes" },
      new BodyPart { Name="Hat" },
      new BodyPart { Name="Mask", BlendMode=BlendMode.Cutout, CutoffAlpha=0.75f },
  };

    [MenuItem("Mixamo/Import Character")]
    static void ImportCharacter()
    {
        // Prompt user for FBX file.
        var selected = EditorUtility.OpenFilePanel("Select FBX file created by Mixamo", "", "fbx");
        if (selected.Length == 0)
        {
            // No path given
            return;
        }

        // Interpret the selected filename.
        var src_folder = Path.GetDirectoryName(selected);
        var filename = Path.GetFileName(selected);
        var character_name = Path.GetFileNameWithoutExtension(selected);
        // Folder within Assets
        var folder = "Mixamo/" + character_name;

        // Check that a folder exists with textures exported from Adobe Fuse
        if (!Directory.Exists(src_folder + "/" + character_name))
        {
            Debug.Log(
              "No folder exists beside the FBX file containing exported textures. " +
              "The folder must have the same name as the FBX file.");
            return;
        }

        // Check that the destination folder doesn't exist.
        if (Directory.Exists(Application.dataPath + "/" + folder))
        {
            Debug.Log("Asset already exists: " + folder);
            return;
        }

        // Prepare the asset processor.
        MixamoAssetProcessor.Init(folder);

        // Create folders.
        _create_folder("Mixamo");
        var abs_folder = _create_folder(folder);

        // Copy file into the new folder.
        File.Copy(selected, abs_folder + "/" + filename, overwrite: true);
        // Cause Unity to import any changed assets.
        AssetDatabase.Refresh();

        // Delete all imported textures since we use ones exported from Adobe Fuse
        AssetDatabase.DeleteAsset("Assets/" + folder + "/" + character_name + ".fbm");

        // Copy the textures exported from Adobe Fuse
        _directory_copy(src_folder + "/" + character_name, abs_folder + "/Textures", true);
        AssetDatabase.Refresh();

        // Disable the asset processor.
        MixamoAssetProcessor.Disable();

        // Process materials.
        _process_model(character_name, folder, filename);

        // Move the source FBX model file to a Model folder.
        AssetDatabase.CreateFolder("Assets/" + folder, "Model");
        AssetDatabase.MoveAsset("Assets/" + folder + "/" + character_name + ".fbx", "Assets/" + folder + "/Model/" + character_name + ".fbx");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Asset post processor class to modify textures during import.
    /// 
    /// WARNING: if assets are moved around outside of Unity then this script will
    ///          *not* be run when files are re-imported so they will be broken :(
    /// 
    /// </summary>
    public class MixamoAssetProcessor : AssetPostprocessor
    {
        private static string ExpectedPath = null;
        private string lowerPath = null;

        public static void Init(string expectedPath)
        {
            ExpectedPath = expectedPath.ToLower();
        }

        public static void Disable()
        {
            ExpectedPath = null;
        }

        private bool IsExpected()
        {
            if (ExpectedPath == null)
                return false;
            lowerPath = assetPath.ToLower();
            return lowerPath.Contains(ExpectedPath);
        }

        void OnPreprocessTexture()
        {
            if (!IsExpected())
                return;
            var importer = (TextureImporter)assetImporter;
            if (lowerPath.Contains("_normal"))
            {
                importer.textureType = TextureImporterType.NormalMap;
            }
            else
            {
                importer.alphaIsTransparency = true;
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            if (!IsExpected())
                return;

            // Invert the alpha channel of the MetallicAndSmoothness images exported from Adobe Fuse.
            if (!lowerPath.Contains("_metallicandsmoothness"))
                return;
            for (int m = 0; m < texture.mipmapCount; m++)
            {
                Color[] c = texture.GetPixels(m);
                for (int i = 0; i < c.Length; i++)
                {
                    c[i].a = 1 - c[i].a;
                }
                texture.SetPixels(c, m);
            }
            // Instead of setting pixels for each mip map levels, you can also
            // modify only the pixels in the highest mip level. And then simply use
            // texture.Apply(true); to generate lower mip levels.
        }
    }

    /// <summary>
    /// Create a folder within the Assets folder.
    /// Does nothing if it already exists.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Returns the absolute path.</returns>
    private static string _create_folder(string path)
    {
        var abs = Application.dataPath + "/" + path;
        if (Directory.Exists(abs))
        {
            return abs;
        }
        var parent = Path.GetDirectoryName(path);
        var folder = Path.GetFileName(path);
        AssetDatabase.CreateFolder("Assets/" + parent, folder);
        return abs;
    }

    private static void _directory_copy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                _directory_copy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    static void _process_model(string character_name, string folder, string filename)
    {
        //var abs_folder = Application.dataPath + "/" + folder;
        folder = "Assets/" + folder;

        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(folder + "/" + filename);

        // Create a game object in the hierarchy.
        var holder = GameObject.Find("Mixamo");
        if (holder == null)
        {
            holder = new GameObject("Mixamo");
        }
        //var model = GameObject.Instantiate(asset, holder.transform);
        var model = (GameObject)PrefabUtility.InstantiatePrefab(asset, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        model.transform.parent = holder.transform;
        model.name = character_name;

        // Collect all the materials.
        var meshes = asset.GetComponentsInChildren<SkinnedMeshRenderer>();

        bool has_eyes = meshes.Any(m => m.name == "Eyes");

        // Rename the "default" object to the correct value.
        var default_object = model.transform.Find("default");
        if (default_object != null)
        {
            if (has_eyes)
            {
                default_object.name = "Eyelashes";
            }
            else
            {
                default_object.name = "Eyes";
            }
        }

        var default_mesh = meshes.FirstOrDefault(m => m.name == "default")?.transform;
        if (default_mesh != null)
        {
            default_mesh.name = has_eyes ? "Eyelashes" : "Eyes";
            Debug.Log("Here I am!" + has_eyes + " " + default_mesh.name);

        }

        var materials = new Dictionary<string, Material>();

        bool body_mat_processed = false;

        AssetDatabase.CreateFolder(folder, "Materials");

        foreach (var mesh in meshes)
        {
            // Sometimes Adobe Fuse model has an "Eyes" object *or* and "Eyelashes" object
            // and the one it doesn't have is called "default".

            //extract embedded materials and adjust mesh
            var shared_mats = new Material[mesh.sharedMaterials.Length];

            for (var i = 0; i < mesh.sharedMaterials.Length; i++)
            {
                var mat = mesh.sharedMaterials[i];

                if (mat.name == "Bodymat")
                {
                    if (!body_mat_processed)
                    {
                        _extract_material(folder, mat, materials, "Eyesmat");
                        _extract_material(folder, mat, materials, "Eyelashesmat");
                        body_mat_processed = true;
                    }

                    if (mesh.name == "Eyes" || mesh.name == "Eyelashes")
                    {
                        shared_mats[i] = materials[mesh.name + "mat"];
                        continue;
                    }
                }

                _extract_material(folder, mat, materials);
                shared_mats[i] = materials[mat.name];
            }

            // directly applying shared_mats to the current mesh seems to be delayed and misses the last item
            // just update the model directly
            model.transform.Find(mesh.name).GetComponent<SkinnedMeshRenderer>().sharedMaterials = shared_mats;
        }



        foreach (var part in _parts)
        {

            var material_name = part.Name + "mat";

            if (!materials.ContainsKey(material_name))
                continue;

            var m = materials[material_name];

            // Set rendering mode.
            m.SetFloat("_Mode", (float)part.BlendMode);

            // Set textures. Use Body for Eyes and Eyelashes.
            var tp = "_" + part.Name;
            if (part.Name == "Eyes" || part.Name == "Eyelashes")
            {
                tp = "_Body";
            }
            var texBase = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + "/Textures/" + character_name + tp + "_BaseColor.png");
            m.SetTexture("_MainTex", texBase);
            m.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture>(folder + "/Textures/" + character_name + tp + "_MetallicAndSmoothness.png"));
            m.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture>(folder + "/Textures/" + character_name + tp + "_Normal.png"));
            m.SetTexture("_OcclusionMap", AssetDatabase.LoadAssetAtPath<Texture>(folder + "/Textures/" + character_name + tp + "_AmbientOcclusion.png"));

            // Override blend mode for non-alpha hair.
            // Override a transparent BlendMode if the texture is actually opaque-only.
            if (part.BlendMode == BlendMode.Fade || part.BlendMode == BlendMode.Transparent)
            {
                // Check texture type.
                switch (texBase.format)
                {
                    // Ignore formats that have alpha.
                    case TextureFormat.ARGB4444:
                    case TextureFormat.RGBA32:
                    case TextureFormat.ARGB32:
                    case TextureFormat.RGBA4444:
                    case TextureFormat.BGRA32:
                    case TextureFormat.RGBAHalf:
                    case TextureFormat.RGBAFloat:
                    case TextureFormat.PVRTC_RGBA2:
                    case TextureFormat.PVRTC_RGBA4:
                    case TextureFormat.ETC2_RGBA8:
                    case TextureFormat.ETC2_RGBA1:
                    case TextureFormat.ASTC_RGBA_4x4:
                    case TextureFormat.ASTC_RGBA_5x5:
                    case TextureFormat.ASTC_RGBA_6x6:
                    case TextureFormat.ASTC_RGBA_8x8:
                    case TextureFormat.ASTC_RGBA_10x10:
                    case TextureFormat.ASTC_RGBA_12x12:
                    case TextureFormat.DXT5: // NOTE: DXT5 has alpha, DXT1 is usually non-alpha
                        break;
                    default:
                        // Fall back to opaque.
                        m.SetFloat("_Mode", (float)BlendMode.Opaque);
                        break;
                }
            }

            // Configure the part parameters.
            m.SetFloat("_GlossMapScale", part.GlossScale);
            m.SetFloat("_BumpScale", part.BumpScale);

            if (part.BlendMode == BlendMode.Cutout)
            {
                m.SetFloat("_Cutoff", part.CutoffAlpha);
            }

            // Find which parameters are which...
            //m.SetFloat("_Glossiness", 0.1f);
            //m.SetFloat("_GlossMapScale", 0.2f);
            //m.SetFloat("_SmoothnessTextureChannel", 0.3f);
            //m.SetFloat("_Metallic", 0.4f);
            //m.SetFloat("_SpecularHighlights", 0.5f);
            //m.SetFloat("_GlossyReflections", 0.6f);
        }

        // Create prefab to hold changes.
        var old_model = model;
        var prefab = PrefabUtility.SaveAsPrefabAsset(old_model, folder + "/" + character_name + ".prefab");

        // Recreate the instance from the new prefab.
        model = (GameObject)PrefabUtility.InstantiatePrefab(prefab, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        model.transform.parent = holder.transform;

        // Remove the original prefab.
        GameObject.DestroyImmediate(old_model);

        // Must do this so that the changes are immediately visible in the editor.
        foreach (var m in materials.Values)
        {
            MaterialUpdater.MaterialChanged(m, MaterialUpdater.WorkflowMode.Metallic);
        }

        // Select the new game object.
        _expand_hierarchy_object(holder);
        Selection.activeGameObject = model;
    }

    private static void _extract_material(string folder, Material mat, Dictionary<string, Material> materials, string name = null)
    {
        if (name == null)
            name = mat.name;

        if (!materials.ContainsKey(name))
        {
            var asset_mat = new Material(mat);
            asset_mat.name = name;

            var path = folder + "/Materials/" + name + ".mat";
            AssetDatabase.CreateAsset(asset_mat, path);
            materials[name] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }
    }

    public static void _expand_hierarchy_object(GameObject go, bool collapse = false)
    {
        // bail out immediately if the go doesn't have children
        if (go.transform.childCount == 0) return;
        // get a reference to the hierarchy window
        var windowName = "General/Hierarchy";
        EditorApplication.ExecuteMenuItem("Window/" + windowName);
        var hierarchy = EditorWindow.focusedWindow;
        // select our go
        Selection.activeGameObject = go;
        // create a new key event (RightArrow for collapsing, LeftArrow for folding)
        var key = new Event { keyCode = collapse ? KeyCode.RightArrow : KeyCode.LeftArrow, type = EventType.KeyDown };
        // finally, send the window the event
        hierarchy.SendEvent(key);
    }


    /// <summary>
    /// Below code is based on code from builtin_shaders-5.6.0f3.zip
    /// Source file is builtin_shaders-5.6.0f3/Editor/StandardShaderGUI.cs
    /// 
    /// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
    /// </summary>
    class MaterialUpdater
    {
        public enum WorkflowMode
        {
            Specular,  // The older classic mode
            Metallic,  // Normal mode for Standard shader
            Dielectric
        }

        public enum SmoothnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
        }

        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", "");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    break;
                case BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
            }
        }

        static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
        {
            int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int)SmoothnessMapChannel.AlbedoAlpha)
                return SmoothnessMapChannel.AlbedoAlpha;
            else
                return SmoothnessMapChannel.SpecularMetallicAlpha;
        }

        static void SetMaterialKeywords(Material material, WorkflowMode workflowMode)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
            if (workflowMode == WorkflowMode.Specular)
                SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
            else if (workflowMode == WorkflowMode.Metallic)
                SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap"));
            SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
            SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));

            // A material's GI flag internally keeps track of whether emission is enabled at all, it's enabled but has no effect
            // or is enabled and may be modified at runtime. This state depends on the values of the current flag and emissive color.
            // The fixup routine makes sure that the material is in the correct state if/when changes are made to the mode or color.
            MaterialEditor.FixupEmissiveFlag(material);
            bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
            SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

            if (material.HasProperty("_SmoothnessTextureChannel"))
            {
                SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha);
            }
        }

        // Metallic is the normal mode for the Standard shader.
        public static void MaterialChanged(Material material, WorkflowMode workflowMode = WorkflowMode.Metallic)
        {
            SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));
            SetMaterialKeywords(material, workflowMode);
        }

        static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }
    }

}
