using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{


    [Header("génération")]
    public float HueMax;
    public float HueMin;
    public float SatMax;
    public float SatMin;
    public float ValMax;
    public float ValMin;









    [Header(" ")]
    [Header(" ")]
    [Header(" ")]

    public bool TextureDebugLive;
    public int TextureDimensionX;
    public int TextureDimensionY;


    public float ystretch;
    public float xstretch;
    public float zoom;
    public float XOffset;
    public float YOffset;
    public Color color1;
    [Range(-1.0f, 1.0f)]
    public float palier1;
    public Color color2;
    [Range(-1.0f, 1.0f)]
    public float palier2;
    public Color color3;
    [Range(-1.0f, 1.0f)]
    public float palier3;
    public Color color4;



    public float ystretch2;
    public float xstretch2;
    public float zoom2;
    public float XOffset2;
    public float YOffset2;
    public Color color12;
    [Range(-1.0f, 1.0f)]
    public float palier12;
    public Color color22;
    [Range(-1.0f, 1.0f)]
    public float palier22;
    public Color color32;
    [Range(-1.0f, 1.0f)]
    public float palier32;
    public Color color42;









    public Texture2D koiMask;

    public ComputeShader shader;
    public RenderTexture texture;
    public Texture2D output;

    [Range(-1.0f, 1.0f)]
    public float R;

    [Range(-1.0f, 1.0f)]
    public float G;

    [Range(-1.0f, 1.0f)]
    public float B;

    public RawImage targetR;

    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(ButtonClick);
    }


    public void ButtonClick()
    {
      RandomizeParameter();
      UpdateTexture();
    }




    void Update()
    {
      if(TextureDebugLive)
      {
        RandomizeParameter();
        UpdateTexture();
      }
    }







    void RandomizeParameter()
    {

      ystretch = Random.Range(0.2f, 2f);
      xstretch = Random.Range(0.1f, 1.15f);
      zoom = Random.Range(0.1f, 6f);
      XOffset = Random.Range(-100f, 100f);
      YOffset = Random.Range(-100f, 100f);
      color1 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color2 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color3 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color4 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      palier1 = Random.Range(0.1f, 0.6f);
      palier2 = Random.Range(palier1, 0.9f);
      palier3 = Random.Range(palier2, 1.0f);


      ystretch2 = Random.Range(0.2f, 2f);
      xstretch2 = Random.Range(0.1f, 1.15f);
      zoom2 = Random.Range(0.1f, 6f);
      XOffset2 = Random.Range(-100f, 100f);
      YOffset2 = Random.Range(-100f, 100f);
      color12 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color22 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color32 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      color42 = Random.ColorHSV(HueMin, HueMax, SatMin, SatMax, ValMin, ValMax);
      palier12 = Random.Range(0.1f, 0.6f);
      palier22 = Random.Range(palier12, 0.9f);
      palier32 = Random.Range(palier22, 1.0f);















      UpdateTexture();

    }

















    void UpdateTexture()
    {
      if( texture == null)
      {
        texture = new RenderTexture(TextureDimensionX,TextureDimensionY,24);
        texture.enableRandomWrite = true;
        texture.Create();
      }
      shader.SetFloat("width", TextureDimensionX);
      shader.SetFloat("height", TextureDimensionY);
      shader.SetFloat("zoom", zoom);




      shader.SetFloat("xoffset", XOffset);
      shader.SetFloat("yoffset", YOffset);

      shader.SetFloat("xstretch", xstretch);
      shader.SetFloat("ystretch", ystretch);
      //paliers
      shader.SetFloat("Palier1", palier1);
      shader.SetFloat("Palier2", palier2);
      shader.SetFloat("Palier3", palier3);

      shader.SetVector("col1", new Vector3(color1.r, color1.g, color1.b));
      shader.SetVector("col2", new Vector3(color2.r, color2.g, color2.b));
      shader.SetVector("col3", new Vector3(color3.r, color3.g, color3.b));
      shader.SetVector("col4", new Vector3(color4.r, color4.g, color4.b));




      shader.SetFloat("zoom2", zoom2);
      shader.SetFloat("xoffset2", XOffset2);
      shader.SetFloat("yoffset2", YOffset2);

      shader.SetFloat("xstretch2", xstretch2);
      shader.SetFloat("ystretch2", ystretch2);
      //paliers
      shader.SetFloat("Palier12", palier12);
      shader.SetFloat("Palier22", palier22);
      shader.SetFloat("Palier32", palier32);

      shader.SetVector("col12", new Vector3(color12.r, color12.g, color12.b));
      shader.SetVector("col22", new Vector3(color22.r, color22.g, color22.b));
      shader.SetVector("col32", new Vector3(color32.r, color32.g, color32.b));
      shader.SetVector("col42", new Vector3(color42.r, color42.g, color42.b));







      shader.SetTexture(0, "Mask", koiMask);

      shader.SetFloat("R", R);
      /*
      shader.SetFloat("G", G);
      shader.SetFloat("B", B);*/

      shader.SetTexture(0, "Result", texture);
      shader.Dispatch(0, texture.width/8, texture.height/8, 1);

      output = toTexture2D(texture);
      targetR.texture = output;

    }












      Texture2D toTexture2D(RenderTexture rTex)
      {
      Texture2D tex = new Texture2D(TextureDimensionX, TextureDimensionY, TextureFormat.ARGB32, false);
      // ReadPixels lo active RenderTexture.
      RenderTexture.active = rTex;
      tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
      tex.Apply();
      return tex;
      }























}
