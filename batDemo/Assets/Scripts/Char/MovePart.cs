using System.Collections.Generic;
using UnityEngine;
/****
移动基类
****/
[AutoRegistLua]
public class MovePart 
{
    //移动的趋向（影响旋转）
    private Vector3 _targetDirection=Vector3.zero;
    private bool _isMoving = false;
    //跳跃
    private bool m_jumping = false;

    //对象移动的正前方; 
    public Vector3 forwardDirection= Vector3.zero;

    //是否面向移动.
    public bool faceToRotation=true;
      //扭矩 旋转速度 速度为0时立刻旋转 不计算转角速度; 7
     public float rotateSpeed=15;

    //移动点数  改变这个只可以加速(buff 增加速度等)或减速(中毒,被重击慢速移动等) 只对使用移动点数有效...;
    public float movePoint = 10000;



    /**单次运动数据*运动完会重置******************************************************************************************
     * 
     */
    private float moveStartTime = 0;
    //移动距离.
    private Vector3 _moveSpeed=Vector3.zero;
    private Vector3 _moveRoate=Vector3.zero;
    //移动速度累计;
    private float _currentSpeed = 0;
    public Vector3 targetPos = Vector3.zero;
    private List<Vector3> targetPosList = new List<Vector3>(); //< 反序路径
    //当前速度
    public float speed=0;

    public bool hasTarget;
    public float AcceleratedSpeed;

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


    //额外摇摆加速度
    public float extraAcSpeed = -1;
    public float extraCurSpeed = 0;
    //额外摇摆速度 如果和向前速度相同会走圆曲线运动。
    public float extraSpeed = 0;
    //额外摇摆方向 +1 -1;
    public float extraDir = 1;
    public float extraMaxSpeed = 0;
    public float extraMoveStartTime = 0;
     
    /**单次运动数据*end**************************************************************************************************
    */

    //暂停
    private bool _pause = false;

    public void PauseMove() {
        this._pause = true;
    }

    public void PesumeMove() {
        this._pause = false;
    }
    
    private  ObjBase obj;
    public MovePart(ObjBase obj)
    {
        this.obj=obj;
    }
    public void Init(){
        this.reset();
        this._pause=false;

        this._moveSpeed.Set(0,0,0);
        this._moveRoate.Set(0,0,0);

        this.targetOffset.Set(0,0,0);
        if (this.faceToRotation) {
           this.forwardDirection=obj.gameObject.transform.forward;
        }else{
            this.forwardDirection.Set(0,0,0);
        }
    }

