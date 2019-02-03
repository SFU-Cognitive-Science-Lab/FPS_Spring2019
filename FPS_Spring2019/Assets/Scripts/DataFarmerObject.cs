using UnityEngine;

public class DataFarmerObject : IDataFarmerObject
{
    readonly protected string tag;
    protected float timestamp;
    public long participant;
    public long trial;
    public ParticipantStatus.Condition condition;
    public string category;

    public DataFarmerObject(string tag)
    {
        this.tag = tag;
        this.timestamp = Time.time;
        ParticipantStatus ps = ParticipantStatus.GetInstance();
        this.participant = ps.GetParticipant();
        this.trial = ps.GetTrial();
        this.condition = ps.GetCondition();
        this.category = ps.GetCategory();
    }
    public float GetTimestamp()
    {
        return timestamp;
    }
    public string GetTag()
    {
        return tag;
    }
    public long GetTrial()
    {
        return this.trial;
    }
    public long GetParticipant()
    {
        return this.participant;
    }
    public virtual string Serialize()
    {
        return string.Format("{0},{1},{2},{3},{4},{5}",
            tag, participant, timestamp, condition, trial, category);
    }
}