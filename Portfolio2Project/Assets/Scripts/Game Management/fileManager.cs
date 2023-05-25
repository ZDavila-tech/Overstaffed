using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class fileManager// : MonoBehaviour
{
    public static float masterVolume = 1;
    public static float effectVolume = 1;
    public static float musicVolume = 1;

    public static bool invertY = false;

    static DirectoryInfo dir = Directory.CreateDirectory("../saves");
    static FileStream saveFile = new FileStream(dir.FullName + "/saveFile", FileMode.Create, FileAccess.ReadWrite);
    


    public static void save()
    {
        using (StreamWriter writer = new StreamWriter(saveFile))
        {
            //writer.
            writer.WriteLine(masterVolume + '\n');
            writer.WriteLine(effectVolume + "\n");
            writer.WriteLine(musicVolume + "\n");
            writer.WriteLine(invertY);
            writer.Close();
        }
    }

    public static void load()
    {
        using (StreamReader reader = new StreamReader(saveFile.Name,false))
        {
            masterVolume = float.Parse(reader.ReadLine());
            effectVolume = float.Parse(reader.ReadLine());
            musicVolume = float.Parse(reader.ReadLine());
            invertY= bool.Parse(reader.ReadLine());
        }
    }
    
}
