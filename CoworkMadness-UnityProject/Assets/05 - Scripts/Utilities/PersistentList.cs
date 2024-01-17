using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PersistentList : MonoBehaviour
{

    [Header("FileName")] public string _fileName;
    [Header("ObjectList")] public List<ScriptableObject> _objects;

    // Start is called before the first frame update
    private void OnEnable()
    {
        foreach (ScriptableObject persistedObject in _objects)
        {

            if(persistedObject == null) continue;
            
            string fileName = Application.persistentDataPath + string.Format("/{0}_{1}.pso", _fileName, persistedObject.name);

            if (File.Exists(fileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(fileName, FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), persistedObject);
                file.Close();
            }
        }
    }

    // Update is called once per frame
    private void OnDisable()
    {

        foreach (ScriptableObject persistedObject in _objects)
        {
            
            if(persistedObject == null) continue;
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", _fileName, persistedObject.name));
            var json = JsonUtility.ToJson(persistedObject);
            bf.Serialize(file, json);
            file.Close();
        }

    }
}
