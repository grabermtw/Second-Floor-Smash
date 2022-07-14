using UnityEngine;
using System.IO;

public class ExportCamera : MonoBehaviour
{
    public Camera Camera3D;
    public int ImageSize;
    public Sprite Result;

    public Transform target;
    public string filename;

    private RenderTexture mRenderTexture;

    private void Awake()
    {
        SetTarget(target);    
    }

    public void SetTarget(Transform target)
    {
        mRenderTexture = new RenderTexture(ImageSize, ImageSize, 32);
        Camera3D.targetTexture = mRenderTexture;
        transform.SetParent(target, false);
        Camera3D.Render();
        transform.SetParent(null, false);

        OnPostRender();
    }

    private void OnPostRender()
    {
        if(mRenderTexture != null)
        {
            RenderTexture.active = mRenderTexture;
            var virtualPhoto = new Texture2D(ImageSize, ImageSize, TextureFormat.RGBA32, false);
            virtualPhoto.ReadPixels(new Rect(0, 0, ImageSize, ImageSize), 0, 0);
            virtualPhoto.Apply();

            RenderTexture.active = null;
            Camera3D.targetTexture = null;

            Result = Sprite.Create(virtualPhoto, new Rect(Vector2.zero, new Vector2(ImageSize, ImageSize)), Vector2.zero);
            SaveTextureToFile(Result.texture, filename);
            mRenderTexture = null;
        }
    }

    private void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes=  texture.EncodeToPNG();
        string path = Application.dataPath + "/" + fileName;
        FileStream file = File.Open(path, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();

        Debug.Log("Wrote sprite to " + path);
    }
}