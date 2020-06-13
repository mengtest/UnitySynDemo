require("common.Class")
require("common/json")
log=print
function haveStr(str)
    return str~=nil and str~=""
end
function isFunction(obj)
    local t=type(obj)
    return t=="function"
end
function isString(str)
    local t=type(str)
    return t=="string"
end
function isTable(obj)
    local t=type(obj)
    return t=="table"
end
---@return table
function tableReadOnly(tab)
    return tab
    --if AppConst.isProduce then
    --    return table
    --else
    --    local robj={}
    --    setmetatable(robj,{__index=table,__newindex=function (table,key,value)
    --        logError("配置表为只读的,不能给字段赋值:"..key)
    --    end})
    --    return robj
    --end
end

function MergeArrToSrc(src,dst)
    for k,v in pairs(dst) do
        table.insert(src,v)
    end
end

---@return boolean
function InArray(array,value)
    if array==nil then
        return false
    end
    for k,v in pairs(array) do
        if v==value then
            return true
        end
    end
    return false
end

---@return int
function InArrayIndex(array,value)
    if array==nil then
        return false
    end
    for k,v in pairs(array) do
        if v==value then
            return k
        end
    end
    return -1
end

---@param str string
---@param strSplit string
function Split(str, strSplit,toNumber)
     local nFindStartIndex = 1
     local nSplitArray = {}
     local v
     while true do
            local nFindLastIndex = string.find(str, strSplit, nFindStartIndex)
            if not nFindLastIndex then
                v=string.sub(str, nFindStartIndex, string.len(str))
                if toNumber then v=tonumber(v) end
                 table.insert(nSplitArray,v)
                 break
               end
            v=string.sub(str, nFindStartIndex, nFindLastIndex - 1)
         if toNumber then v=tonumber(v) end
            table.insert(nSplitArray,v)
            nFindStartIndex = nFindLastIndex + string.len(strSplit)
         end
     return nSplitArray
end