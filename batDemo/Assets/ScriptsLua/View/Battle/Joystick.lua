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
---@param SprintJoyStick RectTransform
---@param Sprint RectTransform
---@param BackGround RectTransform
function Joystick:init(MoveArea,Handle,SprintJoyStick,Sprint,BackGround)
    self.MoveArea=MoveArea
    self.handle=Handle
    self.SprintJoyStick=SprintJoyStick
    self.Sprint=Sprint
    self.BackGround=BackGround
    self.bgOriginLocalPos = self.BackGround.localPosition;
    ---Handle 移动最大半径
    self.maxRadius = self.BackGround.sizeDelta.x / 2;
    self.fingerId=nil;
    ---@type  Vector3
    self.pointerDownPos=nil;
     ---@type  Vector3
    self.pointerRealPos=nil;
    self.isDown=false;
    self.isPress=false;
end

function Joystick:destory()
    self.MoveArea=nil
    self.handle=nil
    self.SprintJoyStick=nil
    self.Sprint=nil
    self.BackGround=nil

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
end
-- 测试event事件.
function Joystick:testEvent(EventKey,value)
    self:dispatchEvent(EventKey,value);
    log("GameObjBase test"..self.name);
end


function Joystick:AddListener()
    log("Joystick AddListener");
    UIEventListener.Get(self.MoveArea.gameObject).onDown = function(eventData)  self:onDown(eventData) end
    UIEventListener.Get(self.MoveArea.gameObject).onDrag = function(eventData)  self:OnDrag(eventData) end
    UIEventListener.Get(self.MoveArea.gameObject).onUp = function(eventData)  self:OnUp(eventData) end
end

function Joystick:RemoveListener()
    log("Joystick RemoveListener");
    UIEventListener.Get(self.MoveArea.gameObject).onDown = nil
    UIEventListener.Get(self.MoveArea.gameObject).onDrag = nil
    UIEventListener.Get(self.MoveArea.gameObject).onUp = nil
end

---@param    eventData UnityEngine.EventSystems.PointerEventData
function Joystick:onDown(eventData)
    log("onDownx "..eventData.position.x)
    log("onDowny "..eventData.position.y)
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
    -----得到BackGround 指向 Handle 的向量
    local direction = eventData.position - self.pointerDownPos;
    ---获取并锁定向量的长度 以控制 Handle 半径
    local radius = Mathf.Clamp(Vector2.Magnitude(direction), 0, self.maxRadius); 
    local localPosition = Vector2((direction.normalized * radius).x, (direction.normalized * radius).y);
    self.handle.localPosition = localPosition;

end

function Joystick:OnUp(eventData)
    ---正确的手指抬起时重置摇杆
    log("OnUp ")
    if self.fingerId ~= eventData.pointerId then return end
    self:Reset();
end

return Joystick