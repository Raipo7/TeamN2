namespace SpaceBattle.Lib;

public class WorkWithGameObjects
{   
    Dictionary<string, object> objects;
    public WorkWithGameObjects(Dictionary<string, object> objects)
    {
        this.objects = objects;
    }
    public object GameObjectGet(string gameItemId)
    {
        return objects[gameItemId];
    }
    public void GameObjectDelete(string gameItemId)
    {
        objects.Remove(gameItemId);
    }
}
