---@class NetMsgHelper 视图管理类
NetMsgHelper = Class("NetMsgHelper")
---@return NetMsgHelper 视图管理类
---
function NetMsgHelper:initialize()
    ---命令号表
    self._cmdIdMap = {}
    self._cmdNameMap = {}
    self:_initCmdMap()
    ---解析表
    self._msgDecodeMap = {}
    -- self:_initDecodeMap()
end

function NetMsgHelper:genResponseCMD(cmd)
    return  cmd * -1
end

function NetMsgHelper:_initCmdMap()
    for k,v in pairs(MsgIDMap) do
        -- if v and type(v) == "table" then
        --     if v.name and type(v.name) == "string" and v.number and type(v.number) == "number" then
        --         self._cmdIdMap[v.name] = v.number
        --         self._cmdNameMap[tostring(v.number)] = v.name
        --         local rspName = v.name .."Rsp"
        --         local rspNumber = self:genResponseCMD(v.number)
        --         self._cmdIdMap[rspName] =rspNumber
        --         self._cmdNameMap[tostring(rspNumber)] = rspName
        --     end
        -- end
        self._cmdIdMap[v] = k
        self._cmdNameMap[k] =  v
    end
end

---请求数据获取对应cmd号
function NetMsgHelper:getCmdId( name )
    if self._cmdIdMap[tostring(name)] then
        return self._cmdIdMap[tostring(name)]
    else
        logError("Network ["..tostring(name).."] get Cmd error : Cmd is nil")
    end
    return nil
end

---请求数据获取对应cmd号
function NetMsgHelper:getCmdName( id )
    if self._cmdNameMap[id] then
        return self._cmdNameMap[id]
    end
    return nil
end

-- 初始化反序列化映射
function NetMsgHelper:_initDecodeMap( )
    local pbList = {cmd_pb, db_pb, enum_pb, game_pb,matchbattle_pb,  matchbattle_db_pb, statuscode_pb, subnotice_pb, cmdpkg_pb, testcase_pb}
    for i=1,#pbList do
        local data_pb = pbList[i]
        if data_pb ~= nil then
            for k,v in pairs(data_pb) do
                if v and type(v) == "table" then
                    local meta = getmetatable(v)
                    if meta and meta.ParseFromString then
                        self._msgDecodeMap[string.lower(k)] = v
                    end
                end
            end
        end
    end
end

---数据获取对应反序列化pb结构
function NetMsgHelper:getDecodePbWithCmdId( id )
    local name = self:getCmdName(id)
    if name then
        return self:_getDecodePbWithName(name)
    end
    return nil
end

---数据获取对应反序列化pb结构
function NetMsgHelper:_getDecodePbWithName( name )
    if self._msgDecodeMap[string.lower(name)] then
        return self._msgDecodeMap[string.lower(name)]
    end
    return nil
end

return NetMsgHelper
