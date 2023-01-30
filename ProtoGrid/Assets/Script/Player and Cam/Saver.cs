using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Saver 
{
    public static void SaveScore(SaveSystem save)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scores.saves";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData(save);

        formater.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScore()
    {
        string path = Application.persistentDataPath + "/scores.saves";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData data = formatter.Deserialize(stream) as ScoreData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
