table.merge = function(dest, src)
	if type(dest) ~= "table" or type(src) ~= "table" then
		return
	end
	for k, v in pairs(src) do
		dest[k] = v
	end
end

-- 遍历table，执行指定函数
table.doFunc = function(tab, func)
    for k, v in pairs(tab) do
        func(k, v)
    end
end

table.removeOne = function(tab, value)
    for k, v in pairs(tab) do
        if v == value then
            tab[k] = nil
            return true
        end
    end
    return false
end

table.removeByKey = function(tab,key)
    for k, v in pairs(tab) do
        if k == key then
            tab[k] = nil
            return
        end
    end
end

table.contains = function(tab, value)
    for k, v in pairs(tab) do
        if v == value then return true end
    end
    return false
end

table.length = function(tab)
	local i = 0
	for k, v in pairs(tab) do
		i = i + 1
	end
	return i
end


--深度拷贝table
table.copyTable = function( tableObj )
    if tableObj == nil then
        return nil
    end

    local lookup_table = {}
	local function _copy(tableObj)
		if Utils.isTable(tableObj) == false then
			return tableObj
		elseif lookup_table[tableObj] then
            return lookup_table[tableObj]
		end
		local new_table = {}
		lookup_table[tableObj] = new_table

		for index, value in pairs(tableObj) do
			new_table[_copy(index)] = _copy(value)
		end
		return setmetatable(new_table, getmetatable(tableObj))
    end

	return _copy(tableObj)
end

-- table升序排序 list<int>
table.ascendingSortTable = function( tableObj )
    local function ascendingSort(a,b)
        return tonumber(a) < tonumber(b)
    end
    table.sort(tableObj,ascendingSort)
end

-- table降序排序 list<int>
table.descendingSortTable = function( tableObj )
    local function descendingSort(a,b)
        return tonumber(a) > tonumber(b)
    end
    table.sort(tableObj,descendingSort)
end

-- 获取map长度
table.getMapLength = function(map)
    local len = 0;
    for k,v in pairs(map) do
        len = len + 1
    end
    return len
end

-- 获取第一个
table.getFirst = function(map, needRemove)
    if map == nil then
        return nil
    end
    for k,v in pairs(map) do
        if needRemove == true then
            map[k] = nil
        end
        return v
    end
    return nil
end

table.print =function(msgLint,table)
    log(msgLint)
    for i, v in pairs(table) do
        log(i,v)
    end
end