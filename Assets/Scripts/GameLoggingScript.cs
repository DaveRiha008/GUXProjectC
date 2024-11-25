using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading;
using System.Drawing;


public class GameLoggingScript : MonoBehaviour
{
	private static GameLoggingScript _instance;

    public static string outputPath = "";
	public static string outputFile = "";
	public static string heatmapFile = "";


    private static string outputFileName = "Logs.txt";
	private static string outputFolder = "LabelsLogs/";

    public static Dictionary<int, float[,]> heatmaps = new Dictionary<int, float[,]>();
	//private HeatmapCounter[] counters;

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
        int x_size = 28;
        int y_size = 14;
		var counters = FindObjectsOfType<HeatmapCounter>(); //GetComponents<HeatmapCounter>();
		foreach (var counter in counters)
		{
			heatmaps.Add(counter.id, new float[x_size, y_size]);
            for (int i = 0; i < x_size; i++)
            {
                for (int j = 0; j < y_size; j++)
                {
                    heatmaps[counter.id][i, j] = 0;
                }
            }
            Debug.Log(counter.id);
		}
	}

	private void OnDestroy()
	{
		TimeSpentCounter.WriteToOutput();
		CoinPickUpLogging.WriteToOutput();
        WriteHeatmap();
    }




	private static void WriteHeatmap()
	{
		foreach (var heatmap in heatmaps)
		{
			StreamWriter writer = new StreamWriter(heatmapFile, true);
			
			writer.WriteLine("Heatmap for part: " + heatmap.Key);
            writer.Close();
            WriteHeatmapPart(heatmap.Key, heatmap.Value);
			
            StreamWriter writer2 = new StreamWriter(heatmapFile, true);
			writer2.WriteLine("################################################################");
            writer2.WriteLine();
			writer2.Close();

        }
    }

	private static void WriteHeatmapPart(int id, float[,] heatmapMatrix)
	{
        string str = "";

        int x_size = 28;
        int y_size = 14;

        for (int i = 0; i < y_size; i++)
        {
            for (int j = 0; j < x_size; j++)
            {
                str += heatmapMatrix[j, i] + " ";
            }
            WriteLineToLog(str, heatmapFile);
			//Debug.Log(str);
            str = "";
        }
    }
	public static void WriteLineToLog(string str, string file)
	{
		//Write some text to the test.txt file

		StreamWriter writer = new StreamWriter(file, true);

		writer.WriteLine(str);
		// Debug.Log($"Wrote {str} to {file}");

		writer.Close();
	}

	public static void InteractedWithInteractable(INTERACTABLE_TYPE type, string title)
	{
		Debug.Log($"Just interacted with {type} with title {title} - logging");
		WriteLineToLog($"Interacted with {type.ToString()} {title}", outputFile);
	}


}