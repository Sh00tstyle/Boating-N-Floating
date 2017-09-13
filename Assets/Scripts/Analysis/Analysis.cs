using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class Analysis : MonoBehaviour
{
    public enum playerTypeEnum
    {
        Achiever,
        Socializer,
        Killer,
        Explorer,
    }

    [Header("Player Info")]
    public playerTypeEnum PlayerType;
    public enum genderEnum
    {
        Female,
        Male,
    }
    public genderEnum Gender;
    public int Age;

    [Header("Logging settings")]
    public string MainPath;
    public GameObject Player;
    public float positionInterval;
    public float LineThickness = 5f;

    private List<Vector3> Positions;
    

    public playerInfo playerinfo;

    public class playerInfo
    {
        public playerTypeEnum PlayerType { get; set; }
        public genderEnum Gender { get; set; }
        public int Age { get; set; }
        public int ShotsFired { get; set; }
        public int ShotsHit { get; set; }
        public int DamageTaken { get; set; }
        public List<Vector3> Positions;
        public int SurvivedTime { get; set; }
    }

    [Header("Display data")]
    public TextAsset XMLFile;

    private void RecordPosition()
    {
        Positions.Add(Player.transform.position);
    }

    private void RecordTime()
    {
        playerinfo.SurvivedTime++;
    }

    private void Start()
    {
        playerinfo = new playerInfo();
        Positions = new List<Vector3>();
        InvokeRepeating("RecordPosition", 0, positionInterval);
        InvokeRepeating("RecordTime", 0, 1);
    }

    private void OnApplicationQuit()
    {
        ParseData();
    }

    private void ParseData()
    {
        playerinfo.Age = Age;
        playerinfo.PlayerType = PlayerType;
        playerinfo.Gender = Gender;
        playerinfo.Positions = Positions;
        XMLHelper.Serialize(playerinfo, Application.dataPath + MainPath + "/" + Gender + "/" + PlayerType, (Gender.ToString() + "-" + PlayerType.ToString()) + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString());
        Debug.Log("Parsed Data as " + (Gender.ToString() + "-" + PlayerType.ToString()) + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString());
    }

    public void GeneratePath()
    {

        playerInfo deserializedinfo = XMLHelper.Deserialize<playerInfo>(Path.GetDirectoryName(AssetDatabase.GetAssetPath(XMLFile)), XMLFile.name);

        GameObject positionObject = new GameObject();
        positionObject.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = positionObject.GetComponent<LineRenderer>();
        positionObject.transform.parent = transform;
        positionObject.transform.name = "Player Path";
        lineRenderer.startWidth = LineThickness;
        lineRenderer.endWidth = LineThickness;

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < deserializedinfo.Positions.Count; i++)
        {
            positions.Add(deserializedinfo.Positions[i]);
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void RemovePath()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
 
    }
}


