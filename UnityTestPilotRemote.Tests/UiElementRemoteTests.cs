using System;
using System.Threading;
using System.Threading.Tasks;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine.UI;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class UiElementRemoteTests
    {
        private const string AGENT_PATH = "./Agent/RemoteHost.exe";
        UnityDriverRemote _agent;

        [TestInitialize]
        public async Task TestInitialize()
            //=> _agent = await UnityDriverRemote.Attach();
            => _agent = await UnityDriverRemote.Attach(AGENT_PATH);

        [TestCleanup]
        public async Task TestCleanup() => await _agent.DisposeAsync();

        [TestMethod]
        [DataRow("Searchable_Button")]
        [DataRow("Searchable_Text")]
        [DataRow("Searchable_Image")]
        public void Query_ForActiveElements_ReturnsActive(string elementName)
        {
            // Act
            var query = _agent.FindElement(By.Name(elementName));

            // Assert
            Assert.AreEqual(elementName, query.Name, "Query returned wrong object.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");
        }
        
        [TestMethod]
        [DataRow("DisabledSearchable_Button")]
        public void Query_ForInactiveElements_ReturnsInactive(string elementName)
        {
        
            // Act
            var query = _agent.FindElement(By.Type<Button>(elementName));
            
            // Assert
            Assert.IsFalse(query.IsActive, "Found object was not active.");
        }
        
        [TestMethod]
        [DataRow("Nonexistent_Button")]
        [DataRow("Nonexistent_Text")]
        [DataRow("Nonexistent_Image")]
        public void Query_ForNonexistentElements_ReturnsNullElements(string elementName) {
            
            // Act
            var query = _agent.FindElement(By.Type<Button>(elementName));

            // Assert
            Assert.AreEqual(string.Empty, query.Name);
            Assert.IsFalse(query.IsActive);

        }

        [TestMethod]
        public void LeftClickDown_ClickableButton_DoesNotRelease()
        {
            // Arrange
            
            // Act
            
            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void LeftClickAndHold_WithHoldTime_HoldsThenReleases()
        {
            // Arrange
            const string HOLD_BUTTON_NAME = "Holdable_Button";
            var holdableButtonElement = _agent.FindElement(By.Type<Button>(HOLD_BUTTON_NAME));
            var holdTime = TimeSpan.FromSeconds(1);
            
            // Act
            holdableButtonElement.LeftClickAndHold(holdTime);
            var heldTextBefore = _agent.FindElement(By.Type<Text>("Held_Text"));
            var textContainsReleased = heldTextBefore.Text.Contains("Released");
            Assert.IsTrue(!textContainsReleased);
            
            Thread.Sleep(holdTime);
            
            // Assert
            var clickCounterTextElement = _agent.FindElement(By.Type<Text>("Held_Text"));
            StringAssert.Contains(
                clickCounterTextElement.Text,
                "Released",
                "Release was not regerested after time."
            );
        }

        [TestMethod]
        public void LeftClick_ClickableButton_IncrementsCounterText() {
            
            // Arrange
            const string CLICKABLE_BUTTON_NAME = "Clickable_Button";
            var aboutButtonElement = _agent.FindElement(By.Type<Button>(CLICKABLE_BUTTON_NAME));
            
            // Act
            aboutButtonElement.LeftClick();

            // Assert
            var clickCounterTextElement = _agent.FindElement(By.Type<Text>("ClickableCounter_Text"));
            StringAssert.Contains(
                clickCounterTextElement.Text,
                "1",
                "Counter did not increment."
            );
        }
        
        [TestMethod]
        [DataRow("TwoCities_Text", "It was the best of times")]
        [DataRow("Searchable_Text", "The following elements can be searched:")]
        public void Query_TextElements_ReturnsTextContent(string elementName, string expectedText)
        {
            // Act
            var query = _agent.FindElement(By.Type<Text>(elementName));

            // Assert
            StringAssert.Contains(
                query.Text,
                expectedText,
                "Expected text is not found.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");
        }

        [TestMethod]
        [DataRow("Searchable_Button/Text", "Specific Button")]
        [DataRow("Different_Searchable_Button/Text", "Less Specific Button")]
        public void Query_Path_ReturnsTextContent(string path, string expectedText)
        {
            // Act
            var query = _agent.FindElement(By.Path(path));

            // Assert
            StringAssert.Contains(
                query.Text,
                expectedText,
                "Expected text is not found.");
        }

        [TestMethod]
        [DataRow("Duplicate_Panel_A/Button", "Duplicate_Panel_Output/Text", "A")]
        [DataRow("Duplicate_Panel_B/Button", "Duplicate_Panel_Output/Text", "B")]
        public async Task Query_PathButtonClick_ReturnsTextContent(string pathToClick, string pathToOutput, string expectedText)
        {
            // Act
            var clickQuery = _agent.FindElement(By.Path(pathToClick));
            clickQuery.LeftClick();
            await Task.Delay(200);
            var textQuery = _agent.FindElement(By.Path(pathToOutput));
            await Task.Delay(2000);

            // Assert
            StringAssert.Contains(
                clickQuery.FullPath,
                pathToClick,
                "Returned object does not contain the path.");
            // Assert
            StringAssert.Contains(
                textQuery.Text,
                expectedText,
                "Expected text is not found.");
        }

        [TestMethod]
        [DataRow("ButtonA", "ButtonDupOutputText", "A")]
        [DataRow("ButtonB", "ButtonDupOutputText", "B")]
        public async Task Query_ButtonClick_ReturnsTextContent(string buttonName, string pathToOutput, string expectedText)
        {
            // Act
            var clickQuery = _agent.FindElement(By.Type<Button>(buttonName));
            clickQuery.LeftClick();
            await Task.Delay(200);
            var textQuery = _agent.FindElement(By.Path(pathToOutput));

            // Assert
            StringAssert.Contains(
                textQuery.Text,
                expectedText,
                "Expected text is not found.");
        }

        [TestMethod]
        public async Task Query_PositionedElement_ReturnsPositions()
        {
            // Arrange
            const string EXPECTED_NAME = "PositionedCube";
            float expectedXPos = 25, expectedYPos = 25, expectedZPos = 25;

            // Act
            var query = _agent.FindElement(By.Type<Text>(EXPECTED_NAME));

            // Assert
            Assert.AreSame(expectedXPos, query.LocalPosition.X);
            Assert.AreSame(expectedYPos, query.LocalPosition.Y);
            Assert.AreSame(expectedZPos, query.LocalPosition.Z);
        }
        
        [TestMethod]
        public async Task Query_RotatedElement_ReturnsRotation()
        {
            // Arrange
            const string EXPECTED_NAME = "RotatedCube";
            float expectedXRot = 25, expectedYRot = 25, expectedZRot = 25;

            // Act
            var query = _agent.FindElement(By.Type<Text>(EXPECTED_NAME));

            // Assert
            Assert.AreSame(expectedXRot, query.EulerRotation.X);
            Assert.AreSame(expectedYRot, query.EulerRotation.Y);
            Assert.AreSame(expectedZRot, query.EulerRotation.Z);
        }
    }
}