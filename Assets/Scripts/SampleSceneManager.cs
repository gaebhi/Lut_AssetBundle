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

    public static AssetBundle OtherBundle
    {
        get; private set;
    }

    private const string m_uri = "http://kkd89.cafe24app.com/AssetBundles";

    private const string m_bundleName = "object/env";
    private const string m_otherBundleName = "object/others";

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

        var request = manifestBundle.LoadAssetAsync<AssetBundleManifest>("assetbundlemanifest");
        yield return request;

        var manifest = request.asset as AssetBundleManifest;

        foreach (var temp in manifest.GetAllAssetBundles())
        {
            Debug.Log(temp);
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
        AssetBundleManager otherBundleManager = AssetBundleManager.Factory(uri, m_otherBundleName);
        yield return otherBundleManager.GetAssetBundles();
        OtherBundle = otherBundleManager.Bundle;

        if (OtherBundle != null)
        {
            Debug.Log(OtherBundle);
            foreach (string assetName in OtherBundle.GetAllAssetNames())
            {
                Debug.Log(assetName);
                var abr = OtherBundle.LoadAssetAsync<GameObject>(assetName);
                yield return abr;
                var prefab = abr.asset as GameObject;
                if (prefab != null)
                {
                    Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }
            }
        }

        AssetBundleManager bundleManager = AssetBundleManager.Factory(uri, m_bundleName);
        yield return bundleManager.GetAssetBundles();
        Bundle = bundleManager.Bundle;

        if (Bundle != null)
        {
            foreach (string assetName in Bundle.GetAllAssetNames())
            {
                var abr = Bundle.LoadAssetAsync<GameObject>(assetName);
                yield return abr;
                var prefab = abr.asset as GameObject;
                if (prefab != null)
                {
                    Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }
            }
        }


    }
}
