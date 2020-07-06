using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class NoiseGenerator : EditorWindow
{
    [MenuItem("Tools/生成噪声图")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(NoiseGenerator)).Show();
    }

    private int imageHieght = 256;
    private int imageWidth = 256;
    private int step = 1;
    public int scale = 25;


    private Texture2D texture;

    private void OnGUI()
    {
        float xOffset = 20;
        float lineHeight = 20;
        float lineInterval = 10;
        float x = 0, y = 0;

        x = xOffset;  y += 10;
        {
            EditorGUI.LabelField(new Rect(x, y, 100, lineHeight), "图片尺寸");
            x += 100;
            EditorGUI.LabelField(new Rect(x, y, 30, lineHeight), "宽");
            x += 30;
            imageWidth = EditorGUI.IntField(new Rect(x, y, 30, lineHeight), imageWidth);
            x += 30;
            EditorGUI.LabelField(new Rect(x, y, 30, lineHeight), " x 高");
            x += 30;
            imageHieght = EditorGUI.IntField(new Rect(x, y, 30, lineHeight), imageHieght);
        }

        x = xOffset; y += lineHeight + lineInterval;
        {
            EditorGUI.LabelField(new Rect(x, y, 100, lineHeight), "Step");
            x += 100;
            step = EditorGUI.IntField(new Rect(x, y, this.position.width - x - xOffset, lineHeight), step);
        }

        x = xOffset;  y += lineHeight + lineInterval;
        {
            EditorGUI.LabelField(new Rect(x, y, 100, lineHeight), "Scale");
            x += 100;
            scale = EditorGUI.IntField(new Rect(x, y, this.position.width - x - xOffset, lineHeight), scale);
        }

        x = xOffset;  y += lineHeight + lineInterval;
        {
            const int buttonCount = 3;
            const int buttonOffset = 20;
            float buttonWidth = (this.position.width - x - xOffset - buttonOffset * (buttonCount - 1)) / buttonCount;

            if (GUI.Button(new Rect(x, y, buttonWidth, lineHeight), "生成"))
            {
                if (texture != null)
                {
                    DestroyImmediate(texture);
                }

                texture = GenerateNotiseImage();
            }

            x += buttonWidth + buttonOffset;
            if (GUI.Button(new Rect(x, y, buttonWidth, lineHeight), "随机生成"))
            {
                if (texture != null)
                {
                    DestroyImmediate(texture);
                }
                texture = GenerateNotiseImage(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }

            x += buttonWidth + buttonOffset;
            if (GUI.Button(new Rect(x, y, buttonWidth, lineHeight), "保存") && texture != null)
            {
                string filePath = EditorUtility.SaveFilePanel("保存", "", "", ".png");
                if (!string.IsNullOrEmpty(filePath))
                {
                    string lowerFilePath = filePath.ToLower();
                    if (lowerFilePath.EndsWith(".png"))
                    {
                        File.WriteAllBytes(filePath, texture.EncodeToPNG());
                    }
                    else if(lowerFilePath.EndsWith(".jpg"))
                    {
                        File.WriteAllBytes(filePath, texture.EncodeToJPG());
                    }
                    else if (lowerFilePath.EndsWith(".tga"))
                    {
                        File.WriteAllBytes(filePath, texture.EncodeToTGA());
                    }
                    else
                    {
                        Debug.LogError("图片格式错误，仅支持保存为png、jpg或者tga格式的图片");
                    }
                }
            }
        }

        x = xOffset; y += lineHeight + lineInterval;

        float previewWidth = this.position.width - x - xOffset;
        float previewHeight = this.position.height - y - lineInterval;

        GUI.Box(new Rect(x, y, previewWidth, previewHeight), "");
        if (previewWidth > 0 && previewHeight > 0 && texture != null)
        {
            if (texture.width < previewWidth && texture.height < previewHeight)
            {
                GUI.DrawTexture(new Rect(x + (previewWidth - texture.width) * 0.5f, y + (previewHeight - texture.height) * 0.5f, texture.width, texture.height), texture);
            }
            else
            {
                GUI.DrawTexture(new Rect(x, y, previewWidth, previewHeight), texture, ScaleMode.ScaleToFit);
            }
        }
    }

    private void OnDisable()
    {
        if (texture != null)
        {
            DestroyImmediate(texture);
        }
    }

    private Texture2D GenerateNotiseImage(float xOffset = 0.0f, float yOffset = 0.0f)
    {
        Texture2D tex = new Texture2D(imageWidth, imageHieght, TextureFormat.RGB24, false);
        Color[] pixel = new Color[imageWidth * imageHieght];

        for(int i = 0; i < imageHieght; ++i)
        {
            for(int j = 0; j < imageWidth; ++j)
            {
                float sample = Mathf.Clamp01(Mathf.PerlinNoise(i * scale * 1.0f / imageWidth + xOffset, j * scale * 1.0f / imageHieght + yOffset));
                pixel[i * imageWidth + j] = new Color(sample, sample, sample);
            }
        }
        tex.SetPixels(pixel);
        tex.Apply();

        return tex;
    }
}
