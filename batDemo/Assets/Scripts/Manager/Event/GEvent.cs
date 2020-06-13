
public class GEvent
{
    public string type = "";
    public object data;
    public object param = null;
  //  public GameObject target;
  //  public GEventDispatcherMono targetMono;
    public GEventDispatcher eventTarget;
    public GEvent(string _type, object _data = null)
    {
        type = _type;
        data = _data;
    }
    public void release() {
        data = null;
        param = null;
        eventTarget = null;
    }
}