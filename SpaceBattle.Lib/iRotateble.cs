namespace SpaceBattle.Lib;

public interface IRotateble
{
    public Angle Angle { get; set; }
    public Angle Angular_Velocity { get; }
}