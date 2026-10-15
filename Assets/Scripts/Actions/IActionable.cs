using UnityEngine;

public interface IActionable
{
    bool CanAct { get; set; }
    void ExecuteAction();
}
