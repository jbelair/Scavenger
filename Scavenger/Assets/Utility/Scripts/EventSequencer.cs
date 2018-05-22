using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventSequence
{
    [Header("Description")]
    public string name;

    [Header("Events")]
    public bool started = false;
    public UnityEvent start;
    public UnityEvent end;

    [Header("Duration"), Tooltip("If the sequence is set to be conditional it ignores duration, instead waiting for some other behaviour to call its end method")]
    public bool conditional = false;
    public float duration;
    public float durationCurrent;
}

public class EventSequencer : MonoBehaviour
{
    public enum Step { Update, FixedUpdate, LateUpdate };

    public Sequencer.Format format;
    public Step step;
    public int repetitions = 0;
    public int index = 0;

    public List<EventSequence> sequences;

    void EventStart()
    {
        sequences[Mathf.Abs(index)].start.Invoke();
        sequences[Mathf.Abs(index)].started = true;
    }

    void EventEnd()
    {
        if (repetitions > 0 || repetitions < 0)
        {
            sequences[Mathf.Abs(index)].end.Invoke();
            sequences[Mathf.Abs(index)].started = false;

            if (repetitions != 0)
                repetitions--;

            if (format == Sequencer.Format.Loop)
                index = (index + 1) % sequences.Count;
            else if (format == Sequencer.Format.PingPong)
            {
                if (index >= sequences.Count - 1)
                    index = (sequences.Count - 2) * -1;
                else
                    index++;
            }
        }
    }

    public void End()
    {
        EventEnd();
    }

    void EventUpdate()
    {
        if (!sequences[Mathf.Abs(index)].started)
            EventStart();

        if (!sequences[Mathf.Abs(index)].conditional)
        {
            if (sequences[Mathf.Abs(index)].durationCurrent < sequences[Mathf.Abs(index)].duration)
            {
                sequences[Mathf.Abs(index)].durationCurrent += Time.deltaTime;
            }
            else
            {
                sequences[Mathf.Abs(index)].durationCurrent -= sequences[Mathf.Abs(index)].duration;
                EventEnd();
            }
        }
    }

    private void Start()
    {
        //EventStart();
    }

    private void Update()
    {
        if (step == Step.Update)
        {
            EventUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (step == Step.FixedUpdate)
        {
            EventUpdate();
        }
    }

    private void LateUpdate()
    {
        if (step == Step.LateUpdate)
        {
            EventUpdate();
        }
    }
}
