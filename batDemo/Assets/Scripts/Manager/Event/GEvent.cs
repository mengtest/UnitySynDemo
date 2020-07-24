
public class GEvent
{
    //G
    public int type = 1;
    public object data;
    public object param = null;
    public IDispatcher eventTarget;
    public GEvent(int _type, object _data = null)
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