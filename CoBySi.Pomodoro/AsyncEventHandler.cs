using System;

namespace CoBySi.Pomodoro;

public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e);
