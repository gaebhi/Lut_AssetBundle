using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager
{
    private string m_uri;
    private string m_bundleName;
    public AssetBundle Bundle
    {
        get;
        private set;
    }

    public static AssetBundleManager Factory(string _uri, string _bundleName)
    {
        return new AssetBundleManager(_uri, _bundleName);
    }

    private AssetBundleManager(string _uri, string _bundleName)
    {
        m_uri = _uri;
        m_bundleName = _bundleName;
    }

    public IEnumerator GetAssetBundles()
    {
        using (UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(Path.Combine(m_uri, m_bundleName)))
        {
            DownloadHandlerAssetBundle handler = uwr.downloadHandler as DownloadHandlerAssetBundle;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
                yield break;
            }

            Bundle = handler.assetBundle;
        }
    }
}

