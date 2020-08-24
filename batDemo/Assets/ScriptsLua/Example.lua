---@class Example
Example = {}
---@return Example
---
local AssetReqs;

--lua加载完毕后执行
function Example.Start()
    --Example:Start()  Example.Start(this) 相同 self
    Example.CreatPlayer();
    Example.openPanel();
  -- Example.LoadMonster();
  --     Example.testArray();
end
function Example.CreatPlayer()
  Example.player= GameObject.FindGameObjectWithTag("Player");
 -- log("player "..Example.player.name);
 -- log("ObjType "..GameEnum.ObjType.Player);
 -- log("CtrlType "..GameEnum.CtrlType.JoyCtrl);
  Example.playerObj = GameLuaManager.CreatCharacter("player",Example.player);
 -- Example.playerObj.moveSpeed=4;
 --  Example.playerObj:GetMovePart().rotateSpeed=10;
  CameraManager.Instance.cameraCtrl:init(Example.player.transform);

end
function Example.Class()

    --初始化全局游戏对象
    --   local gameCls = require("Game.Game")
    --   gGame = gameCls:new()
    --   gGame:start()
---  Manager 单例 方法 都用.
---  class方法 都用:  class必须new 继承class
       ---@type GameObjBase
      local obj1 = GameObjBase:new();
      obj1.name="1233334";
      obj1:addListener("event1",function(value)
        log("event Test"..tostring(value));
      end);
        
      
      
      ---@type GameObjBase
      local obj2 = GameObjBase:new(); 
      obj2.name="的范德萨发";
      obj2:addListener("event1",function(value)
        log("event Test"..tostring(value));
    end);
    
      obj2:testEvent("event1","  dddd<<<<<<<<<<>");
     obj1:testEvent("event1","  aabbccc");

     obj2:destroy();
     obj1:destroy();
end

function Example.AssetCallBack(objs)
    if objs~=nil then 
        Example.monster = GameObject.Instantiate(objs[0]);
        Example.cam= GameObject.FindGameObjectWithTag("MainCamera")
        GameLuaManager.RefreshShader(Example.monster);
    end
    log("loader Com");
              ---Example.monster
    TweenManager.tween(Example.cam, {
          --           冲压时间,  冲压次数, 每次衰减
        --来回冲压 旋转         
        {TweenFun.delay, 5},
          ---        冲压力度v3,  冲压时间,  冲压力度, 随机偏移
         {TweenFun.shakeRotation,0,0,6,0.03,60},
         {TweenFun.delay, 0.15},
         {TweenFun.shakeRotation,0,0,6,0.03,60},
         {TweenFun.delay, 0.15},
         {TweenFun.shakeRotation,0,0,6,0.03,60},
         {TweenFun.delay, 0.15},
         {TweenFun.shakeRotation,0,0,6,0.03,60},
         {TweenFun.delay, 2},
         {TweenFun.shakeRotation,0,0,6,0.03,80},


        --                         角度 x,y,z, 冲压时间,  冲压力度, 每次衰减      
     ----   {TweenFun.punchRotation,0,0,6,0.03,1,0},
        {TweenFun.delay, 0.3},

        
        --来回冲压 位移   x,y,z,  冲压时间,  冲压次数, 每次衰减      
       --- {TweenFun.punchPosition,0,1,0,2,5,0.1},
        --来回冲压 缩放          {TweenFun.punchScale,1,1,1,2,5,0.1},
                                {TweenFun.delay, 5},
                                {TweenFun.call, function()
                                    log("loader>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Com");
                                    if AssetReqs~=nil then 
                                        AssetReqs:Unload();
                                        AssetReqs = nil;
                                    end
                                end}});

      -- 完成UI封装 简单    
      --proto 通讯 UIView 封装.                          
end

function Example.openPanel()
    Main.ViewManager:Show(ViewType.HuDBatPanel);
    -- Example.cam= GameObject.FindGameObjectWithTag("MainCamera")
    -- TweenManager.tween(Example.cam, {
    --     {TweenFun.delay, 5},
    --     {TweenFun.call, function()
    --         Main.ViewManager:Show(ViewType.HuDBatPanel,"~~66677");
    --     end},
    --     {TweenFun.delay, 5},
    --     {TweenFun.call, function()
    --         Main.ViewManager:Close(ViewType.HuDBatPanel);
    --     end}
    -- });
    -- Main.ViewManager:Close(ViewType.HuDBatPanel);
end

function Example.LoadMonster()
 
    AssetReqs =  GameLuaManager.LoadAsset("Monster/mst_xg01/mst_xg01_01",typeof(GameObject),Example.AssetCallBack);
end

function Example.testClass()

    local class = require("Class");
    
    local Fruit = class('Fruit') -- 'Fruit' is the class' name
    
    function Fruit:initialize(sweetness)
      self.sweetness = sweetness
    end
    
    Fruit.static.sweetness_threshold = 5 -- class variable (also admits methods)
    
    function Fruit:isSweet()
      return self.sweetness > Fruit.sweetness_threshold
    end
    
    local Lemon = class('Lemon', Fruit) -- subclassing
    
    function Lemon:initialize()
      Fruit.initialize(self, 1) -- invoking the superclass' initializer
    end
    
    local lemon = Lemon:new()
    log(lemon:isSweet()) -- false
end

function Example.testArray()
    require("Common.Extension.Array");

    local arr = Array(1,2,3)
    log(arr[1])   -- 1
    log(arr[4])   -- nil
    arr[1] = 4
    arr:print()     -- 4,3,2
    arr[4] = 'a'    -- warning : [4] index out of range.
    arr[2] = nil    -- warning : can not remove element by using  `nil`.
    arr:insert('a')
    arr:insert('b', 2)
    arr:print()     -- 4,b,2,3,a
    arr:remove(1)
    arr:print()     -- b,2,3,a
    arr:remove(3)
    log(arr[3]);
    log(arr:len().." len~~~~~~~~~");
end

return Example



