using Game.Data;
using Game.Utilities;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

namespace Game.Fishes.Tests
{
    public class SpeechBubbleLogicTests
    {
        private SpeechBubbleLogic logic;
        private ITextMeshProWrapper mockText;
        private ICanvasGroupWrapper mockCanvas;
        private IRectTransformWrapper mockRect;
        private ITimeService mockTimeService;
        private ICoroutineRunner mockCoroutineRunner;
        private IGameObjectFactory mockFactory;
        private SpeechBubbleConfig config;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            // Arrange - Crear todos los mocks
            mockText = Substitute.For<ITextMeshProWrapper>();
            mockCanvas = Substitute.For<ICanvasGroupWrapper>();
            mockRect = Substitute.For<IRectTransformWrapper>();
            mockTimeService = Substitute.For<ITimeService>();
            mockCoroutineRunner = Substitute.For<ICoroutineRunner>();
            mockFactory = Substitute.For<IGameObjectFactory>();
            testGameObject = new GameObject("TestObject");

            config = SpeechBubbleConfig.Default;

            logic = new SpeechBubbleLogic(config, mockText, mockCanvas, mockRect,
                                       mockTimeService, mockCoroutineRunner, mockFactory);
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
                Object.DestroyImmediate(testGameObject);
        }

        [Test]
        public void Show_ShouldSetupTextCorrectly()
        {
            string testText = "Hello test.";
            Vector2 renderedSize = new(100, 50);
            mockText.GetRenderedValues(false).Returns(renderedSize);

            logic.Show(testText, testGameObject);

            mockText.Received(1).FontSize = config.FontSize;
            mockText.Received(1).SetText(testText);
            mockText.Received(1).ForceMeshUpdate();

        }

        [Test]
        public void Show_ShouldAdjustSizeCorrectly()
        {
            string testText = "Hello test.";
            Vector2 renderedSize = new Vector2(100, 50);
            Vector2 expectedSize = renderedSize + config.Padding;
            mockText.GetRenderedValues(false).Returns(renderedSize);

            logic.Show(testText, testGameObject);

            mockRect.Received(1).SizeDelta = expectedSize;
        }

        [Test]
        public void Show_ShouldStartDisplayCoroutine()
        {
            mockText.GetRenderedValues(false).Returns(Vector2.zero);

            logic.Show("test", testGameObject);

            mockCoroutineRunner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }

        [Test]
        public void Show_CalledTwice_ShouldStartCoroutineTwice()
        {
            mockText.GetRenderedValues(false).Returns(Vector2.zero);

            logic.Show("primero", testGameObject);
            logic.Show("segundo", testGameObject);

            //se gestiona a nivel interno el stop, deberiamos hacer un test para este clase
            mockCoroutineRunner.Received(2).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }

        [Test]
        public void Show_ShouldStartCoroutineWitchCorrectGameObject()
        {
            mockText.GetRenderedValues(false).Returns(Vector2.zero);
            IEnumerator capturedRoutine = null;

            //capturamos el objecto que recibe la llamada en el mock
            mockCoroutineRunner.When( x => x.StartDisplayCoroutine(Arg.Any<IEnumerator>()))
                .Do( callObject => capturedRoutine = callObject.Arg<IEnumerator>());

            logic.Show("test", testGameObject);

            Assert.IsNotNull(capturedRoutine);
            mockCoroutineRunner.Received(1).StartDisplayCoroutine(Arg.Any<IEnumerator>());
        }

        [TestCase("")]
        [TestCase("Hola")]
        [TestCase("Texto muy largo que deberia mostrarse correctamente")]
        [TestCase("Texto con caracteres especiales: áéíóú!@#$%")]
        public void Show_WithDifferentTexts_ShouldSetupTextCorrectly(string text)
        {
            Vector2 expectedSize = new Vector2(text.Length * 10, 20);
            mockText.GetRenderedValues(false).Returns(expectedSize);

            logic.Show(text, testGameObject);

            mockText.Received(1).SetText(text);
            mockText.Received(1).FontSize = config.FontSize;
            mockText.Received(1).ForceMeshUpdate();
        }

        [Test]
        public void Show_WithZeroSizedText_ShouldStillApplyPadding()
        {
            mockText.GetRenderedValues(false).Returns(Vector2.zero);
            Vector2 expectedSize = Vector2.zero + config.Padding;

            logic.Show("", testGameObject);

            mockRect.Received(1).SizeDelta = expectedSize;
        }

        [Test]
        public void Show_WithLargeSizedText_ShouldCalculateCorrectSize()
        {
            Vector2 largeTextSize = new Vector2(500, 200);
            mockText.GetRenderedValues(false).Returns(largeTextSize);
            Vector2 expectedSize = largeTextSize + config.Padding;

            logic.Show("Very long text", testGameObject);

            mockRect.Received(1).SizeDelta = expectedSize;
        }

        [Test]
        public void Show_ShouldCallGetRenderedValuesWithCorrectParameter()
        {
            mockText.GetRenderedValues(false).Returns(Vector2.zero);

            logic.Show("test", testGameObject);

            mockText.Received(1).GetRenderedValues(false);
        }

        [Test]
        public void Show_ShouldFollowCorrectSequence()
        {
            mockText.GetRenderedValues(false).Returns(new Vector2(100, 50));

            logic.Show("test", testGameObject);

            Received.InOrder(() =>
            {
                mockText.FontSize = config.FontSize;
                mockText.SetText("test");
                mockText.ForceMeshUpdate();
                mockText.GetRenderedValues(false);
                mockRect.SizeDelta = Arg.Any<Vector2>();
                mockCoroutineRunner.StartDisplayCoroutine(Arg.Any<IEnumerator>());
            });
        }

    }
}
