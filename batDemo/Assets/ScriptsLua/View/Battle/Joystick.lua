---@class Joystick:EventObj
Joystick = Class("Joystick",EventObj)
---@return Joystick


-- 类初始化，类似构造函数
function Joystick:initialize()
   EventObj.initialize(self);
   self.name="Joystick";
end

---@param MoveArea RectTransform
---@param Handle RectTransform
---@param SprintJoyStick Image
---@param Sprint Image
---@param BackGround RectTransform
---@param  SprintOn Image
---@param panelView HuDBatPanel
function Joystick:init(MoveArea,Handle,SprintJoyStick,Sprint,BackGround,SprintOn,panelView)
    self.MoveArea=MoveArea
    self.handle=Handle
    self.SprintJoyStick=SprintJoyStick
    self.Sprint=Sprint
    self.BackGround=BackGround
    self.SprintOn =SprintOn
    self.huDBatPanel=panelView
    self.SprintRectTrans = self.Sprint.gameObject:GetComponent(typeof(RectTransform));
    self.SprintOnRadius= self.SprintRectTrans.rect.width/2 ;
    log(  self.SprintOnRadius..">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
    self.bgOriginLocalPos = self.BackGround.localPosition;
    ---Handle 移动最大半径
    self.maxRadius = self.BackGround.rect.width / 2;
    self.fingerId=nil;
    ---@type  Vector3
    self.pointerDownPos=nil;
     ---@type  Vector3
    self.pointerRealPos=nil;
    self.isDown=false;
    self.isPress=false;
    self.isSprinting=false;
end

function Joystick:destory()
    self.MoveArea=nil
    self.handle=nil
    self.SprintJoyStick=nil
    self.Sprint=nil
    self.SprintOn =nil
    self.SprintRectTrans=nil
    self.BackGround=nil
    self.huDBatPanel=nil

    self.pointerDownPos=nil;
    self.pointerRealPos=nil;
    self.name=nil;
    EventObj.destory(self);
end
function Joystick:Reset()
    self.handle.localPosition = Vector3.zero;
    self.BackGround.localPosition= self.bgOriginLocalPos;
    self.fingerId = nil;
    self.isDown = false;
    self.isPress = false;
    self.Sprint.enabled=false;
end


function Joystick:AddListener()
    log("Joystick AddListener");
    UIEventListener.Get(self.MoveArea.gameObject).onDown = function(eventData)  self:onDown(eventData) end
    UIEventListener.Get(self.MoveArea.gameObject).onDrag = function(eventData)  self:OnDrag(eventData) end
    UIEventListener.Get(self.MoveArea.gameObject).onUp = function(eventData)  self:OnUp(eventData) end
end

function Joystick:RemoveListener()
  --  log("Joystick RemoveListener");
    UIEventListener.Get(self.MoveArea.gameObject).onDown = nil
    UIEventListener.Get(self.MoveArea.gameObject).onDrag = nil
    UIEventListener.Get(self.MoveArea.gameObject).onUp = nil
end


---冲刺模式C
---@param isSprinting boolean 是否冲刺
function Joystick:SetSprint(isSprinting)
   if self.isSprinting==isSprinting then return end
    self.isSprinting=isSprinting;
    self.handle.gameObject:GetComponent(typeof(Image)).enabled = not isSprinting;
    self.BackGround.gameObject:GetComponent(typeof(Image)).enabled  = not isSprinting;
    self.SprintJoyStick.enabled= isSprinting;
    self.huDBatPanel.rightBtnArea:OnSprintState(isSprinting);

    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_SPRINT_STATE,{self.isSprinting});
end


---@param    eventData UnityEngine.EventSystems.PointerEventData
function Joystick:onDown(eventData)
  --  log("onDownx "..eventData.position.x)
 --   log("onDowny "..eventData.position.y)
    if eventData.pointerId<-1 or self.fingerId~=nil then return end
    self.isDown = true;
    self.fingerId = eventData.pointerId;
    self.pointerDownPos = eventData.position;
    self.pointerRealPos = eventData.position;
    self.BackGround.localPosition=Vector3(eventData.position.x,eventData.position.y,0);
end
function Joystick:OnDrag(eventData)
  ---   log("onDrag "..eventData.position)
    if self.fingerId ~= eventData.pointerId then  return end;
    self.isPress = true;
    self.pointerRealPos = eventData.position;
    ---@ type Vector2 得到BackGround 指向 Handle 的向量
    local direction = eventData.position - self.pointerDownPos;
    ---获取并锁定向量的长度 以控制 Handle 半径
    local radius = Mathf.Clamp(Vector2.Magnitude(direction), 0, self.maxRadius);
    --- Vector2.Normalize(direction);
    ---判断冲刺状态.
  ---  direction=direction.normalized;
  ----  direction:Normalize();
   local localPosition = direction.normalized * radius;
    self.handle.localPosition = localPosition;
    ---   Vector2(direction.x * radius, direction.y * radius);
    ---  log("onDrag "..localPosition.x .. " Y "..localPosition.y)
    local isRun =false;
    if radius==self.maxRadius then
        isRun=true;
        ---判断夹角
        local angle = Vector2.Angle(localPosition, Vector2.up);
        if angle<=70 and angle>= -70 then
           ---显示冲锋按钮.
           self.Sprint.enabled=true;
           local dic=  Vector2.Magnitude(Vector2(self.SprintRectTrans.localPosition.x, self.SprintRectTrans.localPosition.y)-direction)
           if   dic<=self.SprintOnRadius then
               -- 亮起按钮
               self.SprintOn.enabled=true;
               self.Sprint.enabled=false;
               self:SetSprint(true);
           else
               self.Sprint.enabled=true;
               self.SprintOn.enabled=false;
               self:SetSprint(false);
           end
        else
            self.Sprint.enabled=false;
            self.SprintOn.enabled=false;
            self:SetSprint(false);
        end
    else
      self.Sprint.enabled=false;
      self.SprintOn.enabled=false;
      self:SetSprint(false);
    end
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,{localPosition,isRun});
end

function Joystick:OnUp(eventData)
    ---正确的手指抬起时重置摇杆
  --  log("OnUp ")
    if self.fingerId ~= eventData.pointerId then return end
    if self.isSprinting==true then
        self.SprintOn.enabled=false;
        self:Reset();
    else
        self:Reset();
        EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE);
    end
    ---判断摇杆抬起时的位置.
end

return Joystick