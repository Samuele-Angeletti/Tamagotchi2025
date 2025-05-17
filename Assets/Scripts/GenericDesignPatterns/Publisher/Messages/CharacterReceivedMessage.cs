public class CharacterReceivedMessage : IPublisherMessage
{
    public CharacterReceivedMessage(CharacterModel model)
    {
        Model = model;
    }

    public CharacterModel Model { get; }
}