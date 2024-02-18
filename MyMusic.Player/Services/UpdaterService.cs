namespace MyMusic.Player.Services
{
  public sealed class UpdaterService
  {
    public static Dictionary<Guid, System.Timers.Timer> FunctionTimers = new();

    public delegate Task UpdaterServiceCallBack();

    // TODO add client logging to local database

    public void AddUpdateCallBack(Guid id, double interval, UpdaterServiceCallBack callback)
    {
      System.Timers.Timer timer = new(interval);

      timer.Enabled = true;
      timer.AutoReset = true;
      timer.Elapsed += (s, e) =>
      {
        callback.Invoke();
      };

      var added = FunctionTimers.TryAdd(id, timer);

      if (!added)
      {
        throw new Exception("Failed to add update call back");
      }

      timer.Start();
    }

    public void RemoveUpdateCallBack(Guid id)
    {
      var timer = FunctionTimers[id];

      if (timer != null)
      {
        timer.Stop();
        timer.Dispose();

        FunctionTimers.Remove(id);
      }
    }
  }
}