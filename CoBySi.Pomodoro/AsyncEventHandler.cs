namespace CoBySi.Pomodoro;

public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs e);
