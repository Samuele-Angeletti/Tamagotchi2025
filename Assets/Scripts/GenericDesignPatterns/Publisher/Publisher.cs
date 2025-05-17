using System;
using System.Collections.Generic;

public static class Publisher
{
    private static Dictionary<Type, List<ISubscriber>> _allSubscribers = new Dictionary<Type, List<ISubscriber>>();

    public static void Subscribe(ISubscriber subscriber, Type messageType)
    {
        // se esiste già la chiave di questo messaggio allora aggiungo il subscriber alla lista
        if (_allSubscribers.ContainsKey(messageType))
        {
            _allSubscribers[messageType].Add(subscriber);
        }
        else
        {
            List<ISubscriber> subscriberList = new List<ISubscriber> { subscriber };

            _allSubscribers.Add(messageType, subscriberList);
        }
    }

    public static void Publish(IPublisherMessage message)
    {
        var messageType = message.GetType();

        if (_allSubscribers.ContainsKey(messageType))
        {
            foreach (ISubscriber subscriber in _allSubscribers[messageType])
            {
                subscriber.OnPublish(message);
            }
        }
    }

    public static void Unsubscribe(ISubscriber subscriber, Type messageType)
    {
        if (_allSubscribers.ContainsKey(messageType))
        {
            _allSubscribers[messageType].Remove(subscriber);
        }
    }

    public static void ClearAllSubscribers()
    {
        _allSubscribers.Clear();
    }
}
