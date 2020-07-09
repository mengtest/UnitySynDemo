---@class PacketBuilder
PacketBuilder = Class("PacketBuilder")
---@return PacketBuilder
---
local pb = require "pb"
-- 包头数据 初始化
function PacketBuilder:initialize(msgCmd, reqMsgs, reqIndex)
    -- packet包
    self._msg = {}
    self._msg.Head = {}
    self._msg.Head.Serial = reqIndex
    self._msg.Head.TransType= 0
    --self._msg.Head.Options  //自定义options可以加很多东西加入,不用写死
    for i,v in ipairs(reqMsgs) do
        -- local bodyData = v:SerializeToString()
        local reqName = netMsgHelper:getCmdName(msgCmd)
        self._msg.Head.Cmd = msgCmd
        self._msg.Body = assert(pb.encode(reqName, v))
    end
    -- self._msg.Head = assert(pb.encode("md.PkgHead", self._msg.Head))
end

-- 获取包的请求名
function PacketBuilder:getReqNameWithMsg( msg )
    local meta = getmetatable(msg)
    if meta and meta._descriptor and meta._descriptor.name then
        return string.lower(meta._descriptor.name)
    end
    return nil
end

-- 获取请求数据
function PacketBuilder:getRequestMsg( )
    -- local reqData = self._msg:SerializeToString()
    -- return LuaUtil.encodeBase64(self._msg)
    
    local bytes = assert(pb.encode("cmdpkg.CmdPacket", self._msg))
    return bytes
end

-- 获取cmd号
function PacketBuilder:getCmdId( )
    if self._msg and self._msg.Head then
        return self._msg.Head.Cmd
    end
    return nil
end

function PacketBuilder:getMsg( )
    return self._msg
end

-- 获取请求序号
function PacketBuilder:getSeq( )
    return self._msg.Head.Serial
end

return PacketBuilder