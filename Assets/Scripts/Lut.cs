using UnityEngine;
namespace estgames.mm.GODA2client
{
    public class Lut : MonoBehaviour
    {
        public Texture2D LookUpTexture;

        private Shader m_shader;
        private Material m_material;
        private Texture3D m_LookUpTexture3D;

        void Awake()
        {
            m_shader = Shader.Find("Hidden/LutSimple");
            m_material = new Material(m_shader);
            m_material.hideFlags = HideFlags.HideAndDontSave;
        }

        public void Convert(Texture2D temp2DTex)
        {
            int dim = temp2DTex.height;//16

            Color[] oldColor = temp2DTex.GetPixels();//4096
            Color[] newColor = new Color[oldColor.Length];

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        int _j = dim - j - 1;
                        newColor[i + (j * dim) + (k * dim * dim)] = oldColor[k * dim + i + _j * dim * dim];
                        //Debug.Log("new:"+ (i + (j * dim) + (k * dim * dim)).ToString());
                        //Debug.Log("old:"+ (k * dim + i + j_ * dim * dim).ToString());
                    }
                }
            }
            m_LookUpTexture3D = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
            m_LookUpTexture3D.SetPixels(newColor);
            m_LookUpTexture3D.Apply();
        }

        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
        {
            Convert(LookUpTexture);
            m_LookUpTexture3D.wrapMode = TextureWrapMode.Clamp;
            m_material.SetTexture("_LutTex", m_LookUpTexture3D);
            Graphics.Blit(sourceTexture, destTexture, m_material);
        }
    }
}
