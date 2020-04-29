// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Client;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using AIR.UnityTestPilot.Interactions;

namespace AIR.UnityTestPilotRemote.Tests
{

    [TestClass]
    public class UnityDriverWaitTests
    {
        private const string AGENT_PATH = "./Agent/RemoteHost.exe";
        private const int TIMEOUT_SECONDS = 1;
        UnityDriver _driver;

        [TestInitialize]
        public async Task Setup()
            => _driver = await UnityDriverRemote.Attach(AGENT_PATH);
            
        [TestMethod]
        public async Task Until_LateElementCreation_ElementFound()
        {
            // Arrange
            var timeout = TimeSpan.FromSeconds(TIMEOUT_SECONDS+1);
            const string DELAY_COUNTER_TEXT_NAME = "Delayed_Counter_Text";
            var textQuery = _driver.FindElement(By.Name(DELAY_COUNTER_TEXT_NAME));

            var currentText = textQuery.Text;
            var text = currentText.Split(new []{' '}).Last();
            var currentCounter = int.Parse(text);

            // Act
            const string DELAY_COUNTER_BUTTON_NAME = "Delayed_Counter_Button";
            var buttonQuery = _driver.FindElement(By.Name(DELAY_COUNTER_BUTTON_NAME));
            buttonQuery.LeftClick();
            
            const string EXPECTED_EFFECT_TEXT_NAME = "DelayedEffect_Text_{0}"; 
            var nameString = string.Format(EXPECTED_EFFECT_TEXT_NAME, currentCounter + 1);
            var wait = new UnityDriverWait(_driver, timeout);
            var expectedTextName = await wait.Until(d => d.FindElement(By.Type<Text>(nameString)));

            // Assert
            Assert.IsTrue(expectedTextName.IsActive, nameString + " not found.");
        }

        [TestMethod]
        public async Task Until_ElementExists_FindsNextFrame()
        {
            // Arrange
            const string SEARCHABLE_TEXT_NAME = "Searchable_Text";
            var timeout = TimeSpan.FromSeconds(TIMEOUT_SECONDS);

            // Act
            var wait = new UnityDriverWait(_driver, timeout);
            var expectedTextName = await wait.Until(d =>
                d.FindElement(By.Type<Text>(SEARCHABLE_TEXT_NAME)));
            
            // Assert
            Assert.IsTrue(expectedTextName.IsActive);
        }

        [TestMethod]
        public async Task Until_MissingElement_TimesOut()
        {
            
            // Arrange
            const string SEARCHABLE_TEXT_NAME = "NotActuallyInScene_Text";
            var timeoutSpan = TimeSpan.FromSeconds(TIMEOUT_SECONDS);
            var endTime = DateTime.Now + timeoutSpan;
            
            // Act
            var wait = new UnityDriverWait(_driver, timeoutSpan);
            await wait.Until(d =>
                d.FindElement(By.Type<Text>(SEARCHABLE_TEXT_NAME)));
            
            // Assert
            Assert.IsTrue(
                DateTime.Now >= endTime,
                "Test finished before timeout.");

        }

    }
}