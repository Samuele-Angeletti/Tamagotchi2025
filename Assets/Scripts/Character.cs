using System;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour, ISubscriber
{
    [SerializeField] AnimationTriggers animationTriggers;
    Animator animator;
    CharacterModel characterModel;
    string[] _animationTriggersArray;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        characterModel = new CharacterModel()
        {
            ID = Guid.NewGuid().ToString(),
            NewAnimation = animationTriggers.Moonwalk
        };

        Publisher.Subscribe(this, typeof(CharacterReceivedMessage));

        _animationTriggersArray = new string[]
        {
            animationTriggers.Moonwalk,
            animationTriggers.Robot,
            animationTriggers.Wave
        };
    }

    private void Start()
    {
        characterModel.Position = new Position()
        {
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z
        };

        characterModel.Rotation = new Rotation()
        {
            X = transform.rotation.eulerAngles.x,
            Y = transform.rotation.eulerAngles.y,
            Z = transform.rotation.eulerAngles.z
        };

        Publisher.Publish(new CharacterBornMessage(characterModel));
    }

    public void OnPublish(IPublisherMessage message)
    {
        if (message is CharacterReceivedMessage receivedMessage)
        {
            if (characterModel.ID != receivedMessage.Model.ID)
                return;

            UpdateCharacter(receivedMessage);
        }
    }

    private void UpdateCharacter(CharacterReceivedMessage receivedMessage)
    {
        characterModel = receivedMessage.Model;
        transform.SetPositionAndRotation(
            new Vector3(
            characterModel.Position.X,
            characterModel.Position.Y,
            characterModel.Position.Z),
            Quaternion.Euler(
                characterModel.Rotation.X,
                characterModel.Rotation.Y,
                characterModel.Rotation.Z));

        animator
            .SetTrigger(_animationTriggersArray
            .FirstOrDefault(x => x.Equals(receivedMessage.Model.NewAnimation)) 
            ?? animationTriggers.Moonwalk);
    }

    public void OnDisableSubscriber()
    {
        Publisher.Unsubscribe(this, typeof(CharacterReceivedMessage));
    }
}

[Serializable]
public class AnimationTriggers
{
    public string Moonwalk = "Moonwalk";
    public string Robot = "Robot";
    public string Wave = "Wave";
}