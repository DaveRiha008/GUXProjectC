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
		Directory.CreateDirectory(Application.dataPath + '/' + outputFolder);
		outputPath = Application.dataPath + '/' +
						outputFolder +
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

		//StreamReader reader = new StreamReader(outputPath);

		//Print the text from the file

		//Debug.Log(reader.ReadToEnd());
		//Debug.Log($"Is the text of {outputPath} file");

		//reader.Close();
		//outputWriter.WriteLine(str);
	}


}