public class CharacterBornMessage : IPublisherMessage
{
    public CharacterBornMessage(CharacterModel character)
    {
        Model = character;
    }

    public CharacterModel Model { get; }
}
