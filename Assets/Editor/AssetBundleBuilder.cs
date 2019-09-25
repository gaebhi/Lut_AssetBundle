using UnityEngine;
using UnityEditor;
using System.IO;
public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("AssetBundle/Build")]
    static void BuildAssetBundles()
    {
        string path = EditorUtility.OpenFolderPanel("폴더를 지정 하세요.", "", "");

        string tempPath = path;

        Debug.Log(path);
        tempPath = path + "/win";
        Debug.Log(tempPath);

        if (!Directory.Exists(tempPath))
            System.IO.Directory.CreateDirectory(tempPath);
        BuildPipeline.BuildAssetBundles(path + "/win", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        tempPath = path + "/aos";
        if (!Directory.Exists(tempPath))
            System.IO.Directory.CreateDirectory(tempPath);
        BuildPipeline.BuildAssetBundles(path + "/aos", BuildAssetBundleOptions.None, BuildTarget.Android);

        tempPath = path + "/ios";
        if (!Directory.Exists(tempPath))
            System.IO.Directory.CreateDirectory(tempPath);
        BuildPipeline.BuildAssetBundles(path + "/ios", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}
