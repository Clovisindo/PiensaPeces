using Assets.Scripts.Components;
using Assets.Scripts.Services.Bounds;

public class NPCFishController : BaseFishController
{
    public void Init(IBoundsService boundsService)
    {
        var limiter = GetComponent<TransformLimiter>();
        limiter?.Init(boundsService);

        stateMachine.ChangeState(new SwimState(this, boundsService, stateMachine, speed));
    }

    //private void Update()
    //{
    //    // cada cierto tiempo cambiar de swin a idle o frases
    //}
}