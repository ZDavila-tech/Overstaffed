using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase : MonoBehaviour
{
    

    private string _commantId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId { get { return _commantId; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string description, string format)
    {
        _commantId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string description, string format, Action command) : base (id, description, format)
    {
        this.command = command;
    }

    public void InvokeCommand()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void InvokeCommand(T1 value)
    {
        command.Invoke(value);
    }
}
