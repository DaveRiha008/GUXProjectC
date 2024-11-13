using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpentCounter : MonoBehaviour
{
    public static Dictionary<int, float> screenTimeSpentDict = new Dictionary<int,float>(); 

    static bool alreadyWrittenToOutput = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddTime(int screenID, float time)
    {
        if (!screenTimeSpentDict.ContainsKey(screenID)) screenTimeSpentDict[screenID] = 0;

        screenTimeSpentDict[screenID] += time;
    }
	private void OnDestroy()
	{
        WriteToOutput();
	}

    public static void WriteToOutput()
    {
        if (alreadyWrittenToOutput) return;
        Debug.Log("Writing screen time to output");
		foreach (int id in screenTimeSpentDict.Keys)
		{
			GameLoggingScript.WriteLineToLog($"Time spent on screen with ID {id} = {screenTimeSpentDict[id]}");
		}
        alreadyWrittenToOutput = true;
	}
}
