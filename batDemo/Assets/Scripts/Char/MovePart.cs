using System.Collections.Generic;
using UnityEngine;
/****
移动基类
****/
public class MovePart 
{
    //移动的趋向（影响旋转）
    private Vector3 _targetDirection=Vector3.zero;
    private bool _isMoving = false;

    //对象正前方; transform自带
    //public Vector3 forwardDirection= Vector3.zero;

    //是否面向移动.
    public bool faceToRotation=false;

    /**单次运动数据*运动完会重置******************************************************************************************
     * 
     */
    private float moveStartTime = 0;
    private Vector3 _moveSpeed=Vector3.zero;
    private Vector3 _moveRoate=Vector3.zero;
    //移动速度累计;
    private float _currentSpeed = 0;
    public Vector3 targetPos = Vector3.zero;
    private List<Vector3> targetPosList = new List<Vector3>(); //< 反序路径
    private float _speed;
     public float Speed
    {
        set {
            _speed = value;
        }
        get
        {
            return _speed;
        }
    }
    public bool hasTarget;
    private float _AcceleratedSpeed;
    public float AcceleratedSpeed
    {
        set {
            AcceleratedSpeed = value;
        }
        get
        {
            return AcceleratedSpeed;
        }
    }
    //最大速度;
    public float MaxSpeed = -1;
    public bool ZeroSpeedStop;
    public bool useMovePoint;
    //是否计算重量;
    public bool useWeightPower;
    //只对单次移动操作有效. 立刻转向目标;
    public bool ImmDir = false;
    //是否跟随目标;
    public ObjBase target;
    //跟随目标偏移;
    public  Vector3 targetOffset = Vector3.zero;
    private float targetLastAngle;
    private Vector3 targetVetOf = Vector3.zero;


    //额外摇摆加速度
    public float extraAcSpeed = -1;
    public float extraCurSpeed = 0;
    //额外摇摆速度 如果和向前速度相同会走圆曲线运动。
    public float extraSpeed = 0;
    //额外摇摆方向 +1 -1;
    public float extraDir = 1;
    public float extraMaxSpeed = 0;
    public float extraMoveStartTime = 0;
     
    /**单次运动数据************24***************************************************************************************
    */

    //暂停
    private bool _pause = false;

    public void pauseMove() {
        this._pause = true;
    }

    public void resumeMove() {
        this._pause = false;
    }

    private  ObjBase obj;
    public MovePart(ObjBase obj)
    {
        this.obj=obj;
    }
    public void init(){
        this.reset();
        this._pause=false;

        this._moveSpeed.Set(0,0,0);
        this._moveRoate.Set(0,0,0);

        this.targetOffset.Set(0,0,0);
        this.targetLastAngle = 9999;
        this.targetVetOf.Set(0,0,0);
        if (this.faceToRotation) {
   //        this.forwardDirection.Set(0,0,0);
        }else{
   //         this.forwardDirection.Set(0,0,0);
        }
    }
    private void FixUpdate(float dt) {
        if (this._pause) return;
        if (!this._isMoving) return;
        //跟随对象检测;
        if (this.target != null) {
            if (this.target.isRecycled) {
             //   MGLog.l("跟随完毕  ");
             //   this.stopMove(true);
                return;
            }
        }
    }
    public void Dispose(){
         obj=null;
    }
    public void reset(bool clearSpeed = true) {
        this._isMoving = false;
        this.moveStartTime = 0;
        this._currentSpeed = 0;
        this.target = null;

        this.targetPos.Set(0,0,0);
        this.targetPosList.Clear();

        this._targetDirection.Set(0,0,0);

        if (clearSpeed) {
            this.Speed = 0;
            this.AcceleratedSpeed = 0;
            this.MaxSpeed = -1;
            this.ZeroSpeedStop = false;
            this.useMovePoint = false;

            this.extraAcSpeed = -1;
            this.extraSpeed = 0;
            this.extraDir = 1;
            this.extraMaxSpeed = 0;
        }
        this.hasTarget = false;
        this.useWeightPower = false;

        this.targetOffset.Set(0,0,0);
        this.targetLastAngle = 9999;
        this.targetVetOf.Set(0,0,0);


        this.extraCurSpeed = 0;
        this.extraMoveStartTime = 0;

        this.ImmDir = false;
        //24
    }
}