using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class fileManager// : MonoBehaviour
{
    public static int masterVolume;
    public static int effectVolume;
    public static int musicVolume;

    public static bool invertY;

    static DirectoryInfo dir = Directory.CreateDirectory("../saves");
    


    public static void save()
    {
        using (StreamWriter writer = new StreamWriter(dir.FullName))
        {
            writer.WriteLine(masterVolume + '\n');
            writer.WriteLine(effectVolume + "\n");
            writer.WriteLine(musicVolume + "\n");
            writer.WriteLine(invertY);
            writer.Close();
        }
    }

    public static void load()
    {
        using (StreamReader reader = new StreamReader(dir.FullName))
        {
            masterVolume = int.Parse(reader.ReadLine());
            effectVolume = int.Parse(reader.ReadLine());
            musicVolume = int.Parse(reader.ReadLine());
            invertY= bool.Parse(reader.ReadLine());
        }
    }
    
}