    public void fixUpdate() {
        if (this._pause) return;
        if (!this._isMoving) return;
        //float dt=Time.fixedDeltaTime;
        //跟随对象检测;
        if (this.target != null) {
            if (this.target.isRecycled) {
             //   MGLog.l("跟随完毕  ");
                this.StopMove(true);
                return;
            }
        }
        this.changeDir(this.rotateSpeed,Time.fixedDeltaTime);
        //移动速度;
        this.calMoveSpeed(Time.fixedDeltaTime);
        //添加移动
        this.obj.OnMove(_moveSpeed);
      //  this.obj.position.addSelf(this._moveSpeed);
        //额外移动
        this.extraMove(Time.fixedDeltaTime);
        this.chkMove();
    }
    //额外移动...Todo 改成多个运动力 motion 控制移动.
    private void extraMove(float number) {
    
    }
    /**********************************************************
    * 行为
    * 移动; 
    */
    public void InitSpeed() {
        if ( this.speed == 0 && this.AcceleratedSpeed == 0) {
            this.speed = this.obj.moveSpeed;
        }
    }
    /**
     * 向某个方向一直移动;
     */
    public void StartMove(Vector3 dir) {
        this.hasTarget = false;
        this._targetDirection=dir;
        this._targetDirection.Normalize();
        this.targetPos.Set(0,0,0);
        this.target = null;
        this.moveStartTime = 0;
        this.extraMoveStartTime = 0;
        this.extraMaxSpeed = this.extraSpeed;
        this.InitSpeed();
        this._isMoving = true;
    }
    /**
     * 移动到某个点上;  数组 从开始点到目标点.
     */
    public void StartMoveToByList(List<Vector3> targetPosList) {
        if (targetPosList==null || targetPosList.Count < 1) {
            return;
        }
        //
        this.startMoveToByListWithReverseList(targetPosList);
    }
    /**
     * 移动到某个点上;
     */
    public void StartMoveTo(Vector3 targetPos) {
        this.hasTarget = true;
        this._targetDirection=targetPos-this.obj.gameObject.transform.position;
        this._targetDirection.Normalize();
        this.targetPos= targetPos;
        this.target = null;
        this.moveStartTime = 0;
        this.extraMoveStartTime = 0;
        this.extraMaxSpeed = this.extraSpeed;
        this.InitSpeed();
        this._isMoving = true;
       // if (this.debug) {
        //    this.clearAllTargetBox();
       //     this.targetPosBox.push(ColorBoxManager.Get().showColorPos(this.targetPos, cc.Color.RED, 50));
      //  }
    }
    public void StopMove(bool needEvent = false,bool resSpeed = true) {
        //  if((this.pos as CharData).charId_Xls==3001){
        //      console.log("stopMove",(this.pos as CharData).pvpId);
        //  }
        this.reset(resSpeed);
        if (needEvent) {
            this.obj.GetEvent().dispatchEvent(CharEvent.MOVE_END);
            //         console.log("Move_End");
        }
    }
    private void stopMoveChk(bool needEvent = false) {
        if (this.targetPosList!=null && this.targetPosList.Count > 0) {
            // console.log("nextPos",this.targetPosList.length,this.targetPosList);
            this.startMoveToByListWithReverseList(this.targetPosList);
        } else {
            this.StopMove(needEvent);
        }
    }
   /**
     * 是否正在移动;
     */
    public bool IsMove() {
        return this._isMoving;
    }
    //是否跟随目标;
    public bool IsFollowTarget() {
        return this.target != null ? true : false;
    }
    //获取下一步移动距离.
    public Vector3  GetNextMoveSpeedDic(){
        return this._moveSpeed;
    }
    //设置移动趋向 改变旋转 
    public void SetTargetRotation(float rotationY) {
        //角度 转方法.
      //  MyMath.RotateToVec2(rotation, this._targetDirection);
        this._targetDirection.Normalize();
    }
    //设置移动趋向
    public void  SetTargetDir(Vector3 targetDir) {
        if (this.hasTarget) {
            this.StartMove(targetDir);
            return;
        }
        this._targetDirection=targetDir;
        this._targetDirection.Normalize();
    }
    public void followMyTarget() {
        if (this.obj != null && this.obj.Target != null && !this.obj.Target.isRecycled  ) {
            this.startFollowTarget(this.obj.Target);
        }
    }
    public bool chkFollowTarget() {
        if (this.target.isDead || this.obj == null || this.obj.Target == null || this.obj.Target.isDead||this.obj.Target.isRecycled) {
            this.cancelFollowTarget();
            return false;
        }
        if (this.obj.Target != this.target) {
            this.SetFollowTarget(this.obj.Target);
        }
        return true;
    }
    public void  startFollowTarget(ObjBase target) {
        if (target != null && !target.isDead) {
            this.StopMove(false, false);
            this.SetFollowTarget(target);
            this._isMoving = true;
        }
    }

