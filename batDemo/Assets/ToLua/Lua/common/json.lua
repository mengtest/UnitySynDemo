local cjson=require("cjson")
json={}
---@param str string
---@return userdata
function json.decode(str)
    local ok,r=pcall(cjson.decode,str)
    if ok then
        return r
    else
        print("该json格式错误:"..str)
    end
end
---@param table userdata
---@return string
function json.encode(table,ignorefun)
    if ignorefun and isTable(table) then
        local temp={}
        for k,v in pairs(table) do
            if not isFunction(v) then
                temp[k]=v
            end
        end
        table=temp
    end
    local ok,r=pcall(cjson.encode,table)
    if ok then
        return r
    else
        --print("json encode失敗")
    end
end