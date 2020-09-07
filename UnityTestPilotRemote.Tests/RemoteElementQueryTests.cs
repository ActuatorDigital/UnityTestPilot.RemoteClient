using System;
using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Client;
using AIR.UnityTestPilotRemote.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class RemoteElementQueryTests
    {
        private const string AGENT_PATH = "./Agent/RemoteHost.exe";
        UnityDriverHostProcess _agent;

        [TestInitialize]
        public async Task TestInitialize() 
            => _agent = await UnityDriverHostProcess.Attach(
                AGENT_PATH,
                new[] { "-loadPlugin", "./Plugin/AIR.UnityTestPilot.Remote.dll" }
            );

        [TestCleanup]
        public async Task TestCleanup() => await _agent.DisposeAsync();

        [TestMethod]
        [DataRow("Searchable_Button")]
        [DataRow("Searchable_Text")]
        [DataRow("Searchable_Image")]
        public async Task Query_ForActiveElements_ReturnsNamesAndActivity(string elementName) {
            
            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Button");
            var query = await _agent.Query(mockQuery);
                
            // Assert
            Assert.AreEqual(elementName, query.Name, "Query returned wrong object.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");

        }

        [TestMethod]
        [DataRow("DisabledSearchable_Button")]
        public async Task Query_ForInactiveElements_ReturnsInactive(string elementName)
        {

            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Button");
            var query = await _agent.Query(mockQuery);

            // Assert
            Assert.IsFalse(query.IsActive, "Found object was not active.");
        }

        [TestMethod]
        [DataRow("Nonexistent_Button")]
        [DataRow("Nonexistent_Text")]
        [DataRow("Nonexistent_Image")]
        public async Task Query_ForNonexistentElements_ReturnsNullElements(string elementName) {
            
            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Button");
            var query = await _agent.Query(mockQuery);
                
            // Assert
            Assert.AreEqual(String.Empty, query.Name);
            Assert.IsFalse(query.IsActive);

        }
        
        [TestMethod]
        public async Task LeftClick_ClickableButton_IncrementsCounterText() {
            
            // Arrange
            const string ABOUT_BUTTON_NAME = "Clickable_Button"; 
            var aboutButtonQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, ABOUT_BUTTON_NAME, "Button");
            var aboutButtonElement = await _agent.Query(aboutButtonQuery);
            
            // Act
            _agent.LeftClick(aboutButtonElement);
                
            // Assert
            var aboutViewQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, "ClickableCounter_Text", "Text");
            var clickCounterText = await _agent.Query(aboutViewQuery);
            StringAssert.Contains(clickCounterText.Text, "1", "Counter did not increment.");
        }


        [TestMethod]
        [DataRow("TwoCities_Text", "It was the best of times")]
        [DataRow("Searchable_Text", "The following elements can be searched:")]
        public async Task Query_TextElements_ReturnsTextContent(string elementName, string expectedText)
        {

            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Text");
            var query = await _agent.Query(mockQuery);

            // Assert
            StringAssert.Contains(
                query.Text,
                expectedText,
                "Expected text is not found.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");
        }

    }
}
