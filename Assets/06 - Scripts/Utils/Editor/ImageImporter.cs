using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class ImageImporter
{
    [MenuItem("Assets/UI Images/Import all as UI", false)]
    public static void ImportAllAsUIImages()
    {
        ImportAllAsUIImages(Vector4.zero);
    }

    [MenuItem("Assets/UI Images/Import all as UI - With border", false)]
    public static void ImportAllAsUIImagesWithBorder()
    {
        Vector4 border = new Vector4(6f, 6f, 6f, 6f);

        ImportAllAsUIImages(border);
    }

    private static void ImportAllAsUIImages(Vector4 border)
    {
        Object[] selectionList = Selection.objects;
        foreach (Object asset in selectionList)
        {
            if (asset is DefaultAsset folder)
            {
                ImportUIImagesInsideFolder(folder, border);
            }
            else if (asset is Texture2D texture)
            {
                ImportAsUIImage(texture, border);
            }
        }

        AssetDatabase.Refresh();
    }

    private static void ImportUIImagesInsideFolder(DefaultAsset containingFolder, Vector4 border)
    {
        string path = AssetDatabase.GetAssetPath(containingFolder.GetInstanceID());
        string[] filePaths = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories)
            .Where(path => !path.EndsWith(".meta"))
            .ToArray();

        foreach (string filePath in filePaths)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(filePath);
            if (asset is Texture2D texture)
            {
                ImportAsUIImage(texture, border);
            }
        }
    }

    private static void ImportAsUIImage(Texture2D texture, Vector4 border)
    {
        string path = AssetDatabase.GetAssetPath(texture.GetInstanceID());
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spriteBorder = border;
        importer.SaveAndReimport();
    }
}