    public void SetFollowTarget(ObjBase target) {
        this.target = target;
        this.hasTarget = true;
        this.InitSpeed();
    }
    public void cancelFollowTarget() {
        this.StopMove();
    }
    public void SetRotation(float dir,bool Imm=false){
        Quaternion rot = Quaternion.Euler(0, dir, 0);
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, rot, Vector3.one);
        Vector3 dirV = mat.GetColumn(2);
        dirV.Normalize();
        this._targetDirection = dirV;
        if(Imm){
            this.forwardDirection=this._targetDirection;
            if (this.faceToRotation) {
                    this.obj.gameObject.transform.forward=this.forwardDirection;
            }
        }
    }
     public Vector3 getRotation(float dir){
        Quaternion rot = Quaternion.Euler(0, dir, 0);
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, rot, Vector3.one);
        Vector3 dirV = mat.GetColumn(2);
        dirV.Normalize();
        return dirV;
    }
    /***
     * 立刻改变方向;
     */
    public void changeDir(float rotateSpeed,float dt) {
        if (this.target != null) {
            if (this.targetOffset==Vector3.zero) {
                this._targetDirection= this.target.gameObject.transform.position-this.obj.gameObject.transform.position;
               // this.target.position.sub(this.pos.position, this._targetDirection);
                this._targetDirection.Normalize();
                 //3D.
            } else {
                // if (this.targetLastAngle!=this.target.angle)) {
                //     let _anlgel: number = BattleUtils.angleToRadius(this.targetData.angle); ///< this.targetData.angle * Math.PI / 180;
                //     let axisXx: number = Math.cos(_anlgel);
                //     let axisXy: number = Math.sin(_anlgel);
                //     this.targetVetOf.x = this.targetOffset.x * axisXx - this.targetOffset.y * axisXy;
                //     this.targetVetOf.y = this.targetOffset.x * axisXy + this.targetOffset.y * axisXx;
                //     this.targetLastAngle = this.target.angle
                // }
                // this.targetData.position.add(this.targetVetOf, this._targetDirection).sub(this.pos.position, this._targetDirection);
                //       console.log(this.targetData.position,this.targetVetOf);
                this._targetDirection =  this.target.gameObject.transform.TransformPoint(this.targetOffset)-this.obj.gameObject.transform.position;
                this._targetDirection.Normalize();
                //3D.
            }

        } else if (this.targetPos!=Vector3.zero) {
            this._targetDirection=  this.targetPos-this.obj.gameObject.transform.position;
            this._targetDirection.Normalize();
             //3D.
        }
        if (this.ImmDir || rotateSpeed== 0) {
            if (this.obj.gameObject.transform.forward!= this._targetDirection) {
                this.forwardDirection=(this._targetDirection);
                //旋转;
                if (this.faceToRotation) {
                    // if (this.mbCharData && (this.pos as CharData).actionCmdCgDir) {

                    // } else {
                      //  this.pos.angle = MyMath.vec2ToRotate(this.forwardDirection);
                        this.obj.gameObject.transform.forward=this.forwardDirection;
                    // }
                }
                //            this._targetDirection=null;
            }
        } else if (this._targetDirection!=null && this.obj.gameObject.transform.forward!= this._targetDirection ) {
             this._moveRoate=this._targetDirection-this.forwardDirection;
          //  this._targetDirection.sub(this.pos.forwardDirection, this._moveRoate);
            if (this._moveRoate.magnitude< 0.01f) {
                //最新方向;
                this.forwardDirection=this._targetDirection;
                //        this._targetDirection=null;
                //        console.log("rotation End");
            } else {
                this._moveRoate=this._targetDirection*(rotateSpeed * dt);
             //   this._targetDirection.mul(rotateSpeed * dt, this._moveRoate);
                //最新方向;
                 this.forwardDirection= this.forwardDirection+this._moveRoate;
                  this.forwardDirection.Normalize();
                //this.forwardDirection.addSelf(this._moveRoate).normalizeSelf();//.normalize();
            }
            //旋转;
            if (this.faceToRotation) {
                // if (this.mbCharData && (this.pos as CharData).actionCmdCgDir) {

                // } else {
                //    this.pos.angle = MyMath.vec2ToRotate(this.forwardDirection);
                   this.obj.gameObject.transform.forward=this.forwardDirection;
                    //        this.pos.angle= MyMath.normalAngle(this.pos.angle);
    //            }
            }
        }
    }
    private void calMoveSpeed(float dt) {
        this.moveStartTime += dt;
     
        if (this.ZeroSpeedStop) {
            //0衰减没有最大速度限制;
            float zeroTime = (-this.speed / this.AcceleratedSpeed);
            //   console.log("this.moveStartTime",this.moveStartTime);
            //   console.log("this.zeroTime",zeroTime);
            if (this.moveStartTime> zeroTime) {
                this.stopMoveChk(true);
                return;
            }
        }

        this._currentSpeed = this.speed + this.AcceleratedSpeed * this.moveStartTime;
        if (this.MaxSpeed >0) {
            this._currentSpeed = Mathf.Min(this._currentSpeed, this.MaxSpeed);
        }
        if (this.useMovePoint) {
            this._currentSpeed *= (this.movePoint / 10000);
        }
        if (this.useWeightPower) {
            this._currentSpeed = this._currentSpeed / this.obj.weight;
        }
      this._moveSpeed =  this.forwardDirection* this._currentSpeed * dt;
    //    MyMath.floor2Vet(this._moveSpeed);
     //  DebugLog.Log("this._moveSpeed",this._moveSpeed);
    }
     private Vector3 chkMpos;
    private void chkMove() {
        if (this.hasTarget) {
            if (this.targetPos!=Vector3.zero) {
                //    console.log("chkMove",this.targetPos,this.pos.position);
                Transform trans= this.obj.gameObject.transform;
                if (this.rotateSpeed==0) {
                    if (this._targetDirection!=null) {
                        chkMpos=trans.position;
                        if (this._targetDirection.x>0 && trans.position.x> this.targetPos.x) {
                            chkMpos.x = this.targetPos.x;
                        }
                        else if (this._targetDirection.x < 0 && trans.position.x<this.targetPos.x) {
                            chkMpos.x = this.targetPos.x;
                        }
                        if (this._targetDirection.y> 0 && trans.position.y>this.targetPos.y) {
                            chkMpos.y = this.targetPos.y;
                        }
                        else if (this._targetDirection.y<0 && trans.position.y<this.targetPos.y) {
                            chkMpos.y = this.targetPos.y;
                        }
                         trans.position=chkMpos;
                        if (this.targetPos == trans.position) {
                            //           console.log("moveCom",this.targetPos);
                            this.stopMoveChk(true);
                        }
                    }
                } else {
                    //有转角速度就判断半径是否碰到了 碰到了就到目标点了 不能精确移动到位置上因为；
                    float dic = this.obj.getDic(this.targetPos, 0, true);
                    if (dic<=0) {
                        this.stopMoveChk(true);
                    }
                }
            }
        }

    }
    public void startMoveToByListWithReverseList(List<Vector3> targetPosList) {
            if (targetPosList==null || targetPosList.Count < 1) {
                return;
            }
            this.hasTarget = true;
            Vector3 nextPos  = targetPosList[0];
             this._targetDirection = nextPos- this.obj.gameObject.transform.position;
            this._targetDirection.Normalize();
            this.targetPos.x = nextPos.x;
            this.targetPos.y = nextPos.y;
             this.targetPos.z=nextPos.z;
             this.targetPosList.RemoveAt(0);
        
            this.target = null;
            this.moveStartTime = 0;
            this.extraMoveStartTime = 0;
            this.extraMaxSpeed = this.extraSpeed;
            this.InitSpeed();
            this._isMoving = true;
            // if (this.debug) {
            //     this.clearAllTargetBox();
            //     for (let index = this.targetPosList.length - 1; index >= 0; --index) {
            //         const pos: cc.Vec2 = this.targetPosList[index];
            //         this.targetPosBox.push(ColorBoxManager.Get().showColorPos(pos, cc.Color.RED, 50));
            //     }
            // }
    }

    public void Dispose(){
         obj=null;
         target = null;
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
            this.speed = 0;
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


        this.extraCurSpeed = 0;
        this.extraMoveStartTime = 0;

        this.ImmDir = false;
        //24
    }
}