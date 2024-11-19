using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class GameLoggingScript : MonoBehaviour
{
	private static GameLoggingScript _instance;

	private static string outputPath = "";

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
	}

	private void Start()
	{	
		Directory.CreateDirectory(Application.persistentDataPath + '/' + outputFolder);
		outputPath = Application.persistentDataPath + '/' + outputFolder +
						DateTime.Now.ToShortDateString() + 
						DateTime.Now.Hour + 
						DateTime.Now.Minute + 
						DateTime.Now.Second + 
						outputFileName;
		File.Create(outputPath);
		Debug.Log("Start of gamelogger ended and writer instantiated");
	}

	private void OnDestroy()
	{
		TimeSpentCounter.WriteToOutput();
		CoinPickUpLogging.WriteToOutput();
	}

	public static void WriteLineToLog(string str)
	{
		//Write some text to the test.txt file

		StreamWriter writer = new StreamWriter(outputPath, true);

		writer.WriteLine(str);
		Debug.Log($"Wrote {str} to {outputPath}");

		writer.Close();
	}

	public static void InteractedWithInteractable(INTERACTABLE_TYPE type, string title)
	{
		Debug.Log($"Just interacted with {type} with title {title} - logging");
		WriteLineToLog($"Interacted with {type.ToString()} {title}");
	}


}