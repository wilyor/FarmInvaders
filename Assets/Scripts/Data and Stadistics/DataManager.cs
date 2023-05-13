using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour
{
    #region VARIABLES
    public Data data;
    public string fileName = "data.dat";

    private string dataPath;

    public static DataManager instance;
    #endregion

    #region EVENTS
    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        dataPath = Application.persistentDataPath + "/" + fileName;
        //Debug.Log(dataPath);

        //Intentamos cargar la info guardada
        LoadData();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Guarda la info de forma persistente
    /// </summary>
    [ContextMenu ("Guardar")]
    public void SaveData()
    {
        //Para serializar y deserializar
        BinaryFormatter bf = new BinaryFormatter();
        //Crea o sobreescribe el archivo
        FileStream file = File.Create(dataPath);
        //Para serializar el contenido del objeto de datos volcandolo al fichero
        bf.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// recupera la info guardada
    /// </summary>
    public void LoadData()
    {
        if (!File.Exists(dataPath)) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(dataPath, FileMode.Open);
        data = (Data)bf.Deserialize(file);
        file.Close();
    }

    /// <summary>
    /// Borra el archivo de data
    /// </summary>
    [ContextMenu ("Delete data")]
    public void DeleteSavedFile()
    {
        File.Delete(dataPath);
    }

    /// <summary>
    /// Para guardar un jugador
    /// </summary>
    public void SavePlayer(string name, int score)
    {
        if (data.players.Count == 0) data.players = new List<PlayerInfo>();
        data.players.Add(new PlayerInfo(name, score));
        SaveData();
    }
    #endregion
}
