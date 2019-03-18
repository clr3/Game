using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour {
    [SerializeField] private string goal;
    TextMeshProUGUI txt;
    private int currentGoal = 0;

    void Start() {
        txt = gameObject.GetComponent<TextMeshProUGUI>();
        txt.text = "x" + currentGoal;
    }

    void Update() {
        txt.text = "x" + currentGoal;
        currentGoal = PlayerPrefs.GetInt(goal);
    }
}