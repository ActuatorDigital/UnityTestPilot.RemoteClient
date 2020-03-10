using System;
using AIR.UnityTestPilot.Remote;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnityTestPilotRemote.Tests
{
    [TestClass]
    public class UiTestAgentTests
    {

        [TestMethod]
        public async Task Build_WithInstalledExecutable_ThrowsNoExceptions() {
            // Arrange
            try {
                // Act
                await using (await UiTestAgent.Build()) { }
            } catch (Exception ex) {
                // Assert
                Assert.Fail("Expected no exception, but caught: " + ex.Message );  
            }

        }

        [TestMethod]
        [DataRow("Example1_Button")]
        [DataRow("Example2_Text")]
        [DataRow("Example3_Text")]
        public async Task Query_ForActiveElements_ReturnsNamesAndActivity(string elementName) {
            // Arrange
            await using var agent = await UiTestAgent.Build();
            
            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Button");
            var query = await agent.Query(mockQuery);
                
            // Assert
            Assert.AreEqual(elementName, query.Name, "Query returned wrong object.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");
        }

        [TestMethod]
        [DataRow("Nonexistent_Button")]
        [DataRow("Nonexistent_Text")]
        [DataRow("Nonexistent_Image")]
        public async Task Query_ForNonexistentElements_ReturnsNullElements(string elementName) {
            // Arrange
            await using var agent = await UiTestAgent.Build();
            
            // Act
            var mockQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, elementName, "Button");
            var query = await agent.Query(mockQuery);
                
            // Assert
            Assert.AreEqual(String.Empty, query.Name);
            Assert.IsFalse(query.IsActive);
        }
        
        [TestMethod]
        public async Task LeftClick_AboutButton_ActivatesAboutMenu() {
            // Arrange
            const string ABOUT_BUTTON_NAME = "ExampleButton"; 
            await using var agent = await UiTestAgent.Build();
            var aboutButtonQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, ABOUT_BUTTON_NAME, "Button");
            var aboutButtonElement = await agent.Query(aboutButtonQuery);
            
            // Act
            agent.LeftClick(aboutButtonElement);
                
            // Assert
            var aboutViewQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, "Example_View", "ExampleView");
            var aboutViewElement = await agent.Query(aboutViewQuery);
            Assert.IsTrue(aboutViewElement.IsActive, "About view was not activated.");
        }

    }
}
