using NSubstitute;
using NUnit.Framework;

namespace Game.StateMachineManager.Tests
{

    public class StateMachineTests
    {
        StateMachine stateMachine;
        IState currentState;
        IState newState;


        [SetUp]
        public void Setup()
        {
            stateMachine = new StateMachine();
            currentState = Substitute.For<IState>();
            newState = Substitute.For<IState>();
        }

        [Test]
        public void ChangeState_FromNullState_NewStateCallsEnter()
        {
            stateMachine.ChangeState(newState);

            newState.Received(1).Enter();
            newState.DidNotReceive().Exit();
            Assert.AreEqual(newState, stateMachine.currentState);
        }

        [Test]
        public void ChangeState_FromCurrentState_CurrentStateExitAndNewStateEnter()
        {
            stateMachine.ChangeState(currentState);
            stateMachine.ChangeState(newState);

            currentState.Received(1).Exit();
            newState.Received(1).Enter();
            Assert.AreEqual(newState, stateMachine.currentState);
        }

        [Test]
        public void Update_WhenHasState_CallsUpdate()
        {
            stateMachine.ChangeState(currentState);
            stateMachine.Update();

            currentState.Received(1).Update();
        }

        [Test]
        public void Update_WhenNoCurrentState_DoesNothing()
        {
            Assert.DoesNotThrow(() => stateMachine.Update());
        }

        [Test]
        public void ChangeState_ToSameState_StillCallsExitAndEnter()
        {
            stateMachine.ChangeState(currentState);
            stateMachine.ChangeState(currentState);

            currentState.Received(1).Exit();
            currentState.Received(2).Enter(); // se llama 2 veces
        }
    }
}
