using System.Threading.Tasks;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class UiElementRemoteTests
    {
        
        private const string AGENT_PATH = "./Agent/RemoteHost.exe";
        UnityDriverRemote _agent;
        
        [TestInitialize]
        public async Task TestInitialize()
        {
            _agent = await UnityDriverRemote.Build(AGENT_PATH);
        }

        [TestCleanup]
        public async Task TestCleanup() {
            await _agent.DisposeAsync();
        }
        
        [TestMethod]
        [DataRow("Searchable_Button")]
        [DataRow("Searchable_Text")]
        [DataRow("Searchable_Image")]
        public void Text_OnTextElement_ReturnsText(string elementName)
        {
            // Act
            var query = _agent.FindElement(By.Name(elementName));

            // Assert
            Assert.AreEqual(elementName, query.Name, "Query returned wrong object.");
            Assert.IsTrue(query.IsActive, "Found object was not active.");
        }
    }
}