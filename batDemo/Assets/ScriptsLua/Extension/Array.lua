---@class Array 数组封装,数组下标从 1 开始
Array = Class("Array")

function Array:ctor()
    self.length = 0
    self._data = {}
end

-- 获取所有数据表
function Array: getData()
    return self._data
end

-- 获取下标为index的值
---@return table
function Array: get(index)
    return self._data[index]
end

-- 添加大量数据
function Array: pushAll(...)
    for i, v in ipairs{...} do
        self: push(v)
    end
end

-- 将参数逐个添加到数组尾部，返回修改后的数组长度
---@return int
function Array: push(val)
    self._data[self.length + 1] = val
    self.length = self.length + 1
    return self.length
end

-- 移除数组中的最后一项，返回移除的项
function Array: pop()
    if self.length == 0 then
        return nil
    end
    local temp = self._data[self.length]
    self._data[self.length] = nil
    self.length = self.length - 1
    return temp
end

-- 移除数组中下标为index的数据，返回移除的数据
---@param index number
function Array: split(index)
    if index < 1 or index > self.length then
        return nil
    end
    local temp = self._data[index]
    table.remove(self._data, index)
    self.length = self.length - 1
    return temp
end

-- 从数组中移除某个值
function Array: remove(val)
    for k,v in pairs(self._data) do
        if v == val then
            self: split(k)
            return
        end
    end
end

-- 数组排序，默认从小到大排序
---@param sortFunc fun(a:V, b:V):boolean
function Array: sort(sortFunc)
    if sortFunc == nil then
        table.ascendingSortTable(self._data)
    else
        table.sort(self._data, sortFunc)
    end
end

-- 顺序遍历数据，执行指定函数
---@param func fun(k, v)
function Array: traverse(func)
    for k = 1, self.length, 1 do
        local v = self:get(k)
        func(k, v)
    end
end

function Array: clearAll()
    self.length = 0
    self._data = {}
end

function Array: toString()
    local res = ""
    self:traverse(function (k, v)
        res = res .. tostring(k) .. ':' .. tostring(v) .. '  '
    end)
    return res
end

-- test
--local arr = Array:new()
--arr:pushAll(9,8,7,6,5,4,3,2,1)
--logError(arr.length)
--logError(arr:toString())
--
--arr:sort()
--logError(arr:toString())
--
--arr:push(999)
--logError(arr.length)
--logError(arr:toString())
--
--arr:split(2)
--logError(arr.length)
--logError(arr:toString())
--
--arr:push(888)
--logError(arr.length)
--logError(arr:toString())
--
--logError(arr:pop())
--logError(arr:toString())
--
--arr:sort()
--logError(arr:toString())

