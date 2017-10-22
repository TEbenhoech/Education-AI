using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class Behavior : MonoBehaviour
{
    public Material lecturing;
    public Material questions;
    private Root behavior;
    public List<StudentBehavior> studentQuestions;

    private void CheckLecturing()
    {
        if (behavior.Blackboard.Get<bool>("lecture"))
        {
            behavior.Blackboard["lecture"] = Time.time < (behavior.Blackboard.Get<float>("time") + 2f);
        }
        else
        {
            behavior.Blackboard["lecture"] = studentQuestions.Count == 0;
        }
    }

    private void answerQuestion()
    {
        if (studentQuestions.Count == 0)
            return;
        switch (studentQuestions[0].question)
        {
            case 0:
                studentQuestions.RemoveAt(0);
                behavior.Blackboard["time"] = Time.time;
                break;
            case 1:
                if(Time.time > behavior.Blackboard.Get<float>("time") + studentQuestions[0].time)
                {
                    studentQuestions[0].question = 0;
                }
                break;
            case 2:
                studentQuestions[0].question = 1;
                behavior.Blackboard["time"] = Time.time;
                break;
        }
    }

    void Start()
    {
        behavior = CreateBehaviorTree();
        behavior.Start();
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behavior;
#endif
    }

    private Root CreateBehaviorTree()
    {
        return new Root(
            new Service(0.5f, () => CheckLecturing(),
            new Selector(
                new BlackboardCondition("lecture", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,
                    new Sequence(new Action(() => gameObject.GetComponent<Renderer>().material = lecturing), new WaitUntilStopped())),
                new BlackboardCondition("lecture", Operator.IS_EQUAL, false, Stops.IMMEDIATE_RESTART,
                    new Sequence(new Action(() => { gameObject.GetComponent<Renderer>().material = questions; answerQuestion();}))))));
    }

}