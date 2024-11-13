using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUpLogging : MonoBehaviour
{
    [SerializeField]
    COIN_TYPE myType = COIN_TYPE.OBVIOUS;

    static bool alreadyWrittenToOutput = false;

    public static Dictionary<int, int> typesCollectedDict = new Dictionary<int, int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(int i in Enum.GetValues(typeof(COIN_TYPE)))
        {
            typesCollectedDict[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnDestroy()
	{
		//WriteToOutput();
	}

	public void PickedUp()
    {
        typesCollectedDict[(int)myType]++;
    }

	public static void WriteToOutput()
	{
		if (alreadyWrittenToOutput) return;
		alreadyWrittenToOutput = true;
		foreach (int i in typesCollectedDict.Keys)
		{
			GameLoggingScript.WriteLineToLog($"Collected coins of type {((COIN_TYPE)i)} = {typesCollectedDict[i]}");
		}
	}
}
enum COIN_TYPE { OBVIOUS, LESS_OBVIOUS, HIDDEN}