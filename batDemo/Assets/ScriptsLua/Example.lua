---@class Example
local Example = {}
---@return Example
---
local AssetReqs;

--lua加载完毕后执行
function Example.Start()
    --Example:Start()  Example.Start(this) 相同 self
 
   ---- Example.LoadMonster();

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
        GameLuaManager.RefreshShader(Example.monster);
    end
    log("loader Com");

    TweenManager.tween(Example.monster, {
          --           冲压时间,  冲压次数, 每次衰减
        --来回冲压 旋转         {TweenFun.punchRotation,0,50,0,2,3,0.7},
        --来回冲压 位移          {TweenFun.punchPosition,0,1,0,2,5,0.1},
        --来回冲压 缩放          {TweenFun.punchScale,1,1,1,2,5,0.1},
                                {TweenFun.delay, 5},
                                {TweenFun.call, function()
                                    log("loader>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Com");
                                    if AssetReqs~=nil then 
                                        AssetReqs:Unload();
                                        AssetReqs = nil;
                                    end
                                end}});


      --proto 通讯 UIView 封装.                          
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


return Example



