using UnityEngine;
/****
显示基类 
****/
public class ObjBase : PoolObj
{
    protected string _name="ObjBase";
     //类型.

    public IData  objData{get; protected set;}
     protected GameObject dataNode=null;
    
    public GameEnum.ObjType charType=GameEnum.ObjType.Obj;
    protected MovePart _move=null;

    //0  layer层 动画控件 整体全身动作层
    protected AniPart aniBasePart=null;

     //事件派发器;
    private GEventDispatcher _event=null;
    //目标对象.
    public ObjBase Target=null;
    //半径 如果是点 半径可以是0
    public float radius=0f;
    //移动速度1秒. 
     public float moveSpeed=6;
    //  //最大速度;
    // public  float maxMoveSpeed = -1;
      //重量....体重...
    public float weight = 1;
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

    public ObjBase()
    {
        this.node =new GameObject(this._name);
        this.dataNode=new GameObject("Data");
        this.dataNode.transform.parent=this.node.gameObject.transform;
        this.dataNode.transform.localPosition=Vector3.zero;
        this.isDestory=false;
        this.isDead=false;
        this.initViewFin=false;
    }
    public override void init(){
        string[] split = poolname.Split('/');
        if(split.Length>0){
            this._name=split[split.Length-1]+this.id;
        }else{
            this._name=poolname+this.id;
        }
        this.node.name=this._name;

        this.initView(poolname);
    }
    public virtual void initData(){
        //每次初始化 应该重置data.防止 数据 没清空.
         ObjData oldData=this.dataNode.GetComponent<ObjData>();
         if(oldData!=null){
             GameObject.DestroyImmediate(oldData);
         }
        this.objData = this.dataNode.AddComponent<ObjData>();
        this.objData.init(this,fixUpdate);
    }
    //显示类可重写.
    public virtual void initView(string prefabPath=""){
        if(this.poolname == prefabPath){
            return;
        }
        this.poolname=prefabPath;
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
    private void onViewloaded(UnityEngine.Object[] objs){
       if(this.isDestory){
           return; 
       }
       if (objs.Length>0){
           //替换node.
            GameObject cc= this.node;
            this.node = GameObject.Instantiate(objs[0]) as GameObject;
            this.node.transform.SetPositionAndRotation(cc.transform.position,cc.transform.rotation);
            this.node.name=this._name;

            GameObject.Destroy(cc);

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
    public virtual void onViewLoadFin(){
        
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
    public GEventDispatcher GetEvent(){
        if(this._event==null){
            this._event=new GEventDispatcher();
        }
        return this._event;
    }
    public MovePart GetMovePart(){
        if(this._move==null){
            this._move=new MovePart(this);
        //    this.needUpdate=true;
        }
        return this._move;
    }
     
    public virtual bool hasAni(int layer=0){
       return this.aniBasePart ==null ? false:true;
    }
   
    public virtual void pauseAni(int layer=0){
          this.GetAniBasePart().pause();
    }
    public virtual void resumeAni(int layer=0){
          this.GetAniBasePart().resume();
    }

    public AniPart GetAniBasePart(){
        if(this.aniBasePart==null){
            this.aniBasePart=new AniPart();
            this.aniBasePart.Init(this,GameEnum.ActionLayer.BaseLayer);
        }
        return this.aniBasePart;
    }
    protected virtual void fixUpdate() {
      //  if(!this.needUpdate)return;
        if(this._move!=null){
            this._move.fixUpdate();
        }
        if(this.aniBasePart!=null){
             this.aniBasePart.fixUpdate();
        }
    }

    public override void onGet()
    {
        this.isDead=false;
        if(this.node!=null){
             this.node.SetActive(true);
        }
        this.initData();
    }
    /**
     回收..
    **/
    public override void onRecycle()
    {
        this.Target=null;
        this.isDead=true;
        if(this.aniBasePart!=null){
             this.aniBasePart.stop();
        }
        if(this._move!=null){
            this._move.Init();
        }
        if(this._event!=null) {
            this._event.ClearAllEvent();
        } 
        if(this.node!=null){
             this.node.SetActive(false);
        }
    }
    /*********
      销毁
    *******/
    public override void onRelease(){
       this.Target=null;
       this.isDead=true;
       this.objData=null;
       this.dataNode=null;
       if(this.aniBasePart!=null){
           this.aniBasePart.Release();
           this.aniBasePart=null;
       }
       if(this._move !=null){
           this._move.Dispose();
           this._move=null;
       }
       if(this.node!=null){
           GameObject.Destroy(this.node);
           this.node=null;
       }
       if(this._viewReqs!=null){
            this._viewReqs.Unload();
            this._viewReqs = null;
       }
       if(this._event!=null) {
            this._event.Dispose();
            this._event=null;
       } 
       this.isDestory=true;
    }
}