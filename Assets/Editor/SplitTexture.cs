using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 将图片分成多张小图
/// </summary>
public static class SplitTexture
{
    [MenuItem("Tools/SplitTexture")]
    static void ImageSplit()
    {
        // 获取选取的图片
        Texture2D texture = Selection.activeObject as Texture2D;

        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(texture));
        string path = rootPath + "/" + texture.name + ".png";
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        // 设置为可读的
        importer.isReadable = true;
        AssetDatabase.ImportAsset(path);

        // 创建文件夹
        AssetDatabase.CreateFolder(rootPath, texture.name);

        foreach (SpriteMetaData metaData in importer.spritesheet)
        {
            int width = (int)metaData.rect.width;
            int height = (int)metaData.rect.height;
            Texture2D smallImg = new Texture2D(width, height);

            int pixelStartX = (int)metaData.rect.x;
            int pixelEndX = pixelStartX + width;
            int pixelStartY = (int)metaData.rect.y;
            int pixelEndY = pixelStartY + height;

            for (int x = pixelStartX; x <= pixelEndX; x++)
            {
                for (int y = pixelStartY; y <= pixelEndY; y++)
                {
                    smallImg.SetPixel(x - pixelStartX, y - pixelStartY, texture.GetPixel(x, y));
                }
            }

            // 转换纹理到 EncodeToPNG 兼容模式
            if (smallImg.format != TextureFormat.ARGB32 && smallImg.format != TextureFormat.RGB24)
            {
                Texture2D img = new Texture2D(smallImg.width, smallImg.height);
                img.SetPixels(smallImg.GetPixels(0), 0);
                smallImg = img;
            }

            // 保存小图文件
            string smallImgPath = rootPath + "/" + texture.name + "/" + metaData.name + ".png";
            File.WriteAllBytes(smallImgPath, smallImg.EncodeToPNG());

            // 刷新资源窗口界面
            AssetDatabase.Refresh();

            // 设置小图的格式
            TextureImporter samllTextureImport = AssetImporter.GetAtPath(smallImgPath) as TextureImporter;

            // 设置为可读
            samllTextureImport.isReadable = true;

            // 设置 alpha 通道
            samllTextureImport.alphaIsTransparency = true;

            // 不开启 mipmap
            samllTextureImport.mipmapEnabled = false;
            AssetDatabase.ImportAsset(smallImgPath);
        }
    }
}
