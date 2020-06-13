function Class(classname, super)
    local cls
    assert(type(classname)=="string","class的第二个参数错误:"..tostring(classname))
    if super~=nil then
        if isFunction(super) or isTable(super) then
        else
            print("class的第二个参数错误:"..tostring(classname))
            super=nil
        end
    end
    if super then
        cls={}
        setmetatable(cls,{__index=super,__call=function(self,...) return cls:new(...) end})
        cls.super=super
    else
        cls=setmetatable({},{__call=function(self,...)return cls:new(...) end})
    end
    cls.__cname=classname
    cls.__index=cls

    function cls:new(...)
        local inst={}
        setmetatable(inst,cls)
        if inst.ctor then
            inst:ctor(...)
        end
        return inst
    end
    _G[classname]=cls
    return cls
end