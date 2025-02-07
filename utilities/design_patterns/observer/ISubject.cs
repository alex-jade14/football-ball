public interface ISubject
{
    void Attach(string eventType, IObserver observer);
    void Detach(string eventType, IObserver observer);
    void Notify(string eventType, Godot.Collections.Dictionary data);
}