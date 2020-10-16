using UnityEngine;

public class Judge : IJudge
{
    public virtual bool judge()
    {
        return true;
    }

    public virtual void dispose(){
    
    }
}
