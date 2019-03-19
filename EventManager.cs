using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class EventManager : MonoBehaviour
{
    private const int numcheck = 5;
    private const int numstats = 12;

    private const string FILE_NAME = "Events.txt";
    List<Event> EventList = new List<Event>();
    static System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadEvents()
    {
        using (TextReader reader = File.OpenText("test.txt"))
        {

            if (reader.Peek() == -1) return;
            string title = reader.ReadLine();

            string text = reader.ReadLine();

            string checkline = reader.ReadLine();
            string[] checkbits = checkline.Split(' ');
            int[] check = new int[numcheck];
            for (int i = 0; i < numcheck; i++) check[i] = int.Parse(checkbits[i]);

            string successline = reader.ReadLine();
            string[] successbits = successline.Split(' ');
            int[] success = new int[numstats];
            for (int i = 0; i < numstats; i++) check[i] = int.Parse(successbits[i]);

            string failureline = reader.ReadLine();
            string[] failurebits = failureline.Split(' ');
            int[] failure = new int[numstats];
            for (int i = 0; i < numstats; i++) check[i] = int.Parse(failurebits[i]);

            Event e = new Event();
            EventList.Add(e);
        }
    }

    Event GetEvent()
    {
        int r = rnd.Next(EventList.Count);
        return EventList[r];
    }
}
