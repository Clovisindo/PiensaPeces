using Game.StateMachineManager;
using NSubstitute;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public class StateManagerTests
    {
        private IStateMachine mockStateMachine;
        private StateManager StateManager;

        [SetUp]
        public void SetUp()
        {
            mockStateMachine = Substitute.For<IStateMachine>();
            StateManager = new StateManager(mockStateMachine);
        }

        [Test]
        public void ApplyState_ShouldChangeState_WhenCurrentStateIsNull()
        {
            mockStateMachine.currentState.Returns((IState)null);
            var newState = new MockStateA();

            StateManager.ApplyState(newState);

            mockStateMachine.Received(1).ChangeState(newState);
        }

        [Test]
        public void ApplyState_ShouldCallChangeState_WhenDifferentType()
        {
            var currentState = new MockStateA();
            var newState = new MockStateB(); // tipo distinto
            mockStateMachine.currentState.Returns(currentState);

            StateManager.ApplyState(newState);

            mockStateMachine.Received(1).ChangeState(newState);
        }

        [Test]
        public void ApplyState_ShouldNotCallChangeState_WhenSameType()
        {
            var currentState = new MockStateA();
            var newState = new MockStateA(); // mismo tipo
            mockStateMachine.currentState.Returns(currentState);

            StateManager.ApplyState(newState);

            mockStateMachine.DidNotReceive().ChangeState(Arg.Any<IState>());
        }
    }
}
