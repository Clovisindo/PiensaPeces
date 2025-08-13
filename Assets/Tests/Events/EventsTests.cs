using NUnit.Framework;
using System;

namespace Game.Events.Tests
{
    public class TestEvent : IEvent { }

    public class EventBindingTests : IEvent
    {
        [Test]
        public void Constructor_WithActionT_Should_StoreAndInvoke()
        {
            bool called = false;
            var binding = new EventBinding<TestEvent>((e) => called = true);

            var interfaceBinding = (IEventBinding<TestEvent>)binding;

            interfaceBinding.OnEvent.Invoke(new TestEvent());

            Assert.IsTrue(called);
        }

        [Test]
        public void Constructor_WithActionNoArgs_Should_StoreAndInvoke()
        {
            bool called = false;
            var binding = new EventBinding<TestEvent>(() => called = true);

            var interfaceBinding = (IEventBinding<TestEvent>)binding;

            interfaceBinding.OnEventNoArgs.Invoke();

            Assert.IsTrue(called);
        }

        [Test]
        public void Add_Remove_ActionT_ShouldWork()
        {
            int callCount = 0;
            Action<TestEvent> handler = (e) => callCount++;

            var binding = new EventBinding<TestEvent>((e) => { });
            binding.Add(handler);

            var interfaceBinding = (IEventBinding<TestEvent>)binding;
            interfaceBinding.OnEvent.Invoke(new TestEvent());
            Assert.AreEqual(1, callCount);

            binding.Remove(handler);
            interfaceBinding.OnEvent.Invoke(new TestEvent());
            Assert.AreEqual(1, callCount); // no incremento
        }

        [Test]
        public void Add_Remove_ActionNoArgs_ShouldWork()
        {
            int callCount = 0;
            Action handler = () => callCount++;

            var binding = new EventBinding<TestEvent>(() => { });
            binding.Add(handler);

            var interfaceBinding = (IEventBinding<TestEvent>)binding;
            interfaceBinding.OnEventNoArgs.Invoke();
            Assert.AreEqual(1, callCount);

            binding.Remove(handler);
            interfaceBinding.OnEventNoArgs.Invoke();
            Assert.AreEqual(1, callCount); // no incremento
        }

        [Test]
        public void MultipleHandlers_ShouldAllBeCalled()
        {
            int count = 0;
            Action<TestEvent> handler1 = (e) => count++;
            Action<TestEvent> handler2 = (e) => count++;

            var binding = new EventBinding<TestEvent>((e) => { });
            binding.Add(handler1);
            binding.Add(handler2);

            var interfaceBinding = (IEventBinding<TestEvent>)binding;
            interfaceBinding.OnEvent.Invoke(new TestEvent());

            Assert.AreEqual(2, count);
        }
    }
}