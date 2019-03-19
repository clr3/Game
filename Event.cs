using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    private const int numcheck = 5;
    private const int numstats = 12;

    public string title;
    public string text;
    public int[] check = new int[numcheck];
    public int[] success = new int[numstats];
    public int[] failure = new int[numstats];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Create(string _title, string _text, int[] _check, int[] _success, int[] _failure)
    {
        title = _title;
        text = _text;
        for (int i = 0; i < numcheck; ++i)
            check[i] = _check[i];
        for (int i = 0; i < numstats; ++i)
            success[i] = _success[i];
        for (int i = 0; i < numstats; ++i)
            failure[i] = _failure[i];
        return;
    }
}
