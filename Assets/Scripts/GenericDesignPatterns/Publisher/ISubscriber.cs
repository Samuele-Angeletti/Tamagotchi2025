public interface ISubscriber
{
    // viene chiamata per gestire il messaggio
    void OnPublish(IPublisherMessage message);

    // deve essere implementata per assicurarsi di non avere istanze morte ancora attive
    void OnDisableSubscriber();
}
