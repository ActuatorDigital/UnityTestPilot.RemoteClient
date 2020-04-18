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
            => _agent = await UnityDriverRemote.Attach();

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
        public void LeftClick_ClickableButton_IncrementsCounterText() {
            
            // Arrange
            const string ABOUT_BUTTON_NAME = "Clickable_Button";
            var aboutButtonElement = _agent.FindElement(By.Type<Button>(ABOUT_BUTTON_NAME));
            
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
        public void Query_TextElemets_ReturnsTextContent(string elementName, string expectedText)
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
    }
}