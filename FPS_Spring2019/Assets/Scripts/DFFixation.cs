using UnityEngine;

public class DFFixation : DataFarmerObject
{
    private string side;
    private int up;
    private int forward;
    private int right;
    private string pointingAt;
    private string displacement;

    public DFFixation(int up, int forward, int right, string side, string displacement) :
        base("fixation")
    {
        this.side = side;
        this.up = up;
        this.forward = forward;
        this.right = right;
        this.displacement = displacement;
    }

    public override string Serialize()
    {
        return string.Format("{0},{1},{2},{3},{4},{5}\n",
                base.Serialize(), up, forward, right, side, displacement);
    }
}
