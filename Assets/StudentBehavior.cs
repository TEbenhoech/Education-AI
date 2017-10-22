using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentBehavior : MonoBehaviour {

    public int question;
    public float time;

    public Material confused;
    public Material gettingAnswered;
    public Material understand;

    private float questionTime;

    private void checkForQuestion() {
        if (Random.Range(0, 100) > 90) {
            question = 2;
            time = Random.Range(0f, 1f);
            GameObject.Find("Professor").GetComponent<Behavior>().studentQuestions.Add(this);
        }
        questionTime = Time.time;
    }
	
    void Start () {
        questionTime = Time.time;
    }

	// Update is called once per frame
	void Update () {
        if (Time.time > questionTime + 1f)
            checkForQuestion();
        switch (question)
        {
            case 0:
                gameObject.GetComponent<Renderer>().material = understand;
                break;
            case 1:
                gameObject.GetComponent<Renderer>().material = gettingAnswered;
                break;
            case 2:
                gameObject.GetComponent<Renderer>().material = confused;
                break;
        }
	}
}
