using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.IO;
using System;

public class HeatmapCounter : MonoBehaviour
{


    private int id = 0;

    private float x_offset = 0;
    private float y_offset = 0;
           
    private int x_size = 28;
    private int y_size = 14;

    private float[,] heatmap;
    private float timeDelta = 0;

    private string outputPath = "";

    private static string outputFileName = "Logs.txt";
    private static string outputFolder = "LabelsLogs/";
    
    [SerializeField]
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        id = GetComponent<WindowTimeCountingScript>().id;

        var ts = transform.position;

        x_offset = ts.x;
        y_offset = ts.y;

        var colliderTS = GetComponent<BoxCollider2D>().size;

        x_size = (int)colliderTS.x;
        y_size = (int)colliderTS.y;


        Debug.Log(GameLoggingScript.outputPath);
        this.outputPath = GameLoggingScript.outputPath + '/' + id + ".txt";
        File.Create(outputPath);
        Debug.Log($"Start of gamelogger for {id}");
        
        heatmap = new float[x_size,y_size];
        //for (int i = 0; i < x_size; i++)
        //{
        //    for (int j = 0; j < y_size; j++)
        //    {
        //        heatmap[i, j] = 0;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Pong");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out player))
        {
            Vector2 pos = collision.transform.position;
            int pos_x = (int)Math.Floor(pos.x - x_offset + x_size - 28 / 2);
            int pos_y = (int)Math.Floor(pos.y - y_offset - 27 / 2 + 20);
            Debug.Log(pos_x + " " + pos_y + " " + outputPath);
            if (pos_x < 0 || pos_x >= x_size || pos_y < 0 || pos_y >= y_size)
            {
                return;
            }
            heatmap[pos_x, pos_y] += Time.deltaTime;
        }

        
    }

    private void OnDestroy()
    {
        string str = "";
        for (int i = 0; i < y_size; i++)
        {
            for (int j = 0; j < x_size; j++)
            {
                str += heatmap[j,i] + " ";
            }
            WriteLineToLog(str);
            Debug.Log(str);
            str = "";
        }
    }

    public void WriteLineToLog(string str)
    {
        //Write some text to the test.txt file

        StreamWriter writer = new StreamWriter(outputPath, true);
        
        writer.WriteLine(str);
        Debug.Log($"Wrote {str} to {outputPath}");
        
        writer.Close();
    }
}
