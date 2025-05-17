using System.Collections.Generic;
using UnityEngine;

public class CharacterCommandControlled : MonoBehaviour
{
    public float moveDistance = 1f;
    private List<ICommand> _commandsHistory = new List<ICommand>();
    private int _currentCommandIndex = -1;

    public void Move(Vector3 direction)
    {
        ICommand command = new MoveCommand(transform, direction * moveDistance);
        command.Execute();

        // questo controllo serve a rimuovere il range davanti all'ultimo comando eseguito
        if (_currentCommandIndex < _commandsHistory.Count - 1)
        {
            _commandsHistory.RemoveRange(_currentCommandIndex + 1, _commandsHistory.Count - _currentCommandIndex - 1);
        }

        _commandsHistory.Add(command);
        _currentCommandIndex++;
    }

    public void Undo()
    {
        // controllo se ci sono comandi da annullare
        if (_currentCommandIndex >= 0)
        {
            _commandsHistory[_currentCommandIndex].Undo();
            _currentCommandIndex--;
        }
    }

    public void Redo()
    {
        // controllo se ci sono comandi da ripristinare
        if (_currentCommandIndex < _commandsHistory.Count - 1)
        {
            _currentCommandIndex++;
            _commandsHistory[_currentCommandIndex].Execute();
        }
    }
}