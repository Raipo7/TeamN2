namespace SpaceBattle.Lib;

public interface IRotateble
{
    public Angle Angle { get; set; }
    public Angle AngleVelocity { get; }
}
