using UnityEngine;
/****
显示基类 
****/
public class EffectBase : PoolObj
{
    protected string _name="EffectBase";
                    
    //半径 如果是点 半径可以是0
    public float radius=0f;
    //根节点.
    protected GameObject node=null;
    /***
    获取gameobj 每帧调用时不能缓存 会更改 会变化 所以需要直接取.
    ****/
    public GameObject gameObject{
        get{
            return this.node;
        }
    }
    //通用_显示对象加载reqs.
    private  GameAssetRequest _viewReqs=null;
    //销毁
    public bool isDestory=false;
    public bool isDead=false;
    public  bool initViewFin=false;
 
    //public bool needUpdate = false;

    public EffectBase()
    {
        this.node =new GameObject(this._name);
        this.isDestory=false;
        this.isDead=false;
        this.initViewFin=false;
    }
    public override void init(){
        string[] split = poolname.Split('/');
        if(split.Length>0){
            this._name=split[split.Length-1];
            //+this.id;
        }else{
            this._name=poolname;
            //+this.id;
        }
        this.node.name=this._name;

    //    this.initView(poolname);
    }
    public void setParent(Transform parent){
        this.node.transform.parent = parent;
        this.node.transform.localPosition = Vector3.zero;
        this.node.transform.localEulerAngles = Vector3.zero;
    }
    public void setDir(Vector3 forward){
        this.node.transform.forward=forward;
    }
    //显示类可重写. 初始化显示对象.
    public virtual void initView(string prefabPath=""){
        if(this.poolname == prefabPath){
            return;
        }
        if(prefabPath==""){
            prefabPath=this.poolname;
        }else{
            this.poolname=prefabPath;
        }
        if(_viewReqs!=null){
            _viewReqs.Unload();
            _viewReqs=null;
        }
        if(prefabPath!=""){
            _viewReqs = GameAssetManager.Instance.LoadAsset<GameObject>(this.poolname,onViewloaded);
        }else{
            //Node
            _viewReqs=null;
        }
    }
    //初始化 替换显示对象.
    public virtual void initView(GameObject obj){
        if(_viewReqs!=null){
            _viewReqs.Unload();
            _viewReqs=null;
        }
        this.ChangeNodeObj(obj,false);
        if(this.isRecycled){
                 this.node.SetActive(false);
        }
        this.initViewFin=true;
        onViewLoadFin();
    }

    private void onViewloaded(UnityEngine.Object[] objs){
       if(this.isDestory){
           return; 
       }
       if (objs.Length>0){
           //替换node.
            GameObject newObj= GameObject.Instantiate(objs[0]) as GameObject;
            this.ChangeNodeObj(newObj);

            if (GameSettings.Instance.useAssetBundle)
            {
                RenderHelper.RefreshShader(ref this.node);
            }
            if(this.isRecycled){
                 this.node.SetActive(false);
            }
            this.initViewFin=true;
            onViewLoadFin();
        }
    }
    public virtual void ChangeNodeObj(GameObject obj,bool resetPos=true){
        GameObject cc= this.node;
        this.node =obj;
       this.node.transform.transform.parent = cc.transform.parent;
        if(resetPos){
            this.node.transform.SetPositionAndRotation(cc.transform.position,cc.transform.rotation);
        }
        this.node.name=this._name;
        GameObject.Destroy(cc);
    }
    public virtual void onViewLoadFin(){
        
    }
    public virtual bool IsGrounded()
	{
		return Physics.Raycast(this.gameObject.transform.position+Vector3.up*0.1f, Vector3.down, 0.2f,LayerHelper.GetGroundLayerMask());
	}
    public virtual bool IsNeedFall(){
        return !Physics.Raycast(this.gameObject.transform.position+Vector3.up*0.1f, Vector3.down, 0.6f,LayerHelper.GetGroundLayerMask());
    }
    /**
    * 计算目标距离;
    * @param Vec2 目標位置
    * @param Radius 目標半徑
    * @param subMyRadius 是否计算自身的半径
    */
    public float getDic(Vector3 targetPos,float tagetRadius,bool subMyRadius = true) {
        float dic = 0;
       Vector3  tempV= targetPos-this.gameObject.transform.position;
        if (subMyRadius) {
            dic = tempV.magnitude - tagetRadius - this.radius;
        } else {
            dic = tempV.magnitude - tagetRadius;
        }
        return dic;
    }
    public void autoDestroy(float time=0){
       if(time>0){
           TimerManager.Instance.Once(time,recycleSelf,false);
       }        
    }
    // private  void Dead(){
    //  //   DebugLog.Log("Dead",delayRecycleTime);
    //     this.recycleSelf();
    // }


    public override void onGet()
    {
        this.isDead=false;
        if(this.node!=null){
             this.node.SetActive(true);
        }
    }
    /**
     回收..
    **/
    public override void onRecycle()
    {
    //     DebugLog.Log("onRecycle");
        this.isDead=true;
        if(this.node!=null){
             this.node.SetActive(false);
        }
    }
    /*********
      销毁
    *******/
    public override void onRelease(){
        this.isDead=true;
        if(this.node!=null){
            GameObject.Destroy(this.node);
            this.node=null;
        }
        if(this._viewReqs!=null){
                this._viewReqs.Unload();
                this._viewReqs = null;
        }
        this.isDestory=true;
    }
}