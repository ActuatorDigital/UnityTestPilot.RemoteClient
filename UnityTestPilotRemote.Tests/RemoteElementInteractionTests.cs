using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Client;
using AIR.UnityTestPilotRemote.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class RemoteElementInteractionTests
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
        public async Task SelectDropdown_DropdownAndOptionClicked_ChangesToTwo()
        {
            // Arrange
            const string OPTION_TEXT_VALUE = "Two";
            const string TEXT_OUTPUT_NAME = "Dropdown_output_Text"; 
            const string DROPDOWN_NAME = "Dropdown";
            const string DROPDOWN_ITEM_NAME = "Item 1: Two";
             var dropdownQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, DROPDOWN_NAME, "");
            var textOutputQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, TEXT_OUTPUT_NAME, "");
            var dropdownItemQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, DROPDOWN_ITEM_NAME, "");
            var dropdownElement = await _agent.Query(dropdownQuery);

            // Act
            _agent.LeftClick(dropdownElement);
            await Task.Delay(1000);
            var dropdownItemElement = await _agent.Query(dropdownItemQuery);
            _agent.LeftClick(dropdownItemElement);
            await Task.Delay(1000);

            // Assert
            var putputTextElement = await _agent.Query(textOutputQuery);
            StringAssert.Contains(putputTextElement.Text, OPTION_TEXT_VALUE, "Output Text does not match selection from Dropdown.");
        }

        [TestMethod]
        public async Task SelectDropdown_DropdownAndOptionPartialNameClicked_ChangesToTwo()
        {
            // Arrange
            const string OPTION_TEXT_VALUE = "Two";
            const string TEXT_OUTPUT_NAME = "Dropdown_output_Text";
            const string DROPDOWN_NAME = "Dropdown";
            const string DROPDOWN_ITEM_NAME = "*[contains(Two)]";
            var dropdownQuery = new RemoteElementQuery(
               QueryFormat.NamedQuery, DROPDOWN_NAME, "");
            var textOutputQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery, TEXT_OUTPUT_NAME, "");
            var dropdownItemQuery = new RemoteElementQuery(
                QueryFormat.PathQuery, DROPDOWN_ITEM_NAME, "");
            var dropdownElement = await _agent.Query(dropdownQuery);

            // Act
            _agent.LeftClick(dropdownElement);
            await Task.Delay(1000);
            var dropdownItemElement = await _agent.Query(dropdownItemQuery);
            _agent.LeftClick(dropdownItemElement);
            await Task.Delay(1000);

            // Assert
            var putputTextElement = await _agent.Query(textOutputQuery);
            StringAssert.Contains(putputTextElement.Text, OPTION_TEXT_VALUE, "Output Text does not match selection from Dropdown.");
        }
    }
}
