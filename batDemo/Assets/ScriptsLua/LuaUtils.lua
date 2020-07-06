---@class LuaUtils
LuaUtils = {}
---@return LuaUtils
-- module('Utils')

local json = require ("Common.ThirdParty.Dkjson")
local prettyPrint = require "Common.ThirdParty.Prettyprint"
-- local Base64 = require "Common.ThirdParty.Base64"

local moneyUnits = {"K", "M", "B", "T"}

function LuaUtils.shortenMoney(num)
    local unit = 0
    local number = num
    while (number >= 1000) and (unit < #moneyUnits) do
        unit = unit + 1
        number = number / 1000;
    end
    if unit >= 1 then
        return number..moneyUnits[unit]
    else
        return number
    end
end

--@param	sName:要切割的字符串
--@param	nShowCount：显示英文字母个数，中文字数为一半,不可为空
--@param    isNeedConact 是否需要点点点，默认需要
function LuaUtils.shortenNick(sName,nShowCount,isNeedConact)
    if sName == nil or nShowCount == nil then
        return
    end
    local sStr = sName
    local tCode = {}
    local tName = {}
    local nLenInByte = #sStr
    local nWidth = 0
    local restNum = 3
    if(isNeedConact ~= nil and isNeedConact == false or nShowCount < 3)then
        restNum = 0
    end
    for i=1,nLenInByte do
        local curByte = string.byte(sStr, i)
        local byteCount = 0;
        if curByte>0 and curByte<=127 then
            byteCount = 1
        elseif curByte>=192 and curByte<223 then
            byteCount = 2
        elseif curByte>=224 and curByte<239 then
            byteCount = 3
        elseif curByte>=240 and curByte<=247 then
            byteCount = 4
        end
        local char = nil
        if byteCount > 0 then
            char = string.sub(sStr, i, i+byteCount-1)
            i = i + byteCount -1
        end
        if byteCount == 1 then
            nWidth = nWidth + 1
            table.insert(tName,char)
            table.insert(tCode,1)
            
        elseif byteCount > 1 then
            nWidth = nWidth + 2
            table.insert(tName,char)
            table.insert(tCode,2)
        end
    end

    local totalNum = nShowCount - restNum
    if nWidth > totalNum then
        local _sN = ""
        local _len = 0
        for i=1,#tName do
            if _len >= totalNum then
                break
            end
            _sN = _sN .. tName[i]
            _len = _len + tCode[i]
        end
        sName = (isNeedConact ~= nil and isNeedConact == false) and _sN or _sN .. "..." 
    end
    return sName
end

function LuaUtils.RGB32(r, g, b)
    return Color(r / 256, g / 256, b / 256, 1)
end

function LuaUtils.RGBA32(r, g, b, a)
    return Color(r / 256, g / 256, b / 256, a / 256)
end

function LuaUtils.setGreyTextEffect(go, isGrey)
    local component = go:GetComponent("TextEffect")
    if component then
        component:SetGreyEffect(isGrey)
    end
end

function LuaUtils.getPrintString( ... )
    return prettyPrint.getString(...)
end

function LuaUtils.getPrintStringAll( ... )
    return prettyPrint.getStringAll(...)
end



--时间戳之间天数差的计算（秒）
function LuaUtils.disDayBetweenTwoTime(oldTime,newTime)
    if(oldTime and newTime and oldTime < newTime)then
        return math.ceil((newTime - oldTime)/(3600 * 24))
    else
        return ""
    end
end

--是否是一个空的GameObject
function LuaUtils.isNilGameObject(go)
    return go == nil or go:Equals(nil)
end

-- 打印table. todo: 在release版本禁用此方法.
function LuaUtils.printTable(tab)
    if type(tab) ~= "table" then
        print(tab)
        return
    end
    local str = {}
    --保存已打印的table，防止有环table死循环
    local printed = {}
    local singleIndent = "    "

    local function basicFormat(o)
        if type(o) == "number" then
            return tostring(o)
        else
            return string.format("%q", o)
        end
    end

    local function internal(tab, str, indent)
        printed[tab] = true
        table.insert(str, indent .. "{")
        local subIndent = indent .. singleIndent
        for k,v in pairs(tab) do
            if type(v) == "table" then
                if not printed[v] then
                    table.insert(str, subIndent .. basicFormat(k).." = ")
                    internal(v, str, subIndent)
                else
                    table.insert(str, subIndent..basicFormat(k).." = [nest table]")
                end
            else
                table.insert(str, subIndent..basicFormat(k).." = "..basicFormat(v))
            end
        end
        table.insert(str, indent .. "}")
    end
    internal(tab, str, "")
    printed = nil
    print(table.concat(str, "\n"))
end

-- table转string json
function LuaUtils.jsonEncode(luaTable)
    if luaTable and type(luaTable) == "table" then
        local str = json.encode(luaTable, { indent = true })
        return str
    end
    return nil
end

-- string json转table
function LuaUtils.jsonDecode(str)
    if str and type(str) == "string" then
        local obj, pos, err = json.decode (str, 1, nil)

        if err then
            return nil
        else
            return obj
        end
    end

    return nil
end

-- 是否存在文件
function LuaUtils.existsFile( path )
    return System.IO.File.Exists(path)
end

-- 根目录 是否存在文件
function LuaUtils.existsFileWithName( fileName )
    if fileName ~= nil then
        return System.IO.File.Exists(Application.persistentDataPath.."/"..fileName)
    end
    return false
end

-- 删除文件
function LuaUtils.deleteFile( path )
    if path then
        System.IO.File.Delete(path)
    end
end

-- 读取文件
function LuaUtils.readFileAllBytes( path )
    if LuaUtils.existsFile(path) then
        return System.IO.File.ReadAllBytes(path)
    end
    return nil
end

-- 读取文件
function LuaUtils.readFileAllText( path )
    if LuaUtils.existsFile(path) then
        return System.IO.File.ReadAllText(path)
    end
    return nil
end


-- 延时回调
function LuaUtils.delayCall(func, delay)
    if delay == 0 or not delay then
        LuaUtils.safeCallback(func)
        return
    end
    local timer = Timer.New(func, delay, 1)
    timer:Start()
    return timer
end

-- 循环回调
function LuaUtils.loopCall(func, interval, times)
    local loopTimes = times ~= nil and times or -1
    local timer = Timer.New(func, interval, loopTimes)
    timer:Start()
    return timer
end

-- 设置随机数种子
function LuaUtils.setRandomseed()
    math.randomseed(tostring(os.time()):reverse():sub(1, 6))
end

-- 分隔数字
function LuaUtils.formatNumberWithSplit(num)
    local tmp = tonumber(num)
	if tmp and type(tmp) == "number" then
		local str = tostring(tmp)
		local len = #str
		local ret = ""
		if len > 3 then
			local splitNum = math.ceil(len / 3) - 1
			local startIndex
			if len % 3 == 0 then
				startIndex = 3
			else 
				startIndex = len % 3
            end
            ret = string.sub(str, 1, startIndex)
            for i = 1, splitNum do
                ret = ret .. "," .. string.sub(str, startIndex + 3 * (i - 1) + 1, startIndex + 3 * (i))
            end
		else 
			ret = str
        end
		return ret
	else 
		return ""
    end
end

function LuaUtils.warpFunc( func, ... )
    local args = {...}
    return function ( )
        if LuaUtils.isFunction(func) then
            func(unpack(args))
        end
    end
end

-- 安全回调
function LuaUtils.safeCallback( callback, ... )
    if LuaUtils.isFunction(callback) then
        callback(...)
    end
end

-- 安全调用方法 如果异常会打印异常 返回值是否调用方法
function LuaUtils.safeCallFunc( func, ... )
    if LuaUtils.isFunction(func) then
        local ret, err = pcall(func, ...)
        if ret then
            return true
        end

        logError("call func : ", tostring(func), " , err : ", err)
        return false
    end
    return false
end

-- 安全调用方法 如果异常会打印异常 带方法返回值
function LuaUtils.safeDoFunc( func, ... )
    if LuaUtils.isFunction(func) then
        local result = {}
        local ret, err = pcall(function( ... )
            result = {func(...)}
        end, ...)
        if ret then
            return unpack(result)
        end
        logError("call func : ", tostring(func), " , err : ", err)
    else
        logError("func not a function")
    end
end

function LuaUtils.shuffle(t)
    if type(t)~="table" then
        return
    end
    local tab = {}
    local index = 1
    while #t~= 0 do
        local n = math.random(0,#t)
        if t[n] ~= nil then
            tab[index] = t[n]
            table.remove(t, n)
            index = index + 1
        end
    end
    return tab
end
--是否是一个空的GameObject
function LuaUtils.isNilGameObject(go)
    return go == nil or go:Equals(nil)
end
-- string类型判断
function LuaUtils.isString( value )
    return value ~= nil and type(value) == "string"
end

-- number类型判断
function LuaUtils.isNumber( value )
    return value ~= nil and type(value) == "number"
end

-- table类型判断
function LuaUtils.isTable( value )
    return value ~= nil and type(value) == "table"
end

function LuaUtils.isBoolean( value )
    return type(value) == "boolean"
end

-- function类型判断
function LuaUtils.isFunction( value )
    return type(value) == "function"
end

-- 移除所有的子节点
function LuaUtils.removeAllChildren(go)
    if go and not go:Equals(nil) then
        local transform = go.transform
        for i = 1, transform.childCount do
            GameObject.DestroyImmediate(transform:GetChild(0).gameObject)
        end
    end
end

function LuaUtils.getTableLan(value)
    if value and type(value)=="table"  then
        local count = 0
        for i, v in pairs(value) do
            count = count + 1
        end
        return count 
    end
    return 0
end


function LuaUtils.catch(what)
    return what[1]
end

function LuaUtils.try(what)
    local status, result = pcall(what[1])
    if not status then
        what[2](result)
    end
    return result
end

-- 数字格式化
function LuaUtils.formatBigNumber(num, fixed)
    if LuaUtils.isNumber(num) then
        local tmp, str, ret, retUnit = num, tostring(num), "", ""
        local len = string.len(str)
        if len >= 13 then
            tmp = tmp / 1000000000000
            retUnit = "T"
        elseif len >= 10 then
            tmp = tmp / 1000000000
            retUnit = "B"
        elseif len >= 7 then
            tmp = tmp / 1000000
            retUnit = "M"
        elseif len >= 5 then
            tmp = tmp / 1000;
            retUnit = "K";
        else
            return LuaUtils.formatNumberWithSplit(num)
        end

        if LuaUtils.isNumber(fixed) and fixed >= 0 then
            fixed = math.floor(fixed)
            local fmt = "%."..tostring(fixed).."f"
            ret = string.format(fmt, tmp)
        else
            ret = string.format("%.0f", math.floor(tmp * 100) / 100)
        end
        return ret .. retUnit
    else
        return ""
    end
end

-- 获得当天日期 yyyy-mm-dd
function LuaUtils.getDayOfYear()
    local time = os.time()
    local tab = os.date("*t",time)
    local str = string.format("%.4d-%.2d-%.2d",tab.year,tab.month,tab.day)
    return str
end

-- 获得当天是一年中第几周 yyyy-week
function LuaUtils.getWeekOfYear()
    local today = os.date("*t",os.time())
    local firstDayTime = os.date("*t",os.time({year = today.year, month = 1, day =1}))

    local firstWeekend = 7 - firstDayTime.wday

    local currentDay = today.yday
    local week = LuaUtils.getCeil((currentDay - firstWeekend) / 7) + 1

    return today.year.."-"..week
end

-- 取整
function LuaUtils.getCeil(value)

    if value <= 0 then
        return math.ceil(value);
     end
     
     if math.ceil(value) == value then
        value = math.ceil(value);
     else
        value = math.ceil(value) - 1;
     end
     return value;
end


function LuaUtils.SetJson(table,ignoreFunc)
    json.encode(table,ignoreFunc)
end

function LuaUtils.GetTable(Json)
    json.decode(Json)
end


function LuaUtils.dump2(tb, dump_metatable, max_level)
	local lookup_table = {}
	local level = 0
	local rep = string.rep
	local dump_metatable = dump_metatable
	local max_level = max_level or 1

	local function _dump(tb, level)
		local str = "\n" .. rep("\t", level) .. "{\n"
		for k,v in pairs(tb) do
			local k_is_str = type(k) == "string" and 1 or 0
			local v_is_str = type(v) == "string" and 1 or 0
			str = str..rep("\t", level + 1).."["..rep("\"", k_is_str)..(tostring(k) or type(k))..rep("\"", k_is_str).."]".." = "
			if type(v) == "table" then
				if not lookup_table[v] and ((not max_level) or level < max_level) then
					lookup_table[v] = true
					str = str.._dump(v, level + 1, dump_metatable).."\n"
				else
					str = str..(tostring(v) or type(v))..",\n"
				end
			else
				str = str..rep("\"", v_is_str)..(tostring(v) or type(v))..rep("\"", v_is_str)..",\n"
			end
		end
		if dump_metatable then
			local mt = getmetatable(tb)
			if mt ~= nil and type(mt) == "table" then
				str = str..rep("\t", level + 1).."[\"__metatable\"]".." = "
				if not lookup_table[mt] and ((not max_level) or level < max_level) then
					lookup_table[mt] = true
					str = str.._dump(mt, level + 1, dump_metatable).."\n"
				else
					str = str..(tostring(v) or type(v))..",\n"
				end
			end
		end
		str = str..rep("\t", level) .. "},"
		return str
	end
	
	return _dump(tb, level)
end

function LuaUtils.catch(what)
    return what[1]
end

function LuaUtils.try(what)
    local status, result = pcall(what[1])
    if not status then
        what[2](result)
    end
    return result
end



return LuaUtils