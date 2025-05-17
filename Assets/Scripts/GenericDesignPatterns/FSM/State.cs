public abstract class State
{
    // -- questi metodi sono fondamentali
    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    // --

    // -- questi metodi sono opzionali in base alla struttura del codice
    public abstract void OnTriggerEnter();
    public abstract void OnTriggerExit();
    public abstract void OnCollisionEnter();
    public abstract void OnCollisionExit();
    // --
}
