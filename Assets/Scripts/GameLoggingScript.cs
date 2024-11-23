using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class GameLoggingScript : MonoBehaviour
{
	private static GameLoggingScript _instance;

	public static string outputPath = "";
	public static string outputFile = "";


    private static string outputFileName = "Logs.txt";
	private static string outputFolder = "LabelsLogs/";

	public static GameLoggingScript Instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad(this.gameObject);
		
		Directory.CreateDirectory(Application.dataPath + '/' + outputFolder);
		outputPath = Application.dataPath + '/' + outputFolder +
						DateTime.Today.Day.ToString() +
						DateTime.Today.Month.ToString() +
						DateTime.Now.Hour +
						DateTime.Now.Minute +
						DateTime.Now.Second;
		//+ outputFileName;
		outputFile = outputPath + outputFileName;
        File.Create(outputFile);
        Directory.CreateDirectory(outputPath);
		Debug.Log("Start of gamelogger ended and writer instantiated");
	}

	private void Start()
	{	

	}

	private void OnDestroy()
	{
		TimeSpentCounter.WriteToOutput();
		CoinPickUpLogging.WriteToOutput();
	}

	public static void WriteLineToLog(string str)
	{
		//Write some text to the test.txt file

		StreamWriter writer = new StreamWriter(outputFile, true);

		writer.WriteLine(str);
		Debug.Log($"Wrote {str} to {outputFile}");

		writer.Close();
	}

	public static void InteractedWithInteractable(INTERACTABLE_TYPE type, string title)
	{
		Debug.Log($"Just interacted with {type} with title {title} - logging");
		WriteLineToLog($"Interacted with {type.ToString()} {title}");
	}


}