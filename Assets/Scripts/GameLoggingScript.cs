using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;


public class GameLoggingScript : MonoBehaviour
{
	private static GameLoggingScript _instance;

    public static string outputPath = "";
	public static string outputFile = "";
	public static string heatmapFile = "";


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
        heatmapFile = outputPath + "heatmap.txt";
		File.Create(heatmapFile);
        Debug.Log("Start of gamelogger ended and writer instantiated");
	}

	private void Start()
	{	
	}

	private void OnDestroy()
	{
		TimeSpentCounter.WriteToOutput();
		CoinPickUpLogging.WriteToOutput();
		StartCoroutine(Wait());
		WriteHeatmap();
    }

	public static void WriteLineToLog(string str)
	{
		//Write some text to the test.txt file

		StreamWriter writer = new StreamWriter(outputFile, true);

		writer.WriteLine(str);
		Debug.Log($"Wrote {str} to {outputFile}");

		writer.Close();
	}

	private static IEnumerator Wait()
	{
        yield return new WaitForSeconds(5);
    }

	private static void WriteHeatmap()
	{
		string heatmapPart = "";
		for (int i = 2; i <= 36; i++)
		{
			heatmapPart = GameLoggingScript.outputPath + '/' + i + ".txt";
			StreamReader reader = new StreamReader(heatmapPart, true);
            StreamWriter writer = new StreamWriter(heatmapFile, true);

			string matrix = reader.ReadToEnd();
			writer.WriteLine("Heatmap for part: "+i);
			writer.WriteLine(matrix);
			writer.WriteLine("################################################################");
			writer.WriteLine();
			writer.Close();
			reader.Close();

        }
    }


	public static void InteractedWithInteractable(INTERACTABLE_TYPE type, string title)
	{
		Debug.Log($"Just interacted with {type} with title {title} - logging");
		WriteLineToLog($"Interacted with {type.ToString()} {title}");
	}


}