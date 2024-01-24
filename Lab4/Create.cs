using Lab4;

internal class Create : Element
{
    public Create(double delay) : base(delay)
    {
        NextTime = 0d;
    }

    public override void OutAct()
    {
        base.OutAct();
        NextTime = CurrentTime + GetDelay();
        NextElement?.InAct();
    }
}