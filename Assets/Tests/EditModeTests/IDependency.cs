using System;

public interface IDependency
{
    void UpdateUI(Item item);
    Item GetNextItem();
    Item GetPreviousItem();
}