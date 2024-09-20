using MedTech.Managers;

[TestFixture]
public class MessageManagerTests
{
    [Test]
    public void AddNewMessageToCollection()
    {
        // Arrange
        MessageManager messageManager = new MessageManager();

        // Act
        int initialCount = messageManager.Messages.Count;
        messageManager.AddMessage("Unit test message");

        // Assert
        Assert.That(messageManager.Messages.Count, Is.EqualTo(initialCount + 1));
        Assert.IsTrue(messageManager.Messages.Any(msg => msg.Text == "Unit test message"));
    }
}