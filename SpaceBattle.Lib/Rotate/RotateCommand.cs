namespace SpaceBattle.Lib;

public class RotateCommand : ICommand
{
    private IRotateble obj;

    public RotateCommand(IRotateble obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        obj.Angle += obj.AngleVelocity;
    }
}
