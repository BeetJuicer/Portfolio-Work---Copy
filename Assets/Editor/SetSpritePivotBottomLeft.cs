using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SetSpritePivotBottomLeft : Editor
{
    [MenuItem("Assets/Set Sprite Pivots to Bottom Left")]
    static void SetPivotsToBottomLeft()
    {
        // Get all selected textures
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.spriteImportMode == SpriteImportMode.Multiple)
            {
                // Get existing sprite metadata
                List<SpriteMetaData> spritesheet = new List<SpriteMetaData>(importer.spritesheet);

                // Update each sprite's pivot to bottom-left (0, 0)
                for (int i = 0; i < spritesheet.Count; i++)
                {
                    SpriteMetaData data = spritesheet[i];
                    data.alignment = (int)SpriteAlignment.BottomLeft;
                    data.pivot = new Vector2(0f, 0f);
                    spritesheet[i] = data;
                }

                // Apply changes
                importer.spritesheet = spritesheet.ToArray();
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();

                Debug.Log($"Updated pivots for: {obj.name}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Sprite pivot update complete!");
    }

    [MenuItem("Assets/Set Sprite Pivots to Bottom Left", true)]
    static bool ValidateSetPivotsToBottomLeft()
    {
        // Only enable menu item if a texture is selected
        foreach (Object obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.spriteImportMode == SpriteImportMode.Multiple)
            {
                return true;
            }
        }
        return false;
    }
}