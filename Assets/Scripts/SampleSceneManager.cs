using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SampleSceneManager : MonoBehaviour
{
    public static AssetBundle Bundle
    {
        get; private set;
    }
    private const string m_uri = "http://localhost:15000/";
    private const string m_bundleName = "object/room_a";
    private string m_platform = "win";

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        m_platform = "win";
        string uri = Path.Combine(m_uri, m_platform);
#endif
        //관련 에셋번들 로드
        var manifestLoader = AssetBundleManager.Factory(uri, "win");
        yield return manifestLoader.GetAssetBundles();
        var manifestBundle = manifestLoader.Bundle;

        if (manifestBundle == null)
        {
            Debug.LogWarning("could not load manifest");
            yield break;
        }

        var op = manifestBundle.LoadAssetAsync<AssetBundleManifest>("assetbundlemanifest");
        yield return op;

        var manifest = op.asset as AssetBundleManifest;

        foreach (var a in manifest.GetAllAssetBundles())
        {
            Debug.Log(a);
        }

        var deps = manifest.GetAllDependencies(m_bundleName);

        //현재는 dependency가 없는듯...
        foreach (var dep in deps)
        {
            Debug.LogFormat("loading dependency {0}", dep);
            var loader = AssetBundleManager.Factory(uri, dep);
            yield return loader.GetAssetBundles();
        }

        //이제 원하는 에셋번들 로드
        AssetBundleManager roomBundleManager = AssetBundleManager.Factory(uri, m_bundleName);
        yield return roomBundleManager.GetAssetBundles();
        Bundle = roomBundleManager.Bundle;

        if (roomBundleManager.Bundle != null)
        {
            foreach (string assetName in Bundle.GetAllAssetNames())
            {
                AssetBundleRequest abr = Bundle.LoadAssetAsync<GameObject>(assetName);
                yield return abr;
                var prefab = abr.asset as GameObject;
                if (prefab != null)
                {
                    GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }
            }
        }
    }
}
