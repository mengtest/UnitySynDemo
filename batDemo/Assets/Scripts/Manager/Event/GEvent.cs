
public class GEvent
{
    public string type = "";
    public object data;
    public object param = null;
    public IDispatcher eventTarget;
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
public interface IDispatcher 
{

}