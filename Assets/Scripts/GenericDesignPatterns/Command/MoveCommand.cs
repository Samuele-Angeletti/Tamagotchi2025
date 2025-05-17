using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform _character;
    private Vector3 _movement;
    private Vector3 _previousPosition;

    public MoveCommand(Transform character, Vector3 movement)
    {
        _character = character;
        _movement = movement;
        _previousPosition = character.position;
    }

    public void Execute()
    {
       _character.position += _movement;
    }

    public void Undo()
    {
        _character.position = _previousPosition;
    }
}
