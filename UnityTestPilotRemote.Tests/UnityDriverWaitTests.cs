// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Client;
using System.Linq;
using UnityEngine.UI;
using System;

namespace AIR.UnityTestPilotRemote.Tests
{

    [TestClass]
    public class UnityDriverWaitTests
    {
        private const string AGENT_PATH = "./Agent/RemoteHost.exe";
        UnityDriver _driver;

        [TestInitialize]
        public async Task Setup()
        {
            _driver = await UnityDriverRemote.Attach(AGENT_PATH);
        }

        [TestCleanup]
        public void TearDown()
        {
            _driver.Dispose();
        }

        [TestMethod]
        public void Until_LateElementCreation_ElementFound()
        {
            // Arrange
            const string DELAY_COUNTER_TEXT_NAME = "Delayed_Counter_Text";
            var textQuery = _driver.FindElement(By.Name(DELAY_COUNTER_TEXT_NAME));

            var currentText = textQuery.Text;
            var text = currentText.Split(new []{' '}).Last();
            var currentCounter = int.Parse(text);

            // Act
            const string DELAY_COUNTER_BUTTON_NAME = "Delayed_Counter_Button";
            var buttonQuery = _driver.FindElement(By.Name(DELAY_COUNTER_BUTTON_NAME));
            buttonQuery.LeftClick();

            // TODO: Replace.
            System.Threading.Thread.Sleep((int)TimeSpan.FromSeconds(2).TotalMilliseconds);

            const string EXPECTED_EFFECT_TEXT_NAME = "DelayedEffect_Text_{0}"; 
            var nameString = string.Format(EXPECTED_EFFECT_TEXT_NAME, currentCounter + 1);
            var expectedTextName = _driver.FindElement(By.Type<Text>(nameString));

            // Assert
            Assert.IsTrue(expectedTextName.IsActive, nameString + " not found.");
        }

        [TestMethod]
        public async Task Until_ElementExists_FindsNextFrame()
        {
            // // Arrange
            // // _testGo = new GameObject(TEST_GO_NAME);
            // // yield return null;
            // const int NEXT_FRAME = 1;
            // UiElement element = null;
            // var startFrame = Time.frameCount;
            // var wait = new UnityDriverWait(_driver, TimeSpan.FromSeconds(1f));
            //
            // // Act
            // yield return wait.Until(
            //     d => d.FindElement(By.Name(TEST_GO_NAME)),
            //     (v) => element = v
            // );
            //
            // // Assert
            // Assert.IsNotNull(element);
            // Assert.AreEqual(startFrame + NEXT_FRAME, Time.frameCount);
        }

        [TestMethod]
        public async Task Until_MissingElement_TimesOut()
        {
            // // Arrange
            // const int TIMEOUT = 1;
            // float startTime = Time.time;
            // var wait = new UnityDriverWait(_driver, TimeSpan.FromSeconds(TIMEOUT));
            //
            // // Act
            // await wait.Until(
            //     d => null,
            //     (e) => { }
            // );
            //
            // // Assert
            // Assert.Greater(Time.time, startTime + TIMEOUT);
        }

        // private IEnumerator DelayedSpawnGO(string goName, float delay)
        // {
        //     yield return new WaitForSeconds(delay);
        //     _testGo = new GameObject(goName);
        // }
    }
}