using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

namespace _Features.Editor
{
    public class FixTexturePlatformSettings: AssetPostprocessor
    {
        #if UNITY_EDITOR
        void OnPostprocessTexture(Texture2D texture)
        {
            EditorCoroutineUtility.StartCoroutine(Fix($"{assetPath}.meta"), this);
        }
   
        private IEnumerator Fix(string metafile)
        {
            // Wait for .meta to be created
            while (!File.ReadAllText(metafile).Contains("platformSettings:"))
                yield return null;

            // Read .meta file
            string original = File.ReadAllText(metafile);
            StringBuilder meta = new(original);

            if (meta.ToString().Contains("iPhone"))
            {
                meta.Replace("iPhone", "iOS");
                Debug.Log("Replaced iPhone to iOS");
            }

            // Save .meta file
            if (meta.ToString() != original)
            {
                File.WriteAllText(metafile, meta.ToString());
                AssetDatabase.Refresh();
            }
        }
        #endif
    }
}