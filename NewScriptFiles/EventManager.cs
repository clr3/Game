using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{

    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI eventTitle;
    [SerializeField] private TextMeshProUGUI eventDesc;
    [SerializeField] private Button challengeStart;
    [SerializeField] private Button eventResolve;
    [SerializeField] private SceneControl challengeSceneControllor;

    private Unit activeUnit;

    Event currentEvent;
    private const int numcheck = 5;
    private const int numstats = 12;

    private const string FILE_NAME = "Events.txt";
    List<Event> EventList = new List<Event>();
    static System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start() {
        LoadEvents();
        TriggerEvent();
    }

    void LoadEvents()
    {
        using (TextReader reader = File.OpenText(FILE_NAME))
        {
            while (reader.Peek() != -1) {
                string title = reader.ReadLine();

                string text = reader.ReadLine();

                string checkline = reader.ReadLine();
                string[] checkbits = checkline.Split(' ');
                int[] check = new int[numcheck];
                for (int i = 0; i < numcheck; i++) check[i] = int.Parse(checkbits[i]);

                string successline = reader.ReadLine();
                string[] successbits = successline.Split(' ');
                int[] success = new int[numstats];
                for (int i = 0; i < numstats; i++) success[i] = int.Parse(successbits[i]);

                string failureline = reader.ReadLine();
                string[] failurebits = failureline.Split(' ');
                int[] failure = new int[numstats];
                for (int i = 0; i < numstats; i++) failure[i] = int.Parse(failurebits[i]);

                Event e = new Event();
                e.Create(title, text, check, success, failure);
                EventList.Add(e);
            }
        }
    }

    Event GetEvent()
    {
        int r = UnityEngine.Random.Range(0, EventList.Count);
        return EventList[r];
    }

    public void TriggerEvent() {
        currentEvent = this.GetEvent(); //take a a new event

        //set event UI up
        eventTitle.text = currentEvent.title;
        eventDesc.text = currentEvent.text;
        eventPanel.SetActive(true);

        if (IsChallenge(currentEvent.check)) {
            eventResolve.enabled = false;
            challengeStart.enabled = true;
        } else {
            eventResolve.enabled = true;
            challengeStart.enabled = false;
        }
    }

    private bool IsChallenge(int[] check) {
        foreach(int val in check) {
            if(val > 0) {
                return true;
            }
        }
        return false;
    }

    public void StartChallenge() {
        challengeSceneControllor.Initialise(activeUnit, currentEvent.check); //NEEDS VARS - character and currentEvent.check;
    }

    public void ResolveEvent() {
        eventPanel.SetActive(false);
    }

    public void HandleSucess() {
    }

    public void HandleFailure() {

    }
}
